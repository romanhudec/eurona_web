using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ProvidedServiceStorage: MSSQLStorage<ProvidedService>
		{
				public ProvidedServiceStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ProvidedService GetProvidedService( DataRow record )
				{
						ProvidedService providedService = new ProvidedService();
						providedService.Id = Convert.ToInt32( record["ProvidedServiceId"] );
						providedService.InstanceId = Convert.ToInt32( record["InstanceId"] );
						providedService.AccountId = Convert.ToInt32( record["AccountId"] );
						providedService.PaidServiceId = Convert.ToInt32( record["PaidServiceId"] );
						providedService.ObjectId = ConvertNullable.ToInt32( record["ObjectId"] );
						providedService.CreditCost = Convert.ToDecimal( record["CreditCost"] );
						providedService.Name = Convert.ToString( record["Name"] );
						providedService.Notes = Convert.ToString( record["Notes"] );
						providedService.ServiceDate = Convert.ToDateTime( record["ServiceDate"] );
						return providedService;
				}

				public override List<ProvidedService> Read( object criteria )
				{
						if ( criteria is ProvidedService.ReadById ) return LoadById( criteria as ProvidedService.ReadById );
						if ( criteria is ProvidedService.ReadBy ) return LoadBy( criteria as ProvidedService.ReadBy );
						if ( criteria is ProvidedService.ReadByAccountId ) return LoadByAccountId( criteria as ProvidedService.ReadByAccountId );
						if ( criteria is ProvidedService.ReadByPaidServiceId ) return LoadByPaidServiceId( criteria as ProvidedService.ReadByPaidServiceId );
						List<ProvidedService> list = new List<ProvidedService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProvidedServiceId, InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate, [Name], [Notes]
								FROM vProvidedServices WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProvidedService( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ProvidedService> LoadById( ProvidedService.ReadById byProvidedServiceId )
				{
						List<ProvidedService> list = new List<ProvidedService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProvidedServiceId, InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate, [Name], [Notes], CreditCost
								FROM vProvidedServices
								WHERE ProvidedServiceId = @ProvidedServiceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ProvidedServiceId", byProvidedServiceId.ProvidedServiceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProvidedService( dr ) );
						}
						return list;
				}

				private List<ProvidedService> LoadBy( ProvidedService.ReadBy by )
				{
						List<ProvidedService> list = new List<ProvidedService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProvidedServiceId, InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate, [Name], [Notes], CreditCost
								FROM vProvidedServices
								WHERE ObjectId = @ObjectId AND PaidServiceId = @PaidServiceId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ObjectId", by.ObjectId ),
										new SqlParameter( "@PaidServiceId", by.PaidServiceId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProvidedService( dr ) );
						}
						return list;
				}
				private List<ProvidedService> LoadByAccountId( ProvidedService.ReadByAccountId byAccountId )
				{
						List<ProvidedService> list = new List<ProvidedService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProvidedServiceId, InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate, [Name], [Notes], CreditCost
								FROM vProvidedServices
								WHERE AccountId = @AccountId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", byAccountId.AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProvidedService( dr ) );
						}
						return list;
				}

				private List<ProvidedService> LoadByPaidServiceId( ProvidedService.ReadByPaidServiceId byPaidServiceId )
				{
						List<ProvidedService> list = new List<ProvidedService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProvidedServiceId, InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate, [Name], [Notes], CreditCost
								FROM vProvidedServices
								WHERE PaidServiceId = @PaidServiceId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@PaidServiceId", byPaidServiceId.PaidServiceId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProvidedService( dr ) );
						}
						return list;
				}


				public override void Create( ProvidedService providedService )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pProvidedServiceCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@AccountId", providedService.AccountId ),
										new SqlParameter( "@PaidServiceId", providedService.PaidServiceId ),
										new SqlParameter( "@ObjectId", Null( providedService.ObjectId ) ) );
						}
				}

				public override void Update( ProvidedService providedService )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ProvidedService providedService )
				{
						throw new NotImplementedException();
				}

		}
}
