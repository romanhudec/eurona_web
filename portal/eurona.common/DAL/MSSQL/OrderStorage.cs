using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL {
    public sealed class OrderStorage : MSSQLStorage<Order> {
        private const string entitySelect = @"SELECT OrderId, InstanceId, OrderDate, OrderNumber ,CartId, PaydDate ,AccountId, AccountName, OrderStatusCode ,OrderStatusName, OrderStatusIcon ,
						ShipmentCode ,ShipmentName, ShipmentIcon ,ShipmentPrice, ShipmentPriceWVAT ,Price, PriceWVAT, PaymentCode, PaymentName, PaymentIcon, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, Notes, Notified, Exported,
						CurrencyId, CurrencyCode, CurrencySymbol, CreatedByAccountId, ShipmentFrom, ShipmentTo,
						OwnerName, CreatedByName		
						FROM vShpOrders";
        public OrderStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Order GetOrder(DataRow record) {
            Order order = new Order();
            order.Id = Convert.ToInt32(record["OrderId"]);
            order.InstanceId = Convert.ToInt32(record["InstanceId"]);
            order.OrderDate = Convert.ToDateTime(record["OrderDate"]);
            order.OrderNumber = Convert.ToString(record["OrderNumber"]);
            order.OrderStatusCode = Convert.ToString(record["OrderStatusCode"]);
            order.PaydDate = ConvertNullable.ToDateTime(record["PaydDate"]);
            order.CartId = Convert.ToInt32(record["CartId"]);
            order.ShipmentCode = Convert.ToString(record["ShipmentCode"]);
            order.PaymentCode = Convert.ToString(record["PaymentCode"]);
            order.DeliveryAddressId = ConvertNullable.ToInt32(record["DeliveryAddressId"]);
            order.InvoiceAddressId = ConvertNullable.ToInt32(record["InvoiceAddressId"]);
            order.InvoiceUrl = Convert.ToString(record["InvoiceUrl"]);
            order.Notes = Convert.ToString(record["Notes"]);
            order.Price = Convert.ToDecimal(record["Price"]);
            order.PriceWVAT = Convert.ToDecimal(record["PriceWVAT"]);
            order.Notified = Convert.ToBoolean(record["Notified"]);
            order.Exported = Convert.ToBoolean(record["Exported"]);
            order.CurrencyId = ConvertNullable.ToInt32(record["CurrencyId"]);
            order.CurrencyCode = Convert.ToString(record["CurrencyCode"]);
            order.CurrencySymbol = Convert.ToString(record["CurrencySymbol"]);
            order.CreatedByAccountId = Convert.ToInt32(record["CreatedByAccountId"]);

            order.ShipmentFrom = ConvertNullable.ToDateTime(record["ShipmentFrom"]);
            order.ShipmentTo = ConvertNullable.ToDateTime(record["ShipmentTo"]);

            //Joined properties
            order.AccountId = Convert.ToInt32(record["AccountId"]);
            order.AccountName = Convert.ToString(record["AccountName"]);
            order.OrderStatusName = Convert.ToString(record["OrderStatusName"]);
            order.OrderStatusIcon = Convert.ToString(record["OrderStatusIcon"]);
            order.ShipmentName = Convert.ToString(record["ShipmentName"]);
            order.ShipmentIcon = Convert.ToString(record["ShipmentIcon"]);
            order.ShipmentPrice = ConvertNullable.ToDecimal(record["ShipmentPrice"]);
            order.ShipmentPriceWVAT = ConvertNullable.ToDecimal(record["ShipmentPriceWVAT"]);
            order.PaymentName = Convert.ToString(record["PaymentName"]);
            order.PaymentIcon = Convert.ToString(record["PaymentIcon"]);

            order.OwnerName = Convert.ToString(record["OwnerName"]);
            order.CreatedByName = Convert.ToString(record["CreatedByName"]);

            return order;
        }

        public override List<Order> Read(object criteria) {
            if (criteria is Order.ReadById) return LoadById(criteria as Order.ReadById);
            if (criteria is Order.ReadByAccount) return LoadByAccountId(criteria as Order.ReadByAccount);
            
            if (criteria is Order.ReadByCart) return LoadByCartId(criteria as Order.ReadByCart);
            if (criteria is Order.ReadByAccountYearMonth) return LoadByYearMonthForAccount(criteria as Order.ReadByAccountYearMonth);
            if (criteria is Order.ReadByFilter) return LoadByFilter(criteria as Order.ReadByFilter);
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Order> LoadById(Order.ReadById byOrderId) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE OrderId = @OrderId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@OrderId", byOrderId.OrderId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }

        private List<Order> LoadByAccountId(Order.ReadByAccount by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE (AccountId = @AccountId OR CreatedByAccountId=@AccountId) AND InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", by.AccountId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }

        private List<Order> LoadByCartId(Order.ReadByCart by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE CartId = @CartId AND InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartId", by.CartId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }
        private List<Order> LoadByYearMonthForAccount(Order.ReadByAccountYearMonth by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE AccountId=@AccountId AND @Year=YEAR(OrderDate) AND @Month=MONTH(OrderDate) AND InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Year", by.Year),
                        new SqlParameter("@Month", by.Month),
                        new SqlParameter("@AccountId", by.AccountId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }
        private List<Order> LoadByFilter(Order.ReadByFilter by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += @" WHERE InstanceId = @InstanceId AND 
										(@AccountId IS NULL OR (AccountId = @AccountId OR CreatedByAccountId=@AccountId)) AND
										(@OrderNumber IS NULL OR OrderNumber LIKE @OrderNumber + '%') AND
										(@OrderStatusCode IS NULL OR OrderStatusCode = @OrderStatusCode) AND
										(@ShipmentCode IS NULL OR ShipmentCode = @ShipmentCode) AND
										(@Notified IS NULL OR Notified = @Notified) AND
										(@Exported IS NULL OR Exported = @Exported)";
                DataTable table = Query<DataTable>(connection, sql,
                            new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@OrderNumber", Null(string.IsNullOrEmpty(by.OrderNumber) ? null : (object)by.OrderNumber)),
                        new SqlParameter("@OrderStatusCode", Null(by.OrderStatusCode)),
                        new SqlParameter("@ShipmentCode", Null(by.ShipmentCode)),
                        new SqlParameter("@Notified", Null(by.Notified)),
                        new SqlParameter("@Exported", Null(by.Exported)));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }

        public override void Create(Order order) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpOrderCreate",
                        new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)1)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@CartId", order.CartId),
                        new SqlParameter("@OrderDate", order.OrderDate),
                        new SqlParameter("@OrderStatusCode", Null(order.OrderStatusCode)),
                        new SqlParameter("@ShipmentCode", Null(order.ShipmentCode)),
                        new SqlParameter("@ShipmentPrice", Null(order.ShipmentPrice)),
                        new SqlParameter("@ShipmentPriceWVAT", Null(order.ShipmentPriceWVAT)),
                        new SqlParameter("@PaymentCode", Null(order.PaymentCode)),
                        new SqlParameter("@DeliveryAddressId", Null(order.DeliveryAddressId)),
                        new SqlParameter("@InvoiceAddressId", Null(order.InvoiceAddressId)),
                        new SqlParameter("@InvoiceUrl", Null(order.InvoiceUrl)),
                        new SqlParameter("@PaydDate", Null(order.PaydDate)),
                        new SqlParameter("@Notes", Null(order.Notes)),
                        new SqlParameter("@Price", order.Price),
                        new SqlParameter("@PriceWVAT", order.PriceWVAT),
                        new SqlParameter("@Notified", order.Notified),
                        new SqlParameter("@Exported", order.Exported),
                        new SqlParameter("@CurrencyId", Null(order.CurrencyId)),
                        new SqlParameter("@CreatedByAccountId", Null(order.CreatedByAccountId)),
                        new SqlParameter("@ShipmentFrom", Null(order.ShipmentFrom)),
                        new SqlParameter("@ShipmentTo", Null(order.ShipmentTo)),
                        result);

                order.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(Order order) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpOrderModify",
                        new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)1)),
                        new SqlParameter("@OrderId", order.Id),
                        new SqlParameter("@CartId", order.CartId),
                        new SqlParameter("@OrderDate", order.OrderDate),
                        new SqlParameter("@OrderStatusCode", Null(order.OrderStatusCode)),
                        new SqlParameter("@ShipmentCode", Null(order.ShipmentCode)),
                        new SqlParameter("@ShipmentPrice", Null(order.ShipmentPrice)),
                        new SqlParameter("@ShipmentPriceWVAT", Null(order.ShipmentPriceWVAT)),
                        new SqlParameter("@PaymentCode", Null(order.PaymentCode)),
                        new SqlParameter("@PaydDate", Null(order.PaydDate)),
                        new SqlParameter("@InvoiceUrl", Null(order.InvoiceUrl)),
                        new SqlParameter("@Notes", Null(order.Notes)),
                        new SqlParameter("@Price", order.Price),
                        new SqlParameter("@PriceWVAT", order.PriceWVAT),
                        new SqlParameter("@Notified", order.Notified),
                        new SqlParameter("@Exported", order.Exported),
                        new SqlParameter("@CurrencyId", Null(order.CurrencyId)),
                        new SqlParameter("@CreatedByAccountId", Null(order.CreatedByAccountId)),
                        new SqlParameter("@ShipmentFrom", Null(order.ShipmentFrom)),
                        new SqlParameter("@ShipmentTo", Null(order.ShipmentTo))
                        );
            }
        }

        public override void Delete(Order order) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)1));
                SqlParameter id = new SqlParameter("@OrderId", order.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpOrderDelete", result, historyAccount, id);
            }
        }
    }
}
