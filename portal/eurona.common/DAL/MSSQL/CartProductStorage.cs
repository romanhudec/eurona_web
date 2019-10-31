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
    [Serializable]
    public sealed class CartProductStorage : MSSQLStorage<CartProduct> {
        public CartProductStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static CartProduct GetCartProduct(DataRow record, int instanceId) {
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

            cartProduct.CurrencyId = ConvertNullable.ToInt32(record["CurrencyId"]);
            cartProduct.CurrencyCode = Convert.ToString(record["CurrencyCode"]);
            cartProduct.CurrencySymbol = Convert.ToString(record["CurrencySymbol"]);
            cartProduct.MaximalniPocetVBaleni = ConvertNullable.ToInt32(record["MaximalniPocetVBaleni"]);
            cartProduct.MinimalniPocetVBaleni = ConvertNullable.ToInt32(record["MinimalniPocetVBaleni"]);
            cartProduct.CerpatBonusoveKredity = Convert.ToBoolean(record["CerpatBK"]);
            //JOIN-ed properties
            cartProduct.Body = ConvertNullable.ToDecimal(record["Body"]);
            cartProduct.BodyCelkem = ConvertNullable.ToDecimal(record["BodyCelkem"]);
            cartProduct.KatalogPriceWVAT = Convert.ToDecimal(record["KatalogPriceWVAT"]);
            cartProduct.KatalogPriceWVATTotal = Convert.ToDecimal(record["KatalogPriceWVATTotal"]);

            cartProduct.AccountId = ConvertNullable.ToInt32(record["AccountId"]);
            cartProduct.ProductCode = Convert.ToString(record["ProductCode"]);
            cartProduct.ProductName = Convert.ToString(record["ProductName"]);

            cartProduct.ProductAvailability = Convert.ToString(record["ProductAvailability"]);
            cartProduct.BSRProdukt = Convert.ToBoolean(record["BSRProdukt"] == DBNull.Value ? false : record["BSRProdukt"]);
            cartProduct.Alias = Convert.ToString(record["Alias"]);

            /* commented 24.06.2013 - integracia CL do Eurona
            if (instanceId != cartProduct.InstanceId)
                cartProduct.Alias = string.Empty;
            */
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
						Alias, CurrencyId, CurrencyCode, CurrencySymbol, Body, BodyCelkem, KatalogPriceWVAT, KatalogPriceWVATTotal, MaximalniPocetVBaleni, MinimalniPocetVBaleni, CerpatBK, BSRProdukt
				FROM vShpCartProducts WHERE InstanceId=@InstanceId AND Locale=@Locale
                ORDER BY POrder ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId), new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr, InstanceId));
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
						Alias, CurrencyId, CurrencyCode, CurrencySymbol, Body, BodyCelkem, KatalogPriceWVAT, KatalogPriceWVATTotal, MaximalniPocetVBaleni, MinimalniPocetVBaleni, CerpatBK, BSRProdukt
				FROM vShpCartProducts
				WHERE CartProductId = @CartProductId AND Locale=@Locale
                ORDER BY POrder ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartProductId", byCartProductId.CartProductId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr, InstanceId));
            }
            return cartList;
        }

        private List<CartProduct> LoadByCart(CartProduct.ReadByCart by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
						Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
						Alias, CurrencyId, CurrencyCode, CurrencySymbol, Body, BodyCelkem, KatalogPriceWVAT, KatalogPriceWVATTotal, MaximalniPocetVBaleni, MinimalniPocetVBaleni, CerpatBK, BSRProdukt
				FROM vShpCartProducts
				WHERE CartId = @CartId AND ( @InstanceId=0 OR InstanceId=@InstanceId ) AND Locale=@Locale
                ORDER BY POrder ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CartId", Null(by.CartId)),
                        new SqlParameter("@InstanceId", InstanceId == 1/*INTENSA*/ ? 0 : InstanceId),
                        new SqlParameter("@Locale", string.IsNullOrEmpty(by.Locale) ? Locale : by.Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr, InstanceId));
            }
            return cartList;
        }

        private List<CartProduct> LoadByCartProduct(CartProduct.ReadByCartProduct by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
						Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
						Alias, CurrencyId, CurrencyCode, CurrencySymbol, Body, BodyCelkem, KatalogPriceWVAT, KatalogPriceWVATTotal, MaximalniPocetVBaleni, MinimalniPocetVBaleni, CerpatBK, BSRProdukt
				FROM vShpCartProducts
				WHERE CartId = @CartId AND ProductId = @ProductId AND ( @InstanceId=0 OR InstanceId=@InstanceId ) AND Locale=@Locale AND (@CerpatBK IS NULL OR CerpatBK=@CerpatBK)
                ORDER BY POrder ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CerpatBK", Null(by.CerpatBK)),
                        new SqlParameter("@CartId", Null(by.CartId)),
                        new SqlParameter("@ProductId", Null(by.ProductId)),
                        new SqlParameter("@InstanceId", InstanceId == 1 /*EURONA*/ ? 0 : InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr, InstanceId));
            }
            return cartList;
        }
        private List<CartProduct> LoadByAccount(CartProduct.ReadByAccount by) {
            List<CartProduct> cartList = new List<CartProduct>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT CartProductId, InstanceId, CartId, ProductId, Quantity, AccountId, ProductCode, ProductName, 
						Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, ProductAvailability,
						Alias, CurrencyId, CurrencyCode, CurrencySymbol, Body, BodyCelkem, KatalogPriceWVAT, KatalogPriceWVATTotal, MaximalniPocetVBaleni, MinimalniPocetVBaleni, CerpatBK, BSRProdukt
				FROM vShpCartProducts
				WHERE AccountId = @AccountId AND InstanceId=@InstanceId AND Locale=@Locale
                ORDER BY POrder ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", Null(by.AccountId)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetCartProduct(dr, InstanceId));
            }
            return cartList;
        }

        public override void Create(CartProduct cartProduct) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpCartProductCreate",
                        new SqlParameter("@InstanceId", cartProduct.InstanceId),
                        new SqlParameter("@CartId", cartProduct.CartId),
                        new SqlParameter("@ProductId", cartProduct.ProductId),
                        new SqlParameter("@Quantity", cartProduct.Quantity),
                        new SqlParameter("@Price", cartProduct.Price),
                        new SqlParameter("@PriceWVAT", cartProduct.PriceWVAT),
                        new SqlParameter("@VAT", cartProduct.VAT),
                        new SqlParameter("@PriceTotal", cartProduct.PriceTotal),
                        new SqlParameter("@PriceTotalWVAT", cartProduct.PriceTotalWVAT),
                        new SqlParameter("@Discount", cartProduct.Discount),
                        new SqlParameter("@CurrencyId", Null(cartProduct.CurrencyId)),
                        new SqlParameter("@CerpatBK", Null(cartProduct.CerpatBonusoveKredity)),
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
                        new SqlParameter("@Discount", cartProduct.Discount),
                        new SqlParameter("@CurrencyId", Null(cartProduct.CurrencyId))
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
