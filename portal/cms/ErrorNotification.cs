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
using System.Net.Configuration;

namespace CMS {
    public abstract class ErrorNotification {
    }

    public class ErrorEmailNotification : ErrorNotification {
        public ErrorEmailNotification() {
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
            string strUseSSL = ConfigurationManager.AppSettings["SHP:SMTP:UseSSL"];
            bool useSSL = false;
            if (!string.IsNullOrEmpty(strUseSSL))
                Boolean.TryParse(strUseSSL, out useSSL);


            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("errorMailSettings/default");
            SmtpClient smtpClient = new SmtpClient();
            if (section != null) {
                if (section.Network != null) {
                    smtpClient.Host = section.Network.Host;
                    smtpClient.Port = section.Network.Port;
                    smtpClient.UseDefaultCredentials = section.Network.DefaultCredentials;

                    smtpClient.Credentials = new NetworkCredential(section.Network.UserName, section.Network.Password, section.Network.ClientDomain);
                    smtpClient.EnableSsl = useSSL;

                    if (section.Network.TargetName != null)
                        smtpClient.TargetName = section.Network.TargetName;
                }

                smtpClient.DeliveryMethod = section.DeliveryMethod;
                if (section.SpecifiedPickupDirectory != null && section.SpecifiedPickupDirectory.PickupDirectoryLocation != null)
                    smtpClient.PickupDirectoryLocation = section.SpecifiedPickupDirectory.PickupDirectoryLocation;
            }

            using (MailMessage msg = new MailMessage()) {
                if (isBodyHtml) {
                    this.Message += "<br/><br/>" + Resources.Controls.EmailMessageFooter;
                } else {
                    this.Message += (Environment.NewLine + Environment.NewLine + Resources.Controls.EmailMessageFooter);
                }

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
                    smtpClient.EnableSsl = useSSL;
                    smtpClient.Send(msg);

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
