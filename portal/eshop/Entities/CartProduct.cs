using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class CartProduct : Entity {
        public CartProduct() {
            this.Quantity = 1;
        }

        public class ReadById {
            public int CartProductId { get; set; }
        }

        public class ReadByCart {
            public int CartId { get; set; }
            public string Locale { get; set; }
        }

        public class ReadByCartProduct {
            public int CartId { get; set; }
            public int ProductId { get; set; }
        }

        public class ReadByAccount {
            public int AccountId { get; set; }
        }

        public int InstanceId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        /// <summary>
        /// Množstvo
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Cena produktu bez DPH
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Cena produktu sDPH
        /// </summary>
        public decimal PriceWVAT { get; set; }
        /// <summary>
        /// DPH
        /// </summary>
        public decimal VAT { get; set; }
        /// <summary>
        /// Suma zlavy na produkt bez DPH
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Celkova cena = cena zo zlavou * mnozstvo
        /// </summary>
        public decimal PriceTotal { get; set; }
        /// <summary>
        /// Celkova cena = cena zo zlavou * mnozstvo s DPH
        /// </summary>
        public decimal PriceTotalWVAT { get; set; }
        /// <summary>
        /// Cena produktu po zlave bez DPH
        /// </summary>
        public decimal PriceWithDiscount {
            get { return this.Price - this.Discount; }
        }
        /// <summary>
        /// Cena produktu po zlave s DPH
        /// </summary>
        public decimal PriceWithDiscountWVAT {
            get {
                if (this.Discount == 0) return this.PriceWVAT;
                decimal p = (this.Price - this.Discount) * (1 + this.VAT / 100m);
                return Math.Round(p * (decimal)Math.Pow(10, 2)) / (decimal)Math.Pow(10, 2);
            }
        }
        //Joined properties
        public int? AccountId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductAvailability { get; set; }
        public string Alias { get; set; }//UrlAlias na produkt

    }
}
