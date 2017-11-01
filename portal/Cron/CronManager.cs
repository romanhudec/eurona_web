using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Mothiva.Cron {
    /// <summary>
    /// Implementácia manažera, ktorý riadi vykonávanie činností služby a zabezpečuje
    /// komunukáciu s radičom služby.
    /// </summary>
    sealed class CronManager {
        private static TraceSwitch TraceGeneral = new TraceSwitch("General", "General event log content level switch");

        private object syncRoot = new object();
        private TimeSpan periodTimeout = new TimeSpan(0, 0, 30);
        private const int MaxConnectionAttempts = 60;

        private Cron cron = null;
        private CronService service = null;
        private Thread thread = null;
        private ManualResetEvent eventCancel;
        private SmptNotification notification;

        public CronManager(CronService service, Cron cron) {
            this.eventCancel = new ManualResetEvent(false);
            this.service = service;
            this.cron = cron;
            this.notification = new SmptNotification(service);
        }

        public CronService Service {
            get { return this.service; }
        }
        public Cron Cron {
            get { return this.cron; }
        }

        public EventLog EventLog {
            get { return this.service.EventLog; }
        }

        #region Thread control and synchronization

        public bool IsAlive {
            get {
                Thread obj = null;

                lock (this.syncRoot) {
                    obj = this.thread;
                }

                if (obj == null)
                    return false;

                return obj.IsAlive;
            }
        }

        public void Cancel() {
            lock (this.syncRoot) {
                this.eventCancel.Set();
            }
        }

        public bool CancelPending() {
            return this.eventCancel.WaitOne(0, false);
        }

        public bool CancelPending(TimeSpan timeout) {
            return this.eventCancel.WaitOne(timeout, false);
        }

        public void Join() {
            Thread obj = null;

            lock (this.syncRoot) {
                obj = this.thread;
            }

            if (obj == null)
                return;

            obj.Join();
        }

        public void Join(TimeSpan timeout) {
            Thread obj = null;

            lock (this.syncRoot) {
                obj = this.thread;
            }

            if (obj == null)
                return;

            obj.Join(timeout);
        }

        #endregion

        public void Run() {
            lock (this.syncRoot) {
                if (this.thread != null)
                    return;
            }

            //Nastavenie periody spracovavania.
            this.periodTimeout = new TimeSpan(0, 0, 25);

            this.thread = new Thread(new ThreadStart(OnRun));
            this.thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            this.thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            this.thread.Name = "CronManager";
            this.thread.Start();
        }

        private void OnRun() {
            try {
                Thread.Sleep(1000);

                if (TraceGeneral.TraceInfo) {
                    this.EventLog.WriteEntry("Cron manager successfully started!", EventLogEntryType.Information);
                    Diagnostics.Trace.WriteLine(CronService.ListenerName, "Cron manager successfully started!", null, Diagnostics.TraceCategory.Information);
                }

                while (true) {
                    if (CancelPending(this.periodTimeout)) {
                        this.Cron.Stop();
                        break;
                    }

                    // TODO: Perform an agent actions here in the context of service.
                    List<Cron.Entry> passed = this.Cron.Process(DateTime.Now);
                    foreach (Cron.Entry e in passed) {
                        string message = string.Format("Run entry '{0}' with command '{1}'", e.Name, e.Command);

                        if (e.Run())
                            Diagnostics.Trace.WriteLine(CronService.ListenerName, message, Diagnostics.TraceCategory.Information);
                    }
                }
            } catch (System.Exception ex) {
                //Nastala neodchytená výnimka.
                string message = String.Format("Cron manager internal error : {0}", ex.Message);
                LogManagerException(message, ex, true);
            } finally {
                this.eventCancel.Reset();

                lock (this.syncRoot) {
                    this.thread = null;
                }

                if (TraceGeneral.TraceInfo) {
                    this.EventLog.WriteEntry("Cron manager successfully stoped!", EventLogEntryType.Information);
                    Diagnostics.Trace.WriteLine(CronService.ListenerName, "Cron manager successfully stoped!", null, Diagnostics.TraceCategory.Information);
                }

            }
        }

        #region Logging & helper methods
        /// <summary>
        /// Zaloguje výnimku agenta s možnostou E-mail notifikácie.
        /// </summary>
        private void LogManagerException(string message, Exception ex, bool mailNotication) {
            this.EventLog.WriteEntry(message, EventLogEntryType.Error);

#if _OFFLINE_DEBUG
						message = string.Format( "OFFLINE_DEBUG MODE !!!:{0}", message );
#else
            if (mailNotication)
                this.notification.Send(this.service.ServiceName, message);
#endif
            if (ex != null) {
                StringBuilder sbMessage = new StringBuilder();
                sbMessage.Append(message);

                if (ex.InnerException != null) sbMessage.AppendFormat("\n{0}", ex.InnerException.Message);
                sbMessage.Append(ex.StackTrace);
                Trace.WriteLine(CronService.ListenerName, sbMessage.ToString());
                SendEmail("Mothiva.Cron ERROR", sbMessage.ToString());
            } else
                Trace.WriteLine(CronService.ListenerName, message);
        }

        /// <summary>
        /// Odosle error email na adresu "SMTP:To"
        /// </summary>
        private void SendEmail(string subject, string messsage) {
            try {
                Mothiva.Cron.Email email = new Mothiva.Cron.Email();
                email.Subject = subject;
                email.Message = messsage;
                email.Send();
            } catch (Exception ex) {
                Trace.WriteLine(CronService.ListenerName, ex.Message);
            }
        }

        #endregion
    }
}
