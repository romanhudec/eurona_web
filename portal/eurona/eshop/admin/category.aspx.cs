using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop.Admin
{
		public partial class Category: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminCategory.CategoryId = Convert.ToInt32( this.Request["id"] );
						if ( !string.IsNullOrEmpty( this.Request["parent"] ) )
								this.adminCategory.ParentId = Convert.ToInt32( this.Request["parent"] );
				}
		}
}
