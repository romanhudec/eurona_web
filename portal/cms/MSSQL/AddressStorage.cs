using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class AddressStorage: MSSQLStorage<Address>
		{
				public AddressStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Address GetAddress( DataRow record )
				{
						Address address = new Address();
						address.Id = Convert.ToInt32( record["AddressId"] );
						address.InstanceId = Convert.ToInt32( record["InstanceId"] );
						address.Street = Convert.ToString( record["Street"] );
						address.Zip = Convert.ToString( record["Zip"] );
						address.City = Convert.ToString( record["City"] );
						address.Notes = Convert.ToString( record["Notes"] );

						address.District = Convert.ToString( record["District"] );
						address.Region = Convert.ToString( record["Region"] );
						address.Country = Convert.ToString( record["Country"] );
						address.State = Convert.ToString( record["State"] );

						return address;
				}

				public override List<Address> Read( object criteria )
				{
						if ( criteria is Address.ReadById ) return LoadById( criteria as Address.ReadById );
						List<Address> addesses = new List<Address>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	AddressId, InstanceId, Street, Zip, Notes, City, District, Region, Country, State
								FROM vAddresses WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										addesses.Add( GetAddress( dr ) );
						}
						return addesses;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Address> LoadById( Address.ReadById byAddressId )
				{
						List<Address> addesses = new List<Address>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	AddressId, InstanceId, Street, Zip, Notes, City, District, Region, Country, State
								FROM vAddresses
								WHERE AddressId = @AddressId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@AddressId", byAddressId.AddressId ) );
								foreach ( DataRow dr in table.Rows )
										addesses.Add( GetAddress( dr ) );
						}
						return addesses;
				}

				public override void Create( Address address )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pAddressCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@City", address.City ),
										new SqlParameter( "@Street", address.Street ),
										new SqlParameter( "@Zip", address.Zip ),
										new SqlParameter( "@District", address.District ),
										new SqlParameter( "@Region", address.Region ),
										new SqlParameter( "@Country", address.Country ),
										new SqlParameter( "@State", address.State ),
										new SqlParameter( "@Notes", address.Notes ),
										result );

								address.Id = Convert.ToInt32( result.Value );
						}
				}

				public override void Update( Address address )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pAddressModify",
									new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@AddressId", address.Id ),
										new SqlParameter( "@City", address.City ),
										new SqlParameter( "@Street", address.Street ),
										new SqlParameter( "@Zip", address.Zip ),
										new SqlParameter( "@District", address.District ),
										new SqlParameter( "@Region", address.Region ),
										new SqlParameter( "@Country", address.Country ),
										new SqlParameter( "@State", address.State ),
										new SqlParameter( "@Notes", address.Notes ) );
						}
				}

				public override void Delete( Address address )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter addressId = new SqlParameter( "@AddressId", address.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pAddressDelete", result, historyAccount, addressId );
						}
				}

		}
}
