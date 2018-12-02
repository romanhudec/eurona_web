using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Data.SqlClient;
using System.Data;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;

namespace Eurona.user.advisor.reports {
    public partial class osobniPrehledPoradce : System.Web.UI.UserControl {

        private DateTime aktualniObdobiUzavierkaFrom;
        private DateTime aktualniObdobiUzavierkaTo;

        protected void Page_Load(object sender, EventArgs e) {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            #region Uzavierka
            UzavierkaEntity uzavierkaBefore = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.EuronaBefor });
            if (uzavierkaBefore == null) {
                uzavierkaBefore = new UzavierkaEntity();
                DateTime beforeOd = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                aktualniObdobiUzavierkaFrom = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 0, 0, 0);
                aktualniObdobiUzavierkaTo = new DateTime(beforeOd.Year, beforeOd.Month, beforeOd.Day, 23, 59, 59);
            } else {
                //Dopracovane 02.12.2018
                {
                    if (uzavierkaBefore.UzavierkaDo.Value.Month < DateTime.Now.Month)
                        aktualniObdobiUzavierkaFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    else
                        aktualniObdobiUzavierkaFrom = uzavierkaBefore.UzavierkaDo.Value;
                }
                //Zakomentovane 02.12.2018
                //aktualniObdobiUzavierkaFrom = uzavierkaBefore.UzavierkaDo.Value;
            }
            UzavierkaEntity uzavierka = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
            if (uzavierka == null) {
                uzavierka = new UzavierkaEntity();
                DateTime datumDo = GetLastDayOfMonth(DateTime.Now.Year, DateTime.Now.Month);
                aktualniObdobiUzavierkaFrom = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 0, 0, 0);
                aktualniObdobiUzavierkaTo = new DateTime(datumDo.Year, datumDo.Month, datumDo.Day, 23, 59, 59);
            } else {
                aktualniObdobiUzavierkaTo = uzavierka.UzavierkaDo.Value;
            }
            #endregion

            GridViewDataBind(!IsPostBack);
        }

        private DateTime GetLastDayOfMonth(int year, int month)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        /// <summary>
        /// Report pre poradcu
        /// </summary>
        public OrganizationEntity ForAdvisor { get; set; }
        /// <summary>
        /// Prihlaseny poradca
        /// </summary>
        public OrganizationEntity LogedAdvisor { get; set; }

        public int? Obdobi { get; set; }
        /// <summary>
        /// Aktualne obdobie RRRRMM
        /// </summary>
        public int CurrentObdobiRRRRMM {
            get {
                int year = DateTime.Now.Year * 100;
                return year + DateTime.Now.Month;
            }
        }
        /// <summary>
        /// Vrati connection string do TVD
        /// </summary>
        public string ConnectionString {
            get {
                return ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            }
        }

        public void GridViewDataBind(bool bind) {
            int? obdobi = null;
            object filter = this.Obdobi;
            if (filter != null) obdobi = (int)filter;
            if (obdobi.HasValue == false || obdobi == 0) return;

            int rok = Convert.ToInt32(obdobi.ToString().Substring(0, 4));
            int mesic = obdobi.Value - rok * 100;
            /*
            #region Uzavierka
            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            if (obdobi == currentObdobiRRRRMM) {
                if (aktualniObdobiUzavierkaTo <= DateTime.Now) {
                    int year = aktualniObdobiUzavierkaTo.Year * 100;
                    currentObdobiRRRRMM = year + aktualniObdobiUzavierkaTo.Month;
                    if (this.CurrentObdobiRRRRMM == currentObdobiRRRRMM) {
                        DateTime date = aktualniObdobiUzavierkaTo.AddMonths(-1);
                        year = date.Year * 100;
                        currentObdobiRRRRMM = year + date.Month;
                    }
                } else if (aktualniObdobiUzavierkaFrom <= DateTime.Now && aktualniObdobiUzavierkaTo >= DateTime.Now && aktualniObdobiUzavierkaFrom.Month != aktualniObdobiUzavierkaTo.Month) {
                    int year = aktualniObdobiUzavierkaFrom.Year * 100;
                    currentObdobiRRRRMM = year + aktualniObdobiUzavierkaFrom.Month;
                }
            }
            #endregion
            */

            #region Uzavierka
            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            if (/*obdobi == currentObdobiRRRRMM*/true) {
                if (aktualniObdobiUzavierkaTo <= DateTime.Now) {
                    int year = aktualniObdobiUzavierkaTo.Year * 100;
                    currentObdobiRRRRMM = year + aktualniObdobiUzavierkaTo.Month;
                    if (this.CurrentObdobiRRRRMM == currentObdobiRRRRMM) {
                        DateTime date = aktualniObdobiUzavierkaTo.AddMonths(-1);
                        year = date.Year * 100;
                        currentObdobiRRRRMM = year + date.Month;
                    }
                } else if (aktualniObdobiUzavierkaFrom <= DateTime.Now && aktualniObdobiUzavierkaTo >= DateTime.Now && aktualniObdobiUzavierkaFrom.Month != aktualniObdobiUzavierkaTo.Month) {
                    DateTime date = aktualniObdobiUzavierkaTo.AddMonths(-1);
                    int year = date.Year * 100;
                    currentObdobiRRRRMM = year + date.Month;
                }
            }

            #endregion

            if (obdobi == 0) {
                obdobi = currentObdobiRRRRMM;
            }

            this.sqlDirectGroup.ConnectionString = this.ConnectionString;
            this.sqlDirectGroup.SelectParameters["Id_odberatele"].DefaultValue = this.ForAdvisor.TVD_Id.ToString();

            this.sqlBonusGroup.ConnectionString = this.ConnectionString;
            this.sqlBonusGroup.SelectParameters["Id_odberatele"].DefaultValue = this.ForAdvisor.TVD_Id.ToString();

            this.sqlTotalGroup.ConnectionString = this.ConnectionString;
            this.sqlTotalGroup.SelectParameters["Id_odberatele"].DefaultValue = this.ForAdvisor.TVD_Id.ToString();

            this.sqlRestMarginPrice.ConnectionString = this.ConnectionString;
            this.sqlRestMarginPrice.SelectParameters["rok"].DefaultValue = rok.ToString();
            this.sqlRestMarginPrice.SelectParameters["mesic"].DefaultValue = mesic.ToString();
            this.sqlRestMarginPrice.SelectParameters["Id_odberatele"].DefaultValue = this.ForAdvisor.TVD_Id.ToString();

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(this.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,p.Narok_sleva_nrz ,p.Narok_eurokredit,
										o.Kod_odberatele, o.Nazev_firmy, o.Telefon, o.Telefon_prace, o.Mobil, Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.E_mail, o.Fax, o.Icq, o.skype,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
										Kod_meny = p.Provize_vyplata_kod_meny,
										ecredit = p.Eurokredit_vlastni+p.Eurokredit_registrace-p.Eurokredit_vyber
										FROM provize_aktualni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele 
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM  AND ( p.Id_odberatele=@Id_odberatele )";

                if (obdobi < currentObdobiRRRRMM)
                    sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,Narok_sleva_nrz=0 ,Narok_eurokredit=0,
										o.Kod_odberatele, o.Nazev_firmy, o.Telefon, o.Telefon_prace, o.Mobil, Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.E_mail, o.Fax, o.Icq, o.skype,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
										Kod_meny = p.Provize_vyplata_kod_meny,
										ecredit = p.Eurokredit_vlastni+p.Eurokredit_registrace-p.Eurokredit_vyber
										FROM provize_finalni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										WHERE o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele=@Id_odberatele )";

                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql,
                        new SqlParameter("@RRRRMM", obdobi.HasValue ? (object)obdobi.Value : DBNull.Value),
                        new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

                this.formActivity.DataSource = dt;
            }

            if (bind) formActivity.DataBind();
        }
    }
}