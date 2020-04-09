using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;

namespace QueueIT.KnownUserV3.SDK {
    public static class EvenLog {
        public static void WritoToEventLog(string message, EventLogEntryType evType) {
            try {
                //Nastavenie permission pre zapis do Event logu.
                WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero);

                string source = "QueueIT";
                EventLog.WriteEntry(source, message, evType);

                wic.Undo();
            } catch {
                ;
            }
        }

        public static void WritoToEventLog(Exception ex) {
            try {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.Source);
                sb.AppendLine(ex.StackTrace);
                if (ex.InnerException != null) {
                    sb.AppendLine("Inner exeption");
                    sb.AppendLine(ex.InnerException.Message);
                    sb.AppendLine(ex.InnerException.Source);
                    sb.AppendLine(ex.InnerException.StackTrace);
                }
                //Nastavenie permission pre zapis do Event logu.
                WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero);

                string source = "QueueIT";
                EventLog.WriteEntry(source, sb.ToString(), EventLogEntryType.Warning);

                wic.Undo();
            } catch {
                ;
            }
        }
    }
}
