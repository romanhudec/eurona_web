using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using System.Text;
using System.Collections;
using System.Web.Security;

namespace Eurona
{
		public partial class _Info: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						StringBuilder sb = new StringBuilder();
						if ( Security.IsLogged( false ) )
						{
								sb.AppendFormat( "Logged User.Id: {0}<br/>", Security.Account.Id );
								sb.AppendFormat( "Logged User.Login: {0}<br/>", Security.Account.Login );
								sb.AppendFormat( "Logged User.Locale: {0}<br/>", Security.Account.Locale );
								sb.AppendFormat( "Logged User.Roles: {0}<br/>", Security.Account.RoleString );

								sb.AppendFormat( "Context.User: {0}<br/>", Context.User.ToString() );
								sb.AppendFormat( "Context.User.Identity.Name: {0}<br/>", Context.User.Identity.Name );
								if ( HttpContext.Current.User.Identity is FormsIdentity )
								{
										FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
										FormsAuthenticationTicket ticket = id.Ticket;
										sb.AppendFormat( "FormsAuthenticationTicket.UserData: {0}<br/>", ticket.UserData );
								}
						}
						else
						{
								sb.Append( "No logged user!<br.>" );
						}
						sb.Append( "<hr/>" );
						foreach ( string ck in Request.Cookies.AllKeys )
						{
								HttpCookie c = Request.Cookies[ck];
								StringBuilder cookie = new StringBuilder();
								cookie.AppendFormat( "{0}={1}<br>", c.Name, c.Value );
								foreach ( string key in c.Values.AllKeys )
								{
										cookie.AppendFormat( "&nbsp;&nbsp;&nbsp;&nbsp;{0}={1}<br>", key, c.Values[key] );
								}
								cookie.AppendFormat( "Path={0}<br>", c.Path );
								cookie.AppendFormat( "Expires={0}<br>", c.Expires );
								sb.Append( "<br>" );
								sb.AppendLine( cookie.ToString() );
						}
						info.InnerHtml = sb.ToString();
				}
		}
}
