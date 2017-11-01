using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities
{
		public class ProductReviews: Entity
		{
				public class ReadById
				{
						public int ProductReviewsId { get; set; }
				}

				public class ReadByProduct
				{
						public int? ProductId { get; set; }
				}

				public int InstanceId { get; set; }
				public int ArticleId { get; set; }
				public int ProductId { get; set; }

				//Joined properties
				public string Icon { get; set; }
				public string Title { get; set; }
				public string Teaser { get; set; }
				public int? RoleId { get; set; }

				public string Country { get; set; }
				public string City { get; set; }
				public DateTime ReleaseDate { get; set; }
				public string Alias { get; set; }//UrlAlias na clanok
		}
}
