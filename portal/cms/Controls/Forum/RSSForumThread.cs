using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ForumEntinty = CMS.Entities.Forum;

namespace CMS.Controls.Forum
{
		public class RSSForumThread
		{
				public static int ForumThreadId { get; set; }
				public static string Name { get; set; }
				public static string Link { get; set; }
				public static string Description { get; set; }

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
						writer.WriteElementString( "title", RSSForumThread.Name );
						writer.WriteElementString( "link", RSSForumThread.Link );
						writer.WriteElementString( "description", RSSForumThread.Description );
						writer.WriteElementString( "ttl", "60" );

						// write out an <item> element for each of the first X articles
						List<ForumEntinty> list = Storage<ForumEntinty>.Read( new ForumEntinty.ReadByForumThreadId { ForumThreadId = ForumThreadId } );
						foreach ( ForumEntinty item in list )
						{
								// write out <item>
								writer.WriteStartElement( "item" );

								// write out <item>-level information
								writer.WriteElementString( "title", item.Name );
								writer.WriteElementString( "link", String.Format( RSSForumThread.ItemFormatUrl, item.ForumThreadId ) );
								writer.WriteElementString( "description", item.Description);
								writer.WriteElementString( "author", "" );
								// use DateTimeFormatInfo "r" to use RFC 1123 date formatting (same as RFC 822)
								writer.WriteElementString( "pubDate", DateTime.Now.ToString("r") );

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
