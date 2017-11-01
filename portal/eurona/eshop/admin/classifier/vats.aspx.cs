using System;
using SHP.Controls.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class VATsPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						VATsControl ctrl = new VATsControl();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "vat.aspx";
						ctrl.EditUrlFormat = "vat.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
