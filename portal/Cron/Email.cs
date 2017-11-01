using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Mothiva.Cron
{
	public class Email
	{
		public Email()
		{
		}

		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		//public event SendCompletedEventHandler SendCompleted;

		public bool Send()
		{
			return Send(false);
		}

		public bool Send(bool isBodyHtml)
		{
			using (MailMessage msg = new MailMessage()) {
				if (String.IsNullOrEmpty(From)) From = ConfigurationManager.AppSettings["SMTP:From"];
				if (String.IsNullOrEmpty(To)) To = ConfigurationManager.AppSettings["SMTP:To"];
				string[] toAddresses = To.Split(';');

				msg.From = new MailAddress(From, From);
				foreach (string addressTo in toAddresses) msg.To.Add(new MailAddress(addressTo, addressTo));
				msg.Subject = Subject;
				msg.IsBodyHtml = isBodyHtml;
				msg.Body = Message;
				SmtpClient smtp = new SmtpClient();
				//if (SendCompleted != null) smtp.SendCompleted += SendCompleted;
				try {
					smtp.Send(msg);
				} catch {
					return false;
				}
			}
			return true;
		}
	}
}
