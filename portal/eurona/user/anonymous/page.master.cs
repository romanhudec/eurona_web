using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using CMS;
using CMS.Utilities;

namespace Eurona.User.Anonymous
{
	public partial class PageMasterPage : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Eurona.Common.Application.IsDebugVersion)
				this.debugVersion.Visible = true;

			if (Security.IsInRole(Role.ADMINISTRATOR))
			{
				MenuItem mi = new MenuItem(Resources.Strings.Navigation_Administration, "-100", string.Empty, Page.ResolveUrl("~/admin"));
				this.sitemenu.AddMenuItem(mi);
			}
			else if (Security.IsInRole(Role.OPERATOR))
			{
				MenuItem mi = new MenuItem(Resources.Strings.Navigation_Administration, "-100", string.Empty, Page.ResolveUrl("~/user/operator"));
				this.sitemenu.AddMenuItem(mi);
			}

			AliasUtilities aliasUtils = new AliasUtilities();
			string alias = aliasUtils.Resolve("~/eshop/pageFB.aspx?name=eshop-action-products", this.Page);
			if (string.IsNullOrEmpty(alias)) return;
			this.lblAkcniNabidky.HRef = alias;
		}
		/// <summary>
		/// Vrati aktualnu url
		/// </summary>
		public string CurrentUrl
		{
			get { return Session["CurrentUrl"] != null ? Session["CurrentUrl"].ToString() : string.Empty; }
			set { Session["CurrentUrl"] = value; }
		}

		private string root = String.Empty;
		protected string Root
		{
			get
			{
				if (!String.IsNullOrEmpty(root)) return root;
				root = Utilities.Root(Request);
				return root;
			}
		}


		public CMS.Controls.Menu.NavigationMenuControl SiteMenu
		{
			get { return this.sitemenu; }
		}

		/// <summary>
		/// Update informácie v nákupnom košiku.
		/// </summary>
		public void UpdateCartInfo()
		{
			this.cartInfoControl.UpdateControl(true);
		}

		protected override void OnInit(EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request.ServerVariables["http_user_agent"]))
			{
				if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
					Page.ClientTarget = "uplevel";
			}
			base.OnInit(e);
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			//Selec current page
			if (!IsPostBack) this.CurrentUrl = Request.RawUrl;
			for (int m = 0; m < this.sitemenu.MenuItemsCount; m++)
			{
				if (this.sitemenu[m].NavigateUrl != this.CurrentUrl) continue;
				this.sitemenu[m].Selected = true;
				break;
			}

			if (Security.IsLogged(false))
			{
				AliasUtilities aliasUtils = new AliasUtilities();
				string alias = aliasUtils.Resolve("~/login.aspx", this.Page);
				if (string.IsNullOrEmpty(alias)) return;

				MenuItem mi = new MenuItem(string.Format("{0}({1}) - {2}", Resources.Strings.LoginControl_Welcome, Security.Account.Login, Resources.Strings.LoginControl_LogoutButton));
				mi.ToolTip = Security.Account.Login;
				mi.NavigateUrl = Page.ResolveUrl("~/logout.aspx");
				mi.SeparatorImageUrl = Page.ResolveUrl(this.sitemenu.MenuItemSeparatorImageUrl);
				this.sitemenu.AddAt(0, mi);

				if (Security.IsInRole(Role.ADVISOR))
				{
					int? indexHostAccesss = null;
					string aliasAdvisor = aliasUtils.Resolve("~/user/advisor/default.aspx", this.Page);
					string aliasHostAccess = aliasUtils.Resolve("~/user/host/default.aspx", this.Page);
					for (int m = 0; m < this.sitemenu.MenuItemsCount; m++)
					{
						if (this.sitemenu[m].NavigateUrl == aliasAdvisor)
							this.sitemenu[m].Text = string.Format(Resources.Strings.Navigation_Office);

						if (this.sitemenu[m].NavigateUrl == aliasHostAccess)
							indexHostAccesss = m;
					}

					if (indexHostAccesss.HasValue) this.sitemenu.RemoveAt(indexHostAccesss.Value);
				}
			}
		}
	}
}