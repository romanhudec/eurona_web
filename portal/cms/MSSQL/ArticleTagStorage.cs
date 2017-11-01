using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ArticleTagStorage: MSSQLStorage<ArticleTag>
		{
				public ArticleTagStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ArticleTag GetArticleTag( DataRow record )
				{
						ArticleTag articleTag = new ArticleTag();
						articleTag.Id = Convert.ToInt32( record["ArticleTagId"] );
						articleTag.TagId = Convert.ToInt32( record["TagId"] );
						articleTag.ArticleId = Convert.ToInt32( record["ArticleId"] );
						articleTag.Name = Convert.ToString( record["Tag"] );
						return articleTag;
				}

				public override List<ArticleTag> Read( object criteria )
				{
						if ( criteria is ArticleTag.ReadByTagId ) return LoadByTagId( criteria as ArticleTag.ReadByTagId );
						if ( criteria is ArticleTag.ReadByArticleId ) return LoadByArticleId( criteria as ArticleTag.ReadByArticleId );
						List<ArticleTag> list = new List<ArticleTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleTagId, TagId, ArticleTag
								FROM vArticleTags";
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetArticleTag( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ArticleTag> LoadByTagId( ArticleTag.ReadByTagId byTagId )
				{
						List<ArticleTag> list = new List<ArticleTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleTagId, TagId, ArticleId, Tag
								FROM vArticleTags
								WHERE TagId = @TagId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@TagId", byTagId.TagId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetArticleTag( dr ) );
						}
						return list;
				}

				private List<ArticleTag> LoadByArticleId( ArticleTag.ReadByArticleId byArticleId )
				{
						List<ArticleTag> list = new List<ArticleTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleTagId, TagId, ArticleId, Tag
								FROM vArticleTags
								WHERE ArticleId = @ArticleId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ArticleId", byArticleId.ArticleId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetArticleTag( dr ) );
						}
						return list;
				}

				public override void Update( ArticleTag entity )
				{
						throw new NotImplementedException();
				}

				public override void Create( ArticleTag entity )
				{
						if ( string.IsNullOrEmpty( entity.Name ) )
								return;

						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pArticleTagCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ArticleId", entity.ArticleId ),
										new SqlParameter( "@Tag", entity.Name ) );
						}
				}

				public override void Delete( ArticleTag entity )
				{
						throw new NotImplementedException();
				}
		}
}
