﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;
using Eurona.DAL.Entities;

namespace Eurona.DAL.MSSQL {
    [Serializable]
    public sealed class OrderStorage : MSSQLStorage<Order> {
        private const string entitySelect = @"SELECT TOP 1000 OrderId, InstanceId, OrderDate, OrderNumber ,CartId, PaydDate ,AccountId, AccountName, OrderStatusCode ,OrderStatusName, OrderStatusIcon ,
						ShipmentCode ,ShipmentName, ShipmentIcon ,ShipmentPrice, ShipmentPriceWVAT ,Price, PriceWVAT, PaymentCode, PaymentName, PaymentIcon, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, Notes, Notified, Exported,
						CurrencyId, CurrencyCode, CurrencySymbol, ParentId, AssociationAccountId, AssociationRequestStatus,
						CreatedByAccountId, OwnerName, CreatedByName, ShipmentFrom, ShipmentTo, TVD_Id, NoPostage, ChildsCount, 
                        ZavozoveMisto_Mesto, ZavozoveMisto_Psc, ZavozoveMisto_Popis, ZavozoveMisto_DatumACas, ZavozoveMisto_OsobniOdberVSidleSpolecnosti
						FROM vShpOrders o WITH (NOLOCK)";
        public OrderStorage(int instanceId, Eurona.DAL.Entities.Account account, string connectionString)
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

            order.ShipmentFrom = ConvertNullable.ToDateTime(record["ShipmentFrom"]);
            order.ShipmentTo = ConvertNullable.ToDateTime(record["ShipmentTo"]);

            order.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            order.AssociationAccountId = ConvertNullable.ToInt32(record["AssociationAccountId"]);
            order.AssociationRequestStatus = ConvertNullable.ToInt32(record["AssociationRequestStatus"]);

            order.CreatedByAccountId = Convert.ToInt32(record["CreatedByAccountId"]);
            order.NoPostage = Convert.ToBoolean(record["NoPostage"]);

            order.ZavozoveMisto_Mesto = Convert.ToString(record["ZavozoveMisto_Mesto"]);
            order.ZavozoveMisto_Psc = Convert.ToString(record["ZavozoveMisto_Psc"]);
            order.ZavozoveMisto_Popis = Convert.ToString(record["ZavozoveMisto_Popis"]);
            order.ZavozoveMisto_DatumACas = ConvertNullable.ToDateTime(record["ZavozoveMisto_DatumACas"]);
            order.ZavozoveMisto_OsobniOdberVSidleSpolecnosti = Convert.ToBoolean(record["ZavozoveMisto_OsobniOdberVSidleSpolecnosti"] == DBNull.Value ? false : record["ZavozoveMisto_OsobniOdberVSidleSpolecnosti"]);

            //Joined properties
            order.AccountId = Convert.ToInt32(record["AccountId"]);
            order.TVD_Id = ConvertNullable.ToInt32(record["TVD_Id"]);
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
            if (criteria is Order.ReadLastByAccount) return LoadLastByAccountId(criteria as Order.ReadLastByAccount);
            if (criteria is Order.ReadByCart) return LoadByCartId(criteria as Order.ReadByCart);
            if (criteria is Order.ReadByFilter) return LoadByFilter(criteria as Order.ReadByFilter);
            if (criteria is Order.ReadByAccountYearMonth) return LoadByYearMonthForAccount(criteria as Order.ReadByAccountYearMonth);
            if (criteria is Order.ReadNot) return LoadByNot(criteria as Order.ReadNot);
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

        private List<Order> LoadLastByAccountId(Order.ReadLastByAccount by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = @";WITH orders (OrderId) AS
                    (                
	                    SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                        INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                    WHERE o.HistoryId IS NULL AND o.InstanceId = @InstanceId
                        AND (c.AccountId = @AccountId )" +
                        (!string.IsNullOrEmpty(by.GreaterAtOrderNumber) ? " AND (OrderNumber LIKE @OrderNumber + '%')" : "") +
                        @"union all
	                    SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                        INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                    WHERE o.HistoryId IS NULL AND o.InstanceId = @InstanceId 
                        AND (CreatedByAccountId=@AccountId)" +
                        (!string.IsNullOrEmpty(by.GreaterAtOrderNumber) ? " AND (OrderNumber LIKE @OrderNumber + '%')" : "") +
                    @")
                    SELECT ors.OrderId, o.InstanceId, o.OrderDate, o.OrderNumber ,o.CartId, o.PaydDate ,o.AccountId, o.AccountName, o.OrderStatusCode ,o.OrderStatusName, 
                    o.OrderStatusIcon , o.ShipmentCode ,o.ShipmentName, o.ShipmentIcon ,o.ShipmentPrice, o.ShipmentPriceWVAT ,o.Price, o.PriceWVAT, o.PaymentCode, o.PaymentName, 
                    o.PaymentIcon, o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.Notes, o.Notified, o.Exported,  o.CurrencyId, o.CurrencyCode, o.CurrencySymbol, o.ParentId, 
                    o.AssociationAccountId, o.AssociationRequestStatus, o.CreatedByAccountId, o.OwnerName, o.CreatedByName, o.ShipmentFrom, o.ShipmentTo, o.TVD_Id, o.NoPostage, o.ChildsCount, 
                    o.ZavozoveMisto_Mesto, o.ZavozoveMisto_Psc, o.ZavozoveMisto_Popis, o.ZavozoveMisto_DatumACas, o.ZavozoveMisto_OsobniOdberVSidleSpolecnosti
                    FROM ( select * FROM orders group by OrderId )  as ors INNER JOIN vShpOrders o ON o.OrderId=ors.OrderId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", by.AccountId),
                        new SqlParameter("@OrderNumber", Null(by.GreaterAtOrderNumber)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }


        private List<Order> LoadByAccountId(Order.ReadByAccount by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = @";WITH orders (OrderId) AS
                (                
	                SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                    INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                WHERE o.HistoryId IS NULL AND o.InstanceId = @InstanceId
                    AND (c.AccountId = @AccountId )
                    union all
	                SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                    INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                WHERE o.HistoryId IS NULL AND o.InstanceId = @InstanceId 
                    AND (CreatedByAccountId=@AccountId)
                )
                SELECT ors.OrderId, o.InstanceId, o.OrderDate, o.OrderNumber ,o.CartId, o.PaydDate ,o.AccountId, o.AccountName, o.OrderStatusCode ,o.OrderStatusName, 
                o.OrderStatusIcon , o.ShipmentCode ,o.ShipmentName, o.ShipmentIcon ,o.ShipmentPrice, o.ShipmentPriceWVAT ,o.Price, o.PriceWVAT, o.PaymentCode, o.PaymentName, 
                o.PaymentIcon, o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.Notes, o.Notified, o.Exported,  o.CurrencyId, o.CurrencyCode, o.CurrencySymbol, o.ParentId, 
                o.AssociationAccountId, o.AssociationRequestStatus, o.CreatedByAccountId, o.OwnerName, o.CreatedByName, o.ShipmentFrom, o.ShipmentTo, o.TVD_Id, o.NoPostage, o.ChildsCount, 
                o.ZavozoveMisto_Mesto, o.ZavozoveMisto_Psc, o.ZavozoveMisto_Popis, o.ZavozoveMisto_DatumACas, o.ZavozoveMisto_OsobniOdberVSidleSpolecnosti
                FROM ( select * FROM orders group by OrderId )  as ors INNER JOIN vShpOrders o ON o.OrderId=ors.OrderId";
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
                string sql = @";WITH orders (OrderId) AS
                (                
	                SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                    INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                WHERE o.CartId=@CartId AND o.HistoryId IS NULL AND o.InstanceId = @InstanceId
                )
                SELECT ors.OrderId, o.InstanceId, o.OrderDate, o.OrderNumber ,o.CartId, o.PaydDate ,o.AccountId, o.AccountName, o.OrderStatusCode ,o.OrderStatusName, 
                o.OrderStatusIcon ,o.ShipmentCode ,o.ShipmentName, o.ShipmentIcon ,o.ShipmentPrice, o.ShipmentPriceWVAT ,o.Price, o.PriceWVAT, o.PaymentCode, o.PaymentName, 
                o.PaymentIcon, o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.Notes, o.Notified, o.Exported, o.CurrencyId, o.CurrencyCode, o.CurrencySymbol, o.ParentId, 
                o.AssociationAccountId, o.AssociationRequestStatus, o.CreatedByAccountId, o.OwnerName, o.CreatedByName, o.ShipmentFrom, o.ShipmentTo, o.TVD_Id, o.NoPostage, o.ChildsCount, 
                o.ZavozoveMisto_Mesto, o.ZavozoveMisto_Psc, o.ZavozoveMisto_Popis, o.ZavozoveMisto_DatumACas, o.ZavozoveMisto_OsobniOdberVSidleSpolecnosti
                FROM ( select * FROM orders group by OrderId )  as ors INNER JOIN vShpOrders o ON o.OrderId=ors.OrderId";
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
                sql += @" WHERE InstanceId = @InstanceId";
                sql += by.AccountId.HasValue ? " AND (AccountId = @AccountId OR CreatedByAccountId=@AccountId)" : "";
                sql += !string.IsNullOrEmpty(by.OrderNumber) ? " AND (OrderNumber LIKE @OrderNumber + '%')" : "";
                sql += !string.IsNullOrEmpty(by.OrderStatusCode) ? " AND (OrderStatusCode LIKE @OrderStatusCode)" : "";
                sql += !string.IsNullOrEmpty(by.NotOrderStatusCode) ? " AND (OrderStatusCode NOT LIKE @NotOrderStatusCode)" : "";
                sql += !string.IsNullOrEmpty(by.ShipmentCode) ? " AND (ShipmentCode LIKE @ShipmentCode)" : "";
                sql += by.Notified.HasValue ? " AND (Notified = @Notified)" : "";
                sql += by.Exported.HasValue ? " AND (Exported = @Exported)" : "";
                sql += by.ParentId.HasValue ? " AND (ParentId = @ParentId)" : "";
                sql += by.AssociationAccountId.HasValue ? " AND (AssociationAccountId = @AssociationAccountId)" : "";
                sql += by.AssociationRequestStatus.HasValue ? " AND (AssociationRequestStatus = @AssociationRequestStatus)" : "";
                sql += by.CreatedByAccountId.HasValue ? " AND (CreatedByAccountId = @CreatedByAccountId)" : "";
                sql += by.HasChilds.HasValue ? " AND (( @HasChilds = 1 AND ChildsCount != 0) OR ( @HasChilds = 0 AND ChildsCount=0))" : "";
                sql += by.OnlyLastMonths.HasValue ? " AND (OrderDate >= DATEADD(M, @OnlyLastMonths*-1, GETDATE()))" : "";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@OrderNumber", Null(string.IsNullOrEmpty(by.OrderNumber) ? null : (object)by.OrderNumber)),
                        new SqlParameter("@OrderStatusCode", Null(by.OrderStatusCode)),
                        new SqlParameter("@NotOrderStatusCode", Null(by.NotOrderStatusCode)),
                        new SqlParameter("@ShipmentCode", Null(by.ShipmentCode)),
                        new SqlParameter("@Notified", Null(by.Notified)),
                        new SqlParameter("@Exported", Null(by.Exported)),
                        new SqlParameter("@ParentId", Null(by.ParentId)),
                        new SqlParameter("@AssociationAccountId", Null(by.AssociationAccountId)),
                        new SqlParameter("@AssociationRequestStatus", Null(by.AssociationRequestStatus)),
                        new SqlParameter("@CreatedByAccountId", Null(by.CreatedByAccountId)),
                        new SqlParameter("@HasChilds", Null(by.HasChilds)),
                        new SqlParameter("@OnlyLastMonths", Null(by.OnlyLastMonths))
                        );
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrder(dr));
            }
            return list;
        }

