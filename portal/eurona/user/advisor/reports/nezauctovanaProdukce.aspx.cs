using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using OrderEntity = Eurona.DAL.Entities.Order;
using AccountEntity = Eurona.DAL.Entities.Account;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Telerik.Web.UI;

namespace Eurona.User.Advisor.Reports
{
	public partial class NezauctovanaProdukceReport : ReportPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.ForAdvisor == null) return;
			if (this.ForAdvisor.TVD_Id == null) return;

			//Ak nie je vierihodny - len seba
			if (this.ForAdvisor.RestrictedAccess == 1)
			{
				this.txtAdvisorCode.Enabled = false;
			}

			if (!IsPostBack) this.txtAdvisorCode.Text = this.ForAdvisor.Code;

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
			if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text))
			{
				if (this.txtAdvisorCode.Text != this.ForAdvisor.Code)
				{
					int? obdobi = null;
					object filter = GetFilter();
					if (filter != null) obdobi = (int)filter;

					OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
					if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/nezauctovanaProdukce.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
				}
			}

			GridViewDataBind(true);
		}

		private void GridViewDataBind(bool bind)
		{
			int? obdobi = null;
			object filter = GetFilter();
			if (filter != null) obdobi = (int)filter;
			if (!obdobi.HasValue || obdobi.ToString().Length != 6) return;

			int rok = Convert.ToInt32(obdobi.ToString().Substring(0, 4));
			int mesic = obdobi.Value - rok * 100;

			CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(base.ConnectionString);
			using (SqlConnection connection = tvdStorage.Connect())
			{
				string sql = @"SELECT objem_pro_marzi = SUM( fr.zapocet_mj_marze*fr.mnozstvi), cena_mj_katalogova=SUM(fr.cena_mj_katalogova*mnozstvi), f.kod_meny,
										p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,
										Objem_vlastni=SUM( fr.zapocet_mj_provize_czk*fr.mnozstvi),
										Body_vlastni=SUM(fr.zapocet_mj_body*fr.mnozstvi), 
										p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy, Stav='Zpracováva se' 
								FROM www_faktury f
								INNER JOIN odberatele o  ON o.Id_odberatele = f.Id_odberatele
								INNER JOIN provize_aktualni p ON p.Id_odberatele = o.Id_odberatele
								INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
								WHERE f.cislo_objednavky_eurosap IS NOT NULL 
								AND f.cislo_objednavky_eurosap NOT IN (select Cislo_objednavky from provize_aktualni_objednavky ) -- objednavka nesmie byt zauctovana
								AND f.cislo_objednavky_eurosap NOT IN (select id_objednavky from objednavkyfaktury where StavK2=0 ) -- objednavka nesmie byt stornovana
								AND YEAR(f.datum_vystaveni_objednavky)=@rok AND MONTH(f.datum_vystaveni_objednavky)=@mesic AND p.RRRRMM = (@rok*100+@mesic)
								AND f.id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele,(@rok*100+@mesic) )) 
								GROUP BY f.kod_meny,p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Body_vlastni, p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy 
								ORDER BY p.Vnoreni";
				//Clear data
				DataTable dt = tvdStorage.Query(connection, sql,
						new SqlParameter("@rok", rok),
						new SqlParameter("@mesic", mesic),
						new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));


				// rozpracovane objednavky
				string sql2 = @"SELECT CartId=f.id_prepoctu, objem_pro_marzi = SUM( fr.zapocet_mj_marze*fr.mnozstvi), cena_mj_katalogova=SUM(fr.cena_mj_katalogova*mnozstvi), f.kod_meny,
								p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,
										Objem_vlastni=SUM( fr.zapocet_mj_provize_czk*fr.mnozstvi),
										Body_vlastni=SUM(fr.zapocet_mj_body*fr.mnozstvi), 
										p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy, Stav='Čeká na zpracování'  
								FROM www_faktury f
								INNER JOIN odberatele o  ON o.Id_odberatele = f.Id_odberatele
								INNER JOIN provize_aktualni p ON p.Id_odberatele = o.Id_odberatele
								INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
								WHERE f.id_prepoctu < 0 AND YEAR(f.datum_vystaveni_objednavky)=@rok AND MONTH(f.datum_vystaveni_objednavky)=@mesic AND p.RRRRMM = (@rok*100+@mesic)
								AND f.id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, (@rok*100+@mesic))) 
								GROUP BY f.id_prepoctu, f.kod_meny,p.RRRRMM, p.Id_odberatele, p.Id_nadrizeneho,p.Objem_vlastni,p.Body_vlastni, p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy 
								ORDER BY p.Vnoreni";
				//Clear data
				DataTable dtCNS = tvdStorage.Query(connection, sql2,
						new SqlParameter("@rok", rok),
						new SqlParameter("@mesic", mesic),
						new SqlParameter("@Id_odberatele", this.ForAdvisor.TVD_Id));

				List<OrderEntity> orders = Storage<OrderEntity>.Read(new OrderEntity.ReadByFilter { OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString() });

				foreach (OrderEntity order in orders)
				{
					DataRow[] rows = dtCNS.Select(string.Format("CartId={0}", (-1 * order.CartId)));

					foreach (DataRow row in rows)
					{
						//objem_pro_marzi , cena_mj_katalogova,kod_meny, RRRRMM,Id_odberatele,Id_nadrizeneho,Objem_vlastni,Body_vlastni, Vnoreni, Kod_odberatele, Nazev_firmy 
						dt.Rows.Add(new object[] { row["objem_pro_marzi"], row["cena_mj_katalogova"], row["kod_meny"], row["RRRRMM"], row["Id_odberatele"], row["Id_nadrizeneho"], row["Objem_vlastni"], row["Body_vlastni"], row["Vnoreni"], row["Kod_odberatele"], row["Nazev_firmy"], row["Stav"] });
					}
				}

				DataView dv = dt.DefaultView;
				dv.Sort = "Vnoreni ASC";
				this.gridView.DataSource = dv;
			}

			if (bind) gridView.DataBind();
		}
	}
}