using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL
{
    public sealed class CartStorage : MSSQLStorage<Cart>
    {
        private const string entitySelect = @"SELECT CartId, InstanceId, AccountId, SessionId, Created, Closed, PriceTotal, PriceTotalWVAT, Discount,
								ShipmentCode ,ShipmentName ,ShipmentPrice ,PaymentCode, PaymentName, DeliveryAddressId, InvoiceAddressId, Notes, Status,
								BodyEurosapTotal, KatalogovaCenaCelkemByEurosap, DopravneEurosap
								FROM vShpCarts";

        public CartStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private static Cart GetCart(DataRow record)
        {
            Cart cart = new Cart();
            cart.Id = Convert.ToInt32(record["CartId"]);
            cart.InstanceId = Convert.ToInt32(record["InstanceId"]);
            cart.AccountId = ConvertNullable.ToInt32(record["AccountId"]);
            cart.SessionId = ConvertNullable.ToInt32(record["SessionId"]);
            cart.ShipmentCode = Convert.ToString(record["ShipmentCode"]);
            cart.PaymentCode = Convert.ToString(record["PaymentCode"]);
            cart.DeliveryAddressId = Convert.ToInt32(record["DeliveryAddressId"]);
            cart.InvoiceAddressId = Convert.ToInt32(record["InvoiceAddressId"]);
            cart.Created = Convert.ToDateTime(record["Created"]);
            cart.Closed = ConvertNullable.ToDateTime(record["Closed"]);
            cart.PriceTotal = ConvertNullable.ToDecimal(record["PriceTotal"]);
            cart.PriceTotalWVAT = ConvertNullable.ToDecimal(record["PriceTotalWVAT"]);
            cart.Discount = ConvertNullable.ToDecimal(record["Discount"]);
            cart.Notes = Convert.ToString(record["Notes"]);
            cart.Status = Convert.ToInt32(record["Status"]);

            cart.DopravneEurosap = record["DopravneEurosap"] == DBNull.Value ? 0 : Convert.ToDecimal(record["DopravneEurosap"]);
            cart.BodyEurosapTotal = record["BodyEurosapTotal"] == DBNull.Value ? 0 : Convert.ToInt32(record["BodyEurosapTotal"]);
            cart.KatalogovaCenaCelkemByEurosap = record["KatalogovaCenaCelkemByEurosap"] == DBNull.Value ? 0m : Convert.ToDecimal(record["KatalogovaCenaCelkemByEurosap"]);

            return cart;
        }

        public override List<Cart> Read(object criteria)
        {
            if (criteria is Cart.ReadById) return LoadById(criteria as Cart.ReadById);
            if (criteria is Cart.ReadByAccount) return LoadByAccountId(criteria as Cart.ReadByAccount);
            if (criteria is Cart.ReadOpenByAccount) return LoadOpenByAccountId(criteria as Cart.ReadOpenByAccount);
            if (criteria is Cart.ReadBySessionId) return LoadBySessionId(criteria as Cart.ReadBySessionId);
            List<Cart> cartList = new List<Cart>();
            using (SqlConnection connection = Connect())
            {
                DataTable table = QueryProc<DataTable>(connection, "pShpCarts", new SqlParameter("@InstanceId", InstanceId), new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCart(dr));
            }
            return cartList;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }

        private List<Cart> LoadById(Cart.ReadById byCartId)
        {
            List<Cart> cartList = new List<Cart>();
            using (SqlConnection connection = Connect())
            {
                DataTable table = QueryProc<DataTable>(connection, "pShpCarts", new SqlParameter("@CartId", byCartId.CartId), new SqlParameter("@InstanceId", Null(null)), new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCart(dr));
            }
            return cartList;
        }

        private List<Cart> LoadOpenByAccountId(Cart.ReadOpenByAccount by)
        {
            List<Cart> cartList = new List<Cart>();
            using (SqlConnection connection = Connect())
            {
                DataTable table = QueryProc<DataTable>(connection, "pShpCarts",
                        new SqlParameter("@Closed", 1),
                        new SqlParameter("@AccountId", by.AccountId),
                        new SqlParameter("@InstanceId", Account.InstanceId == 1/*EURONA*/ ? Null(null) : (object)InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCart(dr));
            }
            return cartList;
        }

        private List<Cart> LoadByAccountId(Cart.ReadByAccount by)
        {
            List<Cart> cartList = new List<Cart>();
            using (SqlConnection connection = Connect())
            {
                DataTable table = QueryProc<DataTable>(connection, "pShpCarts",
                        new SqlParameter("@AcountId", by.AccountId),
                        new SqlParameter("@InstanceId", Account.InstanceId == 1/*EURONA*/ ? Null(null) : (object)InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCart(dr));
            }
            return cartList;
        }

        private List<Cart> LoadBySessionId(Cart.ReadBySessionId by)
        {
            List<Cart> cartList = new List<Cart>();
            using (SqlConnection connection = Connect())
            {
                DataTable table = QueryProc<DataTable>(connection, "pShpCarts",
                        new SqlParameter("@SessionId", by.SessionId),
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCart(dr));
            }
            return cartList;
        }

        public override void Create(Cart cart)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                DataSet ds = QueryProc<DataSet>(connection, "pShpCartCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@AccountId", Null(cart.AccountId)),
                        new SqlParameter("@SessionId", Null(cart.SessionId)),
                        new SqlParameter("@ShipmentCode", Null(cart.ShipmentCode)),
                        new SqlParameter("@PaymentCode", Null(cart.PaymentCode)),
                        new SqlParameter("@Closed", cart.Closed),
                        new SqlParameter("@Notes", Null(cart.Notes)),

                        new SqlParameter("@Price", Null(cart.PriceTotal)),
                        new SqlParameter("@PriceWVAT", Null(cart.PriceTotalWVAT)),
                        new SqlParameter("@Discount", Null(cart.Discount)),
                        new SqlParameter("@Status", Null(cart.Status)),
                        new SqlParameter("@BodyEurosapTotal", Null(cart.BodyEurosapTotal)),
                        new SqlParameter("@DopravneEurosap", Null(cart.DopravneEurosap)),
                        new SqlParameter("@KatalogovaCenaCelkemByEurosap", Null(cart.KatalogovaCenaCelkemByEurosap)),
                        result);

                DataTable dtDeliveryAddres = ds.Tables[0];
                DataTable dtInvoiceAddres = ds.Tables[1];
                DataTable dtCart = ds.Tables[2];

                cart.DeliveryAddressId = Convert.ToInt32(dtDeliveryAddres.Rows[0][0]);
                cart.InvoiceAddressId = Convert.ToInt32(dtInvoiceAddres.Rows[0][0]);
                cart.Id = Convert.ToInt32(dtCart.Rows[0][0]);
            }

        }

        public override void Update(Cart cart)
        {
            using (SqlConnection connection = Connect())
            {
                ExecProc(connection, "pShpCartModify",
                        new SqlParameter("@CartId", cart.Id),
                        new SqlParameter("@AccountId", Null(cart.AccountId)),
                        new SqlParameter("@SessionId", Null(cart.SessionId)),
                        new SqlParameter("@ShipmentCode", Null(cart.ShipmentCode)),
                        new SqlParameter("@PaymentCode", Null(cart.PaymentCode)),
                        new SqlParameter("@Closed", cart.Closed),
                        new SqlParameter("@Notes", Null(cart.Notes)),
                        new SqlParameter("@Price", Null(cart.PriceTotal)),
                        new SqlParameter("@PriceWVAT", Null(cart.PriceTotalWVAT)),
                        new SqlParameter("@Discount", Null(cart.Discount)),
                        new SqlParameter("@Status", Null(cart.Status)),
                        new SqlParameter("@DopravneEurosap", Null(cart.DopravneEurosap)),
                        new SqlParameter("@BodyEurosapTotal", Null(cart.BodyEurosapTotal)),
                        new SqlParameter("@KatalogovaCenaCelkemByEurosap", Null(cart.KatalogovaCenaCelkemByEurosap))
                        );
            }
        }

        public override void Delete(Cart cart)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter id = new SqlParameter("@CartId", cart.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpCartDelete", result, id);
            }
        }
    }
}
