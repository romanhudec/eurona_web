using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.IO;
using Mothiva.Cron;
using System.Data.SqlClient;
using Mothiva.Cron.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Cron.Eurona {
    public class CronCommand : Mothiva.Cron.CronCommandBase {
        public static object SyncObject_Product = new Object();
        public static object SyncObject_Account = new Object();
        private static string CommandLogSeparator = "========================================================";
        private static System.Diagnostics.TraceSwitch TraceGeneral = new System.Diagnostics.TraceSwitch("General", "General event log content level switch");

        public CronCommand(string entryName)
            : base(entryName) {
        }

        #region ICronCommand Members

        public override bool Exec(string command, Dictionary<string, string> parameters) {
            switch (command.ToUpper()) {
                case "REMOVE_UNCOMPLETED_REGISTRATION_ACCOUNTS":
                    RemoveUncompletedRegistrationAccounts(parameters);
                    break;

                //EURONA
                case "EURONA_EMPTY_CARTS":
                    EuronaEmptyCarts(parameters);
                    break;

                case "EURONA_REKLAMNI_ZASILKY_SYNC":
                    SyncReklamniZasilky(parameters);
                    break;
                case "EURONA_CALCULATE_TVD_USER_TREE":
                    EuronaCalculateTVDUserTree(parameters);
                    break;
                case "EURONA_CALCULATE_TVD_USER_Z_TREE":
                    EuronaCalculateTVDUserZTree(parameters);
                    break;
                case "EURONA_REMOVE_EUROSAP_ACCOUNTS":
                    RemoveEuronaAccountsByEurosap(parameters);
                    break;
                case "EURONA_ACCOUNT_SYNC":
                    SyncEuronaAccount(parameters);
                    break;
                case "EURONA_ORDER_SYNC":
                    SyncEuronaOrder(parameters);
                    break;
                case "EURONA_IMPORT_PRODUCT_STOCK":
                    ImportEuronaProductStock(parameters);
                    break;
                case "EURONA_IMPORT_PRODUCT":
                    ImportEuronaProduct(parameters);
                    break;
                case "EURONA_BONUSKREDIT_NAROK_SYNC":
                    SyncEuronaBonusoveKredityUzivatele(parameters);
                    break;
                //CERNY FRO LIFE
                case "CERNYFORLIFE_CALCULATE_TVD_USER_TREE":
                    CernyForLifeCalculateTVDUserTree(parameters);
                    break;
                case "CERNYFORLIFE_REMOVE_EUROSAP_ACCOUNTS":
                    RemoveCernyForLifeAccountsByEurosap(parameters);
                    break;
                case "CERNYFORLIFE_ACCOUNT_SYNC":
                    SyncCernyForLifeAccount(parameters);
                    break;
                case "CERNYFORLIFE_ORDER_SYNC":
                    SyncCernyForLifeOrder(parameters);
                    break;
                case "CERNYFORLIFE_IMPORT_PRODUCT_STOCK":
                    ImportCernyForLifeProductStock(parameters);
                    break;
                case "CERNYFORLIFE_IMPORT_PRODUCT":
                    ImportCernyForLifeProduct(parameters);
                    break;
                case "CERNYFORLIFE_BONUSKREDIT_NAROK_SYNC":
                    SyncCernyForLifeBonusoveKredityUzivatele(parameters);
                    break;

                default:
                    throw new Exception(string.Format("Invalid command name : {0}", command));
            }

            return true;
        }

        #endregion
        /// <summary>
        /// Odosle error report dealerovi.
        /// </summary>
        private void SendEmail(string to, string subject, string messsage) {
            try {
                if (!string.IsNullOrEmpty(to)) {
                    Mothiva.Cron.Email email = new Mothiva.Cron.Email();
                    email.To = to;
                    email.Subject = subject;
                    email.Message = messsage;
                    email.Send();
                }
            } catch (Exception ex) {
                string message = string.Format("Can not send error report to email {0} : {1}", to, ex.Message);
                if (TraceGeneral.TraceError) base.WriteLogLine(message, ex);
            }
        }
        /// <summary>
        /// Odstrani vsetky nedokoncene registraciem kde ucet je minimalne den stary
        /// </summary>
        internal void RemoveUncompletedRegistrationAccounts(Dictionary<string, string> parameters) {
            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorage = new CMS.Pump.MSSQLStorage(parameters["connectionString"]);
            SqlConnection connection = mssqStorage.Connect();

            string sql = @"
						SELECT a.AccountId from tAccount a WITH (NOLOCK)
						LEFT JOIN vPersons p WITH (NOLOCK) ON p.AccountId = a.AccountId
						LEFT JOIN vOrganizations o WITH (NOLOCK) ON o.AccountId = a.AccountId
						WHERE a.AccountId != 1/*SYSTEM*/ AND a.AccountId != 2/*EURONA*/ AND a.HistoryId IS NULL AND
								p.AccountId IS NULL AND o.AccountId IS NULL AND
								((a.Roles LIKE '%'+'Advisor' + '%' OR a.Roles IS NULL) OR a.Roles='') AND
								a.HistoryStamp < ( DATEADD( DAY, -1, GETDATE() ) )";

            DataTable dt = mssqStorage.Query(connection, sql);

            foreach (DataRow row in dt.Rows) {
                int id = Convert.ToInt32(row["AccountId"]);

                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", 1);
                SqlParameter accountId = new SqlParameter("@AccountId", id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                mssqStorage.ExecProc(connection, "pAccountDelete", result, historyAccount, accountId);

                if (TraceGeneral.TraceInfo) base.WriteLogLine(
                      string.Format("Uncopleted registration account id={0} was successfully deleted!", id),
                      TraceCategory.Information);
            }
        }

        #region Eurona command

        /// <summary>
        /// Synchronizacia poradcou TVD->Eurona
        /// </summary>
        internal void EuronaEmptyCarts(Dictionary<string, string> parameters) {
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorage = new CMS.Pump.MSSQLStorage(parameters["connectionString"]);
            SqlConnection connection = mssqStorage.Connect();

            string sql = @"SELECT * FROM tSettings WHERE Code='ESHOP_VYSYPANIVSECHKOSIKU' AND LOWER(Value) LIKE 'true%'";
            DataTable dt = mssqStorage.Query(connection, sql);
            foreach (DataRow row in dt.Rows) {
                string value = row["Value"].ToString();
                string[] data = value.Split(';');
                if (data.Length != 2) continue;
                string casString = data[1];
                string[] timeSequence = casString.Split(':');
                if (timeSequence.Length != 2) continue;

                int hour = Convert.ToInt32(timeSequence[0]);
                int minute = Convert.ToInt32(timeSequence[1]);

                if (hour == DateTime.Now.Hour && minute == DateTime.Now.Minute) {
                    string sqlEmptyCartsProduct = @"delete from tShpCartProduct where CartId NOT IN (SELECT CartId FROM tShpOrder)";
                    mssqStorage.Query(connection, sqlEmptyCartsProduct);
                    string sqlEmptyCarts = @"delete from tShpCart where CartId NOT IN (SELECT CartId FROM tShpOrder)";
                    mssqStorage.Query(connection, sqlEmptyCarts);

                    if (TraceGeneral.TraceInfo) base.WriteLogLine("Empty carts was successfully executed!", TraceCategory.Information);
                }

            }

        }

        internal void SyncReklamniZasilky(Dictionary<string, string> parameters) {
            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorageEurona = new CMS.Pump.MSSQLStorage(parameters["connectionStringEurona"]);
            CMS.Pump.MSSQLStorage mssqStorageTVD = new CMS.Pump.MSSQLStorage(parameters["connectionStringTVD"]);

            using (SqlConnection connectionEurona = mssqStorageEurona.Connect()) {
                using (SqlConnection connectionTVD = mssqStorageTVD.Connect()) {
                    //Sync Reklamni zasilky TVD->Eurona
                    DataTable tvdReklamniZasilky = mssqStorageTVD.Query(mssqStorageTVD.Connection, "SELECT * FROM reklamni_zasilky");
                    foreach (DataRow row in tvdReklamniZasilky.Rows) {

                        string sql = @"
                        IF EXISTS (SELECT 1 FROM tReklamniZasilky WHERE Id_zasilky=@Id_zasilky)
                        BEGIN
                          UPDATE tReklamniZasilky SET Popis=@Popis, Default_souhlas=@Default_souhlas WHERE Id_zasilky=@Id_zasilky
                        END
                        ELSE
                        BEGIN
                          INSERT INTO tReklamniZasilky (Id_zasilky, Popis, Default_souhlas) VALUES (@Id_zasilky, @Popis, @Default_souhlas)
                        END
                        ";
                        mssqStorageEurona.Exec(mssqStorageEurona.Connection, sql,
                            new SqlParameter("@Id_zasilky", row["Id_zasilky"]), new SqlParameter("@Popis", row["Popis"]), new SqlParameter("@Default_souhlas", row["Default_souhlas"]));

                    }
                    /*
                    //Sync Reklamni zasilky souhlas TVD->Eurona
                    DataTable tvdReklamniZasilkySouhlas = mssqStorageTVD.Query(mssqStorageTVD.Connection, "SELECT * FROM reklamni_zasilky_souhlas");
                    foreach (DataRow row in tvdReklamniZasilkySouhlas.Rows) {

                        string sql = @"IF EXISTS (SELECT 1 FROM tReklamniZasilkySouhlas WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele)
                        BEGIN
                          UPDATE tReklamniZasilkySouhlas SET Souhlas=@Souhlas, Datum_zmeny=@Datum_zmeny WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele
                        END
                        ELSE
                        BEGIN
                          INSERT INTO tReklamniZasilkySouhlas (Id_zasilky, Id_odberatele, Souhlas, Datum_zmeny) VALUES (@Id_zasilky, @Id_odberatele, @Souhlas, @Datum_zmeny)
                        END
                        ";
                        mssqStorageEurona.Exec(mssqStorageEurona.Connection, sql,
                            new SqlParameter("@Id_zasilky", row["Id_zasilky"]),
                            new SqlParameter("@Id_odberatele", row["Id_odberatele"]),
                            new SqlParameter("@Souhlas", row["Souhlas"]),
                            new SqlParameter("@Datum_zmeny", row["Datum_zmeny"]));

                    }
                    */
                    //Sync Reklamni zasilky souhlas Eurona->TVD
                    DataTable tvdReklamniZasilkySouhlas = mssqStorageEurona.Query(mssqStorageEurona.Connection, @"
                    SELECT DISTINCT Id_odberatele = a.TVD_Id, rz.Id_zasilky, rz.Souhlas, rz.Datum_zmeny FROM tReklamniZasilkySouhlas rz 
                    INNER JOIN tAccount a ON a.TVD_Id=rz.Id_odberatele
                    ORDER BY a.TVD_Id, rz.Id_zasilky");
                    int success = 0;
                    int failed = 0;
                    foreach (DataRow row in tvdReklamniZasilkySouhlas.Rows) {
                        int Id_zasilky = Convert.ToInt32(row["Id_zasilky"]);
                        int Id_odberatele = Convert.ToInt32(row["Id_odberatele"]);
                        //if (TraceGeneral.TraceInfo)
                        //    base.WriteLogLine(string.Format("Id_zasilky:{0}, Id_odberatele:{1}", Id_zasilky, Id_odberatele), TraceCategory.Information);

                        string sql = @"IF EXISTS (SELECT 1 FROM reklamni_zasilky_souhlas WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele)
                        BEGIN
                          UPDATE reklamni_zasilky_souhlas SET Souhlas=@Souhlas, Datum_zmeny=@Datum_zmeny WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele
                        END
                        ELSE
                        BEGIN
                          INSERT INTO reklamni_zasilky_souhlas (Id_zasilky, Id_odberatele, Souhlas, Datum_zmeny) VALUES (@Id_zasilky, @Id_odberatele, @Souhlas, @Datum_zmeny)
                        END
                        ";
                        try {
                            mssqStorageTVD.Exec(mssqStorageTVD.Connection, sql,
                                new SqlParameter("@Id_zasilky", Id_zasilky),
                                new SqlParameter("@Id_odberatele", Id_odberatele),
                                new SqlParameter("@Souhlas", row["Souhlas"]),
                                new SqlParameter("@Datum_zmeny", row["Datum_zmeny"]));
                            success++;
                        } catch (Exception ex) {
                            base.WriteLogLine(string.Format("Id_zasilky:{0}, Id_odberatele:{1}, Exception:{2}", Id_zasilky, Id_odberatele, ex.Message), TraceCategory.Error);
                            failed++;
                        }
                    }
                    base.WriteLogLine(string.Format("Success:{0}, Failed:{1}", success, failed), TraceCategory.Information);
                }
            }
        }

        /// <summary>
        /// Vypocita v TVD Strom pouzivatelov
        /// </summary>
        internal void EuronaCalculateTVDUserTree(Dictionary<string, string> parameters) {
            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorage = new CMS.Pump.MSSQLStorage(parameters["connectionString"]);
            SqlConnection connection = mssqStorage.Connect();

//            string sql = @"
//				DELETE FROM www_odberatele_strom
//				INSERT INTO www_odberatele_strom
//				SELECT * FROM dbo.fOdberateleStrom(1)";

            string sql = @"BEGIN TRAN T1
                CREATE TABLE #tmp_odberatele_strom(
	                [Id] [int] NOT NULL,
	                [Id_Odberatele] [int] NOT NULL,
	                [Level] [int] NOT NULL,
	                [LineageId] [nvarchar](2000) NOT NULL,
                )
                INSERT INTO #tmp_odberatele_strom
                SELECT * FROM dbo.fOdberateleStrom(1)

                DELETE FROM www_odberatele_strom
                INSERT INTO www_odberatele_strom
                SELECT * FROM #tmp_odberatele_strom

                COMMIT TRAN T1";

            int count = mssqStorage.Exec(connection, sql);
            if (TraceGeneral.TraceInfo)
                base.WriteLogLine(string.Format("TVD Users tree recalculated. Count = {0}", count), TraceCategory.Information);

        }

        /// <summary>
        /// Vypocita v TVD Strom pouzivatelov
        /// </summary>
        internal void EuronaCalculateTVDUserZTree(Dictionary<string, string> parameters) {
            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorage = new CMS.Pump.MSSQLStorage(parameters["connectionString"]);
            SqlConnection connection = mssqStorage.Connect();

            //Deleted tree calculation
            string sql = @"
				DELETE FROM www_odberatele_stromZ
				INSERT INTO www_odberatele_stromZ
				SELECT * FROM dbo.fOdberateleStromZ(1)";

            int count = mssqStorage.Exec(connection, sql);
            if (TraceGeneral.TraceInfo)
                base.WriteLogLine(string.Format("TVD Users tree Z recalculated. Count = {0}", count), TraceCategory.Information);
        }
        /// <summary>
        /// Vymaze registracie podla Eurosapu
        /// </summary>
        internal void RemoveEuronaAccountsByEurosap(Dictionary<string, string> parameters) {
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);
            mssqStorageSrc.Connect();
            mssqStorageDst.Connect();

            string sql = @"SELECT Id_odberatele ,Kod_odberatele ,Datum_vymazu ,Stav
						FROM odberatele_vymaz WITH (NOLOCK) WHERE Stav=1";

            DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql);
            foreach (DataRow row in dt.Rows) {
                int idOdberatele = Convert.ToInt32(row["Id_odberatele"]);
                int id = Eurona.Import.EuronaDAL.Account.GetAccountId(mssqStorageDst, idOdberatele);

                //Ak je ID == 0 pouzivatel nie je platny v ESHOP
                if (id != 0) {
                    //Delete account in EURONA
                    SqlParameter historyAccount = new SqlParameter("@HistoryAccount", 1);
                    SqlParameter accountId = new SqlParameter("@AccountId", id);
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;
                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pAccountDelete", result, historyAccount, accountId);

                    if (TraceGeneral.TraceInfo) base.WriteLogLine(
                          string.Format("Account id={0} was successfully deleted!", id),
                          TraceCategory.Information);
                }
                //Update TVD
                sql = @"UPDATE odberatele_vymaz SET Stav=2 WHERE Id_odberatele=@Id_odberatele";
                mssqStorageSrc.Exec(mssqStorageSrc.Connection, sql, new SqlParameter("Id_odberatele", idOdberatele));
            }
        }
        /// <summary>
        /// Synchronizacia poradcou TVD->Eurona
        /// </summary>
        internal void SyncEuronaAccount(Dictionary<string, string> parameters) {
            int instanceId = 1;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona accounts"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.EuronaAccoutsSynchronize eas = new Import.EuronaAccoutsSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eas.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eas.Synchronize();
            }

        }
        /// <summary>
        /// Synchronizacia objednavok Eurona <--> TVD
        /// </summary>
        internal void SyncEuronaOrder(Dictionary<string, string> parameters) {
            int instanceId = 1;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona orders"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.EuronaOrdersSynchronize eis = new Import.EuronaOrdersSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eis.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eis.Synchronize();
            }

        }
        /// <summary>
        /// Import skladu produktov z TVD
        /// </summary>
        internal void ImportEuronaProductStock(Dictionary<string, string> parameters) {
            int instanceId = 1;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Synchronizacia Produktov
            DataTable dt = Cron.Eurona.Import.EuronaTVDDAL.GetTVDProducts(mssqStorageSrc, instanceId);
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);
                int dispozice_HR = (int)Convert.ToDecimal(row["Dispozice_HR"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona product stock... {0}", productId), TraceCategory.Information);
                #endregion

                int itemsTotal = 0;
                int itemsImported = 0;
                int itemsError = 0;
                string errors = string.Empty;

                Import.EuronaProductStockSynchronize tcs = new Import.EuronaProductStockSynchronize(instanceId, productId, dispozice_HR, mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion
        }
        /// <summary>
        /// Import produktov z TVD
        /// </summary>
        internal void ImportEuronaProduct(Dictionary<string, string> parameters) {
            int instanceId = 1;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            string srcTVDImagePath = parameters["srcTVDImagePath"].ToString();

            string dstProductImagePath = parameters["dstProductImagePath"].ToString();
            string dstVlastnostiImagePath = parameters["dstVlastnostiImagePath"].ToString();
            string dstPiktogramyImagePath = parameters["dstPiktogramyImagePath"].ToString();
            string dstParfumacieImagePath = parameters["dstParfumacieImagePath"].ToString();
            string dstSpecialniUcinkyImagePath = parameters["dstSpecialniUcinkyImagePath"].ToString();
            string dstZadniEtiketyImagePath = parameters["dstZadniEtiketyImagePath"].ToString();

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Synchronizacia obrazkou ciselnikou
            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona images for classifiers... [Parfemace, SpecialniUcinky]"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.EuronaClsImagesSynchronize eis = new Import.EuronaClsImagesSynchronize(srcTVDImagePath, dstParfumacieImagePath, dstSpecialniUcinkyImagePath, mssqStorageSrc);
            eis.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Product) {
                eis.Synchronize();
            }
            #endregion

            #region Synchronizacia kategorii
            DataTable dtCategories = Cron.Eurona.Import.EuronaTVDDAL.GetTVDCategories(mssqStorageSrc);
            foreach (DataRow drCategory in dtCategories.Rows) {
                int categoryId = Convert.ToInt32(drCategory["Kategorie_Id"]);
                int? parentId = Convert.ToInt32(drCategory["Kategorie_Parent"]);
                int shop = Convert.ToInt32(drCategory["Shop"]);
                if (!parentId.HasValue || parentId.Value == 0) parentId = null;

                //Kategorie eurony
                if (shop != 0) continue;

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona categories... {0}", categoryId), TraceCategory.Information);
                #endregion

                itemsTotal = 0;
                itemsImported = 0;
                itemsError = 0;
                errors = string.Empty;

                Import.EuronaCategorySynchronize tcs = new Import.EuronaCategorySynchronize(instanceId, categoryId, parentId, mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion

            #region Synchronizacia Produktov
            DataTable dt = Cron.Eurona.Import.EuronaTVDDAL.GetTVDProducts(mssqStorageSrc, instanceId);
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona product... {0}", productId), TraceCategory.Information);
                #endregion

                itemsTotal = 0;
                itemsImported = 0;
                itemsError = 0;
                errors = string.Empty;

                Import.EuronaProductSynchronize tcs = new Import.EuronaProductSynchronize(
                        instanceId, productId,
                        srcTVDImagePath,
                        dstProductImagePath,
                        dstVlastnostiImagePath,
                        dstPiktogramyImagePath,
                        dstZadniEtiketyImagePath,
                        mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion

            #region Synchronizacia Alternativnych Produktov
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona alternate products for product... {0}", productId), TraceCategory.Information);
                #endregion

                #region Alternativni produkty
                //Odstranenie zvyraznenia produktu
                using (SqlConnection connection = mssqStorageDst.Connect()) {
                    DataTable dtProduktyAlternativni = Cron.Eurona.Import.EuronaTVDDAL.GetTVDProductAlternativni(mssqStorageSrc, productId);
                    Cron.Eurona.Import.EuronaDAL.Product.RemoveProductRelations(mssqStorageDst, instanceId, productId);
                    foreach (DataRow rowP in dtProduktyAlternativni.Rows) {
                        int altProductId = Convert.ToInt32(rowP["Alternativni_C_Zbo"]);
                        Cron.Eurona.Import.EuronaDAL.Product.SyncProductAlternativni(mssqStorageDst, instanceId, productId, altProductId);
                    }
                }
                #endregion
            }
            #endregion

        }

        /// <summary>
        /// Synchronizacia Bonusovych kreditov pouzivatela do TVD
        /// </summary>
        /// <param name="parameters"></param>
        internal void SyncEuronaBonusoveKredityUzivatele(Dictionary<string, string> parameters) {
            int instanceId = 1;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize Eurona bonusove kredity"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.EuronaBonusoveKredityUzivateleSynchronize eas = new Import.EuronaBonusoveKredityUzivateleSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eas.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eas.Synchronize();
            }
        }
        #endregion

        #region Cerny For Life command
        /// <summary>
        /// Vypocita v TVD Strom pouzivatelov
        /// </summary>
        internal void CernyForLifeCalculateTVDUserTree(Dictionary<string, string> parameters) {
            //Destination storage
            CMS.Pump.MSSQLStorage mssqStorage = new CMS.Pump.MSSQLStorage(parameters["connectionString"]);
            SqlConnection connection = mssqStorage.Connect();

            string sql = @"
						DELETE FROM cl_www_odberatele_strom
						INSERT INTO cl_www_odberatele_strom
						SELECT * FROM dbo.fClOdberateleStrom(1)";

            int count = mssqStorage.Exec(connection, sql);
            if (TraceGeneral.TraceInfo)
                base.WriteLogLine(string.Format("TVD Users tree recalculated. Count = {0}", count), TraceCategory.Information);
        }
        /// <summary>
        /// Vymaze registracie podla Eurosapu
        /// </summary>
        internal void RemoveCernyForLifeAccountsByEurosap(Dictionary<string, string> parameters) {
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);
            mssqStorageSrc.Connect();
            mssqStorageDst.Connect();

            string sql = @"SELECT Id_odberatele ,Kod_odberatele ,Datum_vymazu ,Stav
						FROM cl_odberatele_vymaz WITH (NOLOCK) WHERE Stav=1";

            DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql);
            foreach (DataRow row in dt.Rows) {
                int idOdberatele = Convert.ToInt32(row["Id_odberatele"]);
                int id = Eurona.Import.EuronaDAL.Account.GetAccountId(mssqStorageDst, idOdberatele);

                //Ak je ID == 0 pouzivatel nie je platny v ESHOP
                if (id != 0) {
                    //Delete account in EURONA
                    SqlParameter historyAccount = new SqlParameter("@HistoryAccount", 1);
                    SqlParameter accountId = new SqlParameter("@AccountId", id);
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;
                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pAccountDelete", result, historyAccount, accountId);

                    if (TraceGeneral.TraceInfo) base.WriteLogLine(
                          string.Format("Account id={0} was successfully deleted!", id),
                          TraceCategory.Information);
                }
                //Update TVD
                sql = @"UPDATE cl_odberatele_vymaz SET Stav=2 WHERE Id_odberatele=@Id_odberatele";
                mssqStorageSrc.Exec(mssqStorageSrc.Connection, sql, new SqlParameter("Id_odberatele", idOdberatele));
            }
        }
        /// <summary>
        /// Synchronizacia poradcou TVD->Eurona
        /// </summary>
        internal void SyncCernyForLifeAccount(Dictionary<string, string> parameters) {
            int instanceId = 3;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife accounts"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.CernyForLifeAccoutsSynchronize eas = new Import.CernyForLifeAccoutsSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eas.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eas.Synchronize();
            }

        }
        /// <summary>
        /// Synchronizacia objednavok CernyForLife <--> TVD
        /// </summary>
        internal void SyncCernyForLifeOrder(Dictionary<string, string> parameters) {
            int instanceId = 3;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife orders"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.CernyForLifeOrdersSynchronize eis = new Import.CernyForLifeOrdersSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eis.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eis.Synchronize();
            }

        }
        /// <summary>
        /// Import skladu produktov z TVD
        /// </summary>
        internal void ImportCernyForLifeProductStock(Dictionary<string, string> parameters) {
            int instanceId = 3;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            string srcTVDImagePath = parameters["srcTVDImagePath"].ToString();

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Synchronizacia Produktov
            DataTable dt = Cron.Eurona.Import.CernyForLifeTVDDAL.GetTVDProducts(mssqStorageSrc, instanceId);
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);
                int dispozice_HR = (int)Convert.ToDecimal(row["Dispozice_HR"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife product stock... {0}", productId), TraceCategory.Information);
                #endregion

                int itemsTotal = 0;
                int itemsImported = 0;
                int itemsError = 0;
                string errors = string.Empty;

                Import.CernyForLifeProductStockSynchronize tcs = new Import.CernyForLifeProductStockSynchronize(instanceId, productId, dispozice_HR, mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion
        }
        /// <summary>
        /// Import produktov z TVD
        /// </summary>
        internal void ImportCernyForLifeProduct(Dictionary<string, string> parameters) {
            int instanceId = 3;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            string srcTVDImagePath = parameters["srcTVDImagePath"].ToString();

            string dstProductImagePath = parameters["dstProductImagePath"].ToString();
            string dstVlastnostiImagePath = parameters["dstVlastnostiImagePath"].ToString();
            string dstPiktogramyImagePath = parameters["dstPiktogramyImagePath"].ToString();
            string dstParfumacieImagePath = parameters["dstParfumacieImagePath"].ToString();
            string dstSpecialniUcinkyImagePath = parameters["dstSpecialniUcinkyImagePath"].ToString();
            string dstZadniEtiketyImagePath = parameters["dstZadniEtiketyImagePath"].ToString();

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Synchronizacia obrazkou ciselnikou
            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife images for classifiers... [Parfemace, SpecialniUcinky]"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.CernyForLifeClsImagesSynchronize eis = new Import.CernyForLifeClsImagesSynchronize(srcTVDImagePath, dstParfumacieImagePath, dstSpecialniUcinkyImagePath, mssqStorageSrc);
            eis.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eis.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Product) {
                eis.Synchronize();
            }
            #endregion

            #region Synchronizacia kategorii
            DataTable dtCategories = Cron.Eurona.Import.CernyForLifeTVDDAL.GetTVDCategories(mssqStorageSrc);
            foreach (DataRow drCategory in dtCategories.Rows) {
                int categoryId = Convert.ToInt32(drCategory["Kategorie_Id"]);
                int? parentId = Convert.ToInt32(drCategory["Kategorie_Parent"]);
                int shop = Convert.ToInt32(drCategory["Shop"]);
                if (!parentId.HasValue || parentId.Value == 0) parentId = null;

                //Kategorie CL
                //if ( shop != 1 ) continue;

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife categories... {0}", categoryId), TraceCategory.Information);
                #endregion

                itemsTotal = 0;
                itemsImported = 0;
                itemsError = 0;
                errors = string.Empty;

                Import.CernyForLifeCategorySynchronize tcs = new Import.CernyForLifeCategorySynchronize(instanceId, categoryId, parentId, mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion

            #region Synchronizacia Produktov
            DataTable dt = Cron.Eurona.Import.CernyForLifeTVDDAL.GetTVDProducts(mssqStorageSrc, instanceId);
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife product... {0}", productId), TraceCategory.Information);
                #endregion

                itemsTotal = 0;
                itemsImported = 0;
                itemsError = 0;
                errors = string.Empty;

                Import.CernyForLifeProductSynchronize tcs = new Import.CernyForLifeProductSynchronize(
                        instanceId, productId,
                        srcTVDImagePath,
                        dstProductImagePath,
                        dstVlastnostiImagePath,
                        dstPiktogramyImagePath,
                        dstZadniEtiketyImagePath,
                        mssqStorageSrc, mssqStorageDst);
                tcs.Info += (message) => {
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
                };
                tcs.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;

                    if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
                };

                lock (SyncObject_Product) {
                    tcs.Synchronize();
                }
            }
            #endregion

            #region Synchronizacia Alternativnych Produktov
            foreach (DataRow row in dt.Rows) {
                int productId = Convert.ToInt32(row["C_Zbo"]);

                #region Log operation info
                if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
								base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
								//subject.Email = "roman.hudec@rhudec.sk";
#endif
                if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife alternate products for product... {0}", productId), TraceCategory.Information);
                #endregion

                #region Alternativni produkty
                //Odstranenie zvyraznenia produktu
                using (SqlConnection connection = mssqStorageDst.Connect()) {
                    DataTable dtProduktyAlternativni = Cron.Eurona.Import.CernyForLifeTVDDAL.GetTVDProductAlternativni(mssqStorageSrc, productId);
                    Cron.Eurona.Import.EuronaDAL.Product.RemoveProductRelations(mssqStorageDst, instanceId, productId);
                    foreach (DataRow rowP in dtProduktyAlternativni.Rows) {
                        int altProductId = Convert.ToInt32(rowP["Alternativni_C_Zbo"]);
                        Cron.Eurona.Import.EuronaDAL.Product.SyncProductAlternativni(mssqStorageDst, instanceId, productId, altProductId);
                    }
                }
                #endregion
            }
            #endregion

        }

        /// <summary>
        /// Synchronizacia Bonusovych kreditov pouzivatela do TVD
        /// </summary>
        /// <param name="parameters"></param>
        internal void SyncCernyForLifeBonusoveKredityUzivatele(Dictionary<string, string> parameters) {
            int instanceId = 3;
            //Check na zastavenia spracovania
            if (base.BreakPending())
                return;

            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(parameters["connectionStringSrc"]);
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(parameters["connectionStringDst"]);

            #region Log operation info
            if (TraceGeneral.TraceInfo) base.WriteLogLine(CommandLogSeparator, TraceCategory.Information);
#if _OFFLINE_DEBUG
						base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
#endif
            if (TraceGeneral.TraceInfo) base.WriteLogLine(string.Format("Synchronize CernyForLife bonusove kredity"), TraceCategory.Information);
            #endregion
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;
            Import.CernyForLifeBonusoveKredityUzivateleSynchronize eas = new Import.CernyForLifeBonusoveKredityUzivateleSynchronize(instanceId, mssqStorageSrc, mssqStorageDst);
            eas.Info += (message) => {
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.ItemProccessed += (item, totalItems, message) => {
                itemsTotal++;
                itemsImported++;
                if (TraceGeneral.TraceInfo) base.WriteLogLine(message, TraceCategory.Information);
            };
            eas.Error += (message, e) => {
                itemsTotal++;
                itemsError++;
                if (errors.Length != 0) errors += "\n";
                errors += message;

                if (TraceGeneral.TraceWarning) base.WriteLogLine(message, e);
            };

            lock (SyncObject_Account) {
                eas.Synchronize();
            }
        }
        #endregion
    }
}
