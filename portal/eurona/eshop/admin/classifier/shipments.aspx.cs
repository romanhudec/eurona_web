using System;
using SHP.Controls.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class Shipments: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ShipmentsControl ctrl = new ShipmentsControl();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "shipment.aspx";
						ctrl.EditUrlFormat = "shipment.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
