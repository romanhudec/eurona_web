using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ForumTrackingStorage: MSSQLStorage<ForumTracking>
		{
				private const string entitySelect = @"SELECT ForumTrackingId ,ForumId ,AccountId, Email, AccountName
						FROM vForumTrackings";

				public ForumTrackingStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ForumTracking GetForumTracking( DataRow record )
				{
						ForumTracking forumTracking = new ForumTracking();
						forumTracking.Id = Convert.ToInt32( record["ForumTrackingId"] );
						forumTracking.AccountId = Convert.ToInt32( record["AccountId"] );
						forumTracking.ForumId = Convert.ToInt32( record["ForumId"] );

						forumTracking.Email = Convert.ToString( record["Email"] );
						forumTracking.AccountName = Convert.ToString( record["AccountName"] );
						return forumTracking;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<ForumTracking> Read( object criteria )
				{
						if ( criteria is ForumTracking.ReadById ) return LoadById( criteria as ForumTracking.ReadById );
						if ( criteria is ForumTracking.ReadBy ) return LoadBy( criteria as ForumTracking.ReadBy );
						List<ForumTracking> list = new List<ForumTracking>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumTracking( dr ) );
						}
						return list;
				}

				private List<ForumTracking> LoadById( ForumTracking.ReadById by )
				{
						List<ForumTracking> list = new List<ForumTracking>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE ForumTrackingId = @ForumTrackingId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumTrackingId", by.ForumTrackingId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumTracking( dr ) );
						}
						return list;
				}

				private List<ForumTracking> LoadBy( ForumTracking.ReadBy by )
				{
						List<ForumTracking> list = new List<ForumTracking>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE (@AccountId IS NULL OR AccountId = @AccountId) AND (@ForumId IS NULL OR ForumId = @ForumId)";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountId", Null( by.AccountId ) ), new SqlParameter( "@ForumId", Null( by.ForumId ) ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumTracking( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				public override void Create( ForumTracking forumTracking )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pForumTrackingCreate",
										new SqlParameter( "@AccountId", forumTracking.AccountId ),
										new SqlParameter( "@ForumId", forumTracking.ForumId ),
										result );

								forumTracking.Id = Convert.ToInt32( result.Value ); ;
						}
				}

				public override void Update( ForumTracking forumTracking )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pForumTrackingModify",
										new SqlParameter( "@ForumTrackingId", forumTracking.Id ),
										new SqlParameter( "@AccountId", forumTracking.AccountId ),
										new SqlParameter( "@ForumId", forumTracking.ForumId ) );
						}
				}

				public override void Delete( ForumTracking forumTracking )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter pkId = new SqlParameter( "@ForumTrackingId", forumTracking.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pForumTrackingDelete", result, pkId );
						}
				}
		}
}
