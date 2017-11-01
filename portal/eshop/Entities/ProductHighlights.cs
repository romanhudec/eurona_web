using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities
{
		public class ProductHighlights: Entity
		{
				public class ReadById
				{
						public int ProductHighlightsId { get; set; }
				}

				public class ReadByProduct
				{
						public int? ProductId { get; set; }
						public int? HighlightId { get; set; }
				}

				public int InstanceId { get; set; }
				public int HighlightId { get; set; }
				public int ProductId { get; set; }

				//Joined properties
				public string Icon { get; set; }
				public string Name { get; set; }
				public string Code { get; set; }
				public string Notes { get; set; }
		}
}
