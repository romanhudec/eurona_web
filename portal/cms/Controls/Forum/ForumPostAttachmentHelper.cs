using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.UI;
using ForumPostEntity = CMS.Entities.ForumPost;
using ForumPostAttachmentEntity = CMS.Entities.ForumPostAttachment;

namespace CMS.Controls.Forum
{
		public class ForumPostAttachmentHelper
		{
				#region Attachment methods

				/// <summary>
				/// Metóda vymaže existujúci obrázok na danej pozíci z file systému.
				/// </summary>
				private static void RemoveExistingAttachmentByPosition( string forumAttPath, int position )
				{
						//Delete image
						DirectoryInfo di = new DirectoryInfo( forumAttPath );
						FileInfo[] fileInfos = di.GetFiles( string.Format( "{0:0#}_*.*", position ) );
						foreach ( FileInfo fileInfo in fileInfos )
								fileInfo.Delete();
				}
				/// <summary>
				/// Metóda Uploadne/Nahradí fotografiu produktu pre daný produkt.
				/// V pripade, že už sa fotografia na tomto poradi nachádza, nahradí sa.
				/// </summary>
				public static void UpdatePostAttachments( System.Web.UI.Page page, ForumPostEntity forumPost, FileCollectionEventArgs mfuArgs )
				{
						if ( forumPost == null || mfuArgs == null )
								return;

						if ( mfuArgs.PostedFilesInfo.Count == 0 )
								return;

						string forumVirtualAttPath = CMS.Utilities.ConfigUtilities.ConfigValue( "CMS:Forum:Post:StoragePath", page );
						if ( !forumVirtualAttPath.EndsWith( "/" ) ) forumVirtualAttPath += "/";
						forumVirtualAttPath += forumPost.ForumId.ToString() + "/" + forumPost.Id.ToString();
						string forumAttPath = page.Server.MapPath( forumVirtualAttPath );

						if ( !Directory.Exists( forumAttPath ) )
								Directory.CreateDirectory( forumAttPath );

						foreach ( PostedFileInfo fi in mfuArgs.PostedFilesInfo )
						{
								string desc = fi.Description;

								//Delete existing Product photo on position from file system.
								RemoveExistingAttachmentByPosition( forumAttPath, fi.Positon );

								string fileName = string.Format( "{0:0#}_{1}", fi.Positon, Path.GetFileName( fi.File.FileName ) );
								string filePath = Path.Combine( forumAttPath, fileName );

								//Read input stream
								Stream stream = fi.File.InputStream;
								int len = (int)stream.Length;
								if ( len == 0 ) return;
								byte[] data = new byte[len];
								stream.Read( data, 0, len );
								stream.Flush();
								stream.Close();

								//Write new product photo.
								using ( FileStream fs = new FileStream( filePath, FileMode.Create, FileAccess.Write ) )
										fs.Write( data, 0, len );

								ForumPostAttachmentEntity attachment = new ForumPostAttachmentEntity();
								attachment.ForumPostId = forumPost.Id;
								attachment.Url = forumVirtualAttPath + "/" + fileName;
								attachment.Name = Path.GetFileName( fi.File.FileName );
								attachment.Description = fi.Description;
								attachment.Type = IsImageFile( attachment.Name ) ? ForumPostAttachmentEntity.AttachmentType.Image : ForumPostAttachmentEntity.AttachmentType.File;
								attachment.Size = len;
								attachment.Order = fi.Positon;
								Storage<ForumPostAttachmentEntity>.Create( attachment );
						}
				}
				private static bool IsImageFile( string file )
				{
						string extension = Path.GetExtension( file );
						extension = extension.ToUpper();
						if ( extension == ".JPG" || extension == ".PNG" || extension == ".GIF" || extension == ".BMP" ) return true;
						return false;
				}

				#endregion
		}
}
