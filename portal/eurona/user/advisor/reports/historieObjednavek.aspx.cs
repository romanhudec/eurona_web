using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Telerik.Web.UI;
using System.Text;
using System.IO;
using System.Net;

namespace Eurona.User.Advisor.Reports {
    public partial class HistorieObjednavek : ReportPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            //Ak nie je vierihodny - len seba
            if (this.ForAdvisor.RestrictedAccess == 1)
                this.txtAdvisorCode.Enabled = false;

            if (!IsPostBack)
                this.txtAdvisorCode.Text = this.ForAdvisor.Code;

            GridViewDataBind(!IsPostBack);

            ////Funkcia volana v grid showOrder(xxx)
            //string url = Page.ResolveUrl("~/user/advisor/reports/Objednavka.aspx");
            //Utilities.RegisterShowOrderPOSTFunction(this.Page, url);
        }

        public override RadGrid GetGridView() {
            return this.gridView;
        }
        public override object GetFilter() {
            return base.GetFilter();
        }

        public override void OnGenerateReport() {
            if (this.ForAdvisor == null) return;

            if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text)) {
                if (this.txtAdvisorCode.Text != this.ForAdvisor.Code) {
                    int? obdobi = null;
                    object filter = GetFilter();
                    if (filter != null) obdobi = (int)filter;

                    OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
                    if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/historieObjednavek.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    else {
                        forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
                        if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/historieObjednavek.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    }
                }
            }

            GridViewDataBind(true);

        }


        public string GetObjednavkaUrl(object value) {
            string url = Page.ResolveUrl("~/user/advisor/reports/Objednavka.aspx");
            return url + "?" + CMS.Utilities.Cryptographer.Encrypt(value.ToString());
        }
        //protected void onOrderDetailClick(object sender, EventArgs e) {
        //    LinkButton btnlnk = sender as LinkButton;
        //    string orderNumber = btnlnk.Attributes["value"];
        //    string url = Page.ResolveUrl("~/user/advisor/reports/Objednavka.aspx");
        //    Utilities.RedirectWithPost(Page, url, new System.Collections.Specialized.NameValueCollection { { "orderNumber", orderNumber.Trim() } }, true);
        //} 

        private void GridViewDataBind(bool bind) {
            if (this.ForAdvisor == null) return;

            int? obdobi = null;
            object filter = GetFilter();
            if (filter != null) obdobi = (int)filter;

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                sql = @"SELECT f.id_prepoctu, ofr.id_web_objednavky, cislo_objednavky = f.cislo_objednavky_eurosap, f.datum_vystaveni_objednavky, f.celkem_k_uhrade, celkem_bez_dph = f.zaklad_zs, f.dph_zs,
									celkem_katalogova_cena = SUM( fr.cena_mj_katalogova * fr.mnozstvi),
									celkem_body = SUM(fr.zapocet_mj_body * fr.mnozstvi),
									celkem_objem_pro_marzi = SUM(fr.zapocet_mj_marze * fr.mnozstvi),
									celkem_objem_obchodu = SUM(fr.zapocet_mj_provize_czk * fr.mnozstvi),
									Stav_objednavky = ISNULL(ofr.Stav_faktury,1),
									Stav_objednavky_nazev = 
										CASE ISNULL(ofr.Stav_faktury,1) 
										WHEN 0 THEN 'Storno'
										WHEN 1 THEN 'Potvrzená objednávka'
										WHEN 3 THEN 'Připravena k expedici' 
										WHEN 4 THEN 'Vyřízeno'
										WHEN 99 THEN 'Uloženo'
										ELSE ''
										END,
									p.dor_misto,
									p.dor_psc,
									Adresa = (p.dor_ulice + ', ' + p.dor_misto + ', ' + p.dor_psc + ', ' + p.dor_stat)
								FROM www_faktury f WITH (NOLOCK) 
									INNER JOIN www_faktury_radky fr WITH (NOLOCK) ON fr.id_prepoctu = f.id_prepoctu  AND fr.idakce != 10
								    INNER JOIN objednavkyfaktury ofr WITH (NOLOCK) ON ofr.Id_objednavky = f.cislo_objednavky_eurosap
								    LEFT JOIN www_prepocty p WITH (NOLOCK) ON p.id_prepoctu = f.id_prepoctu
								WHERE 
									(YEAR(f.datum_vystaveni_objednavky)*100 +MONTH(f.datum_vystaveni_objednavky)) = @RRRRMM AND
									f.id_odberatele=@Id_odberatele AND
									f.potvrzeno=1

									GROUP BY f.id_prepoctu, ofr.id_web_objednavky, f.cislo_objednavky_eurosap, f.datum_vystaveni_objednavky, f.celkem_k_uhrade, f.zaklad_zs, f.dph_zs,
									p.dor_ulice , p.dor_misto ,p.dor_psc,p.dor_stat, ofr.Stav_faktury";

                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@RRRRMM", obdobi.HasValue ? (object)obdobi.Value : DBNull.Value),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}