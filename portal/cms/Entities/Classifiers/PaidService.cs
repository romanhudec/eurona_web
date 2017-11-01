using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities.Classifiers
{
		public class PaidService : Entity
		{
				public class ReadById
				{
						public int PaidServiceId { get; set; }
				}

				public int InstanceId { get; set; }
				public string Name { get; set; }
				public string Notes { get; set; }
				public decimal CreditCost { get; set; }
		}
}
