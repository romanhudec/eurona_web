using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class ImageGalleriesPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.imageGalleriesControl.TagId = null;

						if ( !string.IsNullOrEmpty( Request["tag"] ) )
								this.imageGalleriesControl.TagId = Convert.ToInt32( Request["tag"] );
				}
		}
}
