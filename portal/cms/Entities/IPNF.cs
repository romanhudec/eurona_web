using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class IPNF: Entity
		{
				public enum IPNFType
				{
						Phone = 1,
						Mobile = 2,
						Fax = 3
				}

				public class ReadById
				{
						public int IPNFId { get; set; }
				}

				public class ReadByType
				{
						public IPNFType Type { get; set; }
						public string Locale { get; set; }
				}

				public int InstanceId { get; set; }
				public IPNFType Type { get; set; }
				public string Locale { get; set; }
				public string IPF { get; set; }
				public string Notes { get; set; }
		}
}
