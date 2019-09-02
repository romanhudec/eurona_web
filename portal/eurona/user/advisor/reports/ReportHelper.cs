using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.User.Advisor.Reports {
    public static class ReportHelper {
        public static int? NextLevel(object dbgrouppoints) {
            if (dbgrouppoints == DBNull.Value) return null;

            int grouppoints = (int)dbgrouppoints;
            if (grouppoints < 200) return 3;
            if (grouppoints < 600) return 6;
            if (grouppoints < 1200) return 9;
            if (grouppoints < 2400) return 12;
            if (grouppoints < 4000) return 15;
            if (grouppoints < 6600) return 18;
            if (grouppoints < 10000) return 21;
            return 21;
        }
        public static int? RestMarginLevel(object dbmargin) {
            if (dbmargin == DBNull.Value) return null;
            Decimal margin = (Decimal)dbmargin;
            if (margin < 19) return 19;
            if (margin < 24) return 24;
            if (margin < 29) return 29;
            return 29;
        }
        public static decimal? RestMarginPrice(int marginlevel, object dbmarginprice, object dbcurrencycode) {
            if (dbmarginprice == DBNull.Value) return null;
            if (dbcurrencycode == DBNull.Value) return null;

            string currencycode = dbcurrencycode.ToString();
            Decimal marginprice = (Decimal)dbmarginprice;

            if (currencycode == "CZK") {
                if (marginlevel == 24) return (marginprice < 2500) ? 2500 - marginprice : 0;
                else if (marginlevel == 29) return (marginprice < 5000) ? 5000 - marginprice : 0;
                else return 0;
            }
            if (currencycode == "PLN") {
                if (marginlevel == 24) return (marginprice < 350) ? 350 - marginprice : 0;
                else if (marginlevel == 29) return (marginprice < 700) ? 700 - marginprice : 0;
                else return 0;
            }
            if (currencycode == "SKK") {
                if (marginlevel == 24) return (marginprice < 3000) ? 3000 - marginprice : 0;
                if (marginlevel == 29) return (marginprice < 6000) ? 6000 - marginprice : 0;
                return 0;
            }
            return 0;
        }

        public static decimal? RestMarginBody(int body, object dbmarginBody) {
            if (dbmarginBody == DBNull.Value) return body;
            int marginBody = (int)dbmarginBody;
            if (marginBody > body) return 0;
            return body - marginBody;
        }

        public static int? RestPoints(object dbgrouppoints) {
            if (dbgrouppoints == DBNull.Value) return null;

            int grouppoints = (int)dbgrouppoints;
            if (grouppoints < 200) return 200 - grouppoints;
            if (grouppoints < 600) return 600 - grouppoints;
            if (grouppoints < 1200) return 1200 - grouppoints;
            if (grouppoints < 2400) return 2400 - grouppoints;
            if (grouppoints < 4000) return 4000 - grouppoints;
            if (grouppoints < 6600) return 6600 - grouppoints;
            if (grouppoints < 10000) return 10000 - grouppoints;
            return 0;
        }

        public static string PriceCurrency(object dbprice) {
            if (dbprice == DBNull.Value) return string.Empty;

            string currency = dbprice.ToString();
            if (currency == "CZK") return "Kč";
            if (currency == "PLN") return "Zl";
            if (currency == "SKK") return "SK";
            if (currency == "EUR") return "Eur";
            return "";
        }

        public static bool GreaterThanZero(object dbnumber) {
            if (dbnumber is DBNull) return false;
            return (int)((decimal)dbnumber) > 0;
        }

        public static bool ShowBonusRubin(int number, object dbnumber) {
            if (dbnumber is DBNull) return false;
            int value = Convert.ToInt32(dbnumber);
            if (value == 0) return false;
            return number <= Convert.ToInt32(dbnumber);
        }

        public static int SumCredit(object dbeb, object dbrb, object dbsb, object dbbb) {
            int sum = 0;
            if (!(dbeb is DBNull)) sum += (int)((decimal)dbeb);
            if (!(dbrb is DBNull)) sum += (int)((decimal)dbrb);
            if (!(dbsb is DBNull)) sum += (int)((decimal)dbsb);
            if (!(dbbb is DBNull)) sum += (int)((decimal)dbbb);

            return sum;
        }

        public static int? ActualCredit(object dbmarginprice, object dbcurrencycode) {
            if (dbmarginprice == DBNull.Value) return null;
            if (dbcurrencycode == DBNull.Value) return null;

            string currencycode = dbcurrencycode.ToString();
            Decimal marginprice = (Decimal)dbmarginprice;
            int limit = 0;
            int step = 0;

            if (currencycode == "CZK") {
                limit = 5000;
                step = 1500;
            } else if (currencycode == "PLN") {
                limit = 700;
                step = 350;
            } else if (currencycode == "SKK") {
                limit = 6000;
                step = 2000;
            } else
                return 0;

            if (marginprice < limit)
                return 0;
            else {
                int actual = ((int)marginprice - limit) / step + 1;
                return actual;
            }
        }

        public static decimal? RestMarginCredit(object dbmarginprice, object dbcurrencycode) {
            if (dbmarginprice == DBNull.Value) return null;
            if (dbcurrencycode == DBNull.Value) return null;

            string currencycode = dbcurrencycode.ToString();
            Decimal marginprice = (Decimal)dbmarginprice;
            int limit = 0;
            int step = 0;

            if (currencycode == "CZK") {
                limit = 5000;
                step = 1500;
            } else if (currencycode == "PLN") {
                limit = 700;
                step = 350;
            } else if (currencycode == "SKK") {
                limit = 6000;
                step = 2000;
            } else
                return 0;

            if (marginprice < limit)
                return limit - marginprice;
            else {
                int actual = ((int)marginprice - limit) / step + 1;
                return (actual) * step - (marginprice - limit);
            }
        }
    }
}