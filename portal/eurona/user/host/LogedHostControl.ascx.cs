using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvisorOrganization = Eurona.Common.DAL.Entities.Organization;
using Eurona.User.Host;

namespace Eurona.User.Host
{
		public partial class LogedHostControl: System.Web.UI.UserControl
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.logedUserTable.Visible = false;
						this.notLogedUserTable.Visible = false;

						if ( this.Advisor != null ) this.logedUserTable.Visible = true;
						else this.notLogedUserTable.Visible = true;
				}

				private AdvisorOrganization advisor = null;
				public AdvisorOrganization Advisor
				{
						get
						{
								if ( advisor != null ) return advisor;

								if ( Page.Session[HostSecurity.HostAdvisorCodeSessionName] == null )
										return null;

								advisor = Storage<AdvisorOrganization>.ReadFirst( new AdvisorOrganization.ReadByCode { Code = this.Session[HostSecurity.HostAdvisorCodeSessionName].ToString() } );
								if ( advisor == null ) advisor = new AdvisorOrganization();
								return advisor;
						}
				}
				protected void OnLogoutClick( object sender, EventArgs e )
				{
						Page.Session[HostSecurity.HostAdvisorCodeSessionName] = null;
						Security.Logout();
						Response.Redirect( "~/" );
				}
				protected void OnYes( object sender, EventArgs e )
				{
						Response.Redirect( Page.ResolveUrl( "~/user/host/loginAsHost.aspx" ) );
				}
				protected void OnNo( object sender, EventArgs e )
				{
						Response.Redirect( Page.ResolveUrl( "~/user/host/loginAnonymous.aspx" ) );
				}
		}
}