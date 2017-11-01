using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class ImageGalleryTag: Entity
		{
				public class ReadByTagId
				{
						public int TagId { get; set; }
				}
				public class ReadByImageGalleryId
				{
						public int ImageGalleryId { get; set; }
				}

				public int TagId { get; set; }
				public int ImageGalleryId { get; set; }
				public string Name { get; set; }
		}
}
