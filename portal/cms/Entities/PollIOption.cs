using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class PollOption: Entity
		{
				public class ReadById
				{
						public int PollOptionId { get; set; }
				}
				public class ReadByPollId
				{
						public int PollId { get; set; }
				}
				public int InstanceId { get; set; }
				public int PollId { get; set; }
				public int? Order { get; set; }
				public string Name { get; set; }

				public int Votes { get; set; }

		}
}
