using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    public class Order : Entity {
        public enum OrderStatus : int {
            None = 0,
            WaitingForProccess = -1,
            InProccess = -2,
            Proccessed = -3,
            Storno = -4
        }

        public static OrderStatus GetOrderStatusFromCode(string code) {
            if (string.IsNullOrEmpty(code))
                return OrderStatus.None;

            int codeId = 0;
            Int32.TryParse(code, out codeId);

            return (OrderStatus)codeId;
        }

        public Order() {
            this.OrderStatusCode = ((int)OrderStatus.WaitingForProccess).ToString();
        }

        public class ReadById {
            public int OrderId { get; set; }
        }

        public class ReadByAccount {
            public int AccountId { get; set; }
        }

        public class ReadByCart {
            public int CartId { get; set; }
        }

        public class ReadByAccountYearMonth {
            public int AccountId { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
        }

        public class ReadByFilter {
            public int? AccountId { get; set; }
            public string OrderNumber { get; set; }
            public string OrderStatusCode { get; set; }
            public string ShipmentCode { get; set; }
            public bool? Notified { get; set; }
            public bool? Exported { get; set; }
        }

        public int InstanceId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? PaydDate { get; set; }
        public int CartId { get; set; }
        public string OrderStatusCode { get; set; }
        public string ShipmentCode { get; set; }
        public string PaymentCode { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string InvoiceUrl { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public decimal PriceWVAT { get; set; }
        public bool Notified { get; set; }
        public bool Exported { get; set; }

        //Joined properties
        /// <summary>
        /// Id pouzivatela, ktoremu objednavka patri
        /// </summary>
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string OrderStatusName { get; set; }
        public string OrderStatusIcon { get; set; }
        public string ShipmentName { get; set; }
        public string ShipmentIcon { get; set; }
        public decimal? ShipmentPrice { get; set; }
        public decimal? ShipmentPriceWVAT { get; set; }
        public string PaymentName { get; set; }
        public string PaymentIcon { get; set; }


        public decimal ProductsPrice {
            get {
                return this.Price - (this.ShipmentPrice.HasValue ? this.ShipmentPrice.Value : 0m);
            }
        }
        public decimal ProductsPriceWVAT {
            get {
                return this.PriceWVAT - (this.ShipmentPriceWVAT.HasValue ? this.ShipmentPriceWVAT.Value : 0m);
            }
        }

        public bool Payed { get { return this.PaydDate.HasValue; } }


        /// <summary>
        /// Adresa pre dorucenie
        /// </summary>
        private Address deliveryAddress = null;
        public Address DeliveryAddress {
            get {
                if (this.deliveryAddress != null) return this.deliveryAddress;
                if (!this.DeliveryAddressId.HasValue) return null;
                this.deliveryAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.DeliveryAddressId.Value });
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
                if (!this.InvoiceAddressId.HasValue) return null;
                this.invoiceAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.InvoiceAddressId.Value });
                return this.invoiceAddress;
            }
        }

    }
}
