using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

namespace Mothiva.Cron
{
		[StructLayout( LayoutKind.Sequential )]
		internal struct SERVICE_STATUS
		{
				public int serviceType;
				public int currentState;
				public int controlsAccepted;
				public int win32ExitCode;
				public int serviceSpecificExitCode;
				public int checkPoint;
				public int waitHint;
		}

		internal enum ServiceState
		{
				SERVICE_STOPPED = 0x00000001,
				SERVICE_START_PENDING = 0x00000002,
				SERVICE_STOP_PENDING = 0x00000003,
				SERVICE_RUNNING = 0x00000004,
				SERVICE_CONTINUE_PENDING = 0x00000005,
				SERVICE_PAUSE_PENDING = 0x00000006,
				SERVICE_PAUSED = 0x00000007,
		}

		partial class CronService: ServiceBase
		{
				public static string ListenerName = "Cron";
				private static System.Diagnostics.TraceSwitch TraceGeneral = new System.Diagnostics.TraceSwitch( "General", "General event log content level switch" );

				#region ADVAPI32.DLL binding

				[DllImport( "ADVAPI32.DLL", CallingConvention = CallingConvention.Winapi )]
				private static extern bool SetServiceStatus( IntPtr hServiceStatus, IntPtr lpServiceStatus );

				#endregion

				private SERVICE_STATUS serviceStatus;
				private CronManager cromManager;
				private ServiceSettings settings = null;

				public CronService()
				{
						InitializeComponent();
						this.EventLog.Source = this.ServiceName;
						string location = Assembly.GetEntryAssembly().Location;
						Diagnostics.Trace.SetFileListener( ListenerName, System.IO.Path.ChangeExtension( location, ".log" ) );
						Diagnostics.Trace.AutoFlush = true;
						Diagnostics.Trace.WriteLine( ListenerName, "###################################################################################################################" );
						Diagnostics.Trace.WriteLine( ListenerName, string.Format( "Cron service, {0}", DateTime.Now ) );
						Diagnostics.Trace.WriteLine( ListenerName, "###################################################################################################################" );

				}

				public ServiceSettings Settings
				{
						get { return this.settings; }
				}

				protected override void OnStart( string[] args )
				{
#if _OFFLINE_DEBUG
            Debugger.Launch();
                        Diagnostics.Trace.WriteLine( ListenerName, "OFFLINE DEBUG MODE !!!", Diagnostics.TraceCategory.Warning );
#endif

                    SetState( ServiceState.SERVICE_START_PENDING );

						try
						{
								this.settings = ConfigurationManager.GetSection( "serviceSettings" ) as ServiceSettings;
						}
						catch ( ConfigurationException ex )
						{
									this.EventLog.WriteEntry( String.Format( "{0}\n\n{1}", "Unable to read service configuration from a configuration file. Check the configuration file for errors.", ex.Message ),
									EventLogEntryType.Error );

								Stop();
								return;
						}

						try
						{
								if ( string.IsNullOrEmpty( ConfigurationManager.AppSettings["SMTP:From"] ) )
										throw new ConfigurationErrorsException( "Missing tag : AppSettings/SMTP:From" );

								if ( string.IsNullOrEmpty( ConfigurationManager.AppSettings["SMTP:To"] ) )
										throw new ConfigurationErrorsException( "Missing tag : AppSettings/SMTP:To" );
						}
						catch ( ConfigurationErrorsException ex )
						{
								this.EventLog.WriteEntry( String.Format( "{0}\n\n{1}", "Unable to read service configuration from a configuration file. Check the configuration file for errors.", ex.Message ),
										EventLogEntryType.Error );

								Stop();
								return;
						}

						//Spustenie manažéra, zabezpečujúceho prislusne operacie v danom case.
						Cron cron = new Cron();
						foreach ( ServiceSettings.CronEntryElement cee in settings.CronEntries )
								cron.Add( cee );

						this.cromManager = new CronManager( this, cron );
						this.cromManager.Run();

						SetState( ServiceState.SERVICE_RUNNING );
						Diagnostics.Trace.WriteLine( ListenerName, "Service started", null, Diagnostics.TraceCategory.Information );
				}

				protected override void OnStop()
				{ 
						SetState( ServiceState.SERVICE_STOP_PENDING );

						if ( this.cromManager != null )
						{
								RequestAdditionalTime( 30000 );

								this.cromManager.Cancel();
								this.cromManager.Join();
								this.cromManager = null;
						}


						if ( this.cromManager == null )
								SetState( ServiceState.SERVICE_STOPPED );
						else
								SetState( ServiceState.SERVICE_RUNNING );

						Diagnostics.Trace.WriteLine( ListenerName, "Service stoped", null, Diagnostics.TraceCategory.Information );
				}

				private void SetState( ServiceState state )
				{
						this.serviceStatus.currentState = (int)state;
					
						unsafe
						{
								fixed ( SERVICE_STATUS* ptr = &this.serviceStatus )
								{
										SetServiceStatus( this.ServiceHandle, (IntPtr)ptr );
								}
						}
					 
				}
		}
}
