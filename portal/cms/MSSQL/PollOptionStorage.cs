using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class PollOptionStorage: MSSQLStorage<PollOption>
		{
				public PollOptionStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static PollOption GetPollOption( DataRow record )
				{
						PollOption pollOption = new PollOption();
						pollOption.Id = Convert.ToInt32( record["PollOptionId"] );
						pollOption.InstanceId = Convert.ToInt32( record["InstanceId"] );
						pollOption.Name = Convert.ToString( record["Name"] );
						pollOption.Order = ConvertNullable.ToInt32( record["Order"] );
						pollOption.PollId = Convert.ToInt32( record["PollId"] );
						pollOption.Votes = Convert.ToInt32( record["Votes"] );

						return pollOption;
				}

				public override List<PollOption> Read( object criteria )
				{
						if ( criteria is PollOption.ReadById ) return LoadById( criteria as PollOption.ReadById );
						if ( criteria is PollOption.ReadByPollId ) return LoadByPollId( criteria as PollOption.ReadByPollId );
						List<PollOption> list = new List<PollOption>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PollOptionId, PollId, InstanceId, [Order], [Name], Votes
								FROM vPollOptions WHERE InstanceId=@InstanceId
								ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPollOption( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						string sql = "SELECT COUNT(*) FROM vPollOptions";
						using ( SqlConnection connection = Connect() )
						{
								if ( criteria is PollOption.ReadByPollId )
								{
										sql = @"SELECT COUNT(*) FROM vPollOptions WHERE PollId = @PollId";
										DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@PollId", ( criteria as PollOption.ReadByPollId ).PollId ) );
										return Convert.ToInt32( table.Rows[0][0] );
								}
								else
								{
										DataTable table = Query<DataTable>( connection, sql );
										return Convert.ToInt32( table.Rows[0][0] );
								}
						}
				}

				private List<PollOption> LoadById( PollOption.ReadById byId )
				{
						List<PollOption> list = new List<PollOption>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PollOptionId, PollId, InstanceId, [Order], [Name], Votes
								FROM vPollOptions
								WHERE PollOptionId = @PollOptionId
								ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@PollOptionId", byId.PollOptionId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPollOption( dr ) );
						}
						return list;
				}

				private List<PollOption> LoadByPollId( PollOption.ReadByPollId byPollId )
				{
						List<PollOption> list = new List<PollOption>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PollOptionId, PollId, InstanceId, [Order], [Name], Votes
								FROM vPollOptions
								WHERE PollId = @PollId
								ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@PollId", byPollId.PollId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPollOption( dr ) );
						}
						return list;
				}

				public override void Create( PollOption pollOption )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pPollOptionCreate",
										new SqlParameter( "@InstanceId", pollOption.InstanceId ),
										new SqlParameter( "@Name", pollOption.Name ),
										new SqlParameter( "@Order", Null( pollOption.Order ) ),
										new SqlParameter( "@PollId", pollOption.PollId ) );
						}
				}

				public override void Update( PollOption pollOption )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pPollOptionModify",
										new SqlParameter( "@PollOptionId", pollOption.Id ),
										new SqlParameter( "@Name", pollOption.Name ),
										new SqlParameter( "@Order", Null( pollOption.Order ) ) );
						}
				}

				public override void Delete( PollOption pollOption )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter pageId = new SqlParameter( "@PollOptionId", pollOption.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pPollOptionDelete", result, pageId );
						}
				}

		}
}
