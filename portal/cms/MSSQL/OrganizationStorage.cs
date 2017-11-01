using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class OrganizationStorage: MSSQLStorage<Organization>
		{
				public OrganizationStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				internal static Organization GetOrganization( DataRow record )
				{
						Organization org = new Organization();
						org.Id = Convert.ToInt32( record["OrganizationId"] );
						org.InstanceId = Convert.ToInt32( record["InstanceId"] );
						org.AccountId = ConvertNullable.ToInt32( record["AccountId"] );
						org.Id1 = Convert.ToString( record["Id1"] );
						org.Id2 = Convert.ToString( record["Id2"] );
						org.Id3 = Convert.ToString( record["Id3"] );
						org.Web = Convert.ToString( record["Web"] );
						org.Name = Convert.ToString( record["Name"] );
						org.RegisteredAddressId = ConvertNullable.ToInt32( record["RegisteredAddressId"] );
						org.RegisteredAddressString = Convert.ToString( record["RegisteredAddressString"] );
						org.CorrespondenceAddressId = ConvertNullable.ToInt32( record["CorrespondenceAddressId"] );
						org.CorrespondenceAddressString = Convert.ToString( record["CorrespondenceAddressString"] );
						org.InvoicingAddressId = ConvertNullable.ToInt32( record["InvoicingAddressId"] );
						org.InvoicingAddressString = Convert.ToString( record["InvoicingAddressString"] );
						org.BankContactId = ConvertNullable.ToInt32( record["BankContactId"] );
						org.ContactPersonId = ConvertNullable.ToInt32( record["ContactPersonId"] );
						org.ContactPersonString = Convert.ToString( record["ContactPersonString"] );
						org.ContactEmail = Convert.ToString( record["ContactEmail"] );
						org.ContactPhone = Convert.ToString( record["ContactPhone"] );
						org.ContactMobile = Convert.ToString( record["ContactMobile"] );
						org.Notes = ConvertNullable.ToString( record["Notes"] );
						return org;
				}

				public override List<Organization> Read( object criteria )
				{
						if ( criteria is Organization.ReadById ) return LoadById( criteria as Organization.ReadById );
						if ( criteria is Organization.ReadByAccountId ) return LoadByAccountId( criteria as Organization.ReadByAccountId );
						List<Organization> orgs = new List<Organization>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
										o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
										o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
										RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
										CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
										InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
										BankContactId = o.BankContactId,
										ContactPersonId = o.ContactPersonId, 
										ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
										ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile
								FROM vOrganizations o
								LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
								LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
								LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
								LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
								WHERE o.InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows ) orgs.Add( GetOrganization( dr ) );
						}
						return orgs;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Organization> LoadById( Organization.ReadById byId )
				{
						List<Organization> orgs = new List<Organization>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
										o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
										o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
										RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
										CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
										InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
										BankContactId = o.BankContactId,
										ContactPersonId = o.ContactPersonId, 
										ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
										ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile
								FROM vOrganizations o
								LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
								LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
								LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
								LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
								WHERE o.OrganizationId = @OrganizationId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@OrganizationId", byId.OrganizationId ) );
								foreach ( DataRow dr in table.Rows ) orgs.Add( GetOrganization( dr ) );
						}
						return orgs;
				}

				private List<Organization> LoadByAccountId( Organization.ReadByAccountId byAccountId )
				{
						List<Organization> orgs = new List<Organization>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
										o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
										o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
										RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
										CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
										InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
										BankContactId = o.BankContactId,
										ContactPersonId = o.ContactPersonId, 
										ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
										ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile
								FROM vOrganizations o
								LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
								LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
								LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
								LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
								WHERE o.AccountId = @AccountId AND o.InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", byAccountId.AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows ) orgs.Add( GetOrganization( dr ) );
						}
						return orgs;
				}

				public override void Create( Organization org )
				{
						using ( SqlConnection connection = Connect() )
						{
								DataSet ds = QueryProc<DataSet>( connection, "pOrganizationCreate",
									new SqlParameter( "@HistoryAccount", Null( Account != null ? Account.Id : (int?)null ) ),
									new SqlParameter( "@InstanceId", InstanceId ),
									new SqlParameter( "@AccountId", Null( org.AccountId ) ),
									new SqlParameter( "@Id1", org.Id1 ),
									new SqlParameter( "@Id2", org.Id2 ),
									new SqlParameter( "@Id3", org.Id3 ),
									new SqlParameter( "@Name", org.Name ),
									new SqlParameter( "@Notes", org.Notes ),
									new SqlParameter( "@Web", org.Web ),
									new SqlParameter( "@ContactEmail", org.ContactEmail ),
									new SqlParameter( "@ContactPhone", org.ContactPhone ),
									new SqlParameter( "@ContactMobile", org.ContactMobile )
								);
								DataTable dtRegAddres = ds.Tables[0];
								DataTable dtCorAddres = ds.Tables[1];
								DataTable dtInvAddres = ds.Tables[2];
								DataTable dtBankContact = ds.Tables[3];
								DataTable dtPersonHomeAddress = ds.Tables[4];
								DataTable dtPersonTempAddress = ds.Tables[5];
								DataTable dtContactPerson = ds.Tables[6];
								DataTable dtOrg = ds.Tables[7];
								org.RegisteredAddressId = ConvertNullable.ToInt32( dtRegAddres.Rows[0]["AddressId"] );
								org.CorrespondenceAddressId = ConvertNullable.ToInt32( dtCorAddres.Rows[0]["AddressId"] );
								org.InvoicingAddressId = ConvertNullable.ToInt32( dtInvAddres.Rows[0]["AddressId"] );
								org.BankContactId = ConvertNullable.ToInt32( dtBankContact.Rows[0]["BankContactId"] );
								org.ContactPersonId = ConvertNullable.ToInt32( dtContactPerson.Rows[0]["PersonId"] );
								org.Id = Convert.ToInt32( dtOrg.Rows[0]["OrganizationId"] );
						}
				}

				public override void Update( Organization org )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pOrganizationModify",
									new SqlParameter( "@HistoryAccount", Null( Account != null ? Account.Id : (int?)null ) ),
									new SqlParameter( "@OrganizationId", org.Id ),
									new SqlParameter( "@Id1", org.Id1 ),
									new SqlParameter( "@Id2", org.Id2 ),
									new SqlParameter( "@Id3", org.Id3 ),
									new SqlParameter( "@Name", org.Name ),
									new SqlParameter( "@Notes", org.Notes ),
									new SqlParameter( "@Web", org.Web ),
									new SqlParameter( "@ContactEmail", org.ContactEmail ),
									new SqlParameter( "@ContactPhone", org.ContactPhone ),
									new SqlParameter( "@ContactMobile", org.ContactMobile )
								);
						}
				}

				public override void Delete( Organization org )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pOrganizationDelete",
									new SqlParameter( "@HistoryAccount", AccountId ),
									new SqlParameter( "@OrganizationId", org.Id )
								);
						}
				}


		}
}
