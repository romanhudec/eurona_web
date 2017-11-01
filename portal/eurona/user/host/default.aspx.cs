using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.User.Host;

namespace Eurona.User.Host
{
		public partial class DefaultPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						//if ( !HostSecurity.IsAutenticated( this ) )
						//    Response.Redirect( ResolveUrl( "~/user/host/login.aspx" ) ); 

						this.Title = this.genericPage.Title;
						this.thisPage.Text = this.genericPage.Title;
				}
		}
}
