using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;

namespace Eurona.RSS
{
		/// <summary>
		/// Summary description for $Articles$
		/// </summary>
		//[WebService( Namespace = "http://X/" )]
		[WebServiceBinding( ConformsTo = WsiProfiles.None )]
		public class Articles: IHttpHandler
		{

				public void ProcessRequest( HttpContext context )
				{
						context.Response.ContentType = "text/xml";
						context.Response.ContentEncoding = Encoding.UTF8;

						CMS.Controls.Article.RSS.Link = Utilities.Root( context.Request );
						CMS.Controls.Article.RSS.Title = Resources.Strings.RSS_ArticleTitle;
						CMS.Controls.Article.RSS.ItemFormatUrl = "../article.aspx?id={0}";
						CMS.Controls.Article.RSS.Generate( context.Response.OutputStream );
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
