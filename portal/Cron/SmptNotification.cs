using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace Mothiva.Cron
{
		/// <summary>
		/// Implementácia triedy pre zasielanie notifikácií cez SMTP protokol.
		/// </summary>
		sealed class SmptNotification
		{
				private readonly EventLog eventLog;

				public readonly string SmtpRecipient;

				public SmptNotification( CronService service )
				{
						this.eventLog = service.EventLog;

						if ( ConfigurationManager.AppSettings["SMTP:From"] == null || ConfigurationManager.AppSettings["SMTP:To"] == null )
								return;

						this.SmtpRecipient = ConfigurationManager.AppSettings["SMTP:To"];
				}

				public void Send( string subject, string message )
				{
						try
						{
								Email mail = new Email();
								mail.To = this.SmtpRecipient;
								mail.Subject = subject;
								mail.Message = message;
						}
						catch ( System.Exception ex )
						{
								this.eventLog.WriteEntry( String.Format( "Send Email failed with error : {0}", ex.Message ), EventLogEntryType.Warning );
						}
				}
		}

}
