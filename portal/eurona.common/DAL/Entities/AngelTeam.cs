using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.Common.DAL.Entities
{
		public class AngelTeam: CMS.Entities.Entity
		{
				public enum AngelTeamId: int
				{
						Eurona = -1001,
						CernyForLife = -3001
				}

				public class ReadById
				{
						public int AngelTeamId { get; set; }
				}

				public int PocetEuronaStarProVstup { get; set; }
				public int PocetEuronaStarProUdrzeni { get; set; }

		}
}
