using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    public class Cart : Entity {
        public Cart() {
        }

        public class ReadById {
            public int CartId { get; set; }
        }

        public class ReadOpenByAccount {
            public int AccountId { get; set; }
        }
        public class ReadByAccount {
            public int AccountId { get; set; }
        }

        public class ReadBySessionId {
            public int SessionId { get; set; }
        }

        public int InstanceId { get; set; }
        public int? AccountId { get; set; }
        public int? SessionId { get; set; }
        public string ShipmentCode { get; set; }
        public string PaymentCode { get; set; }
        public int DeliveryAddressId { get; set; }
        public int InvoiceAddressId { get; set; }
        public string Notes { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Closed { get; set; }
        /// <summary>
        /// Celková suma za všetky produkty v nákupnom košíku.
        /// </summary>
        public decimal? PriceTotal { get; set; }

        /// <summary>
        /// Celková suma za všetky produkty v nákupnom košíku s DPH.
        /// </summary>
        public decimal? PriceTotalWVAT { get; set; }

        //FK

        /// <summary>
        /// Zoznam produktov v nakupnom kosiku.
        /// </summary>
        private List<CartProduct> cartProducts = null;
        public List<CartProduct> CartProducts {
            get {
                if (this.cartProducts != null) return this.cartProducts;
                this.cartProducts = Storage<CartProduct>.Read(new CartProduct.ReadByCart { CartId = this.Id });
                return this.cartProducts;
            }
        }

        private Classifiers.Shipment shipment = null;
        public Classifiers.Shipment Shipment {
            get {
                if (this.shipment != null) return this.shipment;
                this.shipment = Storage<Classifiers.Shipment>.ReadFirst(new Classifiers.Shipment.ReadByCode { Code = this.ShipmentCode });
                return this.shipment;
            }
        }

        /// <summary>
        /// Počet produktov v nákupnom košiku
        /// </summary>
        public int CartProductsCount {
            get {
                int count = 0;
                foreach (CartProduct cp in this.CartProducts)
                    count += cp.Quantity;

                return count;
            }
        }

        /// <summary>
        /// Adresa pre dorucenie
        /// </summary>
        private Address deliveryAddress = null;
        public Address DeliveryAddress {
            get {
                if (this.deliveryAddress != null) return this.deliveryAddress;
                this.deliveryAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.DeliveryAddressId });
                return this.deliveryAddress;
            }
        }

        /// <summary>
        /// Fakturacna adresa
        /// </summary>
        private Address invoiceAddress = null;
        public Address InvoiceAddress {
            get {
                if (this.invoiceAddress != null) return this.invoiceAddress;
                this.invoiceAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.InvoiceAddressId });
                return this.invoiceAddress;
            }
        }
    }
}
