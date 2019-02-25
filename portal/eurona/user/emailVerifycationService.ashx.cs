using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.Common;
using System.Data;
using System.Text;
using System.IO;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;
using System.Web.UI;
using CMS;
using System.Web.SessionState;
using System.Configuration;
using System.Data.SqlClient;

namespace Eurona.User {
    /// <summary>
    /// Summary description for emailVerifycationService
    /// </summary>
    public class emailVerifycationService : IHttpHandler, IReadOnlySessionState {
        public enum JSONResponseStatus : int {
            SUCCESS = 0,
            ERROR = 1,
            MESSAGE = 2,
        }

        public void ProcessRequest(HttpContext context) {
            string method = context.Request["method"];
            if (method == "checkEmail") {
                checkEmail(context);
            } else if (method == "sendEmail2EmailVerify") {
                sendEmail2EmailVerify(context);
            } else if (method == "verify") {
                verify(context);
            } else if (method == "verifyFinish") {
                verifyFinish(context);
            } else if (method == "verifyCancel") {
                verifyCancel(context);
            } else if (method == "getRedirectUrlAfterVerify") {
                getRedirectUrlAfterVerify(context);
            }

        }

        /// <summary>
        /// Metoda, zisti ci dany email uz naahodou nieje overeny.
        /// Ak je overeny vrati chybu.
        /// </summary>
        /// <param name="context"></param>
        private void checkEmail(HttpContext context) {
            string email = GetRequestData(context.Request);
            //Zistim, ci email uz nahodou nieje overeny!
            Account account = Storage<Account>.ReadFirst(new Account.ReadByEmailToVerify { EmailToVerify = email, OnlyEmailVerified = true });
            StringBuilder sbJson = new StringBuilder();

            int status = (int)JSONResponseStatus.SUCCESS;
            string errorMessage = "";
            if (account != null) {
                status = (int)JSONResponseStatus.ERROR;
                errorMessage = Resources.Strings.EmailVerifyControl_EmailValidation_EmailJeJizOveren;
            }

            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
            }

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }

        /// <summary>
        /// Odoslanie emaily poradcovi aj s linkom na overenie emailu
        /// </summary>
        /// <param name="context"></param>
        private void sendEmail2EmailVerify(HttpContext context) {
            string email = GetRequestData(context.Request);
            int status = (int)JSONResponseStatus.SUCCESS;
            string errorMessage = "";
            if (Security.IsLogged(false)) {
                string code = string.Format("{0}|{1}|{2}", email, Security.Account.Id, Utilities.GetUserIP(context.Request));
                code = CMS.Utilities.Cryptographer.Encrypt(code);
                string url = Utilities.Root(context.Request) + "user/emailVerifycation.aspx?code=" + code;

                Security.Account.EmailVerifyCode = code;
                Security.Account.EmailToVerify = email;
                Storage<Account>.Update(Security.Account);
                if (SendEmailVerificationEmail(email, url)) {
                    Security.Account.EmailVerifyStatus = (int)Account.EmailVerifyStatusCode.EMAIL_SEND;
                    Storage<Account>.Update(Security.Account);
                } else {
                    status = (int)JSONResponseStatus.ERROR;
                }
            } else {
                status = (int)JSONResponseStatus.ERROR;
                errorMessage = String.Format("sendEmail2EmailVerify:User not Loged!({0})", email);
            }
            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
            }

