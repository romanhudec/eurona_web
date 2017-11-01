using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class AccountCreditStorage: MSSQLStorage<AccountCredit>
		{
				public AccountCreditStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static AccountCredit GetPoll( DataRow record )
				{
						AccountCredit accountCredit = new AccountCredit();
						accountCredit.Id = Convert.ToInt32( record["AccountCreditId"] );
						accountCredit.InstanceId = Convert.ToInt32( record["InstanceId"] );
						accountCredit.AccountId = Convert.ToInt32( record["AccountId"] );
						accountCredit.Credit = Convert.ToDecimal( record["Credit"] );
						return accountCredit;
				}

				public override List<AccountCredit> Read( object criteria )
				{
						if ( criteria is AccountCredit.ReadByAccountId ) return LoadById( criteria as AccountCredit.ReadByAccountId );
						List<AccountCredit> list = new List<AccountCredit>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountCreditId, InstanceId, AccountId, Credit
								FROM vAccountsCredit WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPoll( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<AccountCredit> LoadById( AccountCredit.ReadByAccountId accountId )
				{
						List<AccountCredit> list = new List<AccountCredit>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountCreditId, InstanceId, AccountId, Credit
								FROM vAccountsCredit
								WHERE AccountId = @AccountId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", accountId.AccountId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPoll( dr ) );
						}
						return list;
				}

				public override void Create( AccountCredit accountCredit )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pAccountCreditCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@AccountId", accountCredit.AccountId ),
										new SqlParameter( "@Credit", accountCredit.Credit ) );
						}
				}

				public override void Update( AccountCredit accountCredit )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pAccountCreditModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@AccountCreditId", accountCredit.Id ),
										new SqlParameter( "@Credit", accountCredit.Credit ) );
						}
				}

				public override void Delete( AccountCredit accountCredit )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter accountCreditId = new SqlParameter( "@AccountCreditId", accountCredit.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAccountCreditDelete", result, historyAccount, accountCreditId );
						}
				}

		}
}
