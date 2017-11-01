using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class AccountProfileStorage: MSSQLStorage<AccountProfile>
		{
				public AccountProfileStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static AccountProfile GetAccountProfile( DataRow record )
				{
						AccountProfile accountProfile = new AccountProfile();
						accountProfile.Id = Convert.ToInt32( record["AccountProfileId"] );
						accountProfile.InstanceId = Convert.ToInt32( record["InstanceId"] );
						accountProfile.AccountId = Convert.ToInt32( record["AccountId"] );
						accountProfile.ProfileId = Convert.ToInt32( record["ProfileId"] );
						accountProfile.ProfileType = Convert.ToInt32( record["ProfileType"] );
						accountProfile.ProfileName = Convert.ToString( record["ProfileName"] );
						accountProfile.Value = Convert.ToString( record["Value"] );

						return accountProfile;
				}

				public override List<AccountProfile> Read( object criteria )
				{
						if ( criteria is AccountProfile.ReadById ) return LoadById( criteria as AccountProfile.ReadById );
						if ( criteria is AccountProfile.ReadByAccountId ) return LoadByAccountId( criteria as AccountProfile.ReadByAccountId );
						if ( criteria is AccountProfile.ReadByAccountAndProfile ) return LoadByAccountAndProfile( criteria as AccountProfile.ReadByAccountAndProfile );
						if ( criteria is AccountProfile.ReadByAccountAndProfileName ) return LoadByAccountAndProfileName( criteria as AccountProfile.ReadByAccountAndProfileName );
						if ( criteria is AccountProfile.ReadByAccountAndProfileType ) return LoadByAccountAndProfileType( criteria as AccountProfile.ReadByAccountAndProfileType );
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<AccountProfile> LoadById( AccountProfile.ReadById byAccountProfileId )
				{
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles
								WHERE AccountProfileId = @AccountProfileId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountProfileId", byAccountProfileId.AccountProfileId ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}

				private List<AccountProfile> LoadByAccountId( AccountProfile.ReadByAccountId byAccountId )
				{
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles
								WHERE AccountId = @AccountId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AccountId", byAccountId.AccountId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}

				private List<AccountProfile> LoadByAccountAndProfile( AccountProfile.ReadByAccountAndProfile by )
				{
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles
								WHERE AccountId = @AccountId AND ProfileId = @ProfileId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", by.AccountId ),
										new SqlParameter( "@ProfileId", by.ProfileId ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}

				private List<AccountProfile> LoadByAccountAndProfileName( AccountProfile.ReadByAccountAndProfileName by )
				{
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles ap
								WHERE AccountId = @AccountId AND ProfileName = @ProfileName";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", by.AccountId ),
										new SqlParameter( "@ProfileName", Null( by.ProfileName ) ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}
				private List<AccountProfile> LoadByAccountAndProfileType( AccountProfile.ReadByAccountAndProfileType by )
				{
						List<AccountProfile> accountProfiles = new List<AccountProfile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AccountProfileId, InstanceId, AccountId, ProfileId, ProfileType, ProfileName, Value
								FROM vAccountProfiles ap
								WHERE AccountId = @AccountId AND ProfileType = @ProfileType";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", by.AccountId ),
										new SqlParameter( "@ProfileType", Null( by.ProfileType ) ) );
								foreach ( DataRow dr in table.Rows )
										accountProfiles.Add( GetAccountProfile( dr ) );
						}
						return accountProfiles;
				}
				public override void Create( AccountProfile accountProfile )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pAccountProfileCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@AccountId", accountProfile.AccountId ),
										new SqlParameter( "@ProfileId", accountProfile.ProfileId ),
										new SqlParameter( "@Value", Null( accountProfile.Value ) ),
										result );

								accountProfile.Id = Convert.ToInt32( result.Value );
						}

				}

				public override void Update( AccountProfile accountProfile )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pAccountProfileModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@AccountProfileId", accountProfile.Id ),
										new SqlParameter( "@AccountId", accountProfile.AccountId ),
										new SqlParameter( "@ProfileId", accountProfile.ProfileId ),
										new SqlParameter( "@Value", accountProfile.Value )
									 );
						}

				}

				public override void Delete( AccountProfile accountProfile )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter accountProfileId = new SqlParameter( "@AccountProfileId", accountProfile.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAccountProfileDelete", result, historyAccount, accountProfileId );
						}
				}
		}
}
