using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace Mothiva.Cron
{
		static class Program
		{
				/// <summary>
				/// The main entry point for the application.
				/// </summary>
				static void Main( string[] args )
				{
						ServiceBase[] ServicesToRun = new ServiceBase[] { new CronService() };
						ServiceBase.Run( ServicesToRun );
				}
		}
}
