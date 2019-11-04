using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.DAL.Entities {
    [Serializable]
    public class Order : Eurona.Common.DAL.Entities.Order {
        public enum AssociationStatus {
            None = 0,
            WaitingToAccept = 1,
            Accepted = 2,
            Rejected = 3
        }

        public int? TVD_Id { get; set; }
        /// <summary>
        /// Odkaz na objednavku, ku ktorej je pridruzena
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Pridruzienie tejto objednavky k objednavke pouzivatela
        /// </summary>
        public int? AssociationAccountId { get; set; }
        /// <summary>
        /// Status poziadavky na pridruzenie
        /// </summary>
        public int? AssociationRequestStatus { get; set; }

        /// <summary>
        /// Bez poštovného
        /// </summary>
        public bool NoPostage { get; set; }

        public string ZavozoveMisto_Mesto { get; set; }
        public DateTime? ZavozoveMisto_DatumACas { get; set; }

        public class ReadLastByAccount {
            public int AccountId { get; set; }
            public string GreaterAtOrderNumber { get; set; }
        }

        public new class ReadByFilter {
            public int? AccountId { get; set; }
            public string OrderNumber { get; set; }
            public string OrderStatusCode { get; set; }
            public string NotOrderStatusCode { get; set; }
            public string ShipmentCode { get; set; }
            public bool? Notified { get; set; }
            public bool? Exported { get; set; }
            public int? ParentId { get; set; }
            public int? AssociationAccountId { get; set; }
            public int? AssociationRequestStatus { get; set; }
            public int? CreatedByAccountId { get; set; }
            public bool? HasChilds { get; set; }
            public int? OnlyLastMonths { get; set; }
        }

        public class ReadNot {
            public string OrderStatusCode { get; set; }
            public int? AccountId { get; set; }
        }

        public string AssociationStatusText {
            get {
                if (!this.AssociationRequestStatus.HasValue) return string.Empty;
                switch ((AssociationStatus)this.AssociationRequestStatus) {
                    case AssociationStatus.None:
                        return string.Empty;
                    case AssociationStatus.WaitingToAccept:
                        return "Čeká na akceptaci";
                    case AssociationStatus.Accepted:
                        return "Přidružená";
                    case AssociationStatus.Rejected:
                        return "Přidružení zamítnuto";
                    default:
                        return string.Empty;
                }
            }
        }

        private CartEntity cartEntity = null;
        public CartEntity CartEntity {
            get {
                if (cartEntity != null) return cartEntity;
                cartEntity = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = this.CartId });
                return cartEntity;
            }
            set {
                cartEntity = value;
            }
        }
    }
}
