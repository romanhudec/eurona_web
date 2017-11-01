using System;
using Eurona.Controls.Classifiers;
using SHP.Entities.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class Payments: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ClassifiersControl<Payment> ctrl = new ClassifiersControl<Payment>();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "payment.aspx";
						ctrl.EditUrlFormat = "payment.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
