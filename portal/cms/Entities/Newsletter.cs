using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Newsletter: Entity
		{
				public class ReadById
				{
						public int NewsletterId { get; set; }
				}

				public int InstanceId { get; set; }
				public string Locale { get; set; }
				public DateTime? Date { get; set; }
				public string Icon { get; set; }
				public string Subject { get; set; }
				public string Content { get; set; }
				public byte[] Attachment { get; set; }
				public string Roles { get; set; }
				public DateTime? SendDate { get; set; }

		}
}
