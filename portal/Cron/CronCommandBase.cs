using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using Mothiva.Cron.Diagnostics;

namespace Mothiva.Cron
{
		/// <summary>
		/// 
		/// </summary>
		public interface ICronCommand
		{
				bool Exec( string command, Dictionary<string, string> parameters );
				void Break();
		}

		public abstract class CronCommandBase: ICronCommand
		{
				protected object syncRoot = new object();
				private ManualResetEvent eventBreak = null;
				
				public CronCommandBase( string entryName )
				{
						this.ListenerName = entryName;
						this.eventBreak = new ManualResetEvent( false );

						lock ( syncRoot )
						{
								string assemblyPath = Assembly.GetAssembly( this.GetType() ).Location;
								string asemblyName = Path.GetFileNameWithoutExtension( assemblyPath );
								string logName = Path.Combine( Path.GetDirectoryName( assemblyPath ), entryName + ".log" );

								Trace.SetFileListener( this.ListenerName, logName );
								Trace.AutoFlush = true;
								WriteLogLine("###################################################################################################################" );
								WriteLogLine( string.Format( "{0} - {1}, {2}", asemblyName, entryName, DateTime.Now ) );
								WriteLogLine( "###################################################################################################################" );
						}
				}

				/// <summary>
				/// Názov listenera, ktory loguje činnosť
				/// </summary>
				protected string ListenerName { get; set; }

				#region ICronCommand Members

				public abstract bool Exec( string command, Dictionary<string, string> parameters );

				public virtual void Break()
				{
						lock ( this.syncRoot )
						{
								this.eventBreak.Set();
						}
				}

				#endregion

				public bool BreakPending()
				{
						return this.eventBreak.WaitOne( 0, false );
				}

				public bool BreakPending( TimeSpan timeout )
				{
						return this.eventBreak.WaitOne( timeout, false );
				}

				#region Log
				public void WriteLogLine( object value )
				{
						Trace.WriteLine( this.ListenerName, value );
				}

				public void WriteLogLine( object value, string message )
				{
						Trace.WriteLine( this.ListenerName, value, message );
				}

				public void WriteLogLine( string message, string category )
				{
						Trace.WriteLine( this.ListenerName, message, category );
				}

				public void WriteLogLine( System.Exception ex )
				{
						Trace.WriteLine( this.ListenerName, ex );
				}

				public void WriteLogLine( System.Exception ex, string category )
				{
						Trace.WriteLine( this.ListenerName, ex, category );
				}

				public void WriteLogLine( string message, System.Exception ex )
				{
						Trace.WriteLine( this.ListenerName, message, ex );
				}
				#endregion
		}
}
