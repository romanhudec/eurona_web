using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace Eurona.User.Advisor.Reports {
    public partial class NoviPoradciReport : ReportPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            string code = this.ForAdvisor.Code;
            GridViewDataBind(!IsPostBack);
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
            int? obdobi = null;
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

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,o.E_mail,p.Objem_os,p.Body_os,
										o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
										Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
										Body_1 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM),
										Body_2 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM1),
										Body_3 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM2)
										FROM provize_aktualni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele)) )  AND
										DATEDIFF(""month"", o.Datum_zahajeni, getdate()) < 3";
                if (obdobi < this.CurrentObdobiRRRRMM) {
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,o.E_mail,p.Objem_os,p.Body_os,
										o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
										Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
										Body_1 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM),
										Body_2 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM1),
										Body_3 = (select Body_vlastni from provize_aktualni where Id_odberatele=p.Id_odberatele and RRRRMM=@RRRRMM2)
										FROM provize_finalni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele)) )  AND
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