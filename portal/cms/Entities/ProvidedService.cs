using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class ProvidedService: Entity
		{
				public class ReadById
				{
						public int ProvidedServiceId { get; set; }
				}
				public class ReadByAccountId
				{
						public int AccountId { get; set; }
				}
				public class ReadByPaidServiceId
				{
						public int PaidServiceId { get; set; }
				}

				public class ReadBy
				{
						public int ObjectId { get; set; }
						public int PaidServiceId { get; set; }
				}

				public int InstanceId { get; set; }
				public int AccountId { get; set; }
				public int PaidServiceId { get; set; }
				public int? ObjectId { get; set; }
				public decimal CreditCost { get; set; }
				public string Name { get; set; }
				public string Notes { get; set; }
				public DateTime ServiceDate { get; set; }
		}
}
