using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;
using Telerik.Web.UI;

namespace Eurona.User.Advisor.Reports {
    public partial class NoviPoradciReport : ReportPage {
        private DateTime uzavierkaFrom;
        private DateTime uzavierkaTo;
        private int? obdobi;
        protected void Page_Load(object sender, EventArgs e) {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            #region Uzavierka
            UzavierkaEntity uzavierkaBefore = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.EuronaBefor });
            if (uzavierkaBefore == null) {
                uzavierkaBefore = new UzavierkaEntity();
                DateTime beforeOd = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                uzavierkaFrom = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 0, 0, 0);
                uzavierkaTo = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 23, 59, 59);
            } else {
                uzavierkaFrom = uzavierkaBefore.UzavierkaDo.Value;
            }
            UzavierkaEntity uzavierka = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
            if (uzavierka == null) {
                uzavierka = new UzavierkaEntity();
                DateTime datumDo = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
                uzavierkaFrom = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 0, 0, 0);
                uzavierkaTo = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 23, 59, 59);
            } else {
                uzavierkaTo = uzavierka.UzavierkaDo.Value;
            }
            #endregion

            string code = this.ForAdvisor.Code;
            GridViewDataBind(!IsPostBack);
        }
        private DateTime GetLastDayOfMonth(int year, int month) {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        public override RadGrid GetGridView() {
            return this.gridView;
        }
        public override object GetFilter() {
            return base.GetFilter();
        }

        public override void OnGenerateReport() {
            GridViewDataBind(true);
        }

        private void GridViewDataBind(bool bind) {
            this.obdobi = null;
            object filter = GetFilter();
            if (filter != null) obdobi = (int)filter;
            if (!obdobi.HasValue || obdobi.ToString().Length != 6) return;

            int mesic = obdobi.Value - DateTime.Now.Year*100;
            int mesic1 = mesic + 1;
            if ((mesic1 % 100) > 12) mesic1 = mesic - (mesic % 100) + 101;
            int mesic2 = mesic1 + 1;
            if ((mesic2 % 100) > 12) mesic2 = mesic1 - (mesic1 % 100) + 101;


            int obdobi1 = Convert.ToInt32(obdobi.ToString().Substring(0, 4)) * 100 + mesic1;
            int obdobi2 = Convert.ToInt32(obdobi.ToString().Substring(0, 4)) * 100 + mesic2;

            #region Uzavierka
            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            if (uzavierkaTo <= DateTime.Now) {
                int year = uzavierkaTo.Year * 100;
                currentObdobiRRRRMM = year + uzavierkaTo.Month;
                if (this.CurrentObdobiRRRRMM == currentObdobiRRRRMM) {
                    DateTime date = uzavierkaTo.AddMonths(-1);
                    year = date.Year * 100;
                    currentObdobiRRRRMM = year + date.Month;
                }
            } else if (uzavierkaFrom <= DateTime.Now && uzavierkaTo >= DateTime.Now && uzavierkaFrom.Month != uzavierkaTo.Month) {
                DateTime date = uzavierkaTo.AddMonths(-1);
                int year = date.Year * 100;
                currentObdobiRRRRMM = year + date.Month;
            }

            #endregion

            if (this.obdobi == 0) {
                this.obdobi = currentObdobiRRRRMM;
            }

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,o.E_mail,p.Objem_os,p.Body_os,
										o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
										Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
										Body_1 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM),
										Body_2 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM1),
										Body_3 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM2),
                                        Registrace_atp = (CASE WHEN o.Registrace_atp=1 THEN 'A' ELSE 'N' END)
										FROM provize_aktualni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
										--WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele)) )  AND
                                        WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM)) )  AND       
										DATEDIFF(""month"", o.Datum_zahajeni, getdate()) < 3";
                if (obdobi < this.CurrentObdobiRRRRMM) {
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,o.E_mail,p.Objem_os,p.Body_os,
										o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
										Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
										Body_1 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM),
										Body_2 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM1),
										Body_3 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM2),
                                        Registrace_atp = (CASE WHEN o.Registrace_atp=1 THEN 'A' ELSE 'N' END)
										FROM provize_finalni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
										--WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele)) )  AND
                                        WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM)) )  AND
										DATEDIFF(""month"", o.Datum_zahajeni, getdate()) < 3";
                }
                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@RRRRMM1", obdobi1),
                        new SqlParameter("@RRRRMM2", obdobi2),
                        new SqlParameter("@RRRRMM", obdobi.HasValue ? (object)obdobi.Value : DBNull.Value),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}