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
    public sealed class OrderFastViewStorage : MSSQLStorage<OrderFastView> {
        private const string entitySelect = @"SELECT TOP 100 OrderId, InstanceId, ParentId, OrderDate, OrderNumber ,CartId ,AccountId, AccountName, OrderStatusCode ,OrderStatusName, OrderStatusIcon ,
		ShipmentCode ,ShipmentName, ShipmentIcon ,ShipmentPrice, ShipmentPriceWVAT ,Price, PriceWVAT,
		ParentId, AssociationRequestStatus, AssociationAccountId, CreatedByAccountId,
		OwnerName, TVD_Id
		FROM vShpOrdersFast o";

        public OrderFastViewStorage(int instanceId, Eurona.DAL.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static OrderFastView GetOrderFastView(DataRow record) {
            OrderFastView order = new OrderFastView();
            order.Id = Convert.ToInt32(record["OrderId"]);
            order.InstanceId = Convert.ToInt32(record["InstanceId"]);
            order.OrderDate = Convert.ToDateTime(record["OrderDate"]);
            order.OrderNumber = Convert.ToString(record["OrderNumber"]);
            order.OrderStatusCode = Convert.ToString(record["OrderStatusCode"]);
            order.CartId = Convert.ToInt32(record["CartId"]);
            order.ShipmentCode = Convert.ToString(record["ShipmentCode"]);
            order.Price = Convert.ToDecimal(record["Price"]);
            order.PriceWVAT = Convert.ToDecimal(record["PriceWVAT"]);
            order.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            order.AssociationRequestStatus = ConvertNullable.ToInt32(record["AssociationRequestStatus"]);
            order.AssociationAccountId = ConvertNullable.ToInt32(record["AssociationAccountId"]);
            order.CreatedByAccountId = Convert.ToInt32(record["CreatedByAccountId"]);

            //Joined properties
            order.AccountId = Convert.ToInt32(record["AccountId"]);
            order.OrderStatusName = Convert.ToString(record["OrderStatusName"]);

            order.OrderStatusIcon = Convert.ToString(record["OrderStatusIcon"]);
            order.ShipmentName = Convert.ToString(record["ShipmentName"]);
            order.ShipmentIcon = Convert.ToString(record["ShipmentIcon"]);
            order.ShipmentPrice = ConvertNullable.ToDecimal(record["ShipmentPrice"]);
            order.ShipmentPriceWVAT = ConvertNullable.ToDecimal(record["ShipmentPriceWVAT"]);

            order.OwnerName = Convert.ToString(record["OwnerName"]);
            return order;
        }

        public override List<OrderFastView> Read(object criteria) {
            if (criteria is OrderFastView.ReadByFilter) return LoadByFilter(criteria as OrderFastView.ReadByFilter);
            if (criteria is OrderFastView.ReadById) return LoadById(criteria as OrderFastView.ReadById);
            List<OrderFastView> list = new List<OrderFastView>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE InstanceId = @InstanceId";
                sql += " ORDER BY OrderId DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrderFastView(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<OrderFastView> LoadById(OrderFastView.ReadById by) {
            List<OrderFastView> list = new List<OrderFastView>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;

                sql += @" WHERE OrderId = @OrderId";            
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@OrderId", by.OrderId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrderFastView(dr));
            }
            return list;
        }

        private List<OrderFastView> LoadByFilter(OrderFastView.ReadByFilter by) {
            List<OrderFastView> list = new List<OrderFastView>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;

                sql += @" WHERE InstanceId = @InstanceId";
                sql += by.AccountId.HasValue ? " AND (AccountId = @AccountId OR CreatedByAccountId=@AccountId)" : "";
                sql += by.CreatedByAccountId.HasValue ? " AND (CreatedByAccountId = @CreatedByAccountId)" : "";
                sql += !string.IsNullOrEmpty(by.OrderNumber) ? " AND (OrderNumber LIKE @OrderNumber + '%')" : "";
                sql += !string.IsNullOrEmpty(by.OwnerName) ? " AND (OwnerName LIKE @OwnerName + '%')" : "";
                sql += !string.IsNullOrEmpty(by.OrderStatusCode) ? " AND (OrderStatusCode LIKE @OrderStatusCode)" : "";
                sql += !string.IsNullOrEmpty(by.OrderStatusName) ? " AND (OrderStatusName LIKE @OrderStatusName + '%')" : "";
                sql += !string.IsNullOrEmpty(by.NotOrderStatusCode) ? " AND (OrderStatusCode NOT LIKE @NotOrderStatusCode)" : "";
                sql += by.ParentId.HasValue ? " AND (ParentId = @ParentId)" : "";
                sql += by.OnlyLastMonths.HasValue ? " AND (OrderDate >= DATEADD(M, @OnlyLastMonths*-1, GETDATE()))" : "";
                sql += " ORDER BY OrderId DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@OrderNumber", Null(string.IsNullOrEmpty(by.OrderNumber) ? null : (object)by.OrderNumber)),
                        new SqlParameter("@OrderStatusCode", Null(by.OrderStatusCode)),
                        new SqlParameter("@OrderStatusName", Null(by.OrderStatusName)),
                        new SqlParameter("@NotOrderStatusCode", Null(by.NotOrderStatusCode)),
                        new SqlParameter("@ParentId", Null(by.ParentId)),
                        new SqlParameter("@CreatedByAccountId", Null(by.CreatedByAccountId)),
                        new SqlParameter("@OwnerName", Null(by.OwnerName)),
                        new SqlParameter("@OnlyLastMonths", Null(by.OnlyLastMonths))
                        );
                foreach (DataRow dr in table.Rows)
                    list.Add(GetOrderFastView(dr));
            }
            return list;
        }

        public override void Create(OrderFastView order) {
            throw new NotImplementedException();
        }

        public override void Update(OrderFastView order) {
            throw new NotImplementedException();
        }

        public override void Delete(OrderFastView order) {
            throw new NotImplementedException();
        }
    }
}
