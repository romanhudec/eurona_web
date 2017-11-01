using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHP.Entities.Classifiers;
using Eurona.Controls.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class Highlights: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ClassifiersControl<Highlight> ctrl = new ClassifiersControl<Highlight>();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "highlight.aspx";
						ctrl.EditUrlFormat = "highlight.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
