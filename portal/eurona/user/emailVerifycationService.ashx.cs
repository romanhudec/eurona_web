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

namespace Eurona.User {
    /// <summary>
    /// Summary description for emailVerifycationService
    /// </summary>
    public class emailVerifycationService : IHttpHandler, IReadOnlySessionState {
        public enum JSONResponseStatus : int {
            SUCCESS = 0,
            ERROR = 1
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
            Account account = Storage<Account>.ReadFirst(new Account.ReadByEmailToVerify { EmailToVerify=email, OnlyEmailVerified=true});
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
                errorMessage = String.Format("Ověření z jiného zařízení! Ověření prosím dokončete na zařízení na které bylo započato.", accountByVerifyCode.EmailToVerify);
                sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.Write(sbJson.ToString());
                context.Response.End();
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
                return;
            }
           /*
            if (accountByVerifyCode != null && accountByVerifyCode.EmailVerified.HasValue) {
                status = (int)JSONResponseStatus.ERROR;
                errorMessage = String.Format("Uživatel {0} je již ověřen!", accountByVerifyCode.EmailToVerify);
                sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"ErrorMessage\":\"{1}\" }}", status, errorMessage);
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.Write(sbJson.ToString());
                context.Response.End();
                EvenLog.WritoToEventLog(errorMessage, System.Diagnostics.EventLogEntryType.Error);
                return;
            }*/
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

                    SendEmailVerificationFinishedEmail(Security.Account.EmailToVerify);
                    message = String.Format(Resources.Strings.EmailVerifyControl_VerifycationSuccessFinish_Message, Security.Account.Login);
                    message = message.Replace("\r\n", "<br/>");
                }

            } else {
                status = (int)JSONResponseStatus.ERROR;
                message = "verifyFinish:User not Loged!";
            }
            
            if (status != (int)JSONResponseStatus.SUCCESS) {
                EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
            }

            StringBuilder sbJson = new StringBuilder();
            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"Message\":\"{1}\", \"ErrorMessage\":\"\" }}", status, message);
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
                Message = String.Format(Resources.Strings.EmailVerifyControl_UserVerificationFinish_Message).Replace("\\n", Environment.NewLine) + "<br/><br/>"
            };

            bool okUser = email2User.Notify(true);
            return okUser;
        }
    }
}