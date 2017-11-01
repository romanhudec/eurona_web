<%@ Application Language="C#" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="CMS" %>
<%@ Import Namespace="Eurona.DAL.Entities" %>

<script RunAt="server">

    public static readonly string COOKIE_ROLES = "mothiva.cms.Eurona";

    protected static bool IsSupportedLocale( string locale )
    {
        string[] supported = new string[] { "sk", "cs", "en", "de", "pl" };
        return supported.Contains( locale );
    }

    protected string GetCurrentCultrure()
    {
        // 1. predefined web language
        string culture = ConfigurationManager.AppSettings["DefaultLocale"];

        string cookieLocaleName = ConfigurationManager.AppSettings["CookieLocaleName"];

        // 2. cookies language
        if ( Request.Cookies[cookieLocaleName] != null )
            culture = Request.Cookies[cookieLocaleName].Value;

        // 5. user language
        if ( Security.IsLogged( false ) )
            culture = Security.Account.Locale;

        if ( !string.IsNullOrEmpty( culture ) && Security.IsLogged( false ) )
            Security.Account.Locale = culture;
            
        return culture;
    }

    protected void Application_Start( object sender, EventArgs e )
    {
        CMS.Security.COOKIE_ROLES = COOKIE_ROLES;
    }

    void Application_AuthenticateRequest( object sender, EventArgs e )
    {
        if ( this.Request.IsAuthenticated )
        {
            Account account = Eurona.Storage<Account>.ReadFirst( new Account.ReadByLogin { Login = this.User.Identity.Name } );
            if ( account == null )
            {
                Security.Logout();
                return;
            }
            if ( ( Request.Cookies[COOKIE_ROLES] == null ) || ( Request.Cookies[COOKIE_ROLES].Value == "" ) )
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                  1,                              // version
                  Context.User.Identity.Name,     // user name
                  DateTime.Now,                   // issue time
                  DateTime.Now.AddHours( 1 ),       // expires every hour
                  true,                           // don't persist cookie
                  account.RoleString              // roles
                  );
                String cookieStr = FormsAuthentication.Encrypt( ticket );
                Response.Cookies[COOKIE_ROLES].Value = cookieStr;
                Response.Cookies[COOKIE_ROLES].Path = "/";
                Response.Cookies[COOKIE_ROLES].Expires = DateTime.Now.AddHours( 1 );
            }
            else
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt( Context.Request.Cookies[COOKIE_ROLES].Value );
                //account.RoleString = ticket.UserData;
            }
            Context.User = new GenericPrincipal( Context.User.Identity, account.RoleArray );
        }
    }

    void Application_AcquireRequestState( Object sender, EventArgs e )
    {
        // Get Current culture
        string culture = GetCurrentCultrure();

        // APPLY language
        CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture( culture );
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    protected void Application_BeginRequest( object sender, EventArgs e )
    {
    }


    protected void Application_Error(object sender, EventArgs e) {
        if (Context.IsCustomErrorEnabled)
            ShowCustomErrorPage(Server.GetLastError());
    }

    private void ShowCustomErrorPage(Exception exception) {
        HttpException httpException = exception as HttpException;
        if (httpException == null)
            httpException = new HttpException(500, "Internal Server Error", exception);
        
        Response.Clear();
        Response.Redirect(String.Format("~/error.aspx?code={0}", httpException.GetHttpCode()));
        Server.ClearError();
    }

      
</script>

