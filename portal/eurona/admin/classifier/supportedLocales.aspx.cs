using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Admin.Controls;
using CMS.Entities.Classifiers;

namespace Eurona.Admin.Classifier
{
		public partial class SupportedLocalesPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						IconClassifiersControl<SupportedLocale> ctrl = new IconClassifiersControl<SupportedLocale>();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "supportedLocale.aspx";
						ctrl.EditUrlFormat = "supportedLocale.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
