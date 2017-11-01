using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ForumFavoritesStorage: MSSQLStorage<ForumFavorites>
		{
				private const string entitySelect = @"SELECT ForumFavoritesId ,ForumId ,AccountId
						FROM vForumFavorites";

				public ForumFavoritesStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ForumFavorites GetForumFavorites( DataRow record )
				{
						ForumFavorites forumFavorites = new ForumFavorites();
						forumFavorites.Id = Convert.ToInt32( record["ForumFavoritesId"] );
						forumFavorites.AccountId = Convert.ToInt32( record["AccountId"] );
						forumFavorites.ForumId = Convert.ToInt32( record["ForumId"] );
						return forumFavorites;
				}

				public override List<ForumFavorites> Read( object criteria )
				{
						if ( criteria is ForumFavorites.ReadById ) return LoadById( criteria as ForumFavorites.ReadById );
						if ( criteria is ForumFavorites.ReadBy ) return LoadBy( criteria as ForumFavorites.ReadBy );
						List<ForumFavorites> list = new List<ForumFavorites>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumFavorites( dr ) );
						}
						return list;
				}

				private List<ForumFavorites> LoadById( ForumFavorites.ReadById by )
				{
						List<ForumFavorites> list = new List<ForumFavorites>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE ForumFavoritesId = @ForumFavoritesId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumFavoritesId", by.ForumFavoritesId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumFavorites( dr ) );
						}
						return list;
				}

				private List<ForumFavorites> LoadBy( ForumFavorites.ReadBy by )
				{
						List<ForumFavorites> list = new List<ForumFavorites>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE (@AccountId IS NULL OR AccountId = @AccountId) AND (@ForumId IS NULL OR ForumId = @ForumId)";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountId", Null( by.AccountId ) ), new SqlParameter( "@ForumId", Null( by.ForumId ) ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumFavorites( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				public override void Create( ForumFavorites forumFavorites )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pForumFavoritesCreate",
										new SqlParameter( "@AccountId", forumFavorites.AccountId ),
										new SqlParameter( "@ForumId", forumFavorites.ForumId ),
										result );

								forumFavorites.Id = Convert.ToInt32( result.Value ); ;
						}
				}

				public override void Update( ForumFavorites forumFavorites )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pForumFavoritesModify",
										new SqlParameter( "@ForumFavoritesId", forumFavorites.Id ),
										new SqlParameter( "@AccountId", forumFavorites.AccountId ),
										new SqlParameter( "@ForumId", forumFavorites.ForumId ) );
						}
				}

				public override void Delete( ForumFavorites forumFavorites )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter pkId = new SqlParameter( "@ForumFavoritesId", forumFavorites.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pForumFavoritesDelete", result, pkId );
						}
				}
		}
}
