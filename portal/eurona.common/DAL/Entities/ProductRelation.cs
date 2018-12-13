using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class ProductRelation : SHP.Entities.ProductRelation {
        public int? CurrencyId { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Cena celkom zo zlavou s DPH 
        /// </summary>
        public new decimal PriceTotalWVAT {
            get { return this.PriceTotal; }
            set { base.PriceTotalWVAT = value; }
        }
    }
}
