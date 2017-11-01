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
		public partial class UrlAliasPrefixesPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						AdminUrlAliasPrefixesControl ctrl = new AdminUrlAliasPrefixesControl();
						ctrl.CssClass = "dataGrid";
						ctrl.EditUrlFormat = "urlAliasPrefix.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
