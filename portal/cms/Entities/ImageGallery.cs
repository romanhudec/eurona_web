using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class ImageGallery: Entity, IUrlAliasEntity
		{
				public ImageGallery()
				{
						this.Visible = true;
						this.EnableComments = true;
						this.EnableVotes = true;
						this.Date = DateTime.Now;
						this.Alias = string.Empty;
				}

				public class ReadById
				{
						public int ImageGalleryId { get; set; }
				}

				public class ReadForCurrentAccount
				{
						public int? TagId { get; set; }
				}

				public class IncrementViewCountCommand
				{
						public int ImageGalleryId { get; set; }
				}

				public int InstanceId { get; set; }
				public int? RoleId { get; set; }
				public string Name { get; set; }
				public string Description { get; set; }
				public DateTime Date { get; set; }
				public bool EnableComments { get; set; }
				public bool EnableVotes { get; set; }
				public bool Visible { get; set; }

				public int CommentsCount { get; set; }
				public int ItemsCount { get; set; }
				public int ViewCount { get; set; }


				//FK TAGS
				private List<ImageGalleryTag> imageGalleryTags = null;
				public List<ImageGalleryTag> ImageGalleryTags
				{
						get
						{
								if ( imageGalleryTags != null ) return imageGalleryTags;
								imageGalleryTags = Storage<ImageGalleryTag>.Read( new ImageGalleryTag.ReadByImageGalleryId { ImageGalleryId = this.Id } );
								return imageGalleryTags;
						}
				}

				#region IUrlAliasEntity Members
				public int? UrlAliasId { get; set; }
				public string Alias { get; set; }
				#endregion

		}
}
