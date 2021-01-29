using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace EuronaImportFromTVD.DAL {
    public static class EuronaDAL {
        public enum BonusovyKreditTyp : int {
            None = 0,
            RucneZadany = 1,
            OdeslanaObjednavka = 2,
            Eurosap = 10
        }

        /// <summary>
        /// Vráti true, ak produkt s ID uz je v databayi Eurona zaevidovany pod nejakou inštanciou
        /// </summary>
        public static bool ExistProduct(CMS.Pump.MSSQLStorage mssqStorageDst, int productId) {
            string sql = "SELECT Count(ProductId) FROM tShpProduct WITH (NOLOCK) WHERE ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@ProductId", productId));
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count != 0;
        }

        public static class Order {
            public enum OrderStatus : int {
                None = 0,
                WaitingForProccess = -1,
                InProccess = -2,
                Proccessed = -3,
                Storno = -4
            }

            /// <summary>
            /// Vrati ID meny podla kodu
            /// </summary>
            private static int GetCurrencyId(CMS.Pump.MSSQLStorage mssqStorageDst, string currencyCode) {
                string sql = @"
				SELECT c.CurrencyId FROM cShpCurrency c 
				INNER JOIN cSupportedLocale l ON l.Code = c.Locale
				WHERE c.Code=@Code";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@Code", currencyCode));
                if (dt.Rows.Count == 0) return 0;
                else return Convert.ToInt32(dt.Rows[0]["CurrencyId"]);
            }


            /// <summary>
            /// Vráti ID accountu z CMS podla jeho TVD_ID
            /// </summary>
            public static int GetOrderId(CMS.Pump.MSSQLStorage mssqStorageDst, string orderNumber) {
                string sql = "SELECT OrderId FROM vShpOrders WHERE OrderNumber=@orderNumber";
                using (SqlConnection connectionDst = mssqStorageDst.Connect()) {
                    DataTable dt = mssqStorageDst.Query(connectionDst, sql, new SqlParameter("@orderNumber", orderNumber));
                    if (dt.Rows.Count == 0) return 0;
                    else return Convert.ToInt32(dt.Rows[0]["OrderId"]);
                }
            }
            /*
            public static void SyncEuronaFinalOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, int orderStatusCode, string currencyCode, string notes) {
                if (orderStatusCode == 2) orderStatusCode = (int)OrderStatus.Storno;
                if (orderStatusCode == 4) orderStatusCode = (int)OrderStatus.Proccessed;
                if (orderStatusCode != (int)OrderStatus.Storno && orderStatusCode != (int)OrderStatus.Proccessed) orderStatusCode = (int)OrderStatus.InProccess;

                int currencyId = GetCurrencyId(mssqStorageDst, currencyCode);
                string sql = @"
				UPDATE tShpOrder SET OrderStatusCode=@OrderStatusCode, CurrencyId=@CurrencyId, Notes=@Notes
				WHERE OrderId=@OrderId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@OrderStatusCode", orderStatusCode.ToString()),
                        new SqlParameter("@CurrencyId", currencyId),
                        new SqlParameter("@Notes", notes));
            }
            public static void SyncEuronaFakturyOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, string currencyCode, string notes) {
                int currencyId = GetCurrencyId(mssqStorageDst, currencyCode);
                string sql = @"
				UPDATE tShpOrder SET CurrencyId=@CurrencyId, Notes=@Notes
				WHERE OrderId=@OrderId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@CurrencyId", currencyId),
                        new SqlParameter("@Notes", notes));
            }*/
            public static void SyncEuronaOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int Id_objednavky, string Id_web_objednavky, int Id_odberatele, DateTime Datum_zapisu_obj, int Zpusob_dodani, DateTime? Dodat_od, DateTime? Dodat_do, int Zpusob_platby, int Stav_faktury, decimal Celkem_k_uhrade, decimal Celkem_bez_dph,
                string Dor_nazev_firmy, string Kod_meny, string Dor_misto, string Dor_ulice, string Dor_psc, string Dor_stat, string Dor_telefon, string Dor_email, string Info_pro_odb_html,
                string zavoz_misto, DateTime? zavoz_datum, string zavoz_psc, DataTable dtRadky) {
                int orderId = GetOrderId(mssqStorageDst, Id_web_objednavky);
                if (orderId != 0) throw new Exception("Objednavka uz v Eurona Existuje!!!!");

                int accountId = Account.GetAccountId(mssqStorageDst, Id_odberatele);
                if (accountId == 0) throw new Exception("Odberatel v Eurona Neexistuje!!!!");

                int orderStatusCode = (int)OrderStatus.InProccess;
                if (Stav_faktury == 2) orderStatusCode = (int)OrderStatus.Storno;
                if (Stav_faktury == 4) orderStatusCode = (int)OrderStatus.Proccessed;
                if (Stav_faktury != (int)OrderStatus.Storno && Stav_faktury != (int)OrderStatus.Proccessed) orderStatusCode = (int)OrderStatus.InProccess;

                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                using (SqlConnection connectionDst = mssqStorageDst.Connect()) {
                    DataSet ds = mssqStorageDst.QueryProcDataSet(connectionDst, "pShpCartCreate",
                            new SqlParameter("@InstanceId", 1),
                            new SqlParameter("@AccountId", accountId),
                            new SqlParameter("@SessionId", DBNull.Value),
                            new SqlParameter("@ShipmentCode", Convert.ToString(Zpusob_dodani)),
                            new SqlParameter("@PaymentCode", ""),
                            new SqlParameter("@Closed", DateTime.Now),
                            new SqlParameter("@Notes", Info_pro_odb_html),
                            new SqlParameter("@Price", Celkem_bez_dph),
                            new SqlParameter("@PriceWVAT", Celkem_k_uhrade),
                            new SqlParameter("@Discount", 0),
                            new SqlParameter("@Status", 0),
                            new SqlParameter("@BodyEurosapTotal", 0),//?????
                            new SqlParameter("@DopravneEurosap", 0),//?????
                            new SqlParameter("@KatalogovaCenaCelkemByEurosap", 0),//????????
                            result);

                    DataTable dtDeliveryAddres = ds.Tables[0];
                    DataTable dtInvoiceAddres = ds.Tables[1];
                    DataTable dtCart = ds.Tables[2];

                    int deliveryAddressId = Convert.ToInt32(dtDeliveryAddres.Rows[0][0]);
                    int invoiceAddressId = Convert.ToInt32(dtInvoiceAddres.Rows[0][0]);
                    int cartId = Convert.ToInt32(dtCart.Rows[0][0]);
                    int currencyId = GetCurrencyId(mssqStorageDst, Kod_meny);

                    foreach (DataRow row in dtRadky.Rows) {
                        //Id_objednavky, Poradi, C_Zbo = Kod_polozky, Mnozstvi, Body_mj, Cena_mj_fakt_sdph, Sazba_dph, Cena_mj_fakt_bezdph
                        int Poradi = Convert.ToInt32(row["Poradi"]);
                        int C_Zbo = Convert.ToInt32(row["C_Zbo"]);
                        int Mnozstvi = Convert.ToInt32(row["Mnozstvi"]);
                        int Body_mj = Convert.ToInt32(row["Body_mj"]);
                        decimal Cena_mj_fakt_sdph = Convert.ToDecimal(row["Cena_mj_fakt_sdph"]);
                        decimal Sazba_dph = Convert.ToDecimal(row["Sazba_dph"]);
                        decimal Cena_mj_fakt_bezdph = Convert.ToDecimal(row["Cena_mj_fakt_bezdph"]);

                        result = new SqlParameter("@Result", -1);
                        result.Direction = ParameterDirection.Output;


                        mssqStorageDst.ExecProc(connectionDst, "pShpCartProductCreate",
                                new SqlParameter("@InstanceId", 1),
                                new SqlParameter("@CartId", cartId),
                                new SqlParameter("@ProductId", C_Zbo),
                                new SqlParameter("@Quantity", Mnozstvi),
                                new SqlParameter("@Price", Cena_mj_fakt_bezdph),
                                new SqlParameter("@PriceWVAT", Cena_mj_fakt_sdph),
                                new SqlParameter("@VAT", Sazba_dph),
                                new SqlParameter("@PriceTotal", Cena_mj_fakt_bezdph),
                                new SqlParameter("@PriceTotalWVAT", Cena_mj_fakt_sdph),
                                new SqlParameter("@Discount", 0),
                                new SqlParameter("@CurrencyId", currencyId),
                                new SqlParameter("@CerpatBK", 0),//????
                                result);

                    }

                    //Create Order

                    result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;

                    mssqStorageDst.ExecProc(connectionDst, "pShpOrderCreate",
                            new SqlParameter("@HistoryAccount", 1),
                            new SqlParameter("@InstanceId", 1),
                            new SqlParameter("@CartId", cartId),
                            new SqlParameter("@OrderDate", Datum_zapisu_obj),
                            new SqlParameter("@OrderStatusCode", orderStatusCode),
                            new SqlParameter("@ShipmentCode", Convert.ToString(Zpusob_dodani)),
                            new SqlParameter("@ShipmentPrice", 0),//??????
                            new SqlParameter("@ShipmentPriceWVAT", 0),//???????
                            new SqlParameter("@PaymentCode", DBNull.Value),//???????
                            new SqlParameter("@DeliveryAddressId", deliveryAddressId),
                            new SqlParameter("@InvoiceAddressId", invoiceAddressId),
                            new SqlParameter("@InvoiceUrl", string.Empty),
                            new SqlParameter("@PaydDate", DBNull.Value),
                            new SqlParameter("@Notes", Info_pro_odb_html),
                            new SqlParameter("@Price", Celkem_bez_dph),
                            new SqlParameter("@PriceWVAT", Celkem_k_uhrade),
                            new SqlParameter("@Notified", 0),
                            new SqlParameter("@Exported", 0),
                            new SqlParameter("@CurrencyId", currencyId),
                            new SqlParameter("@CreatedByAccountId", 1),
                            new SqlParameter("@ShipmentFrom", Null(Dodat_od)),
                            new SqlParameter("@ShipmentTo", Null(Dodat_do)),
                            result);
                    orderId = Convert.ToInt32(result.Value);

                    Dor_nazev_firmy = Dor_nazev_firmy.Trim();
                    string firstName = "";
                    string lastName = "";
                    string[] menoPriezvisko = Dor_nazev_firmy.Split(' ');

                    if(menoPriezvisko.Length == 2) {
                        lastName = menoPriezvisko[0];
                        firstName = menoPriezvisko[1];
                    }
                    //Delivery Address
                    string sql = @"UPDATE tShpAddress SET Organization=@Organization, FirstName=@FirstName, LastName=@LastName, City=@City, Street=@Street, Zip=@Zip, State=@State, Phone=@Phone, Email=@Email, Notes=@Notes
				    WHERE AddressId=@AddressId";
                    mssqStorageDst.Exec(connectionDst, sql,
                            new SqlParameter("@AddressId", deliveryAddressId),
                            new SqlParameter("@FirstName", firstName),
                            new SqlParameter("@LastName", lastName),
                            new SqlParameter("@Organization", Dor_nazev_firmy),
                            new SqlParameter("@City", Dor_misto),
                            new SqlParameter("@Street", Dor_ulice),
                            new SqlParameter("@Zip", Dor_psc),
                            new SqlParameter("@State", Dor_stat),
                            new SqlParameter("@Phone", Dor_telefon),
                            new SqlParameter("@Email", Dor_email),
                            new SqlParameter("@Notes", "")
                    );

                    //Invoice Address
                    sql = @"UPDATE tShpAddress SET Organization=@Organization, FirstName=@FirstName, LastName=@LastName, City=@City, Street=@Street, Zip=@Zip, State=@State, Phone=@Phone, Email=@Email, Notes=@Notes
				    WHERE AddressId=@AddressId";
                    mssqStorageDst.Exec(connectionDst, sql,
                            new SqlParameter("@AddressId", invoiceAddressId),
                            new SqlParameter("@FirstName", firstName),
                            new SqlParameter("@LastName", lastName),
                            new SqlParameter("@Organization", Dor_nazev_firmy),
                            new SqlParameter("@City", Dor_misto),
                            new SqlParameter("@Street", Dor_ulice),
                            new SqlParameter("@Zip", Dor_psc),
                            new SqlParameter("@State", Dor_stat),
                            new SqlParameter("@Phone", Dor_telefon),
                            new SqlParameter("@Email", Dor_email),
                            new SqlParameter("@Notes", "")
                    );


                    sql = @"UPDATE tShpOrder SET ZavozoveMisto_Mesto=@ZavozoveMisto_Mesto, ZavozoveMisto_DatumACas=@ZavozoveMisto_DatumACas, ZavozoveMisto_Psc=@ZavozoveMisto_Psc where OrderId=@OrderId";
                    mssqStorageDst.Exec(connectionDst, sql,
                            new SqlParameter("@OrderId", orderId),
                            new SqlParameter("@ZavozoveMisto_Mesto", Null(zavoz_misto)),
                            new SqlParameter("@ZavozoveMisto_DatumACas", Null(zavoz_datum)),
                             new SqlParameter("@ZavozoveMisto_Psc", Null(zavoz_psc))
                            );
                }
            }
        }

        public static class Account {
            public enum AddressType {
                Register,
                Delivery
            }

            /// <summary>
            /// Vráti TVD_ID (ID odberatela z TVD) podla AccountId v CMS
            /// </summary>
            public static int GetTVDAccountId(CMS.Pump.MSSQLStorage mssqStorageDst, int accountId) {
                string sql = "SELECT TVD_Id FROM vAccounts WHERE AccountId=@AccountId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@AccountId", accountId));
                if (dt.Rows.Count == 0) return 0;
                else return Convert.ToInt32(dt.Rows[0]["TVD_Id"]);
            }

            /// <summary>
            /// Vráti ID accountu z CMS podla jeho TVD_ID
            /// </summary>
            public static int GetAccountId(CMS.Pump.MSSQLStorage mssqStorageDst, int tvd_id) {
                string sql = "SELECT AccountId FROM vAccounts WHERE TVD_Id=@TVD_Id";
                using (SqlConnection connectionDst = mssqStorageDst.Connect()) {
                    DataTable dt = mssqStorageDst.Query(connectionDst, sql, new SqlParameter("@TVD_Id", tvd_id));
                    if (dt.Rows.Count == 0) return 0;
                    else return Convert.ToInt32(dt.Rows[0]["AccountId"]);
                }
            }

            public static int SyncAccount(CMS.Pump.MSSQLStorage mssqStorageDst, int accountTVDId, int instanceId, string login, string password, string email, bool enabled, string roles, bool canAccessIntensa, DateTime? registerDate) {
                //string pwd = CMS.Utilities.Cryptographer.MD5Hash(password);
                string pwd = password;

                string sql = "SELECT AccountId FROM vAccounts WHERE TVD_Id=@TVD_Id AND InstanceId = @InstanceId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@TVD_Id", accountTVDId), new SqlParameter("@InstanceId", instanceId));
                bool accountExist = dt.Rows.Count != 0;
                //INSERT
                if (!accountExist) {
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;

                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pAccountCreate",
                            new SqlParameter("@HistoryAccount", 1),
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@Login", email),
                            new SqlParameter("@Password", pwd),
                            new SqlParameter("@Enabled", enabled),
                            new SqlParameter("@Verified", 1),
                            new SqlParameter("@Roles", roles),
                            new SqlParameter("@Email", email),
                            result);

                    if (result.Value == DBNull.Value) return 0;

                    int accountId = Convert.ToInt32(result.Value);
                    sql = @"UPDATE tAccount SET EmailVerified=@registerDate, Email=@email, EmailToVerify=@email, EmailBeforeVerify=@email, EmailVerifyStatus=0 where AccountId=@accountId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@AccountId", accountId),
                            new SqlParameter("@email", email),
                            new SqlParameter("@registerDate", Null(registerDate)));


                    sql = @"UPDATE tAccount SET TVD_Id=@TVD_Id, CanAccessIntensa=@CanAccessIntensa, HistoryStamp=ISNULL(@registerDate, HistoryStamp) WHERE AccountId=@AccountId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@AccountId", accountId),
                            new SqlParameter("@TVD_Id", accountTVDId),
                            new SqlParameter("@CanAccessIntensa", canAccessIntensa),
                            new SqlParameter("@registerDate", Null(registerDate)));

                    return accountId;
                } else {
                    throw new Exception("Account " + accountTVDId.ToString() + " uz v Eurona Existuje!!!");
                }
            }
            public static int SyncOrganization(CMS.Pump.MSSQLStorage mssqStorageDst, int accountId, int instanceId, string code, string email, string name, string ico, string dic, int parentId, bool platceDPH,
                    string street, string city, string psc, string state, string phone, string mobil,
                    string street_d, string city_d, string psc_d, string state_d,
                    string bankCode, string bankAccountNumber, int topManager, string fax, string icq, string skype, string workPhone, string personalCardId, string pf, DateTime? birthDay, string regionCode, decimal marze, int omezenyPristup, string statut,
                    bool angelTeamClen, bool angelTeamManager, int angelTeamManagerTyp) {

                string sql = "SELECT OrganizationId FROM tOrganization WHERE AccountId=@AccountId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@AccountId", accountId));
                bool orgExist = dt.Rows.Count != 0;
                //INSERT
                if (!orgExist) {
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;

                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pOrganizationCreate",
                                    new SqlParameter("@HistoryAccount", 1),
                                    new SqlParameter("@InstanceId", instanceId),
                                    new SqlParameter("@AccountId", accountId),
                                    new SqlParameter("@Id1", ico),
                                    new SqlParameter("@Id2", dic),
                                    new SqlParameter("@Id3", string.Empty),
                                    new SqlParameter("@Name", name),
                                    new SqlParameter("@Notes", string.Empty),
                                    new SqlParameter("@Web", string.Empty),
                                    new SqlParameter("@ContactEmail", email),
                                    new SqlParameter("@ContactPhone", phone),
                                    new SqlParameter("@ContactMobile", mobil),
                                    new SqlParameter("@ParentId", parentId),
                                    new SqlParameter("@VATPayment", platceDPH),
                                    new SqlParameter("@Code", code),
                                    new SqlParameter("@TopManager", topManager),
                                    new SqlParameter("@FAX", Null(fax)),
                                    new SqlParameter("@Skype", Null(skype)),
                                    new SqlParameter("@ICQ", Null(icq)),
                                    new SqlParameter("@ContactBirthDay", Null(birthDay)),
                                    new SqlParameter("@ContactCardId", Null(personalCardId)),
                                    new SqlParameter("@ContactWorkPhone", Null(workPhone)),
                                    new SqlParameter("@PF", Null(pf)),
                                    new SqlParameter("@RegionCode", Null(regionCode)),
                                    new SqlParameter("@UserMargin", Null(marze)),
                                    new SqlParameter("@Statut", Null(statut)),
                                    result
                            );

                    int orgId = Convert.ToInt32(result.Value);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Register, street, city, psc, state);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Delivery, street_d, city_d, psc_d, state_d);
                    SyncBank(mssqStorageDst, orgId, bankCode, bankAccountNumber);

                    return orgId;
                } else {
                    int orgId = Convert.ToInt32(dt.Rows[0][0]);
                    sql = @"UPDATE tOrganization SET Code=@Code, Id1=@Id1, Id2=@Id2, Name=@Name, ContactEmail=@ContactEmail, ContactPhone=@ContactPhone, ContactMobile=@ContactMobile , ParentId=@ParentId, VATPayment=@VATPayment, 
					TopManager=@TopManager, UserMargin=@UserMargin, RestrictedAccess=@RestrictedAccess, Statut=@Statut, RegionCode=@RegionCode,
					Angel_team_clen=@Angel_team_clen, Angel_team_manager=@Angel_team_manager, Angel_team_manager_typ=@Angel_team_manager_typ
					WHERE OrganizationId=@OrganizationId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@Id1", ico),
                            new SqlParameter("@Id2", dic),
                            new SqlParameter("@Name", name),
                            new SqlParameter("@ContactEmail", email),
                            new SqlParameter("@ContactPhone", phone),
                            new SqlParameter("@ContactMobile", mobil),
                            new SqlParameter("@ParentId", parentId),
                            new SqlParameter("@VATPayment", platceDPH),
                            new SqlParameter("@Code", code),
                            new SqlParameter("@OrganizationId", orgId),
                            new SqlParameter("@TopManager", topManager),
                            new SqlParameter("@UserMargin", Null(marze)),
                            new SqlParameter("@RestrictedAccess", Null(omezenyPristup)),
                            new SqlParameter("@Statut", Null(statut)),
                            new SqlParameter("@RegionCode", Null(regionCode)),
                            new SqlParameter("@Angel_team_clen", Null(angelTeamClen)),
                            new SqlParameter("@Angel_team_manager", Null(angelTeamManager)),
                            new SqlParameter("@Angel_team_manager_typ", Null(angelTeamManagerTyp))
                            );

                    SyncAddress(mssqStorageDst, orgId, AddressType.Register, street, city, psc, state);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Delivery, street_d, city_d, psc_d, state_d);
                    SyncBank(mssqStorageDst, orgId, bankCode, bankAccountNumber);

                    return orgId;
                }
            }
            private static void SyncAddress(CMS.Pump.MSSQLStorage mssqStorageDst, int orgId, AddressType addressType, string street, string city, string psc, string state) {
                string sql = "SELECT RegisteredAddress, CorrespondenceAddress FROM tOrganization WHERE OrganizationId=@OrganizationId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@OrganizationId", orgId));

                int addressHomeId = Convert.ToInt32(dt.Rows[0]["RegisteredAddress"]);
                int addressTempId = Convert.ToInt32(dt.Rows[0]["CorrespondenceAddress"]);

                sql = @"UPDATE tAddress SET City=@City, Street=@Street, Zip=@Zip, State=@State
				WHERE AddressId=@AddressId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@City", city),
                        new SqlParameter("@Street", street),
                        new SqlParameter("@Zip", psc),
                        new SqlParameter("@State", state),
                        new SqlParameter("@AddressId", addressType == AddressType.Register ? addressHomeId : addressTempId));
            }

            private static void SyncBank(CMS.Pump.MSSQLStorage mssqStorageDst, int orgId, string kod, string cisloUctu) {
                string sql = "SELECT BankContact FROM tOrganization WHERE OrganizationId=@OrganizationId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@OrganizationId", orgId));

                int bankContact = Convert.ToInt32(dt.Rows[0]["BankContact"]);

                sql = @"UPDATE tBankContact SET BankCode=@BankCode, AccountNumber=@AccountNumber
				WHERE BankContactId=@BankContactId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@BankCode", kod),
                        new SqlParameter("@AccountNumber", cisloUctu),
                        new SqlParameter("@BankContactId", bankContact));
            }
        }

        public static class BonusoveKredityUzivatele {
            /// <summary>
            /// Vráti ID accountu z CMS podla jeho TVD_ID
            /// </summary>
            public static DataTable GetBonusoveKredityUzivatele(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId) {
                string sql = @"SELECT YYYYMM = (YEAR(bk.Datum)*100+MONTH(bk.Datum)), Kredit=SUM(bk.Hodnota), bk.TVD_Id
								FROM vBonusoveKredityUzivatele bk
								INNER JOIN tAccount a ON a.AccountId = bk.AccountId
								WHERE a.InstanceId = @instanceId AND bk.TVD_Id IS NOT NULL
								GROUP BY bk.TVD_Id, (YEAR(bk.Datum)*100+MONTH(bk.Datum))";
                DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql, new SqlParameter("@instanceId", instanceId));
                return dt;
            }

            public static int GetBonusovyKredit(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId, int bkTyp) {
                string sql = @"SELECT TOP 1 [BonusovyKreditId] FROM [tBonusovyKredit] WHERE InstanceId=@instanceId AND [Typ]=@bkTyp";
                using (SqlConnection connectionSrc = mssqStorageSrc.Connect()) {
                    DataTable dt = mssqStorageSrc.Query(connectionSrc, sql, new SqlParameter("@instanceId", instanceId), new SqlParameter("@bkTyp", bkTyp));
                    if (dt == null || dt.Rows.Count == 0) return 0;
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
            }

            public static void InsertBonusovyKredityUzivatele(CMS.Pump.MSSQLStorage mssqStorageSrc, int bonusovyKreditId, int accountId, DateTime platnostOd, DateTime platnostDo, int narok) {
                string sql = @"
				IF EXISTS(SELECT AccountId, BonusovyKreditId  FROM tBonusovyKreditUzivatele WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo ) BEGIN
					UPDATE tBonusovyKreditUzivatele SET Hodnota=@narok WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo AND [Hodnota] <> @narok
				END
				ELSE
				BEGIN
					INSERT INTO tBonusovyKreditUzivatele ( [AccountId] ,[BonusovyKreditId] ,[Datum] ,[PlatnostOd] ,[PLatnostDo] ,[Kod] ,[Hodnota] ,[Poznamka]) 
                    VALUES 
                    (@accountId, @bonusovyKreditId, GETDATE(), @platnostOd, @platnostDo, '', @narok, '')
				END";
                using (SqlConnection connectionDst = mssqStorageSrc.Connect()) {
                    mssqStorageSrc.Exec(connectionDst, sql,
                    new SqlParameter("@accountId", accountId),
                    new SqlParameter("@bonusovyKreditId", bonusovyKreditId),
                    new SqlParameter("@platnostOd", platnostOd),
                    new SqlParameter("@platnostDo", platnostDo),
                    new SqlParameter("@narok", narok));
                }

                /*
                string sqlDelete = @"DELETE FROM tBonusovyKreditUzivatele WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo";
                mssqStorageSrc.Exec(mssqStorageSrc.Connection, sqlDelete);

                string sql = @"INSERT INTO tBonusovyKreditUzivatele ( [AccountId] ,[BonusovyKreditId] ,[Datum] ,[PlatnostOd] ,[PLatnostDo] ,[Kod] ,[Hodnota] ,[Poznamka]) 
                VALUES 
                (@accountId, @bonusovyKreditId, GETDATE(), @platnostOd, @platnostDo, '', @narok, '')";
                DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql,
                    new SqlParameter("@accountId", accountId),
                    new SqlParameter("@bonusovyKreditId", bonusovyKreditId),
                    new SqlParameter("@platnostOd", platnostOd),
                    new SqlParameter("@platnostDo", platnostDo),
                    new SqlParameter("@narok", narok));
                return dt;
                 * */
            }
        }

        #region Helpers methods
        public static string GetAliasName(string name) {
            name = name.ToLower().Trim();
            string alias = CMS.Utilities.StringUtilities.RemoveDiacritics(name);
            alias = alias.Replace("\n", "");
            alias = alias.Replace("\r", "");
            alias = alias.Replace("%", "");
            alias = alias.Replace(",", ""); alias = alias.Replace("?", "");
            alias = alias.Replace(";", ""); alias = alias.Replace("&", "-and-");
            alias = alias.Replace(" ", "-");
            alias = alias.Replace("----", "-");
            alias = alias.Replace("---", "-");
            alias = alias.Replace("--", "-");
            alias = alias.Replace("+", "plus");
            return alias;
        }
        private static object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        private static object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        private static object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        private static object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }
        #endregion
    }
}
