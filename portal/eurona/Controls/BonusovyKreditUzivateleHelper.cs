using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.DAL.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Eurona.Common.DAL.Entities;

namespace Eurona.Controls {
    public class BonusovyKreditUzivateleHelper {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="typ"></param>
        /// <param name="objectValue">Suma na objednavke alebo hodnotenie produktu.</param>
        /// <param name="kod">Email a id produktu ako identifikator pre typ "ProduktEmailPoslatPriteli", id produktu pre Typ "ProduktDetail" a "ProduktEmailFacebook"</param>
        public static void ZaevidujKredit(int AccountId, DAL.Entities.Classifiers.BonusovyKreditTyp typ, decimal? objectValue, string kod, string locale = null) {

            // !!!! POZOR !!!!
            // EVIDENCIA BK BOLA PRESUNUTA DO EUROSAPU 12.04.2017
            // !!!! POZOR !!!!

            /*
            kod = string.IsNullOrEmpty(kod) ? null : kod;
            BonusovyKreditUzivatele bku = null;
            Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = AccountId });

            //Na tieto typy je mozne bonusovy kredit pripocitat len raz za 24 hodin
            if (typ == DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailFacebook ||
                    typ == DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailPoslatPriteli ||
                    typ == DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktDetail) {
                bku = Storage<BonusovyKreditUzivatele>.ReadFirst(new BonusovyKreditUzivatele.ReadLast { AccountId = AccountId, Typ = (int)typ, Kod = kod });
                if (bku != null) {
                    DateTime datum = bku.Datum;
                    DateTime datumAdd24 = datum.AddHours(24);
                    if (datumAdd24 > DateTime.Now) {
                        LogBK(AccountId, (int)typ, "Za tento typ BK, jiz byl pripocten kredit behem 24hodin!. Dalsi je mozne pripocitat az za 24hodin!", "NOK");
                        return;
                    }
                }
            }

            //Za jeden produkt sa kredit pripocitava iba raz
            if (typ == DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni) {
                bku = Storage<BonusovyKreditUzivatele>.ReadFirst(new BonusovyKreditUzivatele.ReadLast { AccountId = AccountId, Typ = (int)typ, Kod = kod });
                if (bku != null) {
                    LogBK(AccountId, (int)typ, "Za hodnoceni produktu Kod:" + kod + " jiz byl predit pripocitan. Kredi za hodnoceni produktu je mozne pripocist pouze jednou!", "NOK");
                    return;
                }
            }

            //Nasledujuci mesiac
            DateTime now = DateTime.Now.AddMonths(1);
            DateTime datumOd = new DateTime(now.Year, now.Month, 1);
            DateTime datumDo = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            //Get ID bonusoveho kreditu
            BonusovyKredit bk = BonusovyKreditUzivateleHelper.GetBonusovyKredit(typ, objectValue, locale);
            if (bk == null) {
                string message = string.Format("Bonusovy kredit NEBYL zaevidovan pro typ : {0}, poradce: {1}, locale: {2}, hodnotu: {3} z duvodu nedosazene minimalni ceny na objednavce pro danou menu!", typ.ToString(), account.Login, locale, objectValue);
                CMS.EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
                LogBK(AccountId, (int)typ, message, "NOK");
                return;
            }

            if (bk.Aktivni == false) {
                string message = string.Format("Bonusovy kredit NEBYL zaevidovan pro typ : {0}. Tento typ NENI Aktivni!", typ.ToString());
                CMS.EvenLog.WritoToEventLog(message, System.Diagnostics.EventLogEntryType.Error);
                LogBK(AccountId, (int)typ, message, "NOK");
                return;
            }

            //Ak pouzivatel dosiahol maximalny mozny kredit, dalsi sa mu nezapisuje
            decimal maximalniPocetBKzaMesic = GetMaximalniPocetBKZaMesic();
            decimal kreditcelkemZaMesic = GetPlatnyKreditCelkem(account, now.Year, now.Month);
            string infoMessage = string.Empty;
            if (kreditcelkemZaMesic >= maximalniPocetBKzaMesic) {
                infoMessage = string.Format("Bonusovy kredit NEBYL zaevidovan z duvodu dosazeni mesicniho maxima pro : {0}, poradce: {1}, locale: {2}, hodnotu: {3}!", typ.ToString(), account.Login, locale, objectValue);
                LogBK(AccountId, (int)typ, infoMessage, "NOK");
                return;
            }

            if (kreditcelkemZaMesic + bk.Kredit > maximalniPocetBKzaMesic)
                bk.Kredit = maximalniPocetBKzaMesic - kreditcelkemZaMesic;

            bku = new BonusovyKreditUzivatele();
            bku.AccountId = AccountId;
            bku.Datum = DateTime.Now;
            bku.Kod = kod;
            bku.Hodnota = bk.Kredit;
            bku.PlatnostOd = datumOd;
            bku.PlatnostDo = datumDo;
            bku.BonusovyKreditId = bk.Id;
            Storage<BonusovyKreditUzivatele>.Create(bku);

            infoMessage = string.Format("Bonusovy kredit byl zaevidovan ve vysi {0}, pro : {1}, poradce: {2}, locale: {3}, hodnotu: {4}!",
                bku.Hodnota, typ.ToString(), account.Login, locale, objectValue);
            CMS.EvenLog.WritoToEventLog(infoMessage, System.Diagnostics.EventLogEntryType.Information);
            LogBK(AccountId, (int)typ, infoMessage, "OK");
             * */
        }

