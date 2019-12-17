using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class ZavozoveMisto : CMS.Entities.Entity {
        public class ReadById {
            public int Id { get; set; }
        }
        public class ReadBy {
            public bool OsobniOdberVSidleSpolecnosti { get; set; }
        }
        public class ReadByMesto {
            public string Mesto { get; set; }
        }
        public class ReadJenAktualiByMesto {
            public string Mesto { get; set; }
        }
        public class ReadJenAktualiByKod {
            public int Kod { get; set; }
        }
        public class ReadOnlyMestoDistinctByStat {
            public string Stat { get; set; }
        }

        public string Stat { get; set; }
        public string Mesto { get; set; }
        public string Psc { get; set; }
        public int Kod { get; set; }
        public string Popis { get; set; }
        public DateTime? DatumACas { get; set; }
        public DateTime? DatumACas_Skryti { get; set; }
        public bool OsobniOdberVSidleSpolecnosti { get; set; }
        public string OsobniOdberPovoleneCasy { get; set; }
        public string OsobniOdberAdresaSidlaSpolecnosti { get; set; }

        public string DisplayMesto {
            get {
                if (string.IsNullOrEmpty(this.Popis)) return this.Mesto;
                return string.Format("{0}, {1}", this.Mesto, this.Popis);
            }
        }

        public static DateTime GetTimeFromString(string timeString) {
            string[] data = timeString.Split(':');
            int hours = Convert.ToInt32(data[0]);
            int minutes = Convert.ToInt32(data[1]);
            return new DateTime(1, 1, 1, hours, minutes, 0);
        }

        public static string GetStatByLocale(string locale) {
            if (locale.ToUpper() == "SK") return "SK";
            if (locale.ToUpper() == "PL") return "PL";
            return "CZ";
        }
    }

    public class ZavozoveMistoLimit {
        private string limitBuilderString = null;
        private List<Limit> limits = null;
        public ZavozoveMistoLimit(string limitBuilderString) {
            this.limitBuilderString = limitBuilderString;
            this.limits = new List<Limit>();
            if (this.limitBuilderString != null && this.limitBuilderString.Length != 0) {
                string[] limitsStrings = this.limitBuilderString.Split(';');
                foreach (string limitString in limitsStrings) {
                    Limit limit = new Limit(limitString);
                    if (!limit.IsValid()) continue;
                    this.limits.Add(limit);
                }
            }
        }

        public List<ZavozoveMistoLimit.Limit> GetData() {
            return limits;
        }

        public void Add(string limitString) {
            Limit limit = new Limit(limitString);
            if (!limit.IsValid()) return;
            this.limits.Add(limit);
        }

        public void Remove(String limitString) {
            foreach (Limit limit in this.limits) {
                if (limit.ToString() == limitString) {
                    this.limits.Remove(limit);
                    return;
                }
            }
        }

        public string ToDisplayString() {
            StringBuilder sb = new StringBuilder();
            foreach (Limit limit in this.limits) {
                if (sb.Length != 0) sb.Append("<br/>");
                sb.Append(limit.DisplayString);
            }

            return sb.ToString();

        }
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (Limit limit in this.limits) {
                if (sb.Length != 0) sb.Append(";");
                sb.Append(limit.ToString());
            }

            return sb.ToString();
        }

        public Boolean IsInLimit(DateTime date) {
            foreach (Limit limit in this.limits) {
                if (limit.IsInLimit(date)) return true;
            }
            return false;
        }

        public class Limit {
            private string limit = null;
            public Limit(string limit /*1|8:00-1|9:00*/ ) {
                this.limit = limit;
                string[] intervalString = this.limit.Split('-');
                if (intervalString.Length != 2) return;

                this.IntervalFrom = new LimitIntervalEntity(intervalString[0]);
                this.IntervalTo = new LimitIntervalEntity(intervalString[1]);
            }

            public static string GetStringLimit(int dayFrom, int hoursFrom, int minutesFrom, int dayTo, int hoursTo, int minutesTo) {
                return string.Format("{0}|{1}:{2}-{3}|{4}:{5}",
                    dayFrom, hoursFrom, minutesFrom, dayTo, hoursTo, minutesTo);
            }


            public Boolean IsValid() {
                return this.IntervalFrom != null && this.IntervalTo != null;
            }

            public string Id { get { return this.ToString(); } }
            public LimitIntervalEntity IntervalFrom { get; set; }
            public LimitIntervalEntity IntervalTo { get; set; }

            public String DisplayString {
                get {
                    string from = this.IntervalFrom.ToString();
                    string to = this.IntervalTo.ToString();

                    CultureInfo ci = Thread.CurrentThread.CurrentCulture;
                    string dayNameFrom = ci.DateTimeFormat.DayNames[(int)this.IntervalFrom.DayInWeek];
                    string dayNameTo = ci.DateTimeFormat.DayNames[(int)this.IntervalTo.DayInWeek];

                    string[] fromItems = from.Split('|');
                    string[] toItems = to.Split('|');

                    string fromTime = fromItems[1];
                    string toTime = toItems[1];

                    /*
                    from = from.Replace(this.IntervalFrom.DayInWeek.ToString() + "|", "od " + dayNameFrom + " ");
                    to = to.Replace(this.IntervalTo.DayInWeek.ToString() + "|", "do " + dayNameTo + " ");

                    return string.Format("{0} {1}", from, to);
                     * */

                    if (dayNameFrom != dayNameTo) {
                        return string.Format("{0} až {1} od {2} do {3}", dayNameFrom, dayNameTo, fromTime, toTime);
                    }
                    return string.Format("{0} od {1} do {2}", dayNameFrom, fromTime, toTime);
                }
            }

            public override string ToString() {
                return string.Format("{0}-{1}", this.IntervalFrom.ToString(), this.IntervalTo.ToString());
            }

            public Boolean IsInLimit(DateTime date) {
                int day = (int)date.DayOfWeek;
                if (day < this.IntervalFrom.DayInWeek) return false;
                if (day > this.IntervalTo.DayInWeek) return false;

                if (date.TimeOfDay >= this.IntervalFrom.Time.TimeOfDay && date.TimeOfDay <= this.IntervalTo.Time.TimeOfDay)
                    return true;

                return false;
            }

        }

        public class LimitIntervalEntity {
            public LimitIntervalEntity(string entityString) {
                string[] data = entityString.Split('|');

                this.DayInWeek = Convert.ToInt32(data[0]);
                this.Time = ZavozoveMisto.GetTimeFromString(data[1]);
            }

            public int DayInWeek { get; set; }
            public DateTime Time { get; set; }

            public override string ToString() {
                return string.Format("{0}|{1:00}:{2:00}", this.DayInWeek, this.Time.Hour, this.Time.Minute);
            }
        }

    }
    
}
