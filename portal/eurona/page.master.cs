using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using CMS;
using CMS.Utilities;

namespace Eurona
{
	public partial class PageMasterPage : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AliasUtilities aliasUtils = new AliasUtilities();
			string alias = aliasUtils.Resolve("~/eshop/pageFB.aspx?name=eshop-action-products", this.Page);
			if (string.IsNullOrEmpty(alias)) return;
			this.lblAkcniNabidky.HRef = alias;
		}

		protected override void RenderChildren(HtmlTextWriter writer)
		{
			//rpPollControl.Visible = pollControl.Visible;
			//rpLatestNews.Visible = latestNewsControl.Visible;
			//rpLatestArticles.Visible = latestArticlesControl.Visible;
			base.RenderChildren(writer);
		}

		/// <summary>
		/// Update informácie v nákupnom košiku.
		/// </summary>
		public void UpdateCartInfo()
		{
			this.cartInfoControl.UpdateControl(true);
		}

		public void HideAkcniNabidky()
		{
			this.trAkcniNabidky.Visible = false;
		}
		public void HideVanoce()
		{
			this.trVanoce.Visible = false;
		}
        public void HideNovinky()
        {
            this.trNovinky.Visible = false;
        }
		public void HideCart()
		{
			this.cartInfoControl.Visible = false;
		}
	}
}
