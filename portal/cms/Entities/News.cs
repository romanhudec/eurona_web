using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class News: Entity, IUrlAliasEntity
		{
				public News()
				{
						this.Alias = string.Empty;
				}

				public class ReadById
				{
						public int NewsId { get; set; }
				}

				public class ReadLatest
				{
						public int Count { get; set; }
				}

				public int InstanceId { get; set; }
				public string Locale { get; set; }
				public DateTime? Date { get; set; }
				public string Icon { get; set; }
				public string Title { get; set; }
				public string Teaser { get; set; }
				public string Content { get; set; }
				public string ContentKeywords { get; set; }

				#region IUrlAliasEntity Members
				public int? UrlAliasId { get; set; }
				public string Alias { get; set; }
				#endregion
		}
}
