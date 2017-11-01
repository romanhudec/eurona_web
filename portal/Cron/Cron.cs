using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Mothiva.Cron {
    /// <summary>
    /// 
    /// original:
    /// 
    /// # .---------------- minute (0 - 59) 
    /// # |  .------------- hour (0 - 23)
    /// # |  |  .---------- day of month (1 - 31)
    /// # |  |  |  .------- month (1 - 12) OR jan,feb,mar,apr ... 
    /// # |  |  |  |  .---- day of week (0 - 6) (Sunday=0 or 7)  OR sun,mon,tue,wed,thu,fri,sat 
    /// # |  |  |  |  |
    ///   *  *  *  *  *  command to be executed
    /// 
    /// enhancement:
    /// 
    /// # .---------------- minute (0 - 59) 
    /// # |  .------------- hour (0 - 23)
    /// # |  |  .---------- day of month (1 - 31)
    /// # |  |  |  .------- month (1 - 12)
    /// # |  |  |  |  .---- day of week (0 - 6) (Sunday=0 or 7)
    /// # |  |  |  |  |  .- year (2000 - 2999)
    /// # |  |  |  |  |  |
    ///   *  *  *  *  *  *  :command to be executed
    /// 
    /// </summary>
    public class Cron {
        public class Entry {
            private static System.Diagnostics.TraceSwitch TraceGeneral = new System.Diagnostics.TraceSwitch("General", "General event log content level switch");
            private static readonly char[] SEP = new char[] { ' ', '\t' };

            public int Minute { get; set; }
            public int Hour { get; set; }
            public int DayOfMonth { get; set; }
            public int Month { get; set; }
            public int DayOfWeek { get; set; }
            public int Year { get; set; }

            internal string Name { get; set; }
            internal string Command { get; set; }
            private Dictionary<string, string> CommandParameters { get; set; }
            private ICronCommand CommandInterface { get; set; }
            private Thread EntryThread = null;

            private DateTime NextRunDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            private string Subst(string s) {
                return s == "*" ? "-1" : s;
            }

            private string Subst(int i) {
                return i == -1 ? "*" : i.ToString();
            }

            /// <summary>
            /// Setup NextRun command.
            /// </summary>
            private void SetupNextRun() {
                DateTime dateTime = this.NextRunDateTime;

                if (this.Month != -1)
                    dateTime = dateTime.AddYears(this.Month == -1 ? 0 : 1);//this.Months ( kazdy februar )
                else if (this.DayOfMonth != -1)
                    dateTime = dateTime.AddMonths(this.DayOfMonth == -1 ? 0 : 1);//this.DayOfMonth ( kazdeho 23. )
                else if (this.DayOfWeek != -1)
                    dateTime = dateTime.AddDays(this.DayOfWeek == -1 ? 0 : 7);//DayOfWeek ( kazdy utorok )
                else if (this.Hour != -1)
                    dateTime = dateTime.AddDays(this.Hour == -1 ? 0 : 1); // Spusti sa vzdy o 22 hodine dna.
                else if (this.Minute != -1)
                    dateTime = dateTime.AddHours(this.Minute == -1 ? 0 : 1); //Spusti sa vzdy o 15 minute hodiny

                //Minimalny krok - 1 minuta
                if (Minute == -1 && Hour == -1 && DayOfMonth == -1 &&
                Month == -1 && DayOfWeek == -1 && Year == -1) {
                    dateTime = dateTime.AddMinutes(1);
                    if (dateTime < DateTime.Now) {
                        DateTime now = DateTime.Now.AddMinutes(1);
                        dateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
                    }
                }

                this.NextRunDateTime = dateTime;
            }

            /// <summary>
            /// Metoda skontrolu platnost dalsieho spustenia prikazu.
            /// V pripade neaktualneho casu spustenia nastavi na spravny cas.
            /// </summary>
            public void CheckEntityRunTime() {
                DateTime nowDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                if (this.NextRunDateTime < nowDateTime)
                    SetupNextRun();
            }

            public Entry(ServiceSettings.CronEntryElement cee) {
                Minute = Int32.Parse(Subst(cee.Schedule.Minute));
                Hour = Int32.Parse(Subst(cee.Schedule.Hour));
                DayOfMonth = Int32.Parse(Subst(cee.Schedule.DayOfMonth));
                Month = Int32.Parse(Subst(cee.Schedule.Month));
                DayOfWeek = Int32.Parse(Subst(cee.Schedule.DayOfWeek));
                if (DayOfWeek == 7) DayOfWeek = 0;
                Year = Int32.Parse(Subst(cee.Schedule.Year));

                this.Name = cee.Name;
                this.Command = cee.Schedule.Command;
                this.CommandParameters = cee.Schedule.CommandParameters.ToDictionary();
                this.CommandInterface = GetCommandInterface(cee);

                //Inicializacia stratovacieho Datumu a casu
                this.NextRunDateTime = new DateTime(
                        Year == -1 ? DateTime.Now.Year : Year,
                        Month == -1 ? DateTime.Now.Month : Month,
                        DayOfMonth == -1 ? DateTime.Now.Day : DayOfMonth,
                        Hour == -1 ? DateTime.Now.Hour : Hour,
                        Minute == -1 ? DateTime.Now.Minute : Minute,
                        0);

                //Nastavenie dalsieho spustenia prikazu.
                if (this.NextRunDateTime < DateTime.Now)
                    SetupNextRun();
            }

            public bool Run() {
                if (this.EntryThread != null &&
                    (this.EntryThread.ThreadState == ThreadState.Running || this.EntryThread.ThreadState == ThreadState.WaitSleepJoin))
                    return false;

                DateTime nowDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                if (this.NextRunDateTime != nowDateTime)
                    return false;

                this.EntryThread = new Thread(new ThreadStart(OnRun));
                this.EntryThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                this.EntryThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                this.EntryThread.Name = this.Command;
                this.EntryThread.Start();

                return true;
            }

            public void Stop() {
                if (this.EntryThread != null)
                    this.CommandInterface.Break();
            }

            private void OnRun() {
                try {
                    this.CommandInterface.Exec(this.Command, this.CommandParameters);

                    //Nastavenie dalsieho spustenia prikazu.
                    SetupNextRun();

                    if (TraceGeneral.TraceInfo) {
                        string message = string.Format("Next run command {0} is set on {1}", this.Command, this.NextRunDateTime);
                        Diagnostics.Trace.WriteLine(CronService.ListenerName, message, Diagnostics.TraceCategory.Information);
                    }
                } catch (Exception ex) {
                    StringBuilder sbMessage = new StringBuilder();
                    sbMessage.Append("Mothiva.Cron ERROR");
                    sbMessage.AppendLine(ex.Message);
                    if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                    sbMessage.AppendLine(ex.StackTrace);

                    SendEmail("Mothiva.Cron ERROR", sbMessage.ToString());
                    Diagnostics.Trace.WriteLine(CronService.ListenerName, ex);
                }
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
                    Diagnostics.Trace.WriteLine(CronService.ListenerName, ex.Message);
                }
            }


            /// <summary>
            /// Get command interface from assembly.
            /// </summary>
            private ICronCommand GetCommandInterface(ServiceSettings.CronEntryElement cee) {
                List<ICronCommand> list = new List<ICronCommand>();

                string path = Assembly.GetEntryAssembly().Location;
                string directory = Path.GetDirectoryName(path);
                string pluginPath = Path.Combine(directory, cee.Assembly);

                Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);

                //Next we'll loop through all the Types found in the assembly
                foreach (Type pluginType in pluginAssembly.GetTypes()) {
                    //Only look at public types
                    if (!pluginType.IsPublic) continue;

                    //Only look at non-abstract types
                    if (pluginType.IsAbstract) continue;

                    //Gets a type object of the interface we need the plugins to match
                    Type typeInterface = pluginType.GetInterface("Mothiva.Cron.ICronCommand", true);

                    //Make sure the interface we want to use actually exists
                    if (typeInterface != null) {
                        ICronCommand newPlugin = (ICronCommand)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()),
                                new object[] { cee.Name });

                        return newPlugin;
                    }
                }

                return null;
            }


            private bool Test(int myPart, int part) {
                return myPart == part || myPart == -1;
            }

            internal bool Test(DateTime dateTime) {
                bool ok = true;
                ok = ok && Test(Minute, dateTime.Minute);
                ok = ok && Test(Hour, dateTime.Hour);
                ok = ok && Test(DayOfMonth, dateTime.Day);
                ok = ok && Test(Month, dateTime.Month);
                ok = ok && Test(DayOfWeek, (int)dateTime.DayOfWeek);
                ok = ok && Test(Year, dateTime.Year);
                return ok;
            }
        }

        private List<Entry> crontab = new List<Entry>();

        public Cron() {
        }

        public Entry Add(ServiceSettings.CronEntryElement cee) {
            Entry cronEntry = new Entry(cee);
            crontab.Add(cronEntry);
            return cronEntry;
        }

        public List<Entry> Process(DateTime dateTime) {
            List<Entry> passed = new List<Entry>();

            //Check entity run time
            foreach (Entry entity in crontab)
                entity.CheckEntityRunTime();

            passed.AddRange(crontab.FindAll(e => e.Test(dateTime)));
            return passed;
        }

        /// <summary>
        /// Stop all cron entries
        /// </summary>
        public void Stop() {
            foreach (Entry e in crontab)
                e.Stop();
        }

    }
}
