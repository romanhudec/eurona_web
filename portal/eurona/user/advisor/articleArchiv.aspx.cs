using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor
{
	public partial class ArticleArchivPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.archivedArticlesControl.CategoryId = null;
			this.archivedArticlesControl.TagId = null;

			if (!string.IsNullOrEmpty(Request["category"]))
				this.archivedArticlesControl.CategoryId = Convert.ToInt32(Request["category"]);

			if (!string.IsNullOrEmpty(Request["tag"]))
				this.archivedArticlesControl.TagId = Convert.ToInt32(Request["tag"]);
		}
	}
}
