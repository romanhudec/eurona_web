using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities.Classifiers;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;

namespace CMS.MSSQL.Classifiers
{
		public sealed class PaidServiceStorage: MSSQLStorage<PaidService>
		{
				public PaidServiceStorage( int instanceId, Account paidService, string connectionString )
						: base( instanceId, paidService, connectionString )
				{
				}

				private PaidService GetPaidService( DataRow record )
				{
						PaidService paidService = new PaidService();
						paidService.Id = Convert.ToInt32( record["PaidServiceId"] );
						paidService.InstanceId = Convert.ToInt32( record["InstanceId"] );
						paidService.Name = Convert.ToString( record["Name"] );
						paidService.Notes = Convert.ToString( record["Notes"] );
						paidService.CreditCost = Convert.ToDecimal( record["CreditCost"] );
						return paidService;
				}

				public override List<PaidService> Read( object criteria )
				{
						if ( criteria is PaidService.ReadById ) return LoadById( ( criteria as PaidService.ReadById ).PaidServiceId );
						List<PaidService> accounts = new List<PaidService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PaidServiceId, InstanceId, [Name], [Notes], [CreditCost]
										FROM vPaidServices WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows ) accounts.Add( GetPaidService( dr ) );
						}
						return accounts;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<PaidService> LoadById( int id )
				{
						List<PaidService> accounts = new List<PaidService>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT PaidServiceId, InstanceId, [Name], [Notes], [CreditCost]
										FROM vPaidServices
										WHERE PaidServiceId = @PaidServiceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@PaidServiceId", id ) );
								if ( table.Rows.Count > 0 )
										accounts.Add( GetPaidService( table.Rows[0] ) );
						}
						return accounts;
				}

				public override void Create( PaidService paidService )
				{
						throw new NotImplementedException();
				}

				public override void Update( PaidService paidService )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) );
								SqlParameter paidServiceId = new SqlParameter( "@PaidServiceId", paidService.Id );
								SqlParameter creditCost = new SqlParameter( "@CreditCost", paidService.CreditCost );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pPaidServiceModify", result, historyAccount, paidServiceId, creditCost );
						}
				}

				public override void Delete( PaidService paidService )
				{
						throw new NotImplementedException();
				}
		}
}
