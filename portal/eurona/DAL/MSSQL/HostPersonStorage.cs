using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Eurona.DAL.Entities;
using CMS.MSSQL;

namespace Eurona.DAL.MSSQL
{
		internal sealed class HostPersonStorage: MSSQLStorage<HostPerson>
		{
				public HostPersonStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private HostPerson GetPerson( DataRow record )
				{
						HostPerson person = new HostPerson();
						person.Id = Convert.ToInt32( record["PersonId"] );
						person.InstanceId = Convert.ToInt32( record["InstanceId"] );
						person.AccountId = ConvertNullable.ToInt32( record["AccountId"] );
						person.Title = Convert.ToString( record["Title"] );
						person.FirstName = Convert.ToString( record["FirstName"] );
						person.LastName = Convert.ToString( record["LastName"] );
						person.Email = Convert.ToString( record["Email"] );
						person.Notes = ConvertNullable.ToString( record["Notes"] );
						person.Mobile = ConvertNullable.ToString( record["Mobile"] );
						person.Phone = ConvertNullable.ToString( record["Phone"] );
						person.AddressHomeId = ConvertNullable.ToInt32( record["AddressHomeId"] );
						person.AddressTempId = ConvertNullable.ToInt32( record["AddressTempId"] );
						person.AdvisorPersonId = ConvertNullable.ToInt32( record["AdvisorPersonId"] );

						person.MakeDisplay();

						return person;
				}

				public override List<HostPerson> Read( object criteria )
				{
						if ( criteria is HostPerson.ReadById ) return LoadById( ( criteria as HostPerson.ReadById ).PersonId );
						if ( criteria is HostPerson.ReadByAccountId ) return LoadByAccountId( ( criteria as HostPerson.ReadByAccountId ).AccountId );
						if ( criteria is HostPerson.ReadByAdvisorId ) return LoadByAdvisorId( ( criteria as HostPerson.ReadByAdvisorId ).AdvisorId );
						List<HostPerson> persons = new List<HostPerson>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AdvisorPersonId = at.AdvisorPersonId
										FROM vPersons p
										INNER JOIN vAccounts a ON a.AccountId = p.AccountId
										INNER JOIN vAccountsExt at ON at.AccountId = p.AccountId
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE a.Roles LIKE '%' + @role + '%' AND p.InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@role", Role.HOST ) );
								foreach ( DataRow dr in table.Rows ) persons.Add( GetPerson( dr ) );
						}
						return persons;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}
				
				private List<HostPerson> LoadById( int id )
				{
						List<HostPerson> persons = new List<HostPerson>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AdvisorPersonId = at.AdvisorPersonId
										FROM vPersons p
										INNER JOIN vAccounts a ON a.AccountId = p.AccountId
										INNER JOIN vAccountsExt at ON at.AccountId = p.AccountId
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE a.Roles LIKE '%' + @role + '%' AND p.PersonId = @PersonId";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@role", Role.HOST ),
										new SqlParameter( "@PersonId", id ) );
								if ( table.Rows.Count < 1 ) return persons;
								DataRow row = table.Rows[0];
								HostPerson person = GetPerson( row );
								persons.Add( person );
						}
						return persons;
				}

				private List<HostPerson> LoadByAccountId( int accountId )
				{
						List<HostPerson> persons = new List<HostPerson>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AdvisorPersonId = at.AdvisorPersonId
										FROM vPersons p
										INNER JOIN vAccounts a ON a.AccountId = p.AccountId
										INNER JOIN vAccountsExt at ON at.AccountId = p.AccountId
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE a.Roles LIKE '%' + @role + '%' AND p.AccountId = @AccountId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@role", Role.HOST ),
										new SqlParameter( "@AccountId", accountId ) );
								if ( table.Rows.Count < 1 ) return persons;
								DataRow row = table.Rows[0];
								HostPerson person = GetPerson( row );
								persons.Add( person );
						}
						return persons;
				}

				private List<HostPerson> LoadByAdvisorId( int advisorId )
				{
						List<HostPerson> persons = new List<HostPerson>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AdvisorPersonId = at.AdvisorPersonId
										FROM vPersons p
										INNER JOIN vAccounts a ON a.AccountId = p.AccountId
										INNER JOIN vAccountsExt at ON at.AccountId = p.AccountId
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE a.Roles LIKE '%' + @role + '%' AND at.AdvisorId = @advisorId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@role", Role.HOST ),
										new SqlParameter( "@advisorId", advisorId ) );
								if ( table.Rows.Count < 1 ) return persons;
								DataRow row = table.Rows[0];
								HostPerson person = GetPerson( row );
								persons.Add( person );
						}
						return persons;
				}

				public override void Create( HostPerson entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( HostPerson entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( HostPerson entity )
				{
						throw new NotImplementedException();
				}

		}
}
