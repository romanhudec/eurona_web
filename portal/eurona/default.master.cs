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
using CMS.Utilities;

public partial class DefaultMasterPage : System.Web.UI.MasterPage {
    private string root = String.Empty;
    protected string Root {
        get {
            if (!String.IsNullOrEmpty(root)) return root;
            root = Utilities.Root(Request);
            return root;
        }
    }

    protected override void OnInit(EventArgs e) {
        if (!string.IsNullOrEmpty(Request.ServerVariables["http_user_agent"])) {
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
                Page.ClientTarget = "uplevel";
        }
        base.OnInit(e);
    }

    /// <summary>
    /// Vrati aktualnu url
    /// </summary>
    public string CurrentUrl {
        get { return Session["CurrentUrl"] != null ? Session["CurrentUrl"].ToString() : string.Empty; }
        set { Session["CurrentUrl"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        //Proccess Single user cookie link
        CookiesUtils.ProcessSingleUserCookiesLink(this.Page);

        if (Eurona.Common.Application.IsDebugVersion)
            this.debugVersion.Visible = true;

        if (Request.QueryString.ToString().Contains("login"))
            this.loginContainer.Visible = true;

        if (Security.IsLogged(false)) {
            if (Security.IsInRole(Role.ADMINISTRATOR)) {
                MenuItem mi = new MenuItem(Resources.Strings.Navigation_Administration, "-100", string.Empty, Page.ResolveUrl("~/admin"));
                this.sitemenu.AddMenuItem(mi);
            } else if (Security.IsInRole(Role.OPERATOR)) {
                MenuItem mi = new MenuItem(Resources.Strings.Navigation_Administration, "-100", string.Empty, Page.ResolveUrl("~/user/operator"));
                this.sitemenu.AddMenuItem(mi);
            }
        }

        string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
        if (Security.IsLogged(false)) locale = Security.Account.Locale;

        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "zopim", "$zopim(function () {$zopim.livechat.setLanguage('" + locale + "');});", true);
        //this.txtSearchKeywords.Attributes.Add( "onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + this.btnSearch.ClientID + "').click();return false;}} else {return true}; " );

        //Nastavenie spravnej meny do Session podla locale
        if (Session["SHP:Currency:Symbol"] == null) {
            List<SHP.Entities.Classifiers.Currency> currencies = Storage<SHP.Entities.Classifiers.Currency>.Read();
            foreach (SHP.Entities.Classifiers.Currency ce in currencies) {
                if (ce.Locale == locale) {
                    Session["SHP:Currency:Id"] = ce.Id;
                    Session["SHP:Currency:Rate"] = ce.Rate;
                    Session["SHP:Currency:Symbol"] = ce.Symbol;
                }
            }
        }
#if N__DEBUG_VERSION
        QueueItKnownUserIntegration.DoValidation(this.Page);
#endif
    }

    protected override void CreateChildControls() {
        base.CreateChildControls();

        //Selec current page
        if (!IsPostBack) this.CurrentUrl = Request.RawUrl;
        for (int m = 0; m < this.sitemenu.MenuItemsCount; m++) {
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

        if (Security.IsInRole(Role.ADVISOR)) {
            int? indexHostAccesss = null;
            string aliasAdvisor = aliasUtils.Resolve("~/user/advisor/default.aspx", this.Page);
            string aliasHostAccess = aliasUtils.Resolve("~/user/host/default.aspx", this.Page);
            for (int m = 0; m < this.sitemenu.MenuItemsCount; m++) {
                if (this.sitemenu[m].NavigateUrl == aliasAdvisor)
                    this.sitemenu[m].Text = string.Format(Resources.Strings.Navigation_Office);

                if (this.sitemenu[m].NavigateUrl == aliasHostAccess)
                    indexHostAccesss = m;
            }

            if (indexHostAccesss.HasValue) this.sitemenu.RemoveAt(indexHostAccesss.Value);
        }
    }

    protected void OnSwitchLocale(Object sender, EventArgs e) {
        ImageButton lb = sender as ImageButton;
        string locale = lb.CommandArgument;
        //TODO: disable locale switch
        if (Security.IsLogged(false) && (Security.Account.IsInRole(Role.OPERATOR) || Security.Account.IsInRole(Role.ADMINISTRATOR))) {
            Account account = Security.Account;
            account.Locale = locale;
            Storage<Account>.Update(account);
        }

        //Save to cooke
        if (ConfigurationManager.AppSettings["CookieLocaleName"] != null) {
            HttpCookie c = new HttpCookie(ConfigurationManager.AppSettings["CookieLocaleName"], locale);
            c.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(c);
        }

        Response.Redirect("~/default.aspx");
        Response.End();
    }

    protected void OnSearch(object sender, EventArgs e) {
        //if ( string.IsNullOrEmpty( this.txtSearchKeywords.Text ) ) return;
        //Page.Response.Redirect( string.Format( "~/search.aspx?keywords={0}", this.txtSearchKeywords.Text ) );
    }
}
