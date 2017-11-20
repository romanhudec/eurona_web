﻿using System;
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

namespace Eurona.User.Advisor.Reports {
    public partial class PrehledObjednavek : ReportPage {

        private DateTime minDate;
        private DateTime now;
        protected void Page_Load(object sender, EventArgs e) {
            this.HideObdobi = true;
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            if (!IsPostBack) {
                now = DateTime.Now;
                minDate = new DateTime(now.Year, now.Month - 1, 1);
                this.dtpDatumOd.MinDate = minDate;
                this.dtpDatumOd.MaxDate = now;
                this.dtpDatumOd.SelectedDate = minDate;

                this.dtpDatumDo.MinDate = minDate;
                this.dtpDatumDo.MaxDate = now;
                this.dtpDatumDo.SelectedDate = now;
            }
            //Ak nie je vierihodny - len seba
            if (this.ForAdvisor.RestrictedAccess == 1)
                this.txtAdvisorCode.Enabled = false;

            if (!IsPostBack)
                this.txtAdvisorCode.Text = this.ForAdvisor.Code;

            GridViewDataBind(!IsPostBack);
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
                    if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledObjednavek.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    else {
                        forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
                        if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledObjednavek.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    }
                }
            }

            GridViewDataBind(true);

        }

        private void GridViewDataBind(bool bind) {
            if (this.ForAdvisor == null) return;

            DateTime? dateOd = this.dtpDatumOd.SelectedDate;
            DateTime? dateDo = this.dtpDatumDo.SelectedDate;

            if (!dateOd.HasValue) return;
            if (!dateDo.HasValue) return;

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                sql = @"SELECT o.Kod_odberatele, f.id_prepoctu, ofr.id_web_objednavky, cislo_objednavky = f.cislo_objednavky_eurosap, f.datum_vystaveni, f.celkem_k_uhrade, celkem_bez_dph = f.zaklad_zs, f.dph_zs,
									celkem_katalogova_cena = SUM( fr.cena_mj_katalogova * fr.mnozstvi),
									celkem_body = SUM(fr.zapocet_mj_body * fr.mnozstvi),
									celkem_objem_pro_marzi = SUM(fr.zapocet_mj_marze * fr.mnozstvi),
									celkem_objem_obchodu = SUM(fr.zapocet_mj_provize_czk * fr.mnozstvi),
									Stav_objednavky = ISNULL(ofr.Stav_faktury,1),
									Stav_objednavky_nazev = 
										CASE ISNULL(ofr.Stav_faktury,1) 
										WHEN 0 THEN 'Storno'
										WHEN 1 THEN 'Potvrzená objednávka'
										WHEN 2 THEN 'Připravena k expedici' 
										WHEN 4 THEN 'Vyřízeno'
										WHEN 99 THEN 'Uloženo'
										ELSE ''
										END,
									ofr.Dor_nazev_firmy,
									ofr.dor_misto,
									ofr.dor_psc, top_manager=oTop.Nazev_firmy									
								FROM www_faktury f 
									INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
								    LEFT JOIN objednavkyfaktury ofr ON ofr.Id_objednavky = f.cislo_objednavky_eurosap
								    --LEFT JOIN www_prepocty p ON p.id_prepoctu = f.id_prepoctu
                                    INNER JOIN odberatele o  ON o.Id_odberatele = f.id_odberatele
                                    LEFT JOIN odberatele oTop  ON oTop.Id_odberatele = ofr.Id_topmanagera
								WHERE 
									    (f.datum_vystaveni >= @dateOd AND f.datum_vystaveni <= @dateDo) AND
									    f.id_odberatele=@Id_odberatele AND
									    f.potvrzeno=1
									GROUP BY o.Kod_odberatele, f.id_prepoctu, ofr.id_web_objednavky, f.cislo_objednavky_eurosap, f.datum_vystaveni, f.celkem_k_uhrade, f.zaklad_zs, f.dph_zs, ofr.Stav_faktury,
									    ofr.Dor_nazev_firmy,
									    ofr.dor_misto,
									    ofr.dor_psc, oTop.Nazev_firmy
                                    ORDER BY f.datum_vystaveni DESC";

                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@dateOd", dateOd.Value),
                        new SqlParameter("@dateDo", dateDo.Value),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}