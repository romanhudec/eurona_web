using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.IO;
using System.Xml;
using Eurona.Common.DAL.Entities;

namespace Eurona.RSS
{
		/// <summary>
		/// Summary description for $Products$
		/// </summary>
		//[WebService( Namespace = "http://X/" )]
		[WebServiceBinding( ConformsTo = WsiProfiles.None )]
		public class Products: IHttpHandler
		{

				public void ProcessRequest( HttpContext context )
				{
						context.Response.ContentType = "text/xml";
						context.Response.ContentEncoding = Encoding.UTF8;

						Eurona.Common.RSS.ProductRSS rss = new Common.RSS.ProductRSS( context );
						rss.Generate( context.Response.OutputStream );
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
