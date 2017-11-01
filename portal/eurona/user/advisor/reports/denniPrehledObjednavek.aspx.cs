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
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;
using OrderEntity = Eurona.DAL.Entities.Order;
using Telerik.Web.UI;
using System.Text;

namespace Eurona.User.Advisor.Reports {
    public partial class DenniPrehledObjednavek : ReportPage {
        private DateTime dateFrom;
        private DateTime dateTo;
        protected void Page_Load(object sender, EventArgs e) {
            this.HideObdobi = true;
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            //Ak nie je vierihodny - len seba
            if (this.ForAdvisor.RestrictedAccess == 1)
                this.txtAdvisorCode.Enabled = false;

            if (!IsPostBack)
                this.txtAdvisorCode.Text = this.ForAdvisor.Code;


            UzavierkaEntity uzavierkaBefore = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.EuronaBefor });
            if (uzavierkaBefore == null) {
                uzavierkaBefore = new UzavierkaEntity();
                DateTime beforeOd = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                uzavierkaBefore.UzavierkaOd = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 0, 0, 0);
                uzavierkaBefore.UzavierkaDo = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 23, 59, 59);
            }
            UzavierkaEntity uzavierka = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
            if (uzavierka == null) {
                uzavierka = new UzavierkaEntity();
                DateTime datumDo = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
                uzavierka.UzavierkaOd = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 0, 0, 0);
                uzavierka.UzavierkaDo = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 23, 59, 59);
            }

            //Ak je nastavena uzavierka na aktualny mesiac
            if (DateTime.Now <= uzavierka.UzavierkaOd.Value) {
                this.dateFrom = uzavierkaBefore.UzavierkaDo.Value;
                this.dateTo = uzavierka.UzavierkaOd.Value;
            } else {
                //Ak este nie je nastavena nova uzavierka na aktualny mesiac
                this.dateFrom = uzavierka.UzavierkaDo.Value;
                DateTime datumDo = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
                this.dateTo = datumDo;
            }

            this.lblObdobi.Text = string.Format("od:<b style='color:#eb0a5b !important;'>{0}</b>&nbsp;do:<b style='color:#eb0a5b !important;'>{1}</b>", this.dateFrom.ToString(), this.dateTo.ToString());

            GridViewDataBind(!IsPostBack, this.dateFrom, this.dateTo);
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
            if (this.ForAdvisor == null) return;

            if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text)) {
                if (this.txtAdvisorCode.Text != this.ForAdvisor.Code) {
                    object filter = GetFilter();
                    OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
                    if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/denniPrehledObjednavek.aspx?id={0}", forAdvisor.TVD_Id)));
                    else {
                        forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
                        if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/denniPrehledObjednavek.aspx?id={0}", forAdvisor.TVD_Id)));
                    }
                }
            }

            GridViewDataBind(true, this.dateFrom, this.dateTo);

        }

        private void GridViewDataBind(bool bind, DateTime dateFrom, DateTime dateTo) {
            if (this.ForAdvisor == null) return;

            object filter = GetFilter();

            StringBuilder paramInIdPrepoctu = new StringBuilder();
            List<OrderEntity> orders = Storage<OrderEntity>.Read(new OrderEntity.ReadByFilter { OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString() });
            foreach (OrderEntity order in orders) {
                if (paramInIdPrepoctu.Length != 0)
                    paramInIdPrepoctu.Append(",");
                paramInIdPrepoctu.AppendFormat("{0}", order.CartId * -1);
            }
            if (paramInIdPrepoctu.Length == 0) paramInIdPrepoctu.Append("-1");
            string sqlParamInIdPrepoctu = "(" + paramInIdPrepoctu.ToString() + ")";
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                /*
                    Stav_objednavky = ISNULL(ofr.stav_faktury,-1),
					Stav_objednavky_nazev = 
						CASE ISNULL(ofr.stav_faktury,-1) 
                        WHEN -1 THEN '-1 - Čeká na zpracování'
						WHEN 0 THEN '0 - Storno'
						WHEN 1 THEN '1 - Zpracováva se'
						WHEN 2 THEN '2 - Zpracováva se' 
						WHEN 4 THEN '4 - Vyřízena'
						ELSE ''
						END,                 
                 */
                sql = @"SELECT f.id_prepoctu, cislo_objednavky = f.cislo_objednavky_eurosap, f.datum_vystaveni, f.celkem_k_uhrade, celkem_bez_dph = f.zaklad_zs, f.dph_zs,
						    celkem_katalogova_cena = SUM( fr.cena_mj_katalogova * fr.mnozstvi),
						    celkem_body = SUM(fr.zapocet_mj_body * fr.mnozstvi),
						    celkem_objem_pro_marzi = SUM(fr.zapocet_mj_marze * fr.mnozstvi),
						    celkem_objem_obchodu = SUM(fr.zapocet_mj_provize_czk * fr.mnozstvi),
                            o.Kod_odberatele, o.Nazev_firmy,
					            Stav_objednavky = 
                                    ISNULL(ofr.stav_faktury, (CASE WHEN f.cislo_objednavky_eurosap IS NULL THEN 99 ELSE 1 END) ),
					            Stav_objednavky_nazev = 
						            CASE ISNULL(ofr.stav_faktury, (CASE WHEN f.cislo_objednavky_eurosap IS NULL THEN 99 ELSE 1 END) )
                                    WHEN 99 THEN '99 - Čeká na zpracování'
						            WHEN 0 THEN '0 - Storno'
						            WHEN 1 THEN '1 - Potvrzená objednávka'
						            WHEN 2 THEN '2 - Zpracováva se' 
                                    WHEN 3 THEN '3 – Připravena k expedici'
						            WHEN 4 THEN '4 - Vyřízena'
						            ELSE ''
						            END,
						    Adresa = (p.dor_ulice + ', ' + p.dor_misto + ', ' + p.dor_psc + ', ' + p.dor_stat)
                        FROM fGetOdberateleStrom(@Id_odberatele) os
						    INNER JOIN www_faktury f ON f.Id_Odberatele = os.Id_Odberatele
						    INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
                            INNER JOIN odberatele o  ON o.Id_odberatele = f.Id_odberatele
						    LEFT JOIN objednavkyfaktury ofr ON ofr.Id_objednavky = f.cislo_objednavky_eurosap
						    LEFT JOIN www_prepocty p ON p.id_prepoctu = f.id_prepoctu
					    WHERE 
						    f.datum_vystaveni_objednavky >=@dateFrom AND f.datum_vystaveni_objednavky<=@dateTo
                            AND ( f.potvrzeno=1 OR f.Id_prepoctu IN " + sqlParamInIdPrepoctu+ @" )
                            -- jen objednavky kterer jeste nebyly prepocteny (neprepocitane objednavky nie su v tabulke cislo_objednavky_eurosap)
                            AND ( f.cislo_objednavky_eurosap IS NULL OR f.cislo_objednavky_eurosap  NOT IN (SELECT Cislo_objednavky FROM provize_aktualni_objednavky))
						    GROUP BY 
                            o.Kod_odberatele, o.Nazev_firmy,
                            f.id_prepoctu, f.cislo_objednavky_eurosap, f.datum_vystaveni, f.celkem_k_uhrade, f.zaklad_zs, f.dph_zs, ofr.stav_faktury,
						    p.dor_ulice , p.dor_misto ,p.dor_psc,p.dor_stat";

                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@dateFrom", dateFrom),
                        new SqlParameter("@dateTo", dateTo),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}