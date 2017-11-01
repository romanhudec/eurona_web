using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Mothiva.Cron.Diagnostics
{
		using AppTrace = System.Diagnostics.Trace;

		public sealed class TraceCategory
		{
				public const string Information = "INFO";
				public const string Error = "CHYBA";
				public const string Warning = "UPOZORNENIE";
		}

		public sealed class Trace
		{
				//Synchronizacny objekt pre viacvlaknove spracovanie
				private static object syncRoot = new object();

				private Trace()
				{
				}

				private static string GetDateTimeCategory( string category )
				{
						DateTime now = DateTime.Now;

						return string.Format( "{0:00}.{1:00}.{2:0000} {3:00}:{4:00}:{5:00} {6,-6} ",
								now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second, category );
				}

				public static void SetFileListener( string name, string filePath )
				{
						StreamWriter stream = new StreamWriter( filePath, true, Encoding.UTF8 );
						stream.AutoFlush = true;
						TextWriterTraceListener twtl = new TextWriterTraceListener( stream );
						twtl.Name = name;
						AppTrace.Listeners.Add( twtl );
				}

				public static bool AutoFlush
				{
						get { return AppTrace.AutoFlush; }
						set { AppTrace.AutoFlush = value; }
				}

				public static int IndentSize
				{
						get { return AppTrace.IndentSize; }
						set { AppTrace.IndentSize = value; }
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, System.Exception ex )
				{
						lock ( syncRoot )
						{
								WriteLine( name, "!!! Neodchytená výnimka !!!", ex, TraceCategory.Error );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, System.Exception ex, string category )
				{
						lock ( syncRoot )
						{
								WriteLine( name, null, ex, category );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, string message, System.Exception ex )
				{
						lock ( syncRoot )
						{
								WriteLine( name, message, ex, TraceCategory.Error );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, string message, System.Exception ex, string category )
				{
						lock ( syncRoot )
						{
								StringBuilder sb = new StringBuilder();
								sb.Append( message );

								if ( ex != null )
								{
										sb.Append( "\n{\n" );//{
										AppTrace.Indent();
										sb.Append( ex.Message );
										if ( ex.InnerException != null ) sb.Append( ex.InnerException.Message );

										if ( !string.IsNullOrEmpty( ex.StackTrace ) )
										{
												sb.Append( "\n\t!!StackTrace!!\n\t{\n" );//{
												sb.Append( ex.StackTrace );
												sb.Append( "\n\t}\n" );//{
										}

										AppTrace.Unindent();
										sb.Append( "\n}\n" );//{
								}

								WriteLine( name, sb.ToString(), category );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( string name, bool condition, System.Exception ex )
				{
						lock ( syncRoot )
						{
								if ( condition )
										WriteLine( name, null, ex, TraceCategory.Error );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( string name, bool condition, System.Exception ex, string category )
				{
						lock ( syncRoot )
						{
								if ( condition )
										WriteLine( name, null, ex, category );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( string name, bool condition, string message, System.Exception ex )
				{
						lock ( syncRoot )
						{
								if ( condition )
										WriteLine(name, message, ex, TraceCategory.Error );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( string name, bool condition, string message, System.Exception ex, string category )
				{
						lock ( syncRoot )
						{
								if ( condition )
										WriteLine(name, message, ex, category );
						}
				}

				#region System.Diagnostics.Trace class supplemental methods

				[Conditional( "TRACE" )]
				public static void Indent()
				{
						lock ( syncRoot )
						{
								AppTrace.Indent();
						}
				}

				[Conditional( "TRACE" )]
				public static void Unindent()
				{
						lock ( syncRoot )
						{
								AppTrace.Unindent();
						}
				}

				[Conditional( "TRACE" )]
				public static void Write( string name, object value )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].Write( value );
						}
				}

				[Conditional( "TRACE" )]
				public static void Write( string name, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].Write( message );
						}
				}

				[Conditional( "TRACE" )]
				public static void Write( string name, object value, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].Write( value, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void Write( string name, string message, string category )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].Write( message, GetDateTimeCategory( category ) );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteIf( bool condition, object value )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteIf( condition, value );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteIf( bool condition, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteIf( condition, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteIf( bool condition, object value, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteIf( condition, value, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteIf( bool condition, string message, string category )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteIf( condition, message, GetDateTimeCategory( category ) );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, object value )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].WriteLine( value );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, object value, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].WriteLine( value, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLine( string name, string message, string category )
				{
						lock ( syncRoot )
						{
								AppTrace.Listeners[name].WriteLine( message, GetDateTimeCategory( category ) );
								//AppTrace.WriteLine( message, GetDateTimeCategory( category ) );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( bool condition, object value )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteLineIf( condition, value );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( bool condition, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteLineIf( condition, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( bool condition, object value, string message )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteLineIf( condition, value, message );
						}
				}

				[Conditional( "TRACE" )]
				public static void WriteLineIf( bool condition, string message, string category )
				{
						lock ( syncRoot )
						{
								AppTrace.WriteLineIf( condition, message, GetDateTimeCategory( category ) );
						}
				}

				#endregion
		}
}
