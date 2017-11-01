using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop.Admin
{
		public partial class Product: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminProduct.ProductId = Convert.ToInt32( this.Request["id"] );
				}
		}
}
