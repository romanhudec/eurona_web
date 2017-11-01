using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using CMS;
using CMS.Utilities;

using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

using NewsEntity = CMS.Entities.News;
using PollEntity = CMS.Entities.Poll;
using ArticleEntity = CMS.Entities.Article;

namespace Eurona.User.Advisor
{
	public partial class PageBKMasterPage : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Eurona.Common.Application.IsDebugVersion)
				this.debugVersion.Visible = true;

			Security.IsLogged(true);
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

			if (this.LogedAdvisor != null)
			{
				this.hlGenerateRegisterForm.NavigateUrl = this.ResolveUrl(string.Format("~/user/advisor/registerDocument.aspx?id={0}", this.LogedAdvisor.Id));

				string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
				if (locale == "pl") this.hlGenerateRegisterForm.NavigateUrl = this.ResolveUrl(string.Format("~/user/advisor/registerDocumentPL.aspx?id={0}", this.LogedAdvisor.Id));

			}

			List<NewsEntity> listNews = Storage<NewsEntity>.Read();
			this.newsLink.Visible = listNews.Count != 0;
			List<PollEntity> listPool = Storage<PollEntity>.Read();
			this.poolsLink.Visible = listPool.Count != 0;
			List<ArticleEntity> listArticle = Storage<ArticleEntity>.Read();
			this.articlesLink.Visible = listArticle.Count != 0;

			//Podpora predaja
			AliasUtilities util = new AliasUtilities();
			string alias = util.Resolve("~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-uspesny-start", this.Page);
			this.hlUspesnyStart.HRef = alias;

			alias = util.Resolve("~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-akcni-cennik", this.Page);
			this.hlAkcnyCennik.HRef = alias;

			alias = util.Resolve("~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-eurona-news", this.Page);
			this.hlEuronaNews.HRef = alias;

			alias = util.Resolve("~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-prezentacni-letaky", this.Page);
			this.hlPrezentacniLetaky.HRef = alias;

			alias = util.Resolve("~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-vzdelavani", this.Page);
			this.hlVzdelavani.HRef = alias;
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

		private OrganizationEntity logedAdvisor = null;
		public OrganizationEntity LogedAdvisor
		{
			get
			{
				if (logedAdvisor != null) return logedAdvisor;
				logedAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
				return logedAdvisor;
			}
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

			if (!Security.IsLogged(false)) return;

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

			this.linkAccount.HRef = String.Format(this.linkAccount.HRef, Security.Account.Id);
			this.linkChangePassword.HRef = String.Format(this.linkChangePassword.HRef, Security.Account.Id);

			if (this.LogedAdvisor != null && !logedAdvisor.Account.Verified)
			{
				Literal lblMessage = new Literal();
				lblMessage.Text = string.Format("{0}&nbsp;<span style='color:#f00;font-weight:bold;font-size:16px;'>{1} {2}</span>", Resources.Strings.NotVerifiedTimeSpanUserMessage, logedAdvisor.LeftToVerification, Resources.Strings.Days);
				this.divNotVerifiedUserMessage.Controls.Add(lblMessage);
			}
		}
	}
}