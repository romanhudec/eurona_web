using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Security.Principal;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CMS.MSSQL;
using CMS.Entities;

namespace CMS {
    public abstract class Notification {
    }

    public class EmailNotification : Notification {
        public EmailNotification() {
        }

        /// <summary>
        /// Emailova/e adresa prijemcu. Viaceri prijemcovia su oddeleny";"
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Emailova/e adresa skryteho prijemcu. Viaceri prijemcovia su oddeleny";"
        /// </summary>
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public List<Attachment> Attachments { get; set; }

        public bool Notify() {
            return Notify(false);
        }

        public virtual bool Notify(bool isBodyHtml, string fromEmail = null) {
            using (MailMessage msg = new MailMessage()) {
                bool useSSL = false;
                string strUseSSL = ConfigurationManager.AppSettings["SHP:SMTP:UseSSL"];
                if (!string.IsNullOrEmpty(strUseSSL))
                    Boolean.TryParse(strUseSSL, out useSSL);

                EmailLog log = new EmailLog();
                log.Email = this.To;
                log.Subject = this.Subject;
                log.Message = this.Message;

                if (this.Attachments == null) this.Attachments = new List<Attachment>();
                foreach (Attachment attachment in this.Attachments) {
                    msg.Attachments.Add(attachment);
                }

                try {
                    if (fromEmail == null)
                        msg.From = new MailAddress(ConfigurationManager.AppSettings["SMTP:From"], ConfigurationManager.AppSettings["SMTP:FromDisplay"]);
                    else
                        msg.From = new MailAddress(fromEmail, fromEmail);

                    List<string> toList = GetEmailAddress(this.To);
                    List<string> bccList = GetEmailAddress(this.Bcc);

                    if (toList.Count == 0 && bccList.Count == 0) return false;

                    foreach (string to in toList)
                        msg.To.Add(new MailAddress(to, to));

                    foreach (string bcc in bccList)
                        msg.Bcc.Add(new MailAddress(bcc, bcc));

                    msg.Subject = this.Subject;
                    msg.IsBodyHtml = isBodyHtml;
                    msg.Body = this.Message;
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = useSSL;
                    smtp.Send(msg);

                    try {
                        log.Status = true;
                        Storage<EmailLog>.Create(log);
                    }catch{
                    }


                } catch (Exception ex) {
                    try {
                        log.Status = false;
                        log.Error = ex.Message;
                        Storage<EmailLog>.Create(log);
                    } catch {
                    }
                    EvenLog.WritoToEventLog(ex);
                    return false;
                }
            }
            return true;
        }

        public bool IsValidEmail(string email) {
            if (string.IsNullOrEmpty(email)) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            if (re.IsMatch(email)) return true;
            return false;
        }

        private List<string> GetEmailAddress(string email) {
            List<string> emailList = new List<string>();
            if (string.IsNullOrEmpty(email)) return emailList;

            string[] addresses = email.Split(';');
            foreach (string address in addresses) {
                if (IsValidEmail(address) == false) continue;
                emailList.Add(address);
            }

            return emailList;
        }
    }
}
