using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Poll : Entity
		{
				public Poll()
				{
						this.DateFrom = DateTime.Now;
				}

				public class ReadById
				{
						public int PollId { get; set; }
				}

				public class ReadActivePoll
				{
				}

				public int InstanceId { get; set; }
				public bool Closed { get; set; }
				public string Locale { get; set; }
				public string Question { get; set; }
				public DateTime DateFrom { get; set; }
				public DateTime? DateTo { get; set; }
				public string Icon { get; set; }
				
				//FK Pool options
				private List<PollOption> options = null;
				public List<PollOption> Options
				{
						get
						{
								if ( options != null ) return options;
								options = Storage<PollOption>.Read( new PollOption.ReadByPollId { PollId = this.Id } );
								return options;
						}
				}

				public int VotesTotal { get; set; }
		}
}
