using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.DAL.Entities
{
	public class OrderFastView : CMS.Entities.Entity
	{
		public int InstanceId { get; set; }
		public DateTime OrderDate { get; set; }
		public string OrderNumber { get; set; }
		public int CartId { get; set; }
		public string OrderStatusCode { get; set; }
		public string ShipmentCode { get; set; }
		public decimal Price { get; set; }
		public decimal PriceWVAT { get; set; }

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

		public string OwnerName { get; set; }
		public int? TVD_Id { get; set; }
		/// <summary>
		/// Odkaz na objednavku, ku ktorej je pridruzena
		/// </summary>
		public int? ParentId { get; set; }

		public int CreatedByAccountId { get; set; }
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

		public class ReadByFilter
		{
			public int? AccountId { get; set; }
			public string OrderNumber { get; set; }
			public string OrderStatusCode { get; set; }
            public string OrderStatusName { get; set; }
			public string NotOrderStatusCode { get; set; }
			public int? ParentId { get; set; }
			public int? OnlyLastMonths { get; set; }
			public int? CreatedByAccountId { get; set; }
            public string OwnerName { get; set; }

            public bool IsEmpty() {
                return !AccountId.HasValue && string.IsNullOrEmpty(OrderNumber) && string.IsNullOrEmpty(OrderStatusName) &&
                    string.IsNullOrEmpty(NotOrderStatusCode) && !ParentId.HasValue && !OnlyLastMonths.HasValue &&
                    !CreatedByAccountId.HasValue && string.IsNullOrEmpty(OwnerName);
            }

            public bool IsEmptyExcludeLastMonths() {
                return !AccountId.HasValue && string.IsNullOrEmpty(OrderNumber) && string.IsNullOrEmpty(OrderStatusName) &&
                    string.IsNullOrEmpty(NotOrderStatusCode) && !ParentId.HasValue &&
                    !CreatedByAccountId.HasValue && string.IsNullOrEmpty(OwnerName);
            }
		}


		public string AssociationStatusText
		{
			get
			{
				if (!this.AssociationRequestStatus.HasValue) return string.Empty;
				switch ((Eurona.DAL.Entities.Order.AssociationStatus)this.AssociationRequestStatus)
				{
					case Eurona.DAL.Entities.Order.AssociationStatus.None:
						return string.Empty;
					case Eurona.DAL.Entities.Order.AssociationStatus.WaitingToAccept:
						return "Čeká na akceptaci";
					case Eurona.DAL.Entities.Order.AssociationStatus.Accepted:
						return "Přidružená";
					case Eurona.DAL.Entities.Order.AssociationStatus.Rejected:
						return "Přidružení zamítnuto";
					default:
						return string.Empty;
				}
			}
		}

		private CartEntity cartEntity = null;
		public CartEntity CartEntity
		{
			get
			{
				if (cartEntity != null) return cartEntity;
				cartEntity = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = this.CartId });
				return cartEntity;
			}
			set
			{
				cartEntity = value;
			}
		}
	}
}
