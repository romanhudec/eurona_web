using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ForumStorage: MSSQLStorage<Forum>
		{
				private const string entitySelect = @"SELECT ForumId ,ForumThreadId ,InstanceId ,Icon ,Name ,Description ,Pinned ,Locked ,ViewCount, ForumPostCount, 
						LastPostId, LastPostDate, LastPostAccountId, LastPostAccountName ,UrlAliasId ,Alias ,Url,
						CreatedByAccountId, CreatedDate, CreatedByAccountName,
						OrderDate = ISNULL(LastPostDate, GETDATE())
						FROM vForums";

				public ForumStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Forum GetForum( DataRow record )
				{
						Forum forum = new Forum();
						forum.Id = Convert.ToInt32( record["ForumId"] );
						forum.ForumThreadId = Convert.ToInt32( record["ForumThreadId"] );
						forum.InstanceId = Convert.ToInt32( record["InstanceId"] );
						forum.Name = Convert.ToString( record["Name"] );
						forum.Description = Convert.ToString( record["Description"] );
						forum.Icon = Convert.ToString( record["Icon"] );

						forum.Pinned = Convert.ToBoolean( record["Pinned"] );
						forum.Locked = Convert.ToBoolean( record["Locked"] );

						forum.UrlAliasId = ConvertNullable.ToInt32( record["UrlAliasId"] );
						forum.Alias = Convert.ToString( record["Alias"] );

						forum.ViewCount = Convert.ToInt32( record["ViewCount"] );
						forum.ForumPostCount = Convert.ToInt32( record["ForumPostCount"] );

						forum.LastPostId = ConvertNullable.ToInt32( record["LastPostId"] );
						forum.LastPostDate = ConvertNullable.ToDateTime( record["LastPostDate"] );
						forum.LastPostAccountId = ConvertNullable.ToInt32( record["LastPostAccountId"] );
						forum.LastPostAccountName = ConvertNullable.ToString( record["LastPostAccountName"] );

						forum.CreatedByAccountId = Convert.ToInt32( record["CreatedByAccountId"] );
						forum.CreatedDate = Convert.ToDateTime( record["CreatedDate"] );
						forum.CreatedByAccountName = Convert.ToString( record["CreatedByAccountName"] );

						return forum;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<Forum> Read( object criteria )
				{
						if ( criteria is Forum.ReadById ) return LoadById( criteria as Forum.ReadById );
						if ( criteria is Forum.ReadByForumThreadId ) return LoadByForumThreadId( criteria as Forum.ReadByForumThreadId );
						if ( criteria is Forum.ReadBy ) return LoadBy( criteria as Forum.ReadBy );
						List<Forum> list = new List<Forum>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE InstanceId = @InstanceId ORDER BY Pinned DESC, OrderDate DESC, Name ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForum( dr ) );
						}
						return list;
				}

				private List<Forum> LoadById( Forum.ReadById by )
				{
						List<Forum> list = new List<Forum>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE ForumId = @ForumId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumId", by.ForumId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForum( dr ) );
						}
						return list;
				}

				private List<Forum> LoadByForumThreadId( Forum.ReadByForumThreadId by )
				{
						List<Forum> list = new List<Forum>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE ForumThreadId = @ForumThreadId ORDER BY Pinned DESC, OrderDate DESC, Name ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumThreadId", by.ForumThreadId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForum( dr ) );
						}
						return list;
				}

				private List<Forum> LoadBy( Forum.ReadBy by )
				{
						List<Forum> list = new List<Forum>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								select f.ForumId ,f.ForumThreadId ,f.InstanceId ,f.Icon ,f.Name ,f.Description ,f.Pinned ,f.Locked ,f.ViewCount, f.ForumPostCount, f.LastPostId, f.LastPostDate, f.LastPostAccountId, f.LastPostAccountName ,f.UrlAliasId ,f.Alias ,f.Url,
								f.CreatedByAccountId, f.CreatedDate, f.CreatedByAccountName
								from vForums f
								inner join vForumPosts fp ON fp.ForumId = f.ForumId
								where fp.AccountId = @AccountId OR 
								(select count(*) from tForum where (ForumId=f.ForumId OR HistoryId=f.ForumId) AND HistoryAccount=@AccountId) != 0";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountId", by.AccountId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForum( dr ) );
						}
						return list;
				}
				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				public override void Create( Forum forum )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pForumCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ForumThreadId", forum.ForumThreadId ),
										new SqlParameter( "@Name", forum.Name ),
										new SqlParameter( "@Description", forum.Description ),
										new SqlParameter( "@Icon", Null( forum.Icon ) ),
										new SqlParameter( "@Pinned", forum.Pinned ),
										new SqlParameter( "@Locked", forum.Locked ),
										new SqlParameter( "@UrlAliasId", Null( forum.UrlAliasId ) ),
										result );

								forum.Id = Convert.ToInt32( result.Value ); ;
						}
				}

				public override void Update( Forum forum )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pForumModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@ForumId", forum.Id ),
										new SqlParameter( "@ForumThreadId", forum.ForumThreadId ),
										new SqlParameter( "@Name", forum.Name ),
										new SqlParameter( "@Description", forum.Description ),
										new SqlParameter( "@Icon", forum.Icon ),
										new SqlParameter( "@Pinned", forum.Pinned ),
										new SqlParameter( "@Locked", forum.Locked ),
										new SqlParameter( "@UrlAliasId", forum.UrlAliasId ) );
						}
				}

				public override void Delete( Forum forum )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter articleId = new SqlParameter( "@ForumId", forum.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pForumDelete", result, historyAccount, articleId );
						}
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( Forum.IncrementViewCountCommand ) )
								return IncrementViewCount( command as Forum.IncrementViewCountCommand ) as R;

						return base.Execute<R>( command );
				}

				private Forum.IncrementViewCountCommand IncrementViewCount( Forum.IncrementViewCountCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter forumId = new SqlParameter( "@ForumId", cmd.ForumId );
								ExecProc( connection, "pForumIncrementViewCount", forumId );
						}

						return cmd;
				}

		}
}
