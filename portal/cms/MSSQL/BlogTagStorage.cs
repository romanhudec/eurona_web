using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class BlogTagStorage: MSSQLStorage<BlogTag>
		{
				public BlogTagStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static BlogTag GetBlogTag( DataRow record )
				{
						BlogTag blogTag = new BlogTag();
						blogTag.Id = Convert.ToInt32( record["BlogTagId"] );
						blogTag.TagId = Convert.ToInt32( record["TagId"] );
						blogTag.BlogId = Convert.ToInt32( record["BlogId"] );
						blogTag.Name = Convert.ToString( record["Tag"] );
						return blogTag;
				}

				public override List<BlogTag> Read( object criteria )
				{
						if ( criteria is BlogTag.ReadByTagId ) return LoadByTagId( criteria as BlogTag.ReadByTagId );
						if ( criteria is BlogTag.ReadByBlogId ) return LoadByBlogId( criteria as BlogTag.ReadByBlogId );
						List<BlogTag> list = new List<BlogTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BlogTagId, TagId, BlogTag
								FROM vBlogTags";
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetBlogTag( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<BlogTag> LoadByTagId( BlogTag.ReadByTagId byTagId )
				{
						List<BlogTag> list = new List<BlogTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BlogTagId, TagId, BlogId, Tag
								FROM vBlogTags
								WHERE TagId = @TagId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@TagId", byTagId.TagId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetBlogTag( dr ) );
						}
						return list;
				}

				private List<BlogTag> LoadByBlogId( BlogTag.ReadByBlogId byBlogId )
				{
						List<BlogTag> list = new List<BlogTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BlogTagId, TagId, BlogId, Tag
								FROM vBlogTags
								WHERE BlogId = @BlogId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@BlogId", byBlogId.BlogId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetBlogTag( dr ) );
						}
						return list;
				}

				public override void Update( BlogTag entity )
				{
						throw new NotImplementedException();
				}

				public override void Create( BlogTag entity )
				{
						if ( string.IsNullOrEmpty( entity.Name ) )
								return;

						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pBlogTagCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@BlogId", entity.BlogId ),
										new SqlParameter( "@Tag", entity.Name ) );
						}
				}

				public override void Delete( BlogTag entity )
				{
						throw new NotImplementedException();
				}
		}
}