            StringBuilder sbJson = new StringBuilder();
            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }

        private string[] decodeVerifyCode(string codeEncrypted) {
            string code = CMS.Utilities.Cryptographer.Decrypt(codeEncrypted);
            string[] data = code.Split('|');
            return data;
        }

        /// <summary>
        /// Overenie emailu a verifikacneho kodu
        /// </summary>
        /// <param name="context"></param>
        private void verify(HttpContext context) {
            int status = (int)JSONResponseStatus.SUCCESS;
            string detailedMessage = "";
            string errorMessage = Resources.Strings.EmailVerifyControl_EmailVerifiedFailed_Message;
            string codeEncrypted = GetRequestData(context.Request);

            string[] data = decodeVerifyCode(codeEncrypted);
            string emailFromCode = data[0];
            int accountFromCode = Convert.ToInt32(data[1]);
            string ipAddress = data[2];
            Account accountByVerifyCode = Storage<Account>.ReadFirst(new Account.ReadByEmailVerifyCode { EmailVerifyCode = codeEncrypted });

            StringBuilder sbJson = new StringBuilder();

            //Check IP
            if (ipAddress != Utilities.GetUserIP(context.Request)) {
                status = (int)JSONResponseStatus.ERROR;
                errorMessage = String.Format( Resources.Strings.EmailVerifyControl_EmailValidation_OvereniZJinehoZarizeni, accountByVerifyCode.EmailToVerify);
                sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.Write(sbJson.ToString());
                context.Response.End();
                return;
            }

            if (accountByVerifyCode != null && accountByVerifyCode.EmailVerified.HasValue) {
                status = (int)JSONResponseStatus.MESSAGE;
                string message = Resources.Strings.EmailVerifyControl_EmailValidation_UcetJeJizOverenPokracovat;
                sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\", \"Message\":\"{2}\" }}", status, "", message);
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.Write(sbJson.ToString());
                context.Response.End();                
                return;
            }

            //Prihlasenie pouzivatela ak nieje prihlaseny
            if (!Security.IsLogged(false) && accountByVerifyCode != null) {
                Security.Login(accountByVerifyCode, false);
            }

            if (Security.IsLogged(false)) {
                if (accountFromCode != Security.Account.Id) {
                    status = (int)JSONResponseStatus.ERROR;
                    detailedMessage = "verify:Verify token falsified! Invalid Account!";
                } else if (emailFromCode != Security.Account.EmailToVerify) {
                    status = (int)JSONResponseStatus.ERROR;
                    detailedMessage = "verify:Verify token falsified! Invalid Email!";
                }

                if (status == (int)JSONResponseStatus.SUCCESS) {
                    if (accountByVerifyCode == null) {
                        status = (int)JSONResponseStatus.ERROR;
                        detailedMessage = "verify:Verify token falsified! Invalid token!";
                    } else if (accountByVerifyCode.Id != Security.Account.Id) {
                        status = (int)JSONResponseStatus.ERROR;
                        detailedMessage = "verify:Verify token falsified! Invalid Account from Token!";
                    } else if (accountByVerifyCode.EmailToVerify != Security.Account.EmailToVerify) {
                        status = (int)JSONResponseStatus.ERROR;
                        detailedMessage = "verify:Verify token falsified! Invalid Email from Token!";
                    }
                }

            } else {
                status = (int)JSONResponseStatus.ERROR;
                detailedMessage = "verify:User not Loged!";
                detailedMessage = detailedMessage + String.Format(" ({0})", emailFromCode);
            }

            if (status == (int)JSONResponseStatus.SUCCESS) {
                Security.Account.EmailVerifyStatus = (int)Account.EmailVerifyStatusCode.DATA_VALIDATED;
                Storage<Account>.Update(Security.Account);
            }
            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(detailedMessage, System.Diagnostics.EventLogEntryType.Error);
            }

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }

        /// <summary>
        /// Konecny zapis overenych udajov
        /// </summary>
        /// <param name="context"></param>
        private void verifyFinish(HttpContext context) {
            int status = (int)JSONResponseStatus.SUCCESS;
            string message = "";
            string errorMessage = "";

            if (Security.IsLogged(false)) {
                if (Security.Account.EmailVerifyStatus != (int)Account.EmailVerifyStatusCode.DATA_VALIDATED) {
                    status = (int)JSONResponseStatus.ERROR;
                    message = "EmailVerifyStatusCode is NOT Validated! ";
                } else {
                    string pwd = GetRequestData(context.Request);
                    pwd = CMS.Utilities.Cryptographer.MD5Hash(pwd);

                    Security.Account.Login = Security.Account.EmailToVerify;
                    Security.Account.Email = Security.Account.EmailToVerify;
                    Security.Account.Password = pwd;
                    Security.Account.EmailVerifyStatus = (int)Account.EmailVerifyStatusCode.VERIFIED;
                    Security.Account.EmailVerified = DateTime.Now;
                    Storage<Account>.Update(Security.Account);

                    //Update Organization
                    Organization organization = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = Security.Account.Id });
                    organization.ContactEmail = Security.Account.EmailToVerify;
                    Storage<Organization>.Update(organization);

                    //Sync TVD User Data
                    if (!UpdateTVDUser(organization)) {
                        Security.Account.EmailVerified = null;
                        Storage<Account>.Update(Security.Account);
                        status = (int)JSONResponseStatus.ERROR;
                        message = "verifyFinish:TVD Error!";
                        errorMessage = "Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!";
                    } else {
                        SendEmailVerificationFinishedEmail(Security.Account.EmailToVerify);
                        message = String.Format(Resources.Strings.EmailVerifyControl_VerifycationSuccessFinish_Message, Security.Account.Login);
                        message = message.Replace("\r\n", "<br/>");
                    }
                }

            } else {
                status = (int)JSONResponseStatus.ERROR;
                message = "verifyFinish:User not Loged!";
            }

            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
            }

            StringBuilder sbJson = new StringBuilder();
            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"Message\":\"{1}\", \"ErrorMessage\":\"{2}\" }}", status, message, errorMessage);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }

        private void getRedirectUrlAfterVerify(HttpContext context) {
            int status = (int)JSONResponseStatus.SUCCESS;
            string message = "";

            string codeEncrypted = GetRequestData(context.Request);
            string[] data = decodeVerifyCode(codeEncrypted);
            string emailFromCode = data[0];
            int accountFromCode = Convert.ToInt32(data[1]);
            Account accountByVerifyCode = Storage<Account>.ReadFirst(new Account.ReadByEmailVerifyCode { EmailVerifyCode = codeEncrypted });
            //Prihlasenie pouzivatela ak nieje prihlaseny
            if (!Security.IsLogged(false) && accountByVerifyCode != null) {
                Security.Login(accountByVerifyCode, false);
            }

            StringBuilder sbJson = new StringBuilder();
            string url = "";
            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
            }

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\", \"Url\":\"{2}\" }}", status, message, url);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }


        /// <summary>
        /// Zrusenie overenia
        /// </summary>
        /// <param name="context"></param>
        private void verifyCancel(HttpContext context) {
            int status = (int)JSONResponseStatus.SUCCESS;
            string message = "";
            Security.Logout();
            StringBuilder sbJson = new StringBuilder();
            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, message);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();

            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
        /// <summary>
        /// Get request string from stream
        /// </summary>
        private string GetRequestData(HttpRequest request) {
            Stream stream = request.InputStream;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();
            return request.ContentEncoding.GetString(data);
        }

        private static bool UpdateTVDUser(Common.DAL.Entities.Organization organization) {
            Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = organization.AccountId.Value });

            Organization parentOrg = new Organization();
            if (organization.ParentId.HasValue) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = organization.ParentId.Value });

            try {
                string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
                CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
                using (SqlConnection connection = tvdStorage.Connect()) {
                    //Vystupne parametra
                    //----------------------------------
                    //Probehlo	bit	1=úspěch, 0=chyba		
                    //Zprava	varchar(255)	text chyby		
                    //Id_odberatele	int	prim. klíč		
                    //Kod_odberatele	char(14)	reg. číslo	
                    SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
                    probehlo.Direction = ParameterDirection.Output;

                    SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
                    zprava.Direction = ParameterDirection.Output;
                    zprava.SqlDbType = SqlDbType.VarChar;
                    zprava.Size = 255;

                    SqlParameter id_odberatele = new SqlParameter("@out_Id_odberatele", -1);
                    id_odberatele.Direction = ParameterDirection.Output;

                    SqlParameter kod_odberatele = new SqlParameter("@out_Kod_odberatele", string.Empty);
                    kod_odberatele.Direction = ParameterDirection.Output;
                    kod_odberatele.SqlDbType = SqlDbType.Char;
                    kod_odberatele.Size = 14;
#if !__DEBUG_VERSION_WITHOUTTVD
                    tvdStorage.ExecProc(connection, "esp_www_registrace",
                            new SqlParameter("Nova_registrace", false),
                            new SqlParameter("Id_odberatele", DatabaseTools.Null(account.TVD_Id)),
                            new SqlParameter("Stat_odberatele", DatabaseTools.Null(organization.RegisteredAddress.State)), // ????
                            new SqlParameter("Kod_nadrizeneho", DatabaseTools.Null(parentOrg.Code)),
                            new SqlParameter("Nazev_firmy", DatabaseTools.Null(organization.Name)),
                            new SqlParameter("Nazev_firmy_radek", DatabaseTools.Null(organization.Name)),
                            new SqlParameter("Ulice", DatabaseTools.Null(organization.RegisteredAddress.Street)),
                            new SqlParameter("Psc", DatabaseTools.Null(organization.RegisteredAddress.Zip)),
                            new SqlParameter("Misto", DatabaseTools.Null(organization.RegisteredAddress.City)),
                            new SqlParameter("Stat", DatabaseTools.Null(organization.RegisteredAddress.State)), // ????
                            new SqlParameter("Dor_nazev_firmy", DatabaseTools.Null(organization.Name)),
                            new SqlParameter("Dor_nazev_firmy_radek", string.Empty),
                            new SqlParameter("Dor_ulice", DatabaseTools.Null(organization.CorrespondenceAddress.Street)),
                            new SqlParameter("Dor_psc", DatabaseTools.Null(organization.CorrespondenceAddress.Zip)),
                            new SqlParameter("Dor_misto", DatabaseTools.Null(organization.CorrespondenceAddress.City)),
                            new SqlParameter("Dor_stat", DatabaseTools.Null(organization.CorrespondenceAddress.State)), // ????
                            new SqlParameter("Telefon", DatabaseTools.Null(organization.ContactPhone)),
                            new SqlParameter("Mobil", DatabaseTools.Null(organization.ContactMobile)),
                            new SqlParameter("E_mail", DatabaseTools.Null(organization.ContactEmail)),
                            new SqlParameter("Cislo_op", DatabaseTools.Null(organization.ContactCardId)), // DOPLNIT
                            new SqlParameter("Ico", DatabaseTools.Null(organization.Id1)),
                            new SqlParameter("Dic", DatabaseTools.Null(organization.Id2)),
                            new SqlParameter("P_f", DatabaseTools.Null(organization.PF)),// P alebo F ??
                            new SqlParameter("Banka", DatabaseTools.Null(organization.BankContact.BankCode)),
                            new SqlParameter("Cislo_uctu", DatabaseTools.Null(organization.BankContact.AccountNumber)),
                            new SqlParameter("Login_www", DatabaseTools.Null(account.Login)),
                            new SqlParameter("Datum_zahajeni", DatabaseTools.Null(DateTime.Now)),
                            new SqlParameter("Platce_dph", DatabaseTools.Null(organization.VATPayment)),
                            new SqlParameter("Statut", DatabaseTools.Null(organization.Statut)), //NRP nebo NRZ // ????
                            new SqlParameter("Spec_symbol", DatabaseTools.Null(string.Empty)), // ????
                            new SqlParameter("Kod_oblasti", DatabaseTools.Null(organization.RegionCode)), // ????
                            new SqlParameter("Datum_narozeni", DatabaseTools.Null(organization.ContactBirthDay)), // DOPLNIT
                            new SqlParameter("Telefon_prace", DatabaseTools.Null(organization.ContactWorkPhone)), // DOPLNIT

                            new SqlParameter("Fax", DatabaseTools.Null(organization.FAX)), // DOPLNIT
                            new SqlParameter("Icq", DatabaseTools.Null(organization.ICQ)), // DOPLNIT
                            new SqlParameter("Skype", DatabaseTools.Null(organization.Skype)), // DOPLNIT

                            new SqlParameter("Top_manager", DatabaseTools.Null(organization.TopManager)), // DOPLNIT

                            probehlo, zprava, id_odberatele, kod_odberatele
                            );

                    //Vystupne parametra
                    //----------------------------------
                    //Probehlo	bit	1=úspěch, 0=chyba		
                    //Zprava	varchar(255)	text chyby		
                    //Id_odberatele	int	prim. klíč		
                    //Kod_odberatele	char(14)	reg. číslo		
#else
					Random r = new Random( 1000 );
					probehlo.Value = true;
					zprava.Value = "";
					id_odberatele.Value = (( 999 ) * 1000) + r.Next();
					kod_odberatele.Value = "555-555555-" + r.Next().ToString();
#endif
                    bool bSuccess = Convert.ToBoolean(probehlo.Value);
                    string message = zprava.Value.ToString();
                    if (bSuccess) {
                        return true;
                    } else {
                        EvenLog.WritoToEventLog("EmailVerifycationService->Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: " + message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }

                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);

                EvenLog.WritoToEventLog("EmailVerifycationService->Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!", System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
        }

        public bool IsReusable {
            get {
                return true;
            }
        }

        /// <summary>
        /// Odoslanie informacneho mailu s linkom pre overenie emailu.
        /// </summary>
        private bool SendEmailVerificationEmail(string email, string url) {
            EmailNotification email2User = new EmailNotification {
                To = email,
                Subject = Resources.Strings.EmailVerifyControl_UserVerificationEmail_Subject,
                Message = String.Format(Resources.Strings.EmailVerifyControl_UserVerificationEmail_Message, url).Replace("\\n", Environment.NewLine) + "<br/><br/>"
            };

            bool okUser = email2User.Notify(true);
            return okUser;
        }

        /// <summary>
        /// Odoslanie informacneho mailu o uspesnom overeni.
        /// </summary>
        private bool SendEmailVerificationFinishedEmail(string email) {
            EmailNotification email2User = new EmailNotification {
                To = email,
                Subject = Resources.Strings.EmailVerifyControl_UserVerificationFinishEmail_Subject,
                Message = String.Format(Resources.Strings.EmailVerifyControl_UserVerificationFinish_Message, email).Replace("\\n", Environment.NewLine) + "<br/><br/>"
            };

            bool okUser = email2User.Notify(true);
            return okUser;
        }
    }

    public static class DatabaseTools {
        public static object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        public static object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        public static object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        public static object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }
    }
}