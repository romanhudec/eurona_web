using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.DAL.Entities;
using System.Data.SqlClient;
using System.Data;

namespace Eurona.DAL.MSSQL
{
		internal sealed class AccountExtStorage: CMS.MSSQL.MSSQLStorage<AccountExt>
		{
				public AccountExtStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private AccountExt GetAccountExt( DataRow record )
				{
						AccountExt account = new AccountExt();
						account.Id = Convert.ToInt32( record["AccountId"] );
						account.AccountId = Convert.ToInt32( record["AccountId"] );
						account.InstanceId = Convert.ToInt32( record["InstanceId"] );
						account.AdvisorId = ConvertNullable.ToInt32( record["AdvisorId"] );
						account.AdvisorPersonId = ConvertNullable.ToInt32( record["AdvisorPersonId"] );
						return account;
				}

				public override List<AccountExt> Read( object criteria )
				{
						if ( criteria is AccountExt.ReadByAccount ) return LoadByAccount( ( criteria as AccountExt.ReadByAccount ).AccountId );
						List<AccountExt> accounts = new List<AccountExt>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT AccountId, InstanceId,  AdvisorId, AdvisorPersonId
										FROM vAccountsExt WHERE ( InstanceId=@InstanceId OR InstanceId=0 )";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows ) accounts.Add( GetAccountExt( dr ) );
						}
						return accounts;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<AccountExt> LoadByAccount( int accountId )
				{
						List<AccountExt> accounts = new List<AccountExt>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT AccountId, InstanceId,  AdvisorId, AdvisorPersonId
										FROM vAccountsExt
										WHERE AccountId = @AccountId AND ( InstanceId=@InstanceId OR InstanceId=0 )";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountId", accountId ), new SqlParameter( "@InstanceId", InstanceId ) );
								if ( table.Rows.Count > 0 )
										accounts.Add( GetAccountExt( table.Rows[0] ) );
						}
						return accounts;
				}

				public override void Create( AccountExt account )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter instanceId = new SqlParameter( "@InstanceId", InstanceId );
								SqlParameter accountId = new SqlParameter( "@AccountId", account.AccountId );
								SqlParameter advisorId = new SqlParameter( "@AdvisorId", account.AdvisorId );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAccountExtCreate", result, instanceId, accountId, advisorId );
								account.Id = Convert.ToInt32( result.Value );
						}
				}

				public override void Update( AccountExt account )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter instanceId = new SqlParameter( "@InstanceId", InstanceId );
								SqlParameter accountId = new SqlParameter( "@AccountId", account.AccountId );
								SqlParameter advisorId = new SqlParameter( "@AdvisorId", account.AdvisorId );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAccountExtModify", result, instanceId, accountId, advisorId );
						}
				}

				public override void Delete( AccountExt account )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter instanceId = new SqlParameter( "@InstanceId", InstanceId );
								SqlParameter accountId = new SqlParameter( "@AccountId", account.AccountId );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAccountExtDelete", result, instanceId, accountId );
						}
				}
		}
}
