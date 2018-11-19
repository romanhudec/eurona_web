using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHP.Entities.Classifiers {
    public class Highlight : CMS.Entities.Classifiers.ClassifierBase { }
    public class OrderStatus : CMS.Entities.Classifiers.ClassifierBase { }
    public class Shipment : CMS.Entities.Classifiers.ClassifierBase {
        public decimal? Price { get; set; }
        public int? VATId { get; set; }
        public decimal? PriceWVAT { get; set; }
        public decimal? VAT { get; set; }
        public string VATDisplay {
            get {
                if (!this.VAT.HasValue) return string.Empty;
                return string.Format("{0}%", this.VAT.Value);
            }
        }
        public string Display {
            get {
                return string.Format("{0} - {1}", this.Name, Utilities.CultureUtilities.CurrencyInfo.ToString(this.Price.HasValue ? this.Price.Value : 0m, null));
            }
        }

        public class ReadDefault{
        }
    }
    public class Payment : CMS.Entities.Classifiers.ClassifierBase { }
    public class VAT : CMS.Entities.Classifiers.ClassifierBase {
        public decimal? Percent { get; set; }
        public string Display {
            get { return string.Format("{0}%", this.Percent); }
        }
    }
    public class Currency : CMS.Entities.Classifiers.ClassifierBase {
        public decimal? Rate { get; set; }
        public string Symbol { get; set; }

        public class ReadByRate {
            public decimal Rate { get; set; }
        }
        public class ReadByLocale {
            public string Locale { get; set; }
        }
    }
}
