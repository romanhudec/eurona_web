using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using PageEntity = CMS.Entities.Page;
using CMS.Entities;
using System.Configuration;

namespace Eurona
{
		public partial class NotFound: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						string page = Request["page"];
						message.InnerText = String.Format( "Page '{0}' not found!", page );
						List<PageEntity> available = Storage<PageEntity>.Read( new PageEntity.ReadByName { Name = page } );
						pages.DataSource = available;
						pages.DataBind();
				}

				protected void OnSwitchLocale( Object sender, EventArgs e )
				{
						LinkButton lb = sender as LinkButton;
						string locale = lb.CommandArgument;
						if ( Security.IsLogged( false ) )
						{
								Account account = Security.Account;
								account.Locale = locale;
								Storage<Account>.Update( account );
						}

						if ( ConfigurationManager.AppSettings["CookieLocaleName"] != null )
						{
								HttpCookie c = new HttpCookie( ConfigurationManager.AppSettings["CookieLocaleName"], locale );
								c.Expires = DateTime.Now.AddYears( 1 );
								Response.Cookies.Add( c );
						}

						Response.Redirect( String.Format( "~/page.aspx?name=" + Request["page"] ) );
						Response.End();
				}

		}
}
