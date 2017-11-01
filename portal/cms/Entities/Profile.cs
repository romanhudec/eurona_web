using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Profile: Entity
		{
				public Profile()
				{
				}

				public class ReadById
				{
						public int ProfileId { get; set; }
				}

				public int InstanceId { get; set; }
				public string Name { get; set; }
				public int Type { get; set; }
				public string Description { get; set; }
		}
}
