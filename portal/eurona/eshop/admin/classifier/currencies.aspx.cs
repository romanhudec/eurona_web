using System;
using Eurona.Controls.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class CurrenciesPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						CurrenciesControl ctrl = new CurrenciesControl();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "currency.aspx";
						ctrl.EditUrlFormat = "currency.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
