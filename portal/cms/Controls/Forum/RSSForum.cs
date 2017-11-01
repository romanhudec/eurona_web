using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ForumPostEntinty = CMS.Entities.ForumPost;

namespace CMS.Controls.Forum
{
		public class RSSForum
		{
				public static string Name { get; set; }
				public static string Link { get; set; }
				public static string Description { get; set; }

				public static int ForumId { get; set; }
				public static string ItemFormatUrl { get; set; }
				
				public static void Generate( Stream stream )
				{
						// Use an XmlTextWriter to write the XML data to a string...
						StringWriter sw = new StringWriter();
						XmlTextWriter writer = new XmlTextWriter( stream, Encoding.UTF8 );

						// write out <rss version="2.0">
						writer.WriteStartElement( "rss" );
						writer.WriteAttributeString( "version", "2.0" );

						// write out <channel>
						writer.WriteStartElement( "channel" );

						// write out <channel>-level elements
						writer.WriteElementString( "title", RSSForum.Name );
						writer.WriteElementString( "link", RSSForum.Link );
						writer.WriteElementString( "description", RSSForum.Description );
						writer.WriteElementString( "ttl", "60" );

						// write out an <item> element for each of the first X articles
						List<ForumPostEntinty> list = Storage<ForumPostEntinty>.Read( new ForumPostEntinty.ReadByForumId { ForumId = ForumId } );
						foreach ( ForumPostEntinty item in list )
						{
								// write out <item>
								writer.WriteStartElement( "item" );

								// write out <item>-level information
								writer.WriteElementString( "title", item.Title );
								writer.WriteElementString( "link", String.Format( RSSForum.ItemFormatUrl, ForumId ) );
								writer.WriteElementString( "description", item.Content);
								writer.WriteElementString( "author", "" );
								// use DateTimeFormatInfo "r" to use RFC 1123 date formatting (same as RFC 822)
								writer.WriteElementString( "pubDate", item.Date.ToString( "r" ) );

								// write out </item>
								writer.WriteEndElement();
						}

						// write out </channel>
						writer.WriteEndElement();

						// write out </rss>
						writer.WriteEndElement();

						writer.Close();

				}
		}
}
