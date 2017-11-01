using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Vocabulary: Entity
		{
				public class ReadById
				{
						public int VocabularyId { get; set; }
				}
				public int InstanceId { get; set; }
				public string Name { get; set; }
				public string Locale { get; set; }
		}
}
