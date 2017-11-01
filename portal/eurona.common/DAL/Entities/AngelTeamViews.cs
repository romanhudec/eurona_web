using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities
{
	public class AngelTeamViews
	{
		public class ReadByAccount
		{
			public int AccountId { get; set; }
		}
		public int AccountId { get; set; }
		public DateTime ViewDate { get; set; }
		public int ViewCount { get; set; }
	}
}
