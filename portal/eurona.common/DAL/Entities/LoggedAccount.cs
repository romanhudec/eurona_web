using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities
{
	public class LoggedAccount : CMS.Entities.Entity
	{
		public class ReadLogged
		{
			public bool? HasTVDId { get; set; }
			public bool? AngelTeamClen { get; set; }
			public bool? AngelTeamManager { get; set; }
			public int? AngelTeamManagerTyp { get; set; }
		}

		public int AccountId { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public DateTime LoggedAt { get; set; }
		public int? TVD_Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int LoggedMinutes { get; set; }
		public bool AngelTeamClen { get; set; }
		public bool AngelTeamManager { get; set; }
		public int InstanceId { get; set; }
		
	}
}
