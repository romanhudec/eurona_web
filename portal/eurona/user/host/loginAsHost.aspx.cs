using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Utilities;
using Eurona.DAL.Entities;
using System.Data;
using Telerik.Web.UI;
using Eurona.Common.DAL.Entities;

namespace Eurona.User.Host
{
		public partial class LoginAsHost: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
				}

				protected void OnLogin( object sender, EventArgs e )
				{
						Organization advisor = Storage<Organization>.ReadFirst( new Organization.ReadByCode { Code = this.txtAdvisorCode.Text } );
						if ( advisor != null )
						{
								Session[HostSecurity.HostNameSessionName] = this.txtName.Text;
								Session[HostSecurity.HostAdvisorCodeSessionName] = advisor.Code;

								AliasUtilities util = new AliasUtilities();
								string alias = util.Resolve( "~/user/host/default.aspx", this );
								if ( string.IsNullOrEmpty( alias ) ) Response.Redirect( ResolveUrl( "~/user/host/default.aspx" ) );
								else Response.Redirect( alias );
						}
				}
		}
}
