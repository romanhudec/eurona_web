using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Eurona;
using Eurona.DAL.Entities;
using System.Globalization;
using System.Threading;
using System.Configuration;
using PageEntity = CMS.Entities.Page;
using CMS.Entities;
using CMS.Utilities;

namespace Eurona.EShop
{
	public partial class DefautMasterPage : System.Web.UI.MasterPage
	{

		private PageEntity pageEntity;
		private PageEntity PageEntity
		{
			get
			{
				if (pageEntity != null) return pageEntity;
				string name = Server.UrlDecode(Request["name"]);
				pageEntity = Storage<PageEntity>.ReadFirst(new PageEntity.ReadByName { Name = "eshop-products-master" });
				return pageEntity;
			}
		}

		/// <summary>
		/// Update informácie v nákupnom košiku.
		/// </summary>
		public void UpdateCartInfo()
		{
			(this.Master as Page3ContentMasterPage).UpdateCartInfo();
		}

		public object SelectedCategory
		{
			get { return this.categoryNavigation.SelectedValue; }
			set { this.categoryNavigation.SelectedValue = value; }
		}
		public int? SelectedProduct
		{
			get { return this.categoryNavigationPathControl.ProductId; }
			set { this.categoryNavigationPathControl.ProductId = value; }
		}
		protected void Page_PreInit(object sender, EventArgs e)
		{
			if (this.PageEntity != null)
			{
				this.Page.MasterPageFile = this.PageEntity.MasterPage.Url;
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			//this.menu.SelectMenuItem( this.Request.RawUrl );
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.txtSearchKeywords.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + this.btnSearch.ClientID + "').click();return false;}} else {return true}; ");

			if (this.PageEntity != null)
			{
				//this.Title = this.PageEntity.Title;
				List<PageEntity> list = this.PageEntity.ChildPages;
				if (list.Count != 0) genericPage1.PageName = list[0].Name;
				if (list.Count > 1) genericPage2.PageName = list[1].Name;
				if (list.Count > 2) genericPage3.PageName = list[2].Name;
				aThisPage.Text = this.PageEntity.Title;
			}

			if (!IsPostBack && !string.IsNullOrEmpty(Request["keywords"]))
				this.txtSearchKeywords.Text = Request["keywords"];

			if (this.SelectedCategory != null)
				this.categoryNavigationPathControl.CategoryId = Convert.ToInt32(this.SelectedCategory);
		}

		protected void OnSearch(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtSearchKeywords.Text)) return;
			Page.Response.Redirect(string.Format("~/eshop/search.aspx?keywords={0}", this.txtSearchKeywords.Text));
		}
	}
}
