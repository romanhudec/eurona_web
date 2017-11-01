using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class BankContact: Entity
		{
				public class ReadById
				{
						public int BankContactId { get; set; }
				}

				public int InstanceId { get; set; }
				public string BankName { get; set; }
				public string BankCode { get; set; }
				public string AccountNumber { get; set; }
				public string IBAN { get; set; }
				public string SWIFT { get; set; }
		}
}