        private List<Order> LoadByNot(Order.ReadNot by) {
            List<Order> list = new List<Order>();
            using (SqlConnection connection = Connect()) {
                string sql = @"WITH orders (OrderId) AS
                (                
	                SELECT  o.OrderId FROM tShpOrder o WITH (NOLOCK)
                    INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	                WHERE o.HistoryId IS NULL AND o.InstanceId = @InstanceId" +
                    (by.AccountId.HasValue ? " AND (AccountId = @AccountId OR CreatedByAccountId=@AccountId)" : "") +
                    (!string.IsNullOrEmpty(by.OrderStatusCode) ? " AND (OrderStatusCode NOT LIKE @OrderStatusCode)" : "") +
                    @"GROUP BY 
	                o.OrderId
                )

                SELECT ors.OrderId, o.InstanceId, o.OrderDate, o.OrderNumber ,o.CartId, o.PaydDate ,o.AccountId, o.AccountName, o.OrderStatusCode ,o.OrderStatusName, 
                o.OrderStatusIcon ,o.ShipmentCode ,o.ShipmentName, o.ShipmentIcon ,o.ShipmentPrice, o.ShipmentPriceWVAT ,o.Price, o.PriceWVAT, o.PaymentCode, o.PaymentName, 
                o.PaymentIcon, o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.Notes, o.Notified, o.Exported, o.CurrencyId, o.CurrencyCode, o.CurrencySymbol, o.ParentId, 
                o.AssociationAccountId, o.AssociationRequestStatus, o.CreatedByAccountId, o.OwnerName, o.CreatedByName, o.ShipmentFrom, o.ShipmentTo, o.TVD_Id, o.NoPostage, o.ChildsCount, 
                o.ZavozoveMisto_Mesto, o.ZavozoveMisto_Psc, o.ZavozoveMisto_Popis, o.ZavozoveMisto_DatumACas, o.ZavozoveMisto_OsobniOdberVSidleSpolecnosti
                FROM orders ors
                INNER JOIN vShpOrders o ON o.OrderId=ors.OrderId";

                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@OrderStatusCode", Null(by.OrderStatusCode)));
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
                        new SqlParameter("@OrderDate", order.OrderDate),
                        new SqlParameter("@CartId", order.CartId),
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
                        new SqlParameter("@ParentId", Null(order.ParentId)),
                        new SqlParameter("@AssociationAccountId", Null(order.AssociationAccountId)),
                        new SqlParameter("@AssociationRequestStatus", Null(order.AssociationRequestStatus)),
                        new SqlParameter("@CreatedByAccountId", Null(order.CreatedByAccountId)),
                        new SqlParameter("@ShipmentFrom", Null(order.ShipmentFrom)),
                        new SqlParameter("@ShipmentTo", Null(order.ShipmentTo)),
                        new SqlParameter("@NoPostage", Null(order.NoPostage)),
                        new SqlParameter("@ZavozoveMisto_Mesto", Null(order.ZavozoveMisto_Mesto)),
                        new SqlParameter("@ZavozoveMisto_Psc", Null(order.ZavozoveMisto_Psc)),
                        new SqlParameter("@ZavozoveMisto_Popis", Null(order.ZavozoveMisto_Popis)),
                        new SqlParameter("@ZavozoveMisto_DatumACas", Null(order.ZavozoveMisto_DatumACas)),
                        new SqlParameter("@ZavozoveMisto_OsobniOdberVSidleSpolecnosti", Null(order.ZavozoveMisto_OsobniOdberVSidleSpolecnosti)),
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
                        new SqlParameter("@ParentId", Null(order.ParentId)),
                        new SqlParameter("@AssociationAccountId", Null(order.AssociationAccountId)),
                        new SqlParameter("@AssociationRequestStatus", Null(order.AssociationRequestStatus)),
                        new SqlParameter("@CreatedByAccountId", Null(order.CreatedByAccountId)),
                        new SqlParameter("@ShipmentFrom", Null(order.ShipmentFrom)),
                        new SqlParameter("@ShipmentTo", Null(order.ShipmentTo)),
                        new SqlParameter("@NoPostage", Null(order.NoPostage)),
                        new SqlParameter("@ZavozoveMisto_Mesto", Null(order.ZavozoveMisto_Mesto)),
                        new SqlParameter("@ZavozoveMisto_Psc", Null(order.ZavozoveMisto_Psc)),
                        new SqlParameter("@ZavozoveMisto_Popis", Null(order.ZavozoveMisto_Popis)),
                        new SqlParameter("@ZavozoveMisto_DatumACas", Null(order.ZavozoveMisto_DatumACas)),
                        new SqlParameter("@ZavozoveMisto_OsobniOdberVSidleSpolecnosti", Null(order.ZavozoveMisto_OsobniOdberVSidleSpolecnosti))
                        );

                //CMS.EvenLog.WritoToEventLog( string.Format( "tShpOrder id={0} updated by account {1}!\n{2}", order.Id, (Account != null ? AccountId : 1), Environment.StackTrace ), System.Diagnostics.EventLogEntryType.Information );
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
