using System;
using System.Collections.Generic;
using System.Text;
using CMS.Pump;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mothiva.Cron.Diagnostics;

namespace Cron.Eurona.Import
{
			/// <summary>
		/// Summary description for Synchronize.
		/// </summary>
		public abstract class Synchronize: CMS.Pump.DataSynchronize
		{
				public Synchronize( MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage )
						: base( srcSqlStorage, dstSqlStorage )
				{
				}


				public new MSSQLStorage SourceDataStorage
				{
						get { return (MSSQLStorage)base.SourceDataStorage; }
				}

				public new MSSQLStorage DestinationDataStorage
				{
						get { return (MSSQLStorage)base.DestinationDataStorage; }
				}

				protected void SendEmail( string to, string subject, string messsage )
				{
						try
						{
								Mothiva.Cron.Email email = new Mothiva.Cron.Email();
								email.To = to;
								email.Subject = subject;
								email.Message = messsage;
								email.Send();
						}
						catch ( Exception ex )
						{
								OnError( string.Format( "Can not send error report to email {0} : {1}", to, ex.Message ) );
						}
				}

				/// <summary>
				/// Odosle email na adresu "SMTP:To"
				/// </summary>
				protected void SendEmail( string subject, string messsage )
				{
						SendEmail( null, subject, messsage );
				}

				protected object Null( object obj )
				{
						return Null( obj, DBNull.Value );
				}

				protected object Null( bool condition, object obj )
				{
						return Null( condition, obj, DBNull.Value );
				}

				protected object Null( object obj, object def )
				{
						return Null( obj != null, obj, def );
				}

				protected object Null( bool condition, object obj, object def )
				{
						return condition ? obj : def;
				}

		}
}
