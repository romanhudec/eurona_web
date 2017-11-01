using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHP.Controls.Classifiers;
using SHP.Entities.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class ShipmentPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ShipmentControl ctrl = new ShipmentControl();
						if ( !string.IsNullOrEmpty( Request["Id"] ) )
								ctrl.Id = Convert.ToInt32( Request["Id"] );

						this.divControls.Controls.Add( ctrl );
				}
		}
}
