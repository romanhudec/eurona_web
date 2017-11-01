using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
		public partial class AdminNavigationMenuItem: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["menuId"] ) )
								this.navigationMenuItem.NavigationMenuId = Convert.ToInt32( Request["menuId"] );
				}
		}
}
