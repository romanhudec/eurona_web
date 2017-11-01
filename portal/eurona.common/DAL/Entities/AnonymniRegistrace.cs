using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Globalization;
using System.Threading;

namespace Eurona.Common.DAL.Entities
{
    public class AnonymniRegistrace : CMS.Entities.Entity
    {
        public enum AnonymniRegistraceId : int
        {
            Eurona = -1001,
            CernyForLife = -3001
        }

        public class ReadById
        {
            public int AnonymniRegistraceId { get; set; }
        }

        //Id
        public bool ZobrazitVSeznamuNeomezene { get; set; }
        public int? ZobrazitVSeznamuDni { get; set; }
        public int? ZobrazitVSeznamuHodin { get; set; }
        public int? ZobrazitVSeznamuMinut { get; set; }

        public int? EuronaReistraceProcent { get; set; }
        public int EuronaReistracePocitadlo { get; set; }
        public int? MaxPocetPrijetychNovacku { get; set; }
        public string ZobrazitVSeznamuLimit { get; set; }
    }


    public class AnonymniRegistraceLimit
    {
        private string vSeznamuLimit = null;
        private List<Limit> limits = null;
        public AnonymniRegistraceLimit(string vSeznamuLimit)
        {
            this.vSeznamuLimit = vSeznamuLimit;
            this.limits = new List<Limit>();
            if (this.vSeznamuLimit != null && this.vSeznamuLimit.Length != 0)
            {
                string[] limitsStrings = this.vSeznamuLimit.Split(';');
                foreach (string limitString in limitsStrings)
                {
                    Limit limit = new Limit(limitString);
                    if (!limit.IsValid()) continue;
                    this.limits.Add(limit);
                }
            }
        }

        public List<AnonymniRegistraceLimit.Limit> GetData()
        {
            return limits;
        }

        public void Add(string limitString)
        {
            Limit limit = new Limit(limitString);
            if (!limit.IsValid()) return;
            this.limits.Add(limit);
        }

        public void Remove(String limitString)
        {
            foreach (Limit limit in this.limits)
            {
                if (limit.ToString() == limitString)
                {
                    this.limits.Remove(limit);
                    return;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Limit limit in this.limits)
            {
                if (sb.Length != 0) sb.Append(";");
                sb.Append(limit.ToString());
            }

            return sb.ToString();
        }

        public Boolean IsInLimitForATPClen(DateTime date)
        {
            foreach (Limit limit in this.limits)
            {
                if (limit.IsInLimitForATPClen(date)) return true;
            }
            return false;
        }

        public Boolean IsInLimitForATPManager(DateTime date)
        {
            foreach (Limit limit in this.limits)
            {
                if (limit.IsInLimitForATPManager(date)) return true;
            }
            return false;
        }

        public class Limit
        {
            private string limit = null;
            public Limit(string limit /*1|8:00-1|9:00*/ )
            {
                this.limit = limit;
                string[] intervalString = this.limit.Split('-');
                if (intervalString.Length != 2) return;

                this.IntervalFrom = new LimitIntervalEntity(intervalString[0]);
                this.IntervalTo = new LimitIntervalEntity(intervalString[1]);
            }

            public static string GetStringLimit(int dayFrom, int hoursFrom, int minutesFrom, int dayTo, int hoursTo, int minutesTo)
            {
                return string.Format("{0}|{1}:{2}-{3}|{4}:{5}",
                    dayFrom, hoursFrom, minutesFrom, dayTo, hoursTo, minutesTo);
            }


            public Boolean IsValid()
            {
                return this.IntervalFrom != null && this.IntervalTo != null;
            }

            public string Id { get { return this.ToString(); } }
            public LimitIntervalEntity IntervalFrom { get; set; }
            public LimitIntervalEntity IntervalTo { get; set; }

            public String DisplayString
            {
                get
                {
                    string from = this.IntervalFrom.ToString();
                    string to = this.IntervalTo.ToString();

                    CultureInfo ci = Thread.CurrentThread.CurrentCulture;
                    string dayNameFrom = ci.DateTimeFormat.DayNames[(int)this.IntervalFrom.DayInWeek];
                    string dayNameTo = ci.DateTimeFormat.DayNames[(int)this.IntervalTo.DayInWeek];

                    from = from.Replace(this.IntervalFrom.DayInWeek.ToString() + "|", "od " + dayNameFrom + " ");
                    to = to.Replace(this.IntervalTo.DayInWeek.ToString() + "|", "do " + dayNameTo + " ");

                    return string.Format("{0} {1}", from, to);
                }
            }

            public override string ToString()
            {
                return string.Format("{0}-{1}", this.IntervalFrom.ToString(), this.IntervalTo.ToString());
            }

            public Boolean IsInLimitForATPClen(DateTime date)
            {
                int day = (int)date.DayOfWeek;
                if (day < this.IntervalFrom.DayInWeek) return false;
                if (day > this.IntervalTo.DayInWeek) return false;

                if (date.TimeOfDay > this.IntervalFrom.Time.TimeOfDay && date.TimeOfDay < this.IntervalTo.Time.TimeOfDay)
                    return true;

                return false;
            }

            /// <summary>
            /// True ak je to v dany den nasledujucu hodinu po limite
            /// </summary>
            /// <param name="date"></param>
            /// <returns></returns>
            public Boolean IsInLimitForATPManager(DateTime date)
            {
                int day = (int)date.DayOfWeek;
                if (day < this.IntervalFrom.DayInWeek) return false;
                if (day > this.IntervalTo.DayInWeek) return false;

                DateTime toTime = this.IntervalTo.Time.AddHours(1);
                if (date.TimeOfDay > this.IntervalTo.Time.TimeOfDay && date.TimeOfDay < toTime.TimeOfDay)
                    return true;

                return false;
            }
        }

        public class LimitIntervalEntity
        {
            public LimitIntervalEntity(string entityString)
            {
                string[] data = entityString.Split('|');

                this.DayInWeek = Convert.ToInt32(data[0]);
                this.Time = this.GetTimeFromString(data[1]);
            }

            public int DayInWeek { get; set; }
            public DateTime Time { get; set; }

            private DateTime GetTimeFromString( string timeString)
            {
                string[] data = timeString.Split(':');
                int hours = Convert.ToInt32(data[0]);
                int minutes = Convert.ToInt32(data[1]);
                return new DateTime(1, 1, 1, hours, minutes, 59);
            }

            public override string ToString()
            {
                return string.Format("{0}|{1}:{2}", this.DayInWeek, this.Time.Hour, this.Time.Minute);
            }
        }
    }

}
