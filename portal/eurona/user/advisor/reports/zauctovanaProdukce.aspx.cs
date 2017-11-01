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
		public partial class ZauctovanaProdukceReport: ReportPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.DisableObdobi = true;
						if ( this.ForAdvisor == null ) return;
						if ( this.ForAdvisor.TVD_Id == null ) return;

						//Ak nie je vierihodny - len seba
						if ( this.ForAdvisor.RestrictedAccess == 1 )
						{
								this.txtAdvisorCode.Enabled = false;
						}

						if ( !IsPostBack ) this.txtAdvisorCode.Text = this.ForAdvisor.Code;

						GridViewDataBind( !IsPostBack );
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
						if ( !string.IsNullOrEmpty( this.txtAdvisorCode.Text ) )
						{
								if ( this.txtAdvisorCode.Text != this.ForAdvisor.Code )
								{
										int? obdobi = null;
										object filter = GetFilter();
										if ( filter != null ) obdobi = (int)filter;

										OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst( new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text } );
										if ( forAdvisor != null ) Response.Redirect( Page.ResolveUrl( string.Format( "~/user/advisor/reports/nezauctovanaProdukce.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi ) ) );
								}
						}

						GridViewDataBind( true );
				}

				private void GridViewDataBind( bool bind )
				{
						int rok = DateTime.Now.Year;
						int mesic = DateTime.Now.Month;
						int den = DateTime.Now.Day;

						CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage( base.ConnectionString );
						using ( SqlConnection connection = tvdStorage.Connect() )
						{
								string sql = @"SELECT objem_pro_marzi = SUM( fr.zapocet_mj_marze*fr.mnozstvi), cena_mj_katalogova=SUM(fr.cena_mj_katalogova*mnozstvi), f.kod_meny,
								p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,
										Objem_vlastni=SUM( fr.zapocet_mj_provize_czk*fr.mnozstvi),
										Body_vlastni=SUM(fr.zapocet_mj_body*fr.mnozstvi), 
										p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy, Stav='Zpracováva se' 
								FROM fGetOdberateleStrom(@Id_odberatele) fo
								INNER JOIN www_faktury f ON f.Id_odberatele = fo.Id_odberatele
								INNER JOIN odberatele o  ON o.Id_odberatele = f.Id_odberatele
								INNER JOIN provize_aktualni p ON p.Id_odberatele = o.Id_odberatele
								INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
								WHERE f.cislo_objednavky_eurosap IS NOT NULL 
								AND f.cislo_objednavky_eurosap IN (select Cislo_objednavky from provize_aktualni_objednavky )
								AND f.cislo_objednavky_eurosap NOT IN (select id_objednavky from objednavkyfaktury where StavK2=0 ) -- objednavka nesmie byt stornovana
								AND YEAR(f.datum_vystaveni)=@rok AND MONTH(f.datum_vystaveni)=@mesic AND DAY(f.datum_vystaveni)=@den AND p.RRRRMM = (@rok*100+@mesic)
								GROUP BY fo.LineageId, f.kod_meny,p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Body_vlastni, p.Vnoreni, o.Kod_odberatele, o.Nazev_firmy 
								ORDER BY fo.LineageId ASC";
								//Clear data
								DataTable dt = tvdStorage.Query( connection, sql,
										new SqlParameter( "@rok", rok ),
										new SqlParameter( "@mesic", mesic ),
										new SqlParameter( "@den", den ),
										new SqlParameter( "@Id_odberatele", this.ForAdvisor.TVD_Id ) );


								this.gridView.DataSource = dt;
						}

						if ( bind ) gridView.DataBind();
				}
		}
}