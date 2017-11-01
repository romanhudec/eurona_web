using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
namespace Mothiva.Cron
{
		/// <summary>
		/// Trieda umožnujúca komprimáciu súborov.
		/// </summary>
		public class Zip
		{
				public class ZipFileInfo
				{
						private string filePath = string.Empty;
						private string folderName = string.Empty;

						public ZipFileInfo()
						{
						}

						public ZipFileInfo( string filePath, string folderName )
						{
								this.filePath = filePath;
								this.folderName = folderName;
						}

						/// <summary>
						/// Cesta k vstupnému súboru, ktorý má byť archivovaný.
						/// </summary>
						public string FilePah
						{
								get { return this.filePath; }
								set { this.filePath = value; }
						}

						/// <summary>
						/// Názov priečinka do ktorého má byť v stupný súbor v zip súbore umiestnený.
						/// </summary>
						public string FolderName
						{
								get { return this.folderName; }
								set { this.folderName = value; }
						}
				}

				#region Private static methods
				/// <summary>
				/// Metóda pridá do výstupného "OutputStream", súbor na zadanej ceste.
				/// </summary>
				private static void AddFileEntry( string filePath, string rootFolderName, ZipOutputStream s, Crc32 crc )
				{
						FileStream fs = null;
						byte[] buffer = null;
						try
						{
								fs = File.OpenRead( filePath );
								buffer = new byte[fs.Length];
								fs.Read( buffer, 0, buffer.Length );

								string entryName = Path.Combine( rootFolderName, Path.GetFileName( filePath ) );

								ZipEntry entry = new ZipEntry( entryName );
								entry.DateTime = DateTime.Now;
								entry.Size = fs.Length;

								crc.Reset();
								crc.Update( buffer );
								entry.Crc = crc.Value;
								s.PutNextEntry( entry );

								s.Write( buffer, 0, buffer.Length );

						}
						finally
						{
								fs.Close();
								buffer = null;
						}
				}

				/// <summary>
				/// Metóda pridá do výstupného "OutputStream", adresár na zadanej ceste.
				/// </summary>
				private static void AddFolderEntry( string folderPath, string rootFolderName, ZipOutputStream s, Crc32 crc )
				{
						//Pridam do ZIP vsetky subory z daneho adresara.
						string[] files = Directory.GetFiles( folderPath );
						foreach ( string file in files )
								AddFileEntry( file, rootFolderName, s, crc );

						//Pridam do ZIP vsetky podadresare z daneho adresara.
						string[] folders = Directory.GetDirectories( folderPath );
						foreach ( string folder in folders )
						{
								DirectoryInfo di = new DirectoryInfo( folder );
								string folderName = Path.Combine( rootFolderName, di.Name );

								AddFolderEntry( folder, folderName, s, crc );
						}
				}
				#endregion

				#region Public static methods
				/// <summary>
				/// Metóda skomprimuje rôzne vstupné súbory do výstupného ZIP súboru.
				/// </summary>
				public static bool ZipFiles( List<ZipFileInfo> filesInfo, string outputFile )
				{
						try
						{
								using ( ZipOutputStream s = new ZipOutputStream( File.Create( outputFile ) ) )
								{
										s.SetLevel( 9 ); // 0 - store only to 9 - means best compression
										Crc32 crc = new Crc32();

										foreach ( ZipFileInfo fi in filesInfo )
												AddFileEntry( fi.FilePah, fi.FolderName, s, crc );

										s.Finish();
										s.Close();
								}

								return true;
						}
						catch ( Exception ex )
						{
								Console.WriteLine( "Exception during processing {0}", ex );
								return false;
						}
				}

				/// <summary>
				/// Metóda skomprimuje vstupný adresár aj s podadresármi do ZIP súboru.
				/// </summary>
				/// <param name="folderPath">Cesta na adresár, ktorý bude komprimovaný.</param>
				/// <param name="zipFilePath">Výstupný zip súbor.</param>
				/// <param name="includeRootFolder">Indikuje či sa má komprimovať celý adresár alebo len jeho obsah.</param>
				public static void ZipFolder( string folderPath, string zipFilePath, bool includeRootFolder )
				{
						using ( ZipOutputStream s = new ZipOutputStream( File.Create( zipFilePath ) ) )
						{
								s.SetLevel( 9 ); // 0 - store only to 9 - means best compression
								Crc32 crc = new Crc32();

								//Defaul sa komprimuje iba obsah daného adresára.
								string rootFolderName = string.Empty;

								//Ak sa má komprimovať celý adresar.
								if ( includeRootFolder )
								{
										DirectoryInfo di = new DirectoryInfo( folderPath );
										rootFolderName = di.Name;
								}

								//Skomprimuje adresár aj z podadresármi.
								AddFolderEntry( folderPath, rootFolderName, s, crc );

								s.Finish();
								s.Close();
						}
				}

				/// <summary>
				/// Metóda dekomprimuje vstupný súbor do cielového adresára.
				/// </summary>
				/// <param name="zipFileName"></param>
				/// <param name="destinationDir"></param>
				public static void Unzip( string zipFileName, string destinationDir )
				{
						ZipInputStream zInStream = new ZipInputStream( File.OpenRead( zipFileName ) );

						ZipEntry theEntry;
						while ( ( theEntry = zInStream.GetNextEntry() ) != null )
						{
								string fileName = Path.GetFileName( theEntry.Name );
								string fullName = destinationDir + Path.DirectorySeparatorChar + fileName;

								if( !Directory.Exists( destinationDir ) )
										Directory.CreateDirectory( destinationDir );

								if ( fileName != String.Empty )
								{
										FileStream streamWriter = File.Create( fullName );

										// ERROR UNZIPPING ZERO BYTE FILES
										if ( theEntry.Size != 0 )
										{
												int size = 2048;
												byte[] data = new byte[2048];
												while ( true )
												{
														size = zInStream.Read( data, 0, data.Length );
														if ( size > 0 )
																streamWriter.Write( data, 0, size );
														else
																break;
												}
										}
										streamWriter.Close();
								}
						}
						zInStream.Close();
				}

				#endregion
		}
}

