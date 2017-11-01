using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    public class Settings : CMS.Entities.Entity {
        public Settings() {
            this.Name = string.Empty;
            this.Code = string.Empty;
            this.GroupName = string.Empty;
            this.Value = string.Empty;
        }
        public class ReadById {
            public int Id { get; set; }
        }
        public class ReadByCode {
            public string Code { get; set; }
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string GroupName { get; set; }
        public string Value { get; set; }

        public static bool IsPaymentAfterLimit(Order order) {
            int limit = GetPlatbaKartouLimit();
            if (limit <= 0) return false;

            DateTime dateFrom = order.OrderDate;
            DateTime dateTo = dateFrom.AddMinutes(limit);
            int seconds = (int)(dateTo - DateTime.Now).TotalSeconds;
            return seconds <= 0;
        }

        public static int GetPlatbaKartouLimit() {
            Settings cardPaySettings = Storage<Settings>.ReadFirst(new Settings.ReadByCode { Code = "ESHOP_PLATBAKARTOU_LIMIT" });
            if (cardPaySettings == null || String.IsNullOrEmpty(cardPaySettings.Value)) return 0;
            return Convert.ToInt32(cardPaySettings.Value);
        }

        public static bool IsPlatbaKartouPovolena() {
            Settings cardPaySettings = Storage<Settings>.ReadFirst(new Settings.ReadByCode { Code = "ESHOP_PLATBAKARTOU" });
            if (cardPaySettings == null || String.IsNullOrEmpty(cardPaySettings.Value)) return true;
            return Convert.ToBoolean(cardPaySettings.Value);
        }
        public static void SetPlatbaKartouPovolena(bool povolena) {
            Settings cardPaySettings = Storage<Settings>.ReadFirst(new Settings.ReadByCode { Code = "ESHOP_PLATBAKARTOU" });
            if (cardPaySettings == null) return;

            cardPaySettings.Value = povolena.ToString();
            Storage<Settings>.Update(cardPaySettings);
        }

        public static bool IsZdruzeneObjednavkyPovolena() {
            Settings settings = Storage<Settings>.ReadFirst(new Settings.ReadByCode { Code = "ESHOP_ZDRUZENE_OBJEDNAVKY" });
            if (settings == null || String.IsNullOrEmpty(settings.Value)) return false;
            return Convert.ToBoolean(settings.Value);
        }

        public static bool IsPlatbaKartou4ZdruzeneObjednavkyPovolena() {
            Settings cardPaySettings = Storage<Settings>.ReadFirst(new Settings.ReadByCode { Code = "ESHOP_PLATBAKARTOU_ZDRUZENE_OBJEDNAVKY" });
            if (cardPaySettings == null || String.IsNullOrEmpty(cardPaySettings.Value)) return false;
            return Convert.ToBoolean(cardPaySettings.Value);
        }

        public static VysypaniVsechKosikuValue ParseVysypaniVsechKosikuStringValue(Settings settingsVysypaniVsechKosiku) {
            VysypaniVsechKosikuValue value = new VysypaniVsechKosikuValue();
            value.Povelena = false;
            value.Cas = "00:00";

            if (string.IsNullOrEmpty(settingsVysypaniVsechKosiku.Value))
                return value;

            string[] data = settingsVysypaniVsechKosiku.Value.Split(';');
            if (data.Length == 2) {
                value.Povelena = Convert.ToBoolean(data[0]);
                value.Cas = data[1];
            }
            return value;
        }

        public class VysypaniVsechKosikuValue {
            public bool Povelena { get; set; }
            public string Cas { get; set; }
        }
    }
}
