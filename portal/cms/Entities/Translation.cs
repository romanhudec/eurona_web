using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Translation: Entity
		{
				public class ReadById
				{
						public int TranslationId { get; set; }
				}

				public class ReadByVocabulary
				{
						public int? VocabularyId { get; set; }
						public string Vocabulary { get; set; }
				}

				public int InstanceId { get; set; }
				public int VocabularyId { get; set; }
				public string Term { get; set; }
				public string Trans { get; set; }
				public string Notes { get; set; }
		}
}
