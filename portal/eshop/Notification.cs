using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Security.Principal;
using System.Diagnostics;
using CMS.Entities;

namespace SHP {

    public class EmailNotification : CMS.EmailNotification {
        public EmailNotification() {
        }

        public override bool Notify(bool isBodyHtml, string fromEmail = null) {
            DateTime atemptTime = DateTime.Now;
            bool successSend = false;
            int repeatedCount = 0;
            do {
                successSend = Notify(isBodyHtml, null, fromEmail);
                atemptTime = DateTime.Now;
                if(!successSend ) System.Threading.Thread.Sleep(3*1000);
                repeatedCount++;
            } while (successSend == false && repeatedCount < 3);

            return successSend;
        }

        public bool Notify(bool isBodyHtml, string[] attachmentFiles , string fromEmail = null) {
            using (MailMessage msg = new MailMessage()) {
                bool useSSL = false;
                string strUseSSL = ConfigurationManager.AppSettings["SHP:SMTP:UseSSL"];
                if (!string.IsNullOrEmpty(strUseSSL))
                    Boolean.TryParse(strUseSSL, out useSSL);

                EmailLog log = new EmailLog();
                log.Email = this.To;
                log.Subject = this.Subject;
                log.Message = this.Message;

                try {
                    if (fromEmail == null)
                        msg.From = new MailAddress(ConfigurationManager.AppSettings["SHP:SMTP:From"], ConfigurationManager.AppSettings["SHP:SMTP:FromDisplay"]);
                    else
                        msg.From = new MailAddress(fromEmail, fromEmail);

                    if( attachmentFiles != null && attachmentFiles.Length != 0){
                        foreach(string fileName in attachmentFiles){
                            Attachment attachment = new Attachment(fileName);
                            msg.Attachments.Add(attachment);
                        }
                    }

                    if (this.Attachments == null) this.Attachments = new List<Attachment>();
                    foreach (Attachment attachment in this.Attachments) {
                        msg.Attachments.Add(attachment);
                    }

                    msg.To.Add(new MailAddress(To, To));
                    msg.Subject = Subject;
                    msg.IsBodyHtml = isBodyHtml;
                    msg.Body = Message;
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = useSSL;
                    smtp.Send(msg);

                    try {
                        log.Status = true;
                        Storage<EmailLog>.Create(log);
                    } catch {
                    }

                } catch (Exception ex) {
                    try {
                        log.Status = false;
                        log.Error = ex.Message;
                        Storage<EmailLog>.Create(log);
                    } catch {
                    }
                    CMS.EvenLog.WritoToEventLog(ex);
                    return false;
                }
            }
            return true;
        }
    }
}
