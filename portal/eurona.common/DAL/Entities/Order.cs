using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities
{
	public class Order : SHP.Entities.Order
	{
		public int? CurrencyId { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrencySymbol { get; set; }

		public DateTime? ShipmentFrom { get; set; }
		public DateTime? ShipmentTo { get; set; }

		/// <summary>
		/// Id pouzivatela, ktory objednavku vytvoril.
		/// </summary>
		public int CreatedByAccountId { get; set; }

		//JOINED
		public string OwnerName { get; set; }
		public string CreatedByName { get; set; }
	}
}
