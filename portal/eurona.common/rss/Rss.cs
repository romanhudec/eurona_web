using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.RSS
{
		public class ProductRSS
		{
				private HttpContext context = null;
				public ProductRSS( HttpContext context )
				{
						this.context = context;
				}

				public void Generate( Stream stream )
				{
						// Use an XmlTextWriter to write the XML data to a string...
						StringWriter sw = new StringWriter();
						XmlTextWriter writer = new XmlTextWriter( stream, Encoding.UTF8 );

						// write out <rss version="2.0">
						writer.WriteStartDocument();
						writer.WriteStartElement( "SHOP" );

						// write out an <item> element for each of the first X articles
						List<Product> products = Storage<Product>.Read( null );
						foreach ( Product item in products )
						{
								// write out <SHOPITEM>
								writer.WriteStartElement( "SHOPITEM" );

								// write out <item>-level information
								writer.WriteElementString( "PRODUCT", item.Name );
								writer.WriteElementString( "DESCRIPTION", item.Description );
								writer.WriteElementString( "URL", this.ResolveUrl( context, item.Alias ) );
								writer.WriteElementString( "IMGURL", this.GetProductImage( item.Id ) );
								writer.WriteElementString( "PRICE", item.PriceTotal.ToString() );
								writer.WriteElementString( "PRICE_VAT", item.PriceTotalWVAT.ToString() );

								// write out </SHOPITEM>
								writer.WriteEndElement();
						}

						// write out </SHOP>
						writer.WriteEndElement();


						writer.Close();
				}

				private string GetProductImage( int productId )
				{
						string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue( "SHP:ImageGallery:Product:StoragePath" );
						string storagePath = string.Format( "{0}{1}/", storageUrl, productId );
						string productImagesPath = this.context.Server.MapPath( storagePath );

						if ( !Directory.Exists( productImagesPath ) )
								return null;

						DirectoryInfo di = new DirectoryInfo( productImagesPath );
						FileInfo[] fileInfos = di.GetFiles( "*.*" );

						if ( fileInfos.Length == 0 )
								return null;


						string urlThumbnail = ResolveUrl(this.context, storagePath) + "_t/" + fileInfos[0].Name;
						return urlThumbnail;
				}

				private string ResolveUrl( HttpContext context, string url )
				{
						System.Web.UI.Page p = context.Handler as System.Web.UI.Page;
						if ( p != null )
								return p.ResolveUrl( url );
						else
						{
								string absUrl = VirtualPathUtility.ToAbsolute( url );
								absUrl = absUrl.StartsWith( "/" ) ? absUrl.Remove( 0, 1 ) : absUrl;
								return CMS.Utilities.ServerUtilities.Root( context.Request ) + absUrl;
						}
				}
		}
}
