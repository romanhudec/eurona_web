using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;

namespace Eurona.Admin
{
		public partial class AdminNavigationMenuItems: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["id"] ) )
						{
								int navigationMenuId = Convert.ToInt32( Request["id"] );

								NavigationMenu navMenu = Storage<NavigationMenu>.ReadFirst( new NavigationMenu.ReadById { NavigationMenuId = navigationMenuId } );
								this.lblNavigationMenu.Text = navMenu.Name;
								this.navigationMenuItems.NavigationMenuId = navigationMenuId;
						}
				}
		}
}
