using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop.Admin
{
		public partial class Attribute: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminAttribute.AttributeId = Convert.ToInt32( this.Request["id"] );
						if ( !string.IsNullOrEmpty( this.Request["category"] ) )
								this.adminAttribute.CategoryId = Convert.ToInt32( this.Request["category"] );
				}
		}
}
