using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CMS {
    public static class Log {
        public static T Error<T>(Exception ex) {
            //ErrorODS.Save(ex, "BusinessLayer");
            //throw ex;
            ////return default(T);

            EvenLog.WritoToEventLog(ex);

            string value = ConfigurationManager.AppSettings["SMTP:Support"];
            if (value != null && !String.IsNullOrEmpty(value)) {
                ErrorEmailNotification email = new ErrorEmailNotification();
                email.To = value;
                email.Subject = string.Format("Application Exeption!");

                email.Message = "<html><body>";
                email.Message += "<style>body{font-size:10px;font-family:Verdana,Tahoma,Arial,sans-serif;}</style>";
                email.Message += "<b>Exception</b>";
                email.Message += "<br/>" + ex.Message;
                email.Message += "<br/> <b>Stack Trace :</b>";
                email.Message += "<br/>" + ex.StackTrace;
                if (ex.InnerException != null) {
                    email.Message += "<br/><b>Inner Exception</b>";
                    email.Message += "<br/>" + ex.InnerException.Message;
                    email.Message += "<br/> <b>Stack Trace :</b>";
                    email.Message += "<br/>" + ex.InnerException.StackTrace;
                }
                email.Message += "</body></html>";
                email.Notify(true);
            }

            throw ex;
        }

        public static void Error(Exception ex) {
            //ErrorODS.Save(ex, "BusinessLayer");
            //throw ex;
            ////return default(T);

            EvenLog.WritoToEventLog(ex);

            string value = ConfigurationManager.AppSettings["SMTP:Support"];
            if (value != null && !String.IsNullOrEmpty(value)) {
                ErrorEmailNotification email = new ErrorEmailNotification();
                email.To = value;
                email.Subject = string.Format("Application Exeption!");

                email.Message = "<html><body>";
                email.Message += "<style>body{font-size:10px;font-family:Verdana,Tahoma,Arial,sans-serif;}</style>";
                email.Message += "<b>Exception</b>";
                email.Message += "<br/>" + ex.Message;
                email.Message += "<br/> <b>Stack Trace :</b>";
                email.Message += "<br/>" + ex.StackTrace;
                if (ex.InnerException != null) {
                    email.Message += "<br/><b>Inner Exception</b>";
                    email.Message += "<br/>" + ex.InnerException.Message;
                    email.Message += "<br/> <b>Stack Trace :</b>";
                    email.Message += "<br/>" + ex.InnerException.StackTrace;
                }
                email.Message += "</body></html>";
                email.Notify(true);
            }

            throw ex;
        }
    }
}
