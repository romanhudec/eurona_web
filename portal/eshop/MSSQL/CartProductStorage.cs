using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;

namespace SHP.MSSQL {
    [Serializable]
    public sealed class CartProductStorage : MSSQLStorage<CartProduct> {
        public CartProductStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static CartProduct GetCartProduct(DataRow record) {
            CartProduct cartProduct = new CartProduct();
            cartProduct.Id = Convert.ToInt32(record["CartProductId"]);
            cartProduct.InstanceId = Convert.ToInt32(record["InstanceId"]);
            cartProduct.CartId = Convert.ToInt32(record["CartId"]);
            cartProduct.ProductId = Convert.ToInt32(record["ProductId"]);
            cartProduct.Quantity = Convert.ToInt32(record["Quantity"]);
            cartProduct.Price = Convert.ToDecimal(record["Price"]);
            cartProduct.PriceWVAT = Convert.ToDecimal(record["PriceWVAT"]);
            cartProduct.VAT = Convert.ToDecimal(record["VAT"]);
            cartProduct.Discount = Convert.ToDecimal(record["Discount"]);
            cartProduct.PriceTotal = Convert.ToDecimal(record["PriceTotal"]);
            cartProduct.PriceTotalWVAT = Convert.ToDecimal(record["PriceTotalWVAT"]);
            //JOIN-ed properties
            cartProduct.AccountId = ConvertNullable.ToInt32(record["AccountId"]);
            cartProduct.ProductCode = Convert.ToString(record["ProductCode"]);
            cartProduct.ProductName = Convert.ToString(record["ProductName"]);

            cartProduct.ProductAvailability = Convert.ToString(record["ProductAvailability"]);
            cartProduct.Alias = Convert.ToString(record["Alias"]);

            return cartProduct;
        }

        public override List<CartProduct> Read(object criteria) {
            if (criteria is CartProduct.ReadById) return LoadById(criteria as CartProduct.ReadById);
            if (criteria is CartProduct.ReadByCart) return LoadByCart(criteria as CartProduct.ReadByCart);
            if (criteria is CartProduct.ReadByCartProduct) return LoadByCartProduct(criteria as CartProduct.ReadByCartProduct);
            if (criteria is CartProduct.ReadByAccount) return LoadByAccount(criteria as CartProduct.ReadByAccount);
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
										Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias
								FROM vShpCartProducts WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr));
            }
            return cartList;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<CartProduct> LoadById(CartProduct.ReadById byCartProductId) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
										Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias
								FROM vShpCartProducts
								WHERE CartProductId = @CartProductId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartProductId", byCartProductId.CartProductId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr));
            }
            return cartList;
        }

        private List<CartProduct> LoadByCart(CartProduct.ReadByCart by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
										Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias
								FROM vShpCartProducts
								WHERE CartId = @CartId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartId", Null(by.CartId)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr));
            }
            return cartList;
        }

        private List<CartProduct> LoadByCartProduct(CartProduct.ReadByCartProduct by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
										Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias
								FROM vShpCartProducts
								WHERE CartId = @CartId AND ProductId = @ProductId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartId", Null(by.CartId)),
                        new SqlParameter("@ProductId", Null(by.ProductId)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr));
            }
            return cartList;
        }
        private List<CartProduct> LoadByAccount(CartProduct.ReadByAccount by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
										Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias
								FROM vShpCartProducts
								WHERE AccountId = @AccountId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr));
            }
            return cartList;
        }

        public override void Create(CartProduct cartProduct) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpCartProductCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@CartId", cartProduct.CartId),
                        new SqlParameter("@ProductId", cartProduct.ProductId),
                        new SqlParameter("@Quantity", cartProduct.Quantity),
                        new SqlParameter("@Price", cartProduct.Price),
                        new SqlParameter("@PriceWVAT", cartProduct.PriceWVAT),
                        new SqlParameter("@VAT", cartProduct.VAT),
                        new SqlParameter("@PriceTotal", cartProduct.PriceTotal),
                        new SqlParameter("@PriceTotalWVAT", cartProduct.PriceTotalWVAT),
                        new SqlParameter("@Discount", cartProduct.Discount),
                        result);

                cartProduct.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(CartProduct cartProduct) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpCartProductModify",
                        new SqlParameter("@CartProductId", cartProduct.Id),
                        new SqlParameter("@CartId", cartProduct.CartId),
                        new SqlParameter("@ProductId", cartProduct.ProductId),
                        new SqlParameter("@Quantity", cartProduct.Quantity),
                        new SqlParameter("@Price", cartProduct.Price),
                        new SqlParameter("@PriceWVAT", cartProduct.PriceWVAT),
                        new SqlParameter("@VAT", cartProduct.VAT),
                        new SqlParameter("@PriceTotal", cartProduct.PriceTotal),
                        new SqlParameter("@PriceTotalWVAT", cartProduct.PriceTotalWVAT),
                        new SqlParameter("@Discount", cartProduct.Discount)
                        );
            }
        }

        public override void Delete(CartProduct cartProduct) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@CartProductId", cartProduct.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpCartProductDelete", result, id);
            }
        }
    }
}
