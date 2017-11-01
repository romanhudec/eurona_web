using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;
using System.Web;

namespace Eurona.Common {
    public static class Application {
        public enum InstanceType {
            Eurona = 1,
            //Intensa = 2,
            CernyForLife = 3
        }

        public static bool IsDebugVersion {
            get {
#if __DEBUG_VERSION
				return true;
#else
                return false;
#endif
            }
        }

        public static BrowserInfo GetBrowserVersion(HttpRequest request) {
            BrowserInfo bi = new BrowserInfo();

            System.Web.HttpBrowserCapabilities browser = request.Browser;
            bi.Type = BrowserInfo.BrowserType.IE;
            bi.Name = browser.Browser;
            bi.Version = browser.Version;
            bi.Major = browser.MajorVersion;
            bi.Minor = browser.MinorVersion;

            string userAgent = request.ServerVariables["http_user_agent"];

            try {
                if (userAgent.IndexOf("Chrome", StringComparison.CurrentCultureIgnoreCase) != -1) {
                    bi.Name = "Chrome";
                    bi.Type = BrowserInfo.BrowserType.Chrome;
                    string[] variables = userAgent.Split(' ');
                    foreach (string variable in variables) {
                        if (variable.ToLower().Contains("chrome/")) {
                            bi.Version = variable.Remove(0, 7);
                            string[] v = bi.Version.Split('.');
                            bi.Major = Convert.ToInt32(v[0]);
                            if (v.Length >= 2) bi.Minor = Convert.ToDouble(v[1]);
                        }
                    }
                } else if (userAgent.IndexOf("Opera", StringComparison.CurrentCultureIgnoreCase) != -1) {
                    bi.Type = BrowserInfo.BrowserType.Opera;
                    string[] variables = userAgent.Split(' ');
                    foreach (string variable in variables) {
                        if (variable.ToLower().Contains("version/")) {
                            bi.Version = variable.Remove(0, 8);
                            string[] v = bi.Version.Split('.');
                            bi.Major = Convert.ToInt32(v[0]);
                            if (v.Length >= 2) bi.Minor = Convert.ToDouble(v[1]);
                        }
                    }
                } else if (userAgent.IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1) {
                    bi.Name = "Safari";
                    bi.Type = BrowserInfo.BrowserType.Safari;
                    string[] variables = userAgent.Split(' ');
                    foreach (string variable in variables) {
                        if (variable.ToLower().Contains("version/")) {
                            bi.Version = variable.Remove(0, 8);
                            string[] v = bi.Version.Split('.');
                            bi.Major = Convert.ToInt32(v[0]);
                            if (v.Length >= 2) bi.Minor = Convert.ToDouble(v[1]);
                        }
                    }
                } else if (userAgent.IndexOf("Firefox", StringComparison.CurrentCultureIgnoreCase) != -1) {
                    bi.Type = BrowserInfo.BrowserType.Firefox;
                }
            } catch {
                bi.Major = 0;
                bi.Minor = 0;
            }

            return bi;
        }

        public class BrowserInfo {
            public enum BrowserType {
                IE = 1,
                Chrome = 2,
                Firefox = 3,
                Opera = 4,
                Safari = 5
            }
            public BrowserType Type { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
            public int Major { get; set; }
            public double Minor { get; set; }

            public bool Validate() {
                bool isValid = true;
                switch (this.Type) {
                    case BrowserType.IE:
                        if (this.Major <= 7) isValid = false;
                        break;
                    case BrowserType.Firefox:
                        if (this.Major <= 9) isValid = false;
                        break;
                    case BrowserType.Chrome:
                        if (this.Major <= 19) isValid = false;
                        break;
                    case BrowserType.Opera:
                        if (this.Major <= 10) isValid = false;
                        break;
                    case BrowserType.Safari:
                        if (this.Major <= 4) isValid = false;
                        break;
                }

                return isValid;
            }
        }

        /// <summary>
        /// Trida popisujuca EURONA uzavireku objednavok pre Poradcov
        /// </summary>
        public static class EuronaUzavierka {
            public static bool IsUzavierka4Advisor() {
                UzavierkaEntity uzavierkaEntity = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
                if (uzavierkaEntity == null) return false;
                if (uzavierkaEntity.Povolena == false) return false;

                return (uzavierkaEntity.UzavierkaOd.HasValue && uzavierkaEntity.UzavierkaDo.HasValue) &&
                    (DateTime.Now >= uzavierkaEntity.UzavierkaOd.Value && DateTime.Now <= uzavierkaEntity.UzavierkaDo.Value);
            }

            public static DateTime GeUzavierka4AdvisorTo() {
                DateTime uzavierkaDo = DateTime.MaxValue;

                UzavierkaEntity uzavierkaEntity = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
                if (uzavierkaEntity == null) return uzavierkaDo;
                if (uzavierkaEntity.Povolena == false) return uzavierkaDo;
                if (uzavierkaEntity.UzavierkaDo.HasValue == false) return uzavierkaDo;

                return uzavierkaEntity.UzavierkaDo.Value;
            }
        }
        public static class CernyForLifeUzavierka {
            public static bool IsUzavierka4Advisor() {
                UzavierkaEntity uzavierkaEntity = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.CernyForLife });
                if (uzavierkaEntity == null) return false;
                if (uzavierkaEntity.Povolena == false) return false;

                return (uzavierkaEntity.UzavierkaOd.HasValue && uzavierkaEntity.UzavierkaDo.HasValue) &&
                    (DateTime.Now >= uzavierkaEntity.UzavierkaOd.Value && DateTime.Now <= uzavierkaEntity.UzavierkaDo.Value);
            }

            public static DateTime GeUzavierka4AdvisorTo() {
                DateTime uzavierkaDo = DateTime.MaxValue;

                UzavierkaEntity uzavierkaEntity = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.CernyForLife });
                if (uzavierkaEntity == null) return uzavierkaDo;
                if (uzavierkaEntity.Povolena == false) return uzavierkaDo;
                if (uzavierkaEntity.UzavierkaDo.HasValue == false) return uzavierkaDo;

                return uzavierkaEntity.UzavierkaDo.Value;
            }
        }

        /// <summary>
        /// Vrati datum s akym sa budu vytvarat objednavky
        /// </summary>
        public static DateTime CurrentOrderDate {
            get {
                DateTime currentOrderDate = DateTime.Now;

                UzavierkaEntity uzavierkaEntity = Storage<UzavierkaEntity>.ReadFirst();
                if (uzavierkaEntity == null) return currentOrderDate;
                if (uzavierkaEntity.Povolena == false) return currentOrderDate;

                if ((uzavierkaEntity.OperatorOrderOd.HasValue && uzavierkaEntity.OperatorOrderDo.HasValue) &&
                    (DateTime.Now >= uzavierkaEntity.OperatorOrderOd.Value && DateTime.Now <= uzavierkaEntity.OperatorOrderDo.Value)) {
                    if (uzavierkaEntity.OperatorOrderDate.HasValue) return uzavierkaEntity.OperatorOrderDate.Value;
                    else return currentOrderDate;
                }

                return currentOrderDate;
            }
        }
    }
}
