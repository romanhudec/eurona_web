using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using SHP.Entities.Classifiers;
using ShpOrderStatus = SHP.Entities.Order.OrderStatus;

namespace Eurona.DAL.MSSQL.Classifiers
{
		internal sealed class OrderStatusStorage: MSSQLStorage<OrderStatus>
		{
				public OrderStatusStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static OrderStatus GetOrderStatus( DataRow record )
				{
						OrderStatus orderStatus = new OrderStatus();
						orderStatus.Id = Convert.ToInt32( record["OrderStatusId"] );
						orderStatus.InstanceId = Convert.ToInt32( record["InstanceId"] );
						orderStatus.Code = Convert.ToString( record["Code"] );
						orderStatus.Name = Convert.ToString( record["Name"] );
						orderStatus.Code = Convert.ToString( record["Code"] );
						orderStatus.Icon = Convert.ToString( record["Icon"] );
						orderStatus.Locale = Convert.ToString( record["Locale"] );
						orderStatus.Notes = Convert.ToString( record["Notes"] );

						return orderStatus;
				}

				public override List<OrderStatus> Read( object criteria )
				{
						if ( criteria is OrderStatus.ReadById ) return LoadById( criteria as OrderStatus.ReadById );
						if ( criteria is OrderStatus.ReadByCode ) return LoadByCode( criteria as OrderStatus.ReadByCode );
						List<OrderStatus> list = new List<OrderStatus>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT OrderStatusId, InstanceId, Code, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpOrderStatuses
								WHERE Locale = @Locale-- AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetOrderStatus( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<OrderStatus> LoadById( OrderStatus.ReadById by )
				{
						List<OrderStatus> list = new List<OrderStatus>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT OrderStatusId, InstanceId, Code, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpOrderStatuses
								WHERE OrderStatusId = @OrderStatusId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@OrderStatusId", by.Id ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetOrderStatus( dr ) );
						}
						return list;
				}

				private List<OrderStatus> LoadByCode( OrderStatus.ReadByCode by )
				{
						List<OrderStatus> list = new List<OrderStatus>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT OrderStatusId, InstanceId, Code, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpOrderStatuses
								WHERE Code = @Code AND Locale=@Locale-- AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Code", by.Code ), new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetOrderStatus( dr ) );
						}
						return list;
				}

				public override void Create( OrderStatus orderStatus )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpOrderStatusCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Name", orderStatus.Name ),
										new SqlParameter( "@Code", orderStatus.Code ),
										new SqlParameter( "@Icon", orderStatus.Icon ),
										new SqlParameter( "@Notes", orderStatus.Notes ),
										new SqlParameter( "@Locale", String.IsNullOrEmpty( orderStatus.Locale ) ? Locale : orderStatus.Locale ) );
						}
				}

				public override void Update( OrderStatus orderStatus )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpOrderStatusModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@OrderStatusId", orderStatus.Id ),
										new SqlParameter( "@Name", orderStatus.Name ),
										new SqlParameter( "@Code", orderStatus.Code ),
										new SqlParameter( "@Icon", orderStatus.Icon ),
										new SqlParameter( "@Notes", orderStatus.Notes ),
										new SqlParameter( "@Locale", orderStatus.Locale ) );
						}
				}

				public override void Delete( OrderStatus orderStatus )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter addressId = new SqlParameter( "@OrderStatusId", orderStatus.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pShpOrderStatusDelete", result, historyAccount, addressId );
						}
				}
		}
}
