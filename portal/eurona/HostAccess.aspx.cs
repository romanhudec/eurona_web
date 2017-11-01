using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class HostAccessPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.Title = this.genericPage.Title;
						this.thisPage.Text = this.genericPage.Title;

						Security.IsLogged( true );
				}
		}
}
