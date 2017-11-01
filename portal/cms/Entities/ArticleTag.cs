using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class ArticleTag: Entity
		{
				public class ReadByTagId
				{
						public int TagId { get; set; }
				}
				public class ReadByArticleId
				{
						public int ArticleId { get; set; }
				}
				public int TagId { get; set; }
				public int ArticleId { get; set; }
				public string Name { get; set; }
		}
}
