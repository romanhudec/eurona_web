using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace Eurona.User.Advisor.Reports
{
    public partial class ZruseniPoradciReport : ReportPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            this.HideObdobi = true;
            string code = this.ForAdvisor.Code;
            if (!IsPostBack)
                this.txtObdobi.Text = string.Format("{0}{1:#00}", DateTime.Now.Year, DateTime.Now.Month);
            GridViewDataBind(!IsPostBack);
        }
        public override RadGrid GetGridView()
        {
            return this.gridView;
        }
        public override object GetFilter()
        {
            return base.GetFilter();
        }

        public override void OnGenerateReport()
        {
            GridViewDataBind(true);
        }

        private void GridViewDataBind(bool bind)
        {
            int obdobi = Convert.ToInt32(string.Format("{0}{1:#00}", DateTime.Now.Year, DateTime.Now.Month));
            Int32.TryParse(this.txtObdobi.Text, out obdobi);

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect())
            {
                string sql = @"";
                if (this.cbSkupina.Checked == false)
                {
                    sql = @"SELECT o.Id_odberatele,o.Cislo_nadrizeneho,o.E_mail,
					o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
					Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
                    ov.Datum_vymazu
					FROM odberatele o
					INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
                    INNER JOIN odberatele_vymaz ov ON ov.Id_odberatele = o.Id_odberatele
					WHERE o.Stav_odberatele='Z' AND
                    (@obdobi IS NULL OR @obdobi = (YEAR(ov.Datum_vymazu)*100) + MONTH(ov.Datum_vymazu))";
                }
                else
                {
                    sql = @"SELECT o.Id_odberatele,o.Cislo_nadrizeneho,o.E_mail,
					o.Datum_zahajeni, o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END),
					Kod_odberatele_sponzor = op.Kod_odberatele, Nazev_firmy_sponzor = op.Nazev_firmy, Telefon_sponzor = (CASE WHEN LEN(op.Telefon)=0 THEN op.Mobil ELSE op.Telefon END),
                    ov.Datum_vymazu
                    FROM fGetOdberateleStromZ(@Id_odberatele) f
					INNER JOIN odberatele o ON o.Id_odberatele = f.Id_odberatele
					INNER JOIN odberatele op ON op.Id_odberatele = o.Cislo_nadrizeneho
                    INNER JOIN odberatele_vymaz ov ON ov.Id_odberatele = o.Id_odberatele
					WHERE o.Stav_odberatele='Z' AND
                    (@obdobi IS NULL OR @obdobi = (YEAR(ov.Datum_vymazu)*100) + MONTH(ov.Datum_vymazu))";
                }
                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id), new SqlParameter("@obdobi", obdobi != 0 ? (object)obdobi : DBNull.Value));                
                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}