using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop.Admin
{
		public partial class Categories: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( IsPostBack ) return;
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.adminCategories.ParentId = Convert.ToInt32( this.Request["id"] );
								this.adminCategories.NewUrl=string.Format("~/eshop/admin/category.aspx?parent={0}", this.adminCategories.ParentId);
						}
				}
		}
}
