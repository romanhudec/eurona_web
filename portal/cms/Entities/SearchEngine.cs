using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public interface ISearchEngine
		{
				string Title { get; set; }
				string Content { get; set; }
				string UrlAlias { get; set; }
				string ImageUrl { get; set; }
				int? RoleId { get; set; }
				string RoleString { get; set; }
		}

		public class SearchEngineBase: Entity, ISearchEngine
		{
				#region ISearchEngine Members
				public string Title { get; set; }
				public string Content { get; set; }
				public string UrlAlias { get; set; }
				public string ImageUrl { get; set; }
				public int? RoleId { get; set; }
				public string RoleString { get; set; }
                public int Relevance { get; set; }
				#endregion
		}

		public class CmsSearchEngineEntity: SearchEngineBase
		{
				#region Search Methods
				public interface ISearch
				{
						string Keywords { get; set; }
				}
				public interface ICommentSearch
				{
						string Keywords { get; set; }
						string CommentAliasPostFix { get; set; }
				}

				public class SearchPages: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchArticles: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchBlogs: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchNews: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchImageGalleries: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchForums: ISearch
				{
						public string Keywords { get; set; }
				}
				public class SearchForumPosts: ISearch
				{
						public string Keywords { get; set; }
				}
				#region comments
				public class SearchArticleComments: ICommentSearch
				{
						public string Keywords { get; set; }
						public string CommentAliasPostFix { get; set; }
				}
				public class SearchBlogComments: ICommentSearch
				{
						public string Keywords { get; set; }
						public string CommentAliasPostFix { get; set; }
				}
				public class SearchImageGalleryComments: ICommentSearch
				{
						public string Keywords { get; set; }
						public string CommentAliasPostFix { get; set; }
				}
				public class SearchImageGalleryItemComments: ICommentSearch
				{
						public string Keywords { get; set; }
						public string CommentAliasPostFix { get; set; }
				}
				#endregion
				#endregion
		}
}
