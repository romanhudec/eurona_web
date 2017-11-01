using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Host
{
		public partial class CartPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.cartControl.OnCartItemsChanged += new EventHandler( cartControl_OnCartItemsChanged );
				}

				void cartControl_OnCartItemsChanged( object sender, EventArgs e )
				{
						( this.Page.Master as Eurona.User.Host.PageMasterPage ).UpdateCartInfo();
				}
		}
}
