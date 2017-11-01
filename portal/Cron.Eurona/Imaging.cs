using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Cron.Eurona
{
		public class Imaging
		{
				#region Static helper methods
				public static System.Drawing.Bitmap BitmapResizeEx( System.Drawing.Bitmap img, int width, int height, bool bBilinear )
				{
						System.Drawing.Bitmap b = new System.Drawing.Bitmap( width, height );
						System.Drawing.Graphics g = System.Drawing.Graphics.FromImage( (System.Drawing.Image)b );

						if ( bBilinear ) g.InterpolationMode = InterpolationMode.HighQualityBilinear;    // Specify here
						else g.InterpolationMode = InterpolationMode.Default;

						g.DrawImage( img, 0, 0, width, height );
						g.Dispose();

						return b;
				}

				private static System.Drawing.Bitmap BitmapResize( System.Drawing.Bitmap b, int nWidth, int nHeight, bool bBilinear )
				{
						System.Drawing.Bitmap bTemp = (System.Drawing.Bitmap)b.Clone();
						b = new System.Drawing.Bitmap( nWidth, nHeight, bTemp.PixelFormat );

						double nXFactor = (double)bTemp.Width / (double)nWidth;
						double nYFactor = (double)bTemp.Height / (double)nHeight;

						if ( bBilinear )
						{
								double fraction_x, fraction_y, one_minus_x, one_minus_y;
								int ceil_x, ceil_y, floor_x, floor_y;
								System.Drawing.Color c1 = new System.Drawing.Color();
								System.Drawing.Color c2 = new System.Drawing.Color();
								System.Drawing.Color c3 = new System.Drawing.Color();
								System.Drawing.Color c4 = new System.Drawing.Color();
								byte red, green, blue;

								byte b1, b2;

								for ( int x = 0; x < b.Width; ++x )
										for ( int y = 0; y < b.Height; ++y )
										{
												// Setup

												floor_x = (int)Math.Floor( x * nXFactor );
												floor_y = (int)Math.Floor( y * nYFactor );
												ceil_x = floor_x + 1;
												if ( ceil_x >= bTemp.Width ) ceil_x = floor_x;
												ceil_y = floor_y + 1;
												if ( ceil_y >= bTemp.Height ) ceil_y = floor_y;
												fraction_x = x * nXFactor - floor_x;
												fraction_y = y * nYFactor - floor_y;
												one_minus_x = 1.0 - fraction_x;
												one_minus_y = 1.0 - fraction_y;

												c1 = bTemp.GetPixel( floor_x, floor_y );
												c2 = bTemp.GetPixel( ceil_x, floor_y );
												c3 = bTemp.GetPixel( floor_x, ceil_y );
												c4 = bTemp.GetPixel( ceil_x, ceil_y );

												// Blue
												b1 = (byte)( one_minus_x * c1.B + fraction_x * c2.B );

												b2 = (byte)( one_minus_x * c3.B + fraction_x * c4.B );

												blue = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

												// Green
												b1 = (byte)( one_minus_x * c1.G + fraction_x * c2.G );

												b2 = (byte)( one_minus_x * c3.G + fraction_x * c4.G );

												green = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

												// Red
												b1 = (byte)( one_minus_x * c1.R + fraction_x * c2.R );

												b2 = (byte)( one_minus_x * c3.R + fraction_x * c4.R );

												red = (byte)( one_minus_y * (double)( b1 ) + fraction_y * (double)( b2 ) );

												b.SetPixel( x, y, System.Drawing.Color.FromArgb( 255, red, green, blue ) );
										}
						}
						else
						{
								for ( int x = 0; x < b.Width; ++x )
										for ( int y = 0; y < b.Height; ++y )
												b.SetPixel( x, y, bTemp.GetPixel( (int)( Math.Floor( x * nXFactor ) ), (int)( Math.Floor( y * nYFactor ) ) ) );
						}

						return b;
				}

				public static MemoryStream GetImageStream( Stream inputStream, int maxWidth, int maxHeight, bool stretch )
				{
						System.Drawing.Bitmap b = null;
						//Resize obrazku.
						try
						{
								b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream( inputStream );
						}
						catch
						{
								return null;
						}

						if ( stretch )
								b = Imaging.BitmapResize( b, maxWidth, maxHeight, true );
						else
						{
								bool recalculate = false;
								recalculate = ( b.Width > maxWidth || b.Height > maxHeight );

								//Resize iba ak je obrazok vacsi ako maximalna povolena velkost.
								if ( recalculate )
								{
										int width = maxWidth;
										int height = maxHeight;
										RecalculateImageSize( b, maxWidth, maxHeight, ref width, ref height );
										b = Imaging.BitmapResize( b, width, height, true );
								}
						}

						MemoryStream str = new MemoryStream();
						b.Save( str, System.Drawing.Imaging.ImageFormat.Jpeg );

						return str;
				}

				public static MemoryStream GetImageStream( Stream inputStream )
				{
						//Resize obrazku.
						System.Drawing.Bitmap b = null;
						try
						{
								b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream( inputStream );
						}
						catch
						{
								return null;
						}

						MemoryStream str = new MemoryStream();
						b.Save( str, System.Drawing.Imaging.ImageFormat.Jpeg );

						return str;
				}

				public static bool ResizeImage( string fileName, string newFileName, int width, int height )
				{
						//Resize obrazku.
						System.Drawing.Bitmap b = null;
						try
						{
								b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile( fileName );
								b = Imaging.BitmapResizeEx( b, width, height, true );
								b.Save( newFileName, System.Drawing.Imaging.ImageFormat.Jpeg );
						}
						catch
						{
								return false;
						}

						return true; ;
				}
				public static bool ResizeImage( string fileName, string newFileName, int width, int height, bool bilinear )
				{
						//Resize obrazku.
						System.Drawing.Bitmap b = null;
						try
						{
								b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile( fileName );
								b = Imaging.BitmapResizeEx( b, width, height, bilinear );
								b.Save( newFileName, System.Drawing.Imaging.ImageFormat.Jpeg );
						}
						catch
						{
								return false;
						}

						return true; ;
				}
				/// <summary>
				/// Prepočíta výšku a šírku obrázku v správnom (povodnom) pomere strán;
				/// </summary>
				private static void RecalculateImageSize( System.Drawing.Bitmap b, int maxWidth, int maxHeight, ref int width, ref int height )
				{
						RecalculateImageSize( b.Width, b.Height, maxWidth, maxHeight, ref width, ref height );
				}

				/// <summary>
				/// Prepočíta výšku a šírku obrázku v správnom (povodnom) pomere strán;
				/// </summary>
				public static void RecalculateImageSize( int imageWidth, int imageHeight, int maxWidth, int maxHeight, ref int width, ref int height )
				{
						if ( maxWidth > imageWidth && maxHeight > imageHeight )
						{
								width = imageWidth;
								height = imageHeight;
								return;
						}

						int imageW = imageWidth;
						int imageH = imageHeight;

						double wIndex = (double)imageWidth / (double)maxWidth;
						double hIndex = (double)imageHeight / (double)maxHeight;

						if ( hIndex > wIndex )
						{
								height = maxHeight;
								width = (int)( ( imageW * height ) / imageH );
						}
						else
						{
								width = maxWidth;
								height = (int)( ( imageH * width ) / imageW );
						}
				}

				public static bool SaveJpeg( string srcPath, string dstPath, long quality )
				{
						// Encoder parameter for image quality
						EncoderParameter qualityParam = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, quality );

						// Jpeg image codec
						ImageCodecInfo jpegCodec = GetEncoderInfo( "image/jpeg" );

						if ( jpegCodec == null ) return false;

						EncoderParameters encoderParams = new EncoderParameters( 1 );
						encoderParams.Param[0] = qualityParam;

						try
						{
								System.Drawing.Image img = System.Drawing.Image.FromFile( srcPath );
								img.Save( dstPath, jpegCodec, encoderParams );

								return true;

						}
						catch
						{
								return false;
						}

				}

				private static ImageCodecInfo GetEncoderInfo( string mimeType )
				{
						// Get image codecs for all image formats
						ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

						// Find the correct image codec
						for ( int i = 0; i < codecs.Length; i++ )
								if ( codecs[i].MimeType == mimeType )
										return codecs[i];
						return null;
				}
				#endregion
		}
}
