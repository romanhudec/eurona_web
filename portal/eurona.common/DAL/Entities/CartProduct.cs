using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class CartProduct : SHP.Entities.CartProduct {
        public decimal? Body { get; set; }
        public decimal? BodyCelkem { get; set; }

        public int? CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public int? MaximalniPocetVBaleni { get; set; }
        public int? MinimalniPocetVBaleni { get; set; }
        public bool CerpatBonusoveKredity { get; set; }

        /// <summary>
        /// Cena produktu po zlave s DPH
        /// </summary>
        public new decimal PriceWithDiscountWVAT {
            get { return this.PriceWithDiscount; }
        }

        public new class ReadByCartProduct {
            public int CartId { get; set; }
            public int ProductId { get; set; }
            public bool? CerpatBK { get; set; }
        }

        /// <summary>
        /// Katalogova cena s DPH
        /// </summary>
        public decimal KatalogPriceWVAT { get; set; }
        public decimal KatalogPriceWVATTotal { get; set; }
    }
}
