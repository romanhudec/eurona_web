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
namespace Eurona.User.Advisor.Reports
{
	public partial class PrehledProdukceSkupinyReportATP : ReportPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.ForAdvisor == null) return;
			if (this.ForAdvisor.TVD_Id == null) return;

			//Ak nie je vierihodny - len seba
			if (this.ForAdvisor.RestrictedAccess == 1)
			{
				this.txtAdvisorCode.Enabled = false;
				this.rbSkupina.Enabled = false;
				this.rbPrvniLinie.Checked = true;
			}

			if (!IsPostBack)
			{
				this.rbPrvniLinie.Checked = true;
				this.txtAdvisorCode.Text = this.ForAdvisor.Code;
			}

			string code = this.ForAdvisor.Code;
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
			if (this.ForAdvisor == null) return;

			if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text))
			{
				if (this.txtAdvisorCode.Text != this.ForAdvisor.Code)
				{
					int? obdobi = null;
					object filter = GetFilter();
					if (filter != null) obdobi = (int)filter;

					OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
					if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledProdukceSkupiny.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
					else
					{
						forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
						if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/prehledProdukceSkupiny.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
					}
				}
			}

			GridViewDataBind(true);
		}

		private void GridViewDataBind(bool bind)
		{
			int? obdobi = null;
			object filter = GetFilter();
			if (filter != null) obdobi = (int)filter;

			if (this.rbPrvniLinie.Checked) this.Title = this.Title + " - " + Resources.Reports.PrvniLinie.ToLower();
			bool include21Pordce = !this.cbBez21.Checked;

            int currentObdobiRRRRMM = this.CurrentObdobiRRRRMM;
            UzavierkaEntity uzavierka = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
            if (uzavierka != null && uzavierka.UzavierkaDo != null && uzavierka.UzavierkaDo <= DateTime.Now) {
                int year = uzavierka.UzavierkaDo.Value.Year * 100;
                currentObdobiRRRRMM = year + uzavierka.UzavierkaDo.Value.Month;
                if (this.CurrentObdobiRRRRMM == currentObdobiRRRRMM) {
                    DateTime date = uzavierka.UzavierkaDo.Value.AddMonths(-1);
                    year = date.Year * 100;
                    currentObdobiRRRRMM = year + date.Month;
                }
            }

			CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
			using (SqlConnection connection = tvdStorage.Connect())
			{
				string sql = @"";
				if (this.rbPrvniLinie.Checked)
				{
					sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,p.Narok_sleva_nrz ,p.Narok_eurokredit,
										o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc, o.E_mail,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
										PocetEuronaStars = ( SELECT ISNULL(SUM(Hvezdy_celkem),0) FROM provize_aktualni_angelteam WHERE Id_odberatele=o.Id_odberatele )
										FROM provize_aktualni p
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
										WHERE (o.Angel_team_clen=1 OR o.Angel_team_manager=1) AND o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele=@Id_odberatele OR p.Id_nadrizeneho=@Id_odberatele )
										AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
                    if (obdobi < currentObdobiRRRRMM)
					{
						sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
												p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
												p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
												p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
												p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
												o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc, o.E_mail,
												Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
												PocetEuronaStars = (SELECT ISNULL(SUM(Hvezdy_celkem),0) FROM provize_aktualni_angelteam WHERE Id_odberatele=o.Id_odberatele )
												FROM provize_finalni p
												INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
												WHERE (o.Angel_team_clen=1 OR o.Angel_team_manager=1) AND o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM AND ( p.Id_odberatele=@Id_odberatele OR p.Id_nadrizeneho=@Id_odberatele )
												AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND p.Hladina != 21))";
					}
				}
				else if (this.rbSkupina.Checked)
				{
					sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
										o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
										Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
										PocetEuronaStars = ( SELECT ISNULL(SUM(Hvezdy_celkem),0) FROM provize_aktualni_angelteam WHERE Id_odberatele=o.Id_odberatele )
										--FROM fGetOdberateleStrom(@Id_odberatele) f
                                        FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
										INNER JOIN provize_aktualni p ON p.Id_odberatele = f.Id_Odberatele
										INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
                                        LEFT JOIN odberatele op  ON op.Id_odberatele = o.cislo_nadrizeneho
                                        LEFT JOIN provize_aktualni pp  ON pp.Id_odberatele = op.Id_odberatele
										WHERE (o.Angel_team_clen=1 OR o.Angel_team_manager=1) AND o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM 
										AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND (p.Hladina != 21 OR pp.Hladina != 21) ))";
                    if (obdobi < currentObdobiRRRRMM)
					{
						sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem,
												p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
												p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
												p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
												p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber ,
												o.Kod_odberatele, o.Nazev_firmy, Telefon = (CASE WHEN LEN(o.Telefon)=0 THEN o.Mobil ELSE o.Telefon END), Adresa = (o.Ulice + ', ' + o.Misto + ', ' + o.Psc + ', ' + o.Stat), o.Psc,  o.E_mail,
												Hladina_rozdil = (SELECT Hladina FROM provize_aktualni WHERE Id_odberatele=@Id_odberatele AND RRRRMM=@RRRRMM ) - p.Hladina,
												PocetEuronaStars = ( SELECT ISNULL(SUM(Hvezdy_celkem),0) FROM provize_aktualni_angelteam WHERE Id_odberatele=o.Id_odberatele )
												--FROM fGetOdberateleStrom(@Id_odberatele) f
                                                FROM fGetOdberateleStrom(@Id_odberatele, @RRRRMM) f
												INNER JOIN provize_finalni p ON p.Id_odberatele = f.Id_Odberatele
												INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
                                                LEFT JOIN odberatele op  ON op.Id_odberatele = o.cislo_nadrizeneho
                                                LEFT JOIN provize_aktualni pp  ON pp.Id_odberatele = op.Id_odberatele
												WHERE (o.Angel_team_clen=1 OR o.Angel_team_manager=1) AND o.Stav_odberatele!='Z' AND p.RRRRMM=@RRRRMM
												AND (@Include21Pordce = 1 OR (@Include21Pordce = 0 AND (p.Hladina != 21 OR pp.Hladina != 21)))";
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