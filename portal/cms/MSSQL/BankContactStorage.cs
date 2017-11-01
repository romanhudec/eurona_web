using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class BankContactStorage: MSSQLStorage<BankContact>
		{
				public BankContactStorage( int instanceId, Account account, string connectionString )
						: base( instanceId,account, connectionString )
				{
				}

				private static BankContact GetBankContact( DataRow record )
				{
						BankContact bank = new BankContact();
						bank.Id = Convert.ToInt32( record["BankContactId"] );
						bank.InstanceId = Convert.ToInt32( record["InstanceId"] );
						bank.BankName = Convert.ToString( record["BankName"] );
						bank.BankCode = Convert.ToString( record["BankCode"] );
						bank.AccountNumber = Convert.ToString( record["AccountNumber"] );
						bank.IBAN = Convert.ToString( record["IBAN"] );
						bank.SWIFT = Convert.ToString( record["SWIFT"] );

						return bank;
				}

				public override List<BankContact> Read( object criteria )
				{
						if ( criteria is BankContact.ReadById ) return LoadById( criteria as BankContact.ReadById );
						List<BankContact> list = new List<BankContact>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BankContactId, InstanceId, BankName, BankCode, AccountNumber, IBAN, SWIFT
								FROM vBankContacts WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetBankContact( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<BankContact> LoadById( BankContact.ReadById byId )
				{
						List<BankContact> list = new List<BankContact>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BankContactId, InstanceId, BankName, BankCode, AccountNumber, IBAN, SWIFT
								FROM vBankContacts
								WHERE BankContactId = @BankContactId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@BankContactId", byId.BankContactId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetBankContact( dr ) );
						}
						return list;
				}

				public override void Create( BankContact bank )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pBankContactCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@BankName", bank.BankName ),
										new SqlParameter( "@BankCode", bank.BankCode ),
										new SqlParameter( "@AccountNumber", bank.AccountNumber ),
										new SqlParameter( "@IBAN", bank.IBAN ),
										new SqlParameter( "@SWIFT", bank.SWIFT ) );
						}
				}

				public override void Update( BankContact bank )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pBankContactModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@BankContactId", bank.Id ),
										new SqlParameter( "@BankName", bank.BankName ),
										new SqlParameter( "@BankCode", bank.BankCode ),
										new SqlParameter( "@AccountNumber", bank.AccountNumber ),
										new SqlParameter( "@IBAN", bank.IBAN ),
										new SqlParameter( "@SWIFT", bank.SWIFT ) );
						}
				}

				public override void Delete( BankContact bank )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter id = new SqlParameter( "@BankContactId", bank.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pBankContactDelete", result, historyAccount, id );
						}
				}

		}
}
