using System;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Threading;

namespace CMS.HttpModules
{
		public class UrlAliasModule: IHttpModule
		{
				/// <summary>
				/// You will need to configure this module in the web.config file of your
				/// web and register it with IIS before being able to use it. For more information
				/// see the following link: http://go.microsoft.com/?linkid=8101007
				/// </summary>
				#region IHttpModule Members

				public void Dispose()
				{
						//clean-up code here.
				}

				public void Init( HttpApplication context )
				{
						context.BeginRequest += OnBeginRequest;
				}

				void OnBeginRequest( object sender, EventArgs e )
				{
						HttpApplication app = (HttpApplication)sender;

						string path = app.Context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
						if ( ( path.IndexOf( ".aspx" ) > 0 ) || ( path.IndexOf( ".css" ) > 0 ) ||
								 ( path.IndexOf( ".js" ) > 0 ) || ( path.IndexOf( ".axd" ) > 0 ) ||
								 ( path.IndexOf( ".png" ) > 0 ) || ( path.IndexOf( ".jpg" ) > 0 ) ||
								 ( path.IndexOf( ".gif" ) > 0 ) )
								return;

						// Get Current culture
						string culture = this.GetCurrentCultrure( app );
						CMS.Entities.UrlAlias alias = Storage<CMS.Entities.UrlAlias>.ReadFirst( new CMS.Entities.UrlAlias.ReadByLocaleAlias { Alias = path, Locale = culture } );
						if ( alias == null ) alias = Storage<CMS.Entities.UrlAlias>.ReadFirst( new CMS.Entities.UrlAlias.ReadByLocaleAlias { Alias = path, Locale = ConfigurationManager.AppSettings["DefaultLocale"] } );
						if ( alias == null ) alias = Storage<CMS.Entities.UrlAlias>.ReadFirst( new CMS.Entities.UrlAlias.ReadByLocaleAlias { Alias = path, Locale = null } );
						if ( alias != null )
						{
								//If is not local path redirect direct to url.
								string url = alias.Url.ToLower();
								if ( url.StartsWith( "http" ) )
								{
										app.Context.Response.Redirect( alias.Url );
										return;
								}

								//Change locale
								if ( alias.Locale != culture )
								{
										//Save to cooke
										if ( ConfigurationManager.AppSettings["CookieLocaleName"] != null )
										{
												HttpCookie c = new HttpCookie( ConfigurationManager.AppSettings["CookieLocaleName"], alias.Locale );
												c.Expires = DateTime.Now.AddYears( 1 );
												app.Context.Response.Cookies.Add( c );
										}

										// APPLY language
										CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture( alias.Locale );
										Thread.CurrentThread.CurrentCulture = cultureInfo;
										Thread.CurrentThread.CurrentUICulture = cultureInfo;
										
										//Redirect to alias with requested locale
										app.Context.Response.Redirect( alias.Alias );
										return;

								}

								string query = app.Request.Url.Query;
								if ( query.Length == 0 )
								{
										app.Context.RewritePath( alias.Url, false );
										return;
								}

								url = alias.Url;
								query = query.Remove( 0, 1 );//Odtranenie ?alebo&
								if ( url.IndexOf( "?" ) > 0 ) url += "&" + query;
								else url += "?" + query;
								app.Context.RewritePath( url, false );
						}
				}

				#endregion

				protected virtual string GetCurrentCultrure( HttpApplication app )
				{
						// 1. predefined web language
						string culture = ConfigurationManager.AppSettings["DefaultLocale"];
						string cookieLocaleName = ConfigurationManager.AppSettings["CookieLocaleName"];

						// 2. cookies language
						if ( app.Request.Cookies[cookieLocaleName] != null )
								culture = app.Request.Cookies[cookieLocaleName].Value;

						// 3. user language
						if ( Security.IsLogged( false ) )
								culture = Security.Account.Locale;

						return culture;
				}
		}
}
