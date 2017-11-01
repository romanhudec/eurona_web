using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL
{
    public sealed class ProductRelationStorage : MSSQLStorage<ProductRelation>
    {
        public ProductRelationStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private static ProductRelation GetProductRelation(DataRow record)
        {
            ProductRelation productRelation = new ProductRelation();
            productRelation.Id = Convert.ToInt32(record["ProductRelationId"]);
            productRelation.InstanceId = Convert.ToInt32(record["InstanceId"]);
            productRelation.ParentProductId = Convert.ToInt32(record["ParentProductId"]);
            productRelation.ProductId = Convert.ToInt32(record["ProductId"]);
            productRelation.RelationType = Convert.ToInt32(record["RelationType"]);
            productRelation.CurrencyId = ConvertNullable.ToInt32(record["CurrencyId"]);
            productRelation.CurrencySymbol = Convert.ToString(record["CurrencySymbol"]);
            productRelation.CurrencyCode = Convert.ToString(record["CurrencyCode"]);
            //JOIN-ed properties
            productRelation.ProductName = Convert.ToString(record["ProductName"]);
            productRelation.ProductPrice = Convert.ToDecimal(record["ProductPrice"]);
            productRelation.ProductDiscount = Convert.ToDecimal(record["ProductDiscount"]);
            productRelation.ProductAvailability = Convert.ToString(record["ProductAvailability"]);
            productRelation.ProductPriceWDiscount = Convert.ToDecimal(record["ProductPriceWDiscount"]);
            productRelation.PriceTotal = Convert.ToDecimal(record["PriceTotal"]);
            productRelation.PriceTotalWVAT = Convert.ToDecimal(record["PriceTotalWVAT"]);
            productRelation.Alias = Convert.ToString(record["Alias"]);

            return productRelation;
        }

        public override List<ProductRelation> Read(object criteria)
        {
            if (criteria is ProductRelation.ReadById) return LoadById(criteria as ProductRelation.ReadById);
            if (criteria is ProductRelation.ReadBy) return LoadBy(criteria as ProductRelation.ReadBy);
            List<ProductRelation> cartList = new List<ProductRelation>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
								SELECT ProductRelationId, InstanceId, ParentProductId, ProductId, RelationType, ProductName, 
										ProductPrice, ProductDiscount, ProductPriceWDiscount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias, CurrencyId, CurrencySymbol, CurrencyCode
								FROM vShpProductRelations WHERE InstanceId=@InstanceId AND Locale=@Locale";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetProductRelation(dr));
            }
            return cartList;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }

        private List<ProductRelation> LoadById(ProductRelation.ReadById byProductRelationId)
        {
            List<ProductRelation> cartList = new List<ProductRelation>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
								SELECT ProductRelationId, InstanceId, ParentProductId, ProductId, RelationType, ProductName, 
										ProductPrice, ProductDiscount, ProductPriceWDiscount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias, CurrencyId, CurrencySymbol, CurrencyCode
								FROM vShpProductRelations
								WHERE ProductRelationId = @ProductRelationId AND Locale=@Locale";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ProductRelationId", byProductRelationId.ProductRelationId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetProductRelation(dr));
            }
            return cartList;
        }


        private List<ProductRelation> LoadBy(ProductRelation.ReadBy by)
        {
            List<ProductRelation> cartList = new List<ProductRelation>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
								SELECT ProductRelationId, InstanceId, ParentProductId, ProductId, RelationType, ProductName, 
										ProductPrice,  ProductDiscount, ProductPriceWDiscount, PriceTotal, PriceTotalWVAT, ProductAvailability,
										Alias, CurrencyId, CurrencySymbol, CurrencyCode
								FROM vShpProductRelations
								WHERE 
										RelationType = ISNULL(@RelationType, RelationType) AND 
										ParentProductId = ISNULL(@ParentProductId,ParentProductId)/* AND 
										InstanceId=@InstanceId */AND Locale=@Locale";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@RelationType", Null(by.RelationType)),
                        new SqlParameter("@ParentProductId", Null(by.ParentProductId)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    cartList.Add(GetProductRelation(dr));
            }
            return cartList;
        }

        public override void Create(ProductRelation productRelation)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpProductRelationCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@ParentProductId", productRelation.ParentProductId),
                        new SqlParameter("@ProductId", productRelation.ProductId),
                        new SqlParameter("@RelationType", productRelation.RelationType),
                        result);

                productRelation.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(ProductRelation entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ProductRelation productRelation)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter id = new SqlParameter("@ProductRelationId", productRelation.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpProductRelationDelete", result, id);
            }
        }
    }
}
