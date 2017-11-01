using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Eurona.DAL.Entities;

namespace Eurona.User.Advisor
{
		public partial class RegisterDocument: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( Request["id"] ) ) return;

						int organizationId = Convert.ToInt32( Request["id"] );
						Organization org = Storage<Organization>.ReadFirst( new Organization.ReadById { OrganizationId = organizationId } );
						if ( org == null ) return;

						Organization parentOrg = null;
						if ( org.ParentId.HasValue ) parentOrg = Storage<Organization>.ReadFirst( new Organization.ReadByTVDId { TVD_Id = org.ParentId.Value } );

						Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = org.AccountId.Value } );
						if ( account == null ) return;

						this.lblJmenoSponzora.Text = "&nbsp;";
						this.lblRegistracniCisloSponzora.Text = "&nbsp;";

						this.lblAdresaDorucovaci.Text = string.Format( "{0}, {1}", org.CorrespondenceAddress.Street, org.CorrespondenceAddress.City ) + "&nbsp;";
						this.lblAdresaSidla.Text = string.Format( "{0}, {1}", org.RegisteredAddress.Street, org.RegisteredAddress.City ) + "&nbsp;";
						this.lblBanka.Text = org.BankContact.BankName + " " + org.BankContact.BankCode + "&nbsp;";
						this.lblCisloUctu.Text = org.BankContact.AccountNumber + "&nbsp;";
						this.lblDatumNarozeni.Text = org.ContactBirthDay.HasValue ? org.ContactBirthDay.Value.ToShortDateString() : "&nbsp;";
						this.lblDatumRegistrace.Text = account.Created.ToShortDateString() + "&nbsp;";
						this.lblDIC.Text = org.Id2 + "&nbsp;";
						this.lblEmail.Text = org.ContactEmail + "&nbsp;";
						this.lblICO.Text = org.Id1 + "&nbsp;";
						this.lblJmeno.Text = org.ContactPersonString + "&nbsp;";
						if ( parentOrg != null )
						{
								this.lblJmenoSponzora.Text = parentOrg.Name + "&nbsp;";
								this.lblRegistracniCisloSponzora.Text = parentOrg.Code + "&nbsp;";
						}
						this.lblMobilniTelefon.Text = org.ContactMobile + "&nbsp;";
						this.lblNazev.Text = org.Name + "&nbsp;";
						this.lblPrijmeni.Text = "&nbsp;";
						this.lblPSCDorucovaci.Text = org.CorrespondenceAddress.Zip + "&nbsp;";
						this.lblPSCSidlo.Text = org.RegisteredAddress.Zip + "&nbsp;";
						this.lblRegistracniCislo.Text = org.Code + "&nbsp;";
						this.lblTelefonDomu.Text = org.ContactPhone + "&nbsp;";
						this.lblTelefonDoZamestnani.Text = org.ContactWorkPhone + "&nbsp;";
						this.lblTitul.Text = "&nbsp;";

						this.DataBind();
				}
		}
}