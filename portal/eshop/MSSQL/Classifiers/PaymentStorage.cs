using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using SHP.Entities.Classifiers;

namespace SHP.MSSQL.Classifiers
{
		public sealed class PaymentStorage: MSSQLStorage<Payment>
		{
				public PaymentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Payment GetPayment( DataRow record )
				{
						Payment payment = new Payment();
						payment.Id = Convert.ToInt32( record["PaymentId"] );
						payment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						payment.Name = Convert.ToString( record["Name"] );
						payment.Code = Convert.ToString( record["Code"] );
						payment.Icon = Convert.ToString( record["Icon"] );
						payment.Locale = Convert.ToString( record["Locale"] );
						payment.Notes = Convert.ToString( record["Notes"] );

						return payment;
				}

				public override List<Payment> Read( object criteria )
				{
						if ( criteria is Payment.ReadById ) return LoadById( criteria as Payment.ReadById );
						if ( criteria is Payment.ReadByCode ) return LoadByCode( criteria as Payment.ReadByCode );
						List<Payment> list = new List<Payment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PaymentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpPayments
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@Locale", Locale ) , new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPayment( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Payment> LoadById( Payment.ReadById by )
				{
						List<Payment> list = new List<Payment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PaymentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpPayments
								WHERE PaymentId = @PaymentId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@PaymentId", by.Id ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPayment( dr ) );
						}
						return list;
				}

				private List<Payment> LoadByCode( Payment.ReadByCode by )
				{
						List<Payment> list = new List<Payment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT PaymentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpPayments
								WHERE Code = @Code AND Locale=@Locale AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Code", by.Code ), new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetPayment( dr ) );
						}
						return list;
				}

				public override void Create( Payment payment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpPaymentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Name", payment.Name ),
										new SqlParameter( "@Code", payment.Code ),
										new SqlParameter( "@Icon", payment.Icon ),
										new SqlParameter( "@Notes", payment.Notes ),
										new SqlParameter( "@Locale", String.IsNullOrEmpty( payment.Locale ) ? Locale : payment.Locale ) );
						}
				}

				public override void Update( Payment payment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpPaymentModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@PaymentId", payment.Id ),
										new SqlParameter( "@Name", payment.Name ),
										new SqlParameter( "@Code", payment.Code ),
										new SqlParameter( "@Icon", payment.Icon ),
										new SqlParameter( "@Notes", payment.Notes ),
										new SqlParameter( "@Locale", payment.Locale ) );
						}
				}

				public override void Delete( Payment payment )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter addressId = new SqlParameter( "@PaymentId", payment.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pShpPaymentDelete", result, historyAccount, addressId );
						}
				}
		}
}
