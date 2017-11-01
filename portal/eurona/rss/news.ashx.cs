using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;

namespace Eurona.RSS
{
		/// <summary>
		/// Summary description for $News$
		/// </summary>
		//[WebService( Namespace = "http://X/" )]
		[WebServiceBinding( ConformsTo = WsiProfiles.None )]
		public class News: IHttpHandler
		{

				public void ProcessRequest( HttpContext context )
				{
						context.Response.ContentType = "text/xml";
						context.Response.ContentEncoding = Encoding.UTF8;

						CMS.Controls.News.RSS.Link = Utilities.Root( context.Request );
						CMS.Controls.News.RSS.Title = Resources.Strings.RSS_NewsTitle;
						CMS.Controls.News.RSS.ItemFormatUrl = "../news.aspx?id={0}";
						CMS.Controls.News.RSS.Generate( context.Response.OutputStream );
				}

				public bool IsReusable
				{
						get
						{
								return false;
						}
				}
		}
}