        public static decimal GetMaximalniPocetBKZaMesic() {
            BonusovyKredit bk = Storage<BonusovyKredit>.ReadFirst(new BonusovyKredit.ReadByTyp { Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.MaximalniPocetBKzaMesic });
            if (bk == null) return 99999999;
            else return bk.Kredit;
        }

        public static DateTime GetCurrentObdobiFromTVD() {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                sql = @"SELECT TOP 1 RRRRMM FROM provize_aktualni";
                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql);
                if (dt == null) return DateTime.Now;
                if (dt.Rows.Count == 0) return DateTime.Now;
                object objectValue = dt.Rows[0]["RRRRMM"];
                if (objectValue == DBNull.Value) return DateTime.Now; ;

                int year = Convert.ToInt32(objectValue.ToString().Substring(0, 4));
                int month = Convert.ToInt32(objectValue.ToString().Substring(4, 2));
                return new DateTime(year, month, 1);
            }
        }

        /// <summary>
        /// Celkovy pocet kreditov nazbieranych tento mesiac
        /// </summary>
        /// <returns></returns>
        public static decimal GetBonusoveKredityUzivateleNazbiraneTentoMesicCelkem(int accountId) {
            int instanceId = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);
            int bkTyp = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.Eurosap;

            DateTime? nowTVD = GetCurrentObdobiFromTVD();
            if (!nowTVD.HasValue) {
                nowTVD = DateTime.Now;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                string sql = @"SELECT Kredit=ISNULL(SUM(Hodnota),0) FROM vBonusoveKredityUzivatele
								WHERE AccountId = @accountId AND InstanceId=@InstanceId AND Typ=@bkTyp AND
								(YEAR(PlatnostOd)=@rok AND MONTH(PlatnostOd)=@mesic)";
                DataTable dt = storage.Query(connection, sql,
                        new SqlParameter("@mesic", nowTVD.Value.Month),
                        new SqlParameter("@rok", nowTVD.Value.Year),
                        new SqlParameter("@accountId", accountId),
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@bkTyp", bkTyp));

                if (dt.Rows.Count == 0) return 0;
                return Convert.ToDecimal(dt.Rows[0]["Kredit"]);
            }
        }

        public static decimal GetPlatnyKreditCelkem(Account account, int rok, int mesic) {
            if (!account.TVD_Id.HasValue) return 0;

            int instanceId = 1;
            Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);
            int bkTyp = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.Eurosap;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            int platnychCelkem = 0;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                string sql = @"SELECT Kredit=ISNULL(SUM(Hodnota),0) FROM vBonusoveKredityUzivatele
															WHERE AccountId = @accountId AND InstanceId=@InstanceId AND Typ=@bkTyp AND
																((YEAR(PlatnostOd)=@rok AND MONTH(PlatnostOd)=@mesic) OR 
																(YEAR(PlatnostDo)=@rok AND MONTH(PlatnostDo)=@mesic))";
                DataTable dt = storage.Query(connection, sql,
                        new SqlParameter("@mesic", mesic),
                        new SqlParameter("@rok", rok),
                        new SqlParameter("@accountId", account.Id),
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@bkTyp", bkTyp));

                if (dt.Rows.Count != 0) {
                    platnychCelkem = Convert.ToInt32(dt.Rows[0]["Kredit"]);
                }
            }

            return platnychCelkem;
        }
        public static decimal GetCerpaniKredituCelkem(Account account, int rok, int mesic) {
            if (!account.TVD_Id.HasValue) return 0;

            int rrrrmm = rok * 100 + mesic;
            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            int kreditCerpanoCelkem = 0;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            try {
                using (SqlConnection connection = tvdStorage.Connect()) {
                    string sql = @"SELECT Kredit=ISNULL(SUM(Cerpano), 0) FROM www_bonuskredit_cerpani_eurosap
                                    WHERE Id_odberatele = @idOdberatele AND RRRRMM=@rrrrmm";
                    DataTable dt = tvdStorage.Query(connection, sql,
                            new SqlParameter("@rrrrmm", rrrrmm),
                            new SqlParameter("@idOdberatele", account.TVD_Id.Value));

                    if (dt.Rows.Count != 0) {
                        kreditCerpanoCelkem = Convert.ToInt32(dt.Rows[0]["Kredit"]);
                    }
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(string.Format("BonusovyKreditUzivateleHelper::v::TVD Exception:{0}", ex.Message),
                    System.Diagnostics.EventLogEntryType.Error);
            }

            return kreditCerpanoCelkem;
        }

        /// <summary>
        /// Vrati celkovy pocet pnusovych kreditov, ktore moze dany pouzivatel za dane obdobie cerpat.
        /// </summary>
        /// 
        public static decimal GetKreditNarokCelkem(int accountId, int tvd_id, int rok, int mesic) {
            int rrrrmm = rok * 100 + mesic;
            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            int instanceId = 1;
            Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);

            int bkTyp = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.Eurosap;

            int kreditNarokCelkem = 0;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                string sql = @"SELECT Kredit=ISNULL(SUM(Hodnota),0) FROM vBonusoveKredityUzivatele
				WHERE AccountId = @accountId AND InstanceId=@InstanceId AND  Typ=@bkTyp AND
					((YEAR(PlatnostOd)=@rok AND MONTH(PlatnostOd)=@mesic) OR 
					(YEAR(PlatnostDo)=@rok AND MONTH(PlatnostDo)=@mesic))";
                DataTable dt = storage.Query(connection, sql,
                        new SqlParameter("@mesic", mesic),
                        new SqlParameter("@rok", rok),
                        new SqlParameter("@accountId", accountId),
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@bkTyp", bkTyp));

                if (dt.Rows.Count != 0) {
                    kreditNarokCelkem = Convert.ToInt32(dt.Rows[0]["Kredit"]);
                }
            }

            int kreditCerpanoCelkem = 0;
            DateTime date = new DateTime(rok, mesic, 1);
            date = date.AddMonths(-1);
            rrrrmm = date.Year * 100 + date.Month;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            try {
                using (SqlConnection connection = tvdStorage.Connect()) {
                    string sql = @"SELECT Kredit=ISNULL(SUM(Cerpano),0) FROM www_bonuskredit_cerpani
					WHERE Id_odberatele = @idOdberatele AND RRRRMM=@rrrrmm";
                    DataTable dt = tvdStorage.Query(connection, sql,
                            new SqlParameter("@rrrrmm", rrrrmm),
                            new SqlParameter("@idOdberatele", tvd_id));

                    if (dt.Rows.Count != 0) {
                        kreditCerpanoCelkem = Convert.ToInt32(dt.Rows[0]["Kredit"]);
                    }
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(string.Format("BonusovyKreditUzivateleHelper::GetKreditNarokCelkem::TVD Exception:{0}", ex.Message),
                    System.Diagnostics.EventLogEntryType.Error);
            }

            return kreditNarokCelkem - kreditCerpanoCelkem;
        }
        private static decimal GetKreditNarokCelkem(Account account, int rok, int mesic) {
            if (!account.TVD_Id.HasValue) return 0;
            return GetKreditNarokCelkem(account.Id, account.TVD_Id.Value, rok, mesic);
        }
  
        /*
        public static void UpdateTVDBonusovyKredit(int productId, decimal? kredit) {
            if (!kredit.HasValue) return;

            //UPDATE EURONADB
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                string sql = @"UPDATE tShpCenyProduktu SET CenaBK=@kredit WHERE ProductId=@productId";
                storage.Exec(connection, sql,
                        new SqlParameter("@kredit", kredit),
                        new SqlParameter("@productId", productId));
            }

            //UPDATE TVD
            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"UPDATE Produkty_Ceny SET Cena_BK=@kredit WHERE C_Zbo=@productId";
                tvdStorage.Exec(connection, sql,
                        new SqlParameter("@kredit", kredit),
                        new SqlParameter("@productId", productId));
            }
        }*/

        public static void LogBK(int accountId, int? typ, string message, string status) {
            BonusovyKreditLog log = new BonusovyKreditLog();
            log.AccountId = accountId;
            log.BonusovyKreditTyp = typ;
            log.Message = message;
            log.Status = status;
            Storage<BonusovyKreditLog>.Create(log);
        }
    }
}