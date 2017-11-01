using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.DAL.Entities
{
		public class AccountExt : CMS.Entities.Entity
		{
				public class ReadByAccount
				{
						public int AccountId { get; set; }
				}
				public int AccountId { get; set; }
				public int? AdvisorId { get; set; }
				public int InstanceId { get; set; }
				public int? AdvisorPersonId { get; set; }
		}
}
