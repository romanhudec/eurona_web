using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Telerik.Web.UI;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;
namespace Eurona.User.Advisor.Reports {
    public partial class PrehledProdukceSkupinyReport : ReportPage {
        private DateTime uzavierkaFrom;
        private DateTime uzavierkaTo;

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
            //Ak nie je vierihodny - len seba
            if (this.ForAdvisor.RestrictedAccess == 1) {
                this.txtAdvisorCode.Enabled = false;
                this.rbSkupina.Enabled = false;
                this.rbPrvniLinie.Checked = true;
            }

            if (!IsPostBack) {
                this.rbPrvniLinie.Checked = true;
                this.txtAdvisorCode.Text = this.ForAdvisor.Code;
            }

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
            if (this.ForAdvisor == null) return;

            if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text)) {
                if (this.txtAdvisorCode.Text != this.ForAdvisor.Code) {
                    int? obdobi = null;
                    object filter = GetFilter();
                    if (filter != null) obdobi = (int)filter;

                    OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
                    if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledProdukceSkupiny.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    else {
                        forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
                        if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledProdukceSkupiny.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    }
                }
            }

            GridViewDataBind(true);
        }

        private void GridViewDataBind(bool bind) {
            int? obdobi = null;
            object filter = GetFilter();
            if (filter != null) obdobi = (int)filter;

            if (this.rbPrvniLinie.Checked) this.Title = this.Title + " - " + Resources.Reports.PrvniLinie.ToLower();
            bool include21Pordce = !this.cbBez21.Checked;
            /*
            #region Uzavierka
            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            if (obdobi == currentObdobiRRRRMM) {
                if (uzavierkaTo <= DateTime.Now) {
                    int year = uzavierkaTo.Year * 100;
                    currentObdobiRRRRMM = year + uzavierkaTo.Month;
                    if (this.CurrentObdobiRRRRMM == currentObdobiRRRRMM) {
                        DateTime date = uzavierkaTo.AddMonths(-1);
                        year = date.Year * 100;
                        currentObdobiRRRRMM = year + date.Month;
                    }
                } else if (uzavierkaFrom <= DateTime.Now && uzavierkaTo >= DateTime.Now && uzavierkaFrom.Month != uzavierkaTo.Month) {
                    int year = uzavierkaFrom.Year * 100;//uzavierkaTo.Year * 100;
                    currentObdobiRRRRMM = year + uzavierkaFrom.Month;
                }
            }
            #endregion
             * */
            #region Uzavierka
            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            if (/*obdobi == currentObdobiRRRRMM*/true) {
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
            }

            #endregion

            if (obdobi == 0) {
                obdobi = currentObdobiRRRRMM;
            }
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"";
                if (this.rbPrvniLinie.Checked) {
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,p.Narok_sleva_nrz ,p.Narok_eurokredit,
										o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc, o.E_mail,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
										FROM provize_aktualni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele=@Id_odberatele OR p.Id_nadrizeneho=@Id_odberatele )
										AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
                    if (obdobi < currentObdobiRRRRMM) {
                        sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
												p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
												p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
												p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
												p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
												o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc, o.E_mail,
												Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
												FROM provize_finalni p
												INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
												WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele=@Id_odberatele OR p.Id_nadrizeneho=@Id_odberatele )
												AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
                    }
                } else if (this.rbSkupina.Checked) {
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
										o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
										FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
										INNER JOIN provize_aktualni p ON p.Id_odberatele = f.Id_Odberatele
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM 
										AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
                    if (obdobi < currentObdobiRRRRMM) {
                        sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
												p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
												p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
												p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
												p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
												o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
												Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
												FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
												INNER JOIN provize_finalni p ON p.Id_odberatele = f.Id_Odberatele
												INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
												WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM
												AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
                    }
                } else if (this.rbBez21PodrizenychSkupin.Checked) {
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
										o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
										FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
										INNER JOIN provize_aktualni p ON p.Id_odberatele = f.Id_Odberatele
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM
                                        AND
                                        f.LineageId NOT LIKE ( 
                                            SELECT ff.LineageId FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) ff 
                                            INNER JOIN provize_aktualni pp ON pp.Id_odberatele = ff.Id_Odberatele
                                            WHERE pp.Hladina=21 AND
                                            LEN(ff.LineageId) = (
	                                            SELECT MIN(LEN(fff.LineageId)) FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) fff 
	                                            INNER JOIN provize_aktualni ppp ON ppp.Id_odberatele = fff.Id_Odberatele
	                                            WHERE ppp.Hladina=21
                                            )
                                        )+'(%'";
                    if (obdobi < currentObdobiRRRRMM) {
                        sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
												p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
												p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
												p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
												p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
												o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
												Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina
												FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
												INNER JOIN provize_finalni p ON p.Id_odberatele = f.Id_Odberatele
												INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
												WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM
                                                AND
                                                f.LineageId NOT LIKE ( 
                                                    SELECT ff.LineageId FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) ff 
                                                    INNER JOIN provize_aktualni pp ON pp.Id_odberatele = ff.Id_Odberatele
                                                    WHERE pp.Hladina=21 AND
                                                    LEN(ff.LineageId) = (
	                                                    SELECT MIN(LEN(fff.LineageId)) FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) fff 
	                                                    INNER JOIN provize_aktualni ppp ON ppp.Id_odberatele = fff.Id_Odberatele
	                                                    WHERE ppp.Hladina=21
                                                    )
                                                )+'(%'";
                    }
                }
                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@RRRRMM", obdobi.HasValue ? (object)obdobi.Value : DBNull.Value),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id),
                        new SqlParameter("@Include21Pordce", include21Pordce));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }
    }
}