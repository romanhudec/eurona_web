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
		public partial class ArticleCategoriesPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ClassifiersControl<ArticleCategory> ctrl = new ClassifiersControl<ArticleCategory>();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "articleCategory.aspx";
						ctrl.EditUrlFormat = "articleCategory.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
