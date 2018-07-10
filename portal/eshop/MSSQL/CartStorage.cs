using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;

namespace SHP.MSSQL
{
		public sealed class CartStorage: MSSQLStorage<Cart>
		{
//                private const string entitySelect = @"SELECT CartId, InstanceId, AccountId, SessionId, Created, Closed, PriceTotal, PriceTotalWVAT,
//								ShipmentCode ,ShipmentName ,ShipmentPrice ,PaymentCode, PaymentName, DeliveryAddressId, InvoiceAddressId, Notes
//								FROM vShpCarts";
            private const string entitySelect = @"SELECT CartId, InstanceId, AccountId, SessionId, Created, Closed, PriceTotal, PriceTotalWVAT,
								ShipmentCode ,PaymentCode, DeliveryAddressId, InvoiceAddressId, Notes
								FROM vShpCarts";

				public CartStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Cart GetCart( DataRow record )
				{
						Cart cart = new Cart();
						cart.Id = Convert.ToInt32( record["CartId"] );
						cart.InstanceId = Convert.ToInt32( record["InstanceId"] );
						cart.AccountId = ConvertNullable.ToInt32( record["AccountId"] );
						cart.SessionId = ConvertNullable.ToInt32( record["SessionId"] );
						cart.ShipmentCode = Convert.ToString( record["ShipmentCode"] );
						cart.PaymentCode = Convert.ToString( record["PaymentCode"] );
						cart.DeliveryAddressId = Convert.ToInt32( record["DeliveryAddressId"] );
						cart.InvoiceAddressId = Convert.ToInt32( record["InvoiceAddressId"] );
						cart.Created = Convert.ToDateTime( record["Created"] );
						cart.Closed = ConvertNullable.ToDateTime( record["Closed"] );
						cart.PriceTotal = ConvertNullable.ToDecimal( record["PriceTotal"] );
						cart.PriceTotalWVAT = ConvertNullable.ToDecimal( record["PriceTotalWVAT"] );
						cart.Notes = Convert.ToString( record["Notes"] );

						return cart;
				}

				public override List<Cart> Read( object criteria )
				{
						if ( criteria is Cart.ReadById ) return LoadById( criteria as Cart.ReadById );
						if ( criteria is Cart.ReadByAccount ) return LoadByAccountId( criteria as Cart.ReadByAccount );
						if ( criteria is Cart.ReadOpenByAccount ) return LoadOpenByAccountId( criteria as Cart.ReadOpenByAccount );
						if ( criteria is Cart.ReadBySessionId ) return LoadBySessionId( criteria as Cart.ReadBySessionId );
						List<Cart> cartList = new List<Cart>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE InstanceId = @InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										cartList.Add( GetCart( dr ) );
						}
						return cartList;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Cart> LoadById( Cart.ReadById byCartId )
				{
						List<Cart> cartList = new List<Cart>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE CartId = @CartId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@CartId", byCartId.CartId ) );
								foreach ( DataRow dr in table.Rows )
										cartList.Add( GetCart( dr ) );
						}
						return cartList;
				}

				private List<Cart> LoadOpenByAccountId( Cart.ReadOpenByAccount by )
				{
						List<Cart> cartList = new List<Cart>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE InstanceId = @InstanceId AND AccountId = @AccountId AND Closed IS NULL";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", by.AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										cartList.Add( GetCart( dr ) );
						}
						return cartList;
				}

				private List<Cart> LoadByAccountId( Cart.ReadByAccount by )
				{
						List<Cart> cartList = new List<Cart>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE InstanceId = @InstanceId AND AcountId = @AccountId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AcountId", by.AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										cartList.Add( GetCart( dr ) );
						}
						return cartList;
				}

				private List<Cart> LoadBySessionId( Cart.ReadBySessionId by )
				{
						List<Cart> cartList = new List<Cart>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE InstanceId = @InstanceId AND SessionId = @SessionId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@SessionId", by.SessionId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										cartList.Add( GetCart( dr ) );
						}
						return cartList;
				}

				public override void Create( Cart cart )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								DataSet ds = QueryProc<DataSet>( connection, "pShpCartCreate",
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@AccountId", Null( cart.AccountId ) ),
										new SqlParameter( "@SessionId", Null( cart.SessionId ) ),
										new SqlParameter( "@ShipmentCode", Null( cart.ShipmentCode ) ),
										new SqlParameter( "@PaymentCode", Null( cart.PaymentCode ) ),
										new SqlParameter( "@Closed", cart.Closed ),
										new SqlParameter( "@Notes", Null( cart.Notes ) ),
										result );

								DataTable dtDeliveryAddres = ds.Tables[0];
								DataTable dtInvoiceAddres = ds.Tables[1];
								DataTable dtCart = ds.Tables[2];

								cart.DeliveryAddressId = Convert.ToInt32( dtDeliveryAddres.Rows[0][0] );
								cart.InvoiceAddressId = Convert.ToInt32( dtInvoiceAddres.Rows[0][0] );
								cart.Id = Convert.ToInt32( dtCart.Rows[0][0] );
						}

				}

				public override void Update( Cart cart )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpCartModify",
										new SqlParameter( "@CartId", cart.Id ),
										new SqlParameter( "@AccountId", Null( cart.AccountId ) ),
										new SqlParameter( "@SessionId", Null( cart.SessionId ) ),
										new SqlParameter( "@ShipmentCode", Null( cart.ShipmentCode ) ),
										new SqlParameter( "@PaymentCode", Null( cart.PaymentCode ) ),
										new SqlParameter( "@Closed", cart.Closed ),
										new SqlParameter( "@Notes", Null( cart.Notes ) )
										);
						}
				}

				public override void Delete( Cart cart )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter id = new SqlParameter( "@CartId", cart.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pShpCartDelete", result, id );
						}
				}
		}
}
