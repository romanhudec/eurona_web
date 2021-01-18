using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;
using EuronaImportFromTVD.DAL;

namespace EuronaImportFromTVD.Tasks {
    /// <summary>
    /// Synchronizacia obrazkou ciselnikou z TVD
    /// </summary>
    public class EuronaOrdersImport : Synchronize {
        private int instanceId = 0;
        private int tvdIdObjednavky = 0;
        public EuronaOrdersImport(int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage, int tvdIdObjednavky)
            : base(srcSqlStorage, dstSqlStorage) {
            this.instanceId = instanceId;
            this.tvdIdObjednavky = tvdIdObjednavky;
        }

        public int InstanceId { get { return this.instanceId; } }

        public override void Synchronize() {
            int addedItems = 0;
            int updatedItems = 0;
            int errorItems = 0;
            int ignoredItems = 0;

            int rowsCount = 0;
            using (SqlConnection connection = this.DestinationDataStorage.Connect()) {
                try {
                    int rowIndex = 0;
                    try {
                        string sql = string.Empty;

                        #region TVD->EURONA
                        DataRow rowObjednavka = EuronaTVDDAL.GetTVDObjednavka(this.SourceDataStorage, this.tvdIdObjednavky);
                        //Id_objednavky, id_web_objednavky, Id_odberatele, Zpusob_dodani, Dodat_od, Dodat_do, Zpusob_platby, Stav_faktury, Celkem_k_uhrade, Celkem_bez_dph, Dor_nazev_firmy, Dor_misto, Dor_ulice, Dor_psc, Dor_stat, Dor_telefon, Dor_email, Info_pro_odb_html
                        int Id_objednavky = Convert.ToInt32(rowObjednavka["Id_objednavky"]);
                        string Id_web_objednavky = Convert.ToString(rowObjednavka["id_web_objednavky"]);
                        int Id_odberatele = Convert.ToInt32(rowObjednavka["Id_odberatele"]);
                        DateTime Datum_zapisu_obj = Convert.ToDateTime(rowObjednavka["Datum_zapisu_obj"]);
                        int Zpusob_dodani = Convert.ToInt32(rowObjednavka["Zpusob_dodani"]);
                        DateTime? Dodat_od = ConvertNullable.ToDateTime(rowObjednavka["Dodat_od"]);
                        DateTime? Dodat_do = ConvertNullable.ToDateTime(rowObjednavka["Dodat_do"]);
                        int Zpusob_platby = Convert.ToInt32(rowObjednavka["Zpusob_platby"]);
                        int Stav_faktury = Convert.ToInt32(rowObjednavka["Stav_faktury"]);
                        decimal Celkem_k_uhrade = Convert.ToDecimal(rowObjednavka["Celkem_k_uhrade"]);
                        decimal Celkem_bez_dph = Convert.ToDecimal(rowObjednavka["Celkem_bez_dph"]);
                        string Dor_nazev_firmy = Convert.ToString(rowObjednavka["Dor_nazev_firmy"]);
                        string Kod_meny = Convert.ToString(rowObjednavka["Kod_meny"]);
                        string Dor_misto = Convert.ToString(rowObjednavka["Dor_misto"]);
                        string Dor_ulice = Convert.ToString(rowObjednavka["Dor_ulice"]);
                        string Dor_psc = Convert.ToString(rowObjednavka["Dor_psc"]);
                        string Dor_stat = Convert.ToString(rowObjednavka["Dor_stat"]);
                        string Dor_telefon = Convert.ToString(rowObjednavka["Dor_telefon"]);
                        string Dor_email = Convert.ToString(rowObjednavka["Dor_email"]);
                        string Info_pro_odb_html = Convert.ToString(rowObjednavka["Info_pro_odb_html"]);

                        string zavoz_misto = Convert.ToString(rowObjednavka["zavoz_misto"]);
                        string zavoz_psc = Convert.ToString(rowObjednavka["zavoz_psc"]);
                        DateTime? zavoz_datum = ConvertNullable.ToDateTime(rowObjednavka["zavoz_datum"]);
                        
                        DataTable dtRadky = EuronaTVDDAL.GetTVDObjednavkaRadky(this.SourceDataStorage, this.tvdIdObjednavky);

                        try {
                            EuronaDAL.Order.SyncEuronaOrder(this.DestinationDataStorage, Id_objednavky, Id_web_objednavky, Id_odberatele, Datum_zapisu_obj, Zpusob_dodani, Dodat_od, Dodat_do, Zpusob_platby, Stav_faktury, Celkem_k_uhrade, Celkem_bez_dph,
                                Dor_nazev_firmy, Kod_meny, Dor_misto, Dor_ulice, Dor_psc, Dor_stat, Dor_telefon, Dor_email, Info_pro_odb_html, zavoz_misto, zavoz_datum, zavoz_psc, dtRadky);
                            OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing order '{0}' : ok", Id_objednavky));
                        } catch (Exception ex) {
                            string errorMessage = string.Format("Proccessing Orders : failed!", Id_objednavky);
                            StringBuilder sbMessage = new StringBuilder();
                            sbMessage.Append(errorMessage);
                            sbMessage.AppendLine(ex.Message);
                            if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                            sbMessage.AppendLine(ex.StackTrace);

                            OnError(errorMessage, ex);
                        }
                        #endregion
                    } catch (Exception ex) {
                        string errorMessage = "Proccessing orders : failed!";
                        StringBuilder sbMessage = new StringBuilder();
                        sbMessage.Append(errorMessage);
                        sbMessage.AppendLine(ex.Message);
                        if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                        sbMessage.AppendLine(ex.StackTrace);

                        OnError(errorMessage, ex);
                        errorItems++;
                    } finally {
                        rowIndex++;
                    }

                } finally {
                    OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
                }
            }
        }

        private string GetString(object obj) {
            if (obj == null) return null;
            return obj.ToString().Trim();
        }

    }
}
