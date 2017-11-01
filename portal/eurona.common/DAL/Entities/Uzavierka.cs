using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities
{
		public class Uzavierka: CMS.Entities.Entity
		{
				public enum UzavierkaId: int
				{
						Eurona = -1001,
                        EuronaBefor = -1000,
						CernyForLife = -3001
				}

				public class ReadById
				{
						public int UzavierkaId { get; set; }
				}

				//Id
				public bool Povolena { get; set; }
				public DateTime? UzavierkaOd { get; set; }
				public DateTime? UzavierkaDo { get; set; }
				public DateTime? OperatorOrderOd { get; set; }
				public DateTime? OperatorOrderDo { get; set; }
				public DateTime? OperatorOrderDate { get; set; }
		}
}
