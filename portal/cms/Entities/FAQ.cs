using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class FAQ : Entity
		{
				public class ReadById
				{
						public int FAQId { get; set; }
				}

				public int InstanceId { get; set; }
				public string Locale { get; set; }
				public int? Order { get; set; }
				public string Question { get; set; }
				public string Answer { get; set; }
		}
}
