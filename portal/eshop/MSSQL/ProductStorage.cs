using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;

namespace SHP.MSSQL {
    [Serializable]
    public sealed class ProductStorage : MSSQLStorage<Product> {
        public ProductStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Product GetProduct(DataRow record) {
            Product product = new Product();
            product.Id = Convert.ToInt32(record["ProductId"]);
            product.InstanceId = Convert.ToInt32(record["InstanceId"]);
            product.Manufacturer = Convert.ToString(record["Manufacturer"]);
            product.Code = Convert.ToString(record["Code"]);
            product.Name = Convert.ToString(record["Name"]);
            product.Description = Convert.ToString(record["Description"]);
            product.DescriptionLong = Convert.ToString(record["DescriptionLong"]);
            product.Availability = Convert.ToString(record["Availability"]);
            product.StorageCount = ConvertNullable.ToInt32(record["StorageCount"]);
            product.Price = Convert.ToDecimal(record["Price"]);
            product.Discount = Convert.ToDecimal(record["Discount"]);
            product.DiscountTypeId = (Product.DiscountType)Convert.ToInt32(record["DiscountTypeId"]);
            product.VATId = ConvertNullable.ToInt32(record["VATId"]);
            product.VAT = Convert.ToDecimal(record["VAT"]);
            product.Locale = Convert.ToString(record["Locale"]);

            product.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            product.Alias = Convert.ToString(record["Alias"]);

            product.CommentsCount = Convert.ToInt32(record["CommentsCount"]);
            product.ViewCount = Convert.ToInt32(record["ViewCount"]);
            product.Votes = Convert.ToInt32(record["Votes"]);
            product.TotalRating = Convert.ToInt32(record["TotalRating"]);
            product.RatingResult = Convert.ToDouble(record["RatingResult"]);

            return product;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<Product> Read(object criteria) {
            if (criteria is Product.ReadById) return LoadById(criteria as Product.ReadById);
            if (criteria is Product.ReadByFilter) return LoadByFilter(criteria as Product.ReadByFilter);
            if (criteria is Product.ReadByCategory) return LoadByCategory(criteria as Product.ReadByCategory);
            if (criteria is Product.ReadAllInCategory) return LoadAllInCategory(criteria as Product.ReadAllInCategory);
            if (criteria is Product.ReadHighlights) return LoadHighlights(criteria as Product.ReadHighlights);
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductId, InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, Discount, DiscountTypeId, VATId, VAT, Locale, UrlAliasId, Alias,
										CommentsCount, ViewCount, Votes, TotalRating, RatingResult
								FROM vShpProducts
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Product> LoadById(Product.ReadById byProductId) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductId,InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, Discount, DiscountTypeId, VATId, VAT, Locale, UrlAliasId, Alias,
										CommentsCount, ViewCount, Votes, TotalRating, RatingResult
								FROM vShpProducts
								WHERE ProductId = @ProductId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ProductId", byProductId.ProductId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadByCategory(Product.ReadByCategory by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT DISTINCT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.DiscountTypeId, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
								FROM vShpProducts p INNER JOIN
										tShpProductCategories pc ON pc.ProductId = p.ProductId
								WHERE p.InstanceId=@InstanceId AND pc.CategoryId=@CategoryId";

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@CategoryId", Null(by.CategoryId)));
                sqlParams.Add(new SqlParameter("@InstanceId", InstanceId));

                DataTable table = Query<DataTable>(connection, sql, sqlParams.ToArray());
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadAllInCategory(Product.ReadAllInCategory by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT DISTINCT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.DiscountTypeId, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
								FROM vShpProducts p INNER JOIN
										tShpProductCategories pc ON pc.ProductId = p.ProductId INNER JOIN
										[dbo].fAllChildCategories(@CategoryId) c ON c.CategoryId = pc.CategoryId
								WHERE p.InstanceId=@InstanceId";

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@CategoryId", Null(by.CategoryId)));
                sqlParams.Add(new SqlParameter("@InstanceId", InstanceId));

                string byFilterWhere = string.Empty;
                if (by.ByFilter != null) {
                    #region Highlights
                    if (by.ByFilter.Highlights != null && by.ByFilter.Highlights.HighlightId.HasValue) {
                        sql = string.Format(@"
												SELECT TOP {0} p.ProductId,p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
														p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
												FROM vShpProducts p
												INNER JOIN tShpProductCategories pc ON pc.ProductId = p.ProductId
												INNER JOIN [dbo].fAllChildCategories(@CategoryId) c ON c.CategoryId = pc.CategoryId
												INNER JOIN vShpProductHighlights ph ON ph.ProductId = p.ProductId
												WHERE ph.HighlightId = @HighlightId AND p.InstanceId=@InstanceId", by.ByFilter.Highlights.MaxCount.HasValue ? by.ByFilter.Highlights.MaxCount.Value.ToString() : "100 PERCENT");

                        sqlParams.Add(new SqlParameter("@HighlightId", by.ByFilter.Highlights.HighlightId));
                    }
                    #endregion

                    BuildByFilter(by.ByFilter, "p", ref byFilterWhere, ref sqlParams);
                    sql = sql + " AND " + byFilterWhere;
                    switch (by.ByFilter.SortBy) {
                        case Product.SortBy.Default:
                        case Product.SortBy.NameASC:
                            sql += "ORDER BY p.[Name] ASC";
                            break;
                        case Product.SortBy.PriceASC:
                            sql += "ORDER BY p.[Price] ASC";
                            break;
                        case Product.SortBy.PriceDESC:
                            sql += "ORDER BY p.[Price] DESC";
                            break;
                    }
                }

                DataTable table = Query<DataTable>(connection, sql, sqlParams.ToArray());
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private List<Product> LoadHighlights(Product.ReadHighlights by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = string.Format(@"
								SELECT TOP {0} p.ProductId,p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.DiscountTypeId, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
								FROM vShpProducts p
								INNER JOIN vShpProductHighlights ph ON ph.ProductId = p.ProductId
								WHERE ph.HighlightId = @HighlightId AND p.InstanceId=@InstanceId
								ORDER BY Name ASC", by.MaxCount.HasValue ? by.MaxCount.Value.ToString() : "100 PERCENT");
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@HighlightId", by.HighlightId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadByFilter(Product.ReadByFilter by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT p.ProductId,p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.DiscountTypeId, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
												p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
								FROM vShpProducts p
								WHERE p.InstanceId = @InstanceId";
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@InstanceId", InstanceId));

                if (by.Highlights != null && by.Highlights.HighlightId.HasValue) {
                    sql = string.Format(@"
										SELECT TOP {0} p.ProductId,p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.Availability, p.StorageCount, p.Price, p.Discount, p.DiscountTypeId, p.VATId, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
												p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult
										FROM vShpProducts p
										INNER JOIN vShpProductHighlights ph ON ph.ProductId = p.ProductId
										WHERE ph.HighlightId = @HighlightId AND p.InstanceId=@InstanceId", by.Highlights.MaxCount.HasValue ? by.Highlights.MaxCount.Value.ToString() : "100 PERCENT");

                    sqlParams.Add(new SqlParameter("@HighlightId", by.Highlights.HighlightId));
                }

                string byFilterWhere = string.Empty;
                if (by != null) {
                    BuildByFilter(by, "p", ref byFilterWhere, ref sqlParams);
                    sql = sql + " AND " + byFilterWhere;
                    switch (by.SortBy) {
                        case Product.SortBy.Default: {
                                if (by.BestSellers.HasValue && by.BestSellers.Value == true)
                                    sql += " ORDER BY p.[SalesCount] DESC, p.[Name] ASC";
                            }
                            break;
                        case Product.SortBy.NameASC:
                            sql += " ORDER BY p.[Name] ASC";
                            break;
                        case Product.SortBy.PriceASC:
                            sql += " ORDER BY p.[Price] ASC";
                            break;
                        case Product.SortBy.PriceDESC:
                            sql += " ORDER BY p.[Price] DESC";
                            break;
                    }
                }

                DataTable table = Query<DataTable>(connection, sql, sqlParams.ToArray());
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private void BuildByFilter(Product.ReadByFilter byFilter, string columnPrefix, ref string where, ref List<SqlParameter> parameters) {
            if (!string.IsNullOrEmpty(columnPrefix))
                columnPrefix = columnPrefix + ".";
            where = string.Format(@" 
										(@BestSellers IS NULL OR ( @BestSellers=1 AND {0}SalesCount != 0 ) )  AND
										(@Manufacturer IS NULL OR {0}Manufacturer = @Manufacturer)  AND
										(@Expression IS NULL OR {0}Name LIKE '%'+@Expression+'%' OR {0}Description LIKE '%'+@Expression+'%')  AND
										(@PriceFrom IS NULL OR {0}Price >= @PriceFrom)  AND
										(@PriceTo IS NULL OR {0}Price <= @PriceTo)",
                            columnPrefix);

            byFilter.Manufacturer = string.IsNullOrEmpty(byFilter.Manufacturer) ? null : byFilter.Manufacturer;
            byFilter.Expression = string.IsNullOrEmpty(byFilter.Expression) ? null : byFilter.Expression;
            parameters.Add(new SqlParameter("@BestSellers", Null(byFilter.BestSellers)));
            parameters.Add(new SqlParameter("@Manufacturer", Null(byFilter.Manufacturer)));
            parameters.Add(new SqlParameter("@Expression", Null(byFilter.Expression)));
            parameters.Add(new SqlParameter("@PriceFrom", Null(byFilter.PriceFrom)));
            parameters.Add(new SqlParameter("@PriceTo", Null(byFilter.PriceTo)));
        }

        private void RemoveProductFromCategories(Product product) {
            using (SqlConnection connection = Connect()) {
                string sql = @"DELETE FROM tShpProductCategories WHERE ProductId=@ProductId";
                SqlParameter productId = new SqlParameter("@ProductId", product.Id);
                Exec(connection, sql, productId);
            }
        }

        private void UpdateProductCategories(Product product) {
            //Nacitanie povodnych hodnot
            List<ProductCategories> productCategories = product.ProductCategories;

            //Vymazanie starych hodnot
            RemoveProductFromCategories(product);

            using (SqlConnection connection = Connect()) {
                foreach (ProductCategories pc in productCategories) {
                    pc.ProductId = product.Id;
                    Storage<ProductCategories>.Create(pc);
                }
            }
        }

        public override R Execute<R>(R command) {
            Type t = typeof(R);
            if (t == typeof(Product.IncrementViewCountCommand))
                return IncrementViewCount(command as Product.IncrementViewCountCommand) as R;

            if (t == typeof(Product.IncrementVoteCommand))
                return IncrementVote(command as Product.IncrementVoteCommand) as R;

            return base.Execute<R>(command);
        }

        private Product.IncrementViewCountCommand IncrementViewCount(Product.IncrementViewCountCommand cmd) {
            using (SqlConnection connection = Connect()) {
                SqlParameter productId = new SqlParameter("@ProductId", cmd.ProductId);
                ExecProc(connection, "pShpProductIncrementViewCount", productId);
            }

            return cmd;
        }

        private Product.IncrementVoteCommand IncrementVote(Product.IncrementVoteCommand cmd) {
            using (SqlConnection connection = Connect()) {
                SqlParameter productId = new SqlParameter("@ProductId", cmd.ProductId);
                SqlParameter rating = new SqlParameter("@Rating", cmd.Rating);
                ExecProc(connection, "pShpProductIncrementVote", productId, rating);

                CMS.Entities.AccountVote accountVote = new CMS.Entities.AccountVote();
                accountVote.AccountId = cmd.AccountId;
                accountVote.ObjectId = cmd.ProductId;
                accountVote.ObjectTypeId = (int)Product.AccountVoteType;
                accountVote.Rating = cmd.Rating;
                Storage<CMS.Entities.AccountVote>.Create(accountVote);
            }

            return cmd;
        }

        public override void Create(Product product) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpProductCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Code", product.Code),
                        new SqlParameter("@Name", product.Name),
                        new SqlParameter("@Manufacturer", product.Manufacturer),
                        new SqlParameter("@Description", product.Description),
                        new SqlParameter("@DescriptionLong", product.DescriptionLong),
                        new SqlParameter("@Availability", product.Availability),
                        new SqlParameter("@StorageCount", Null(product.StorageCount)),
                        new SqlParameter("@Price", product.Price),
                        new SqlParameter("@Discount", product.Discount),
                        new SqlParameter("@DiscountTypeId", (int)product.DiscountTypeId),
                        new SqlParameter("@VATId", product.VATId),
                        new SqlParameter("@UrlAliasId", Null(product.UrlAliasId)),
                        new SqlParameter("@Locale", String.IsNullOrEmpty(product.Locale) ? Locale : product.Locale),
                        result);

                product.Id = Convert.ToInt32(result.Value);
            }

            UpdateProductCategories(product);

        }

        public override void Update(Product product) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpProductModify",
                        new SqlParameter("@HistoryAccount", Account != null ? AccountId : 1),
                        new SqlParameter("@ProductId", product.Id),
                        new SqlParameter("@Code", product.Code),
                        new SqlParameter("@Name", product.Name),
                        new SqlParameter("@Manufacturer", product.Manufacturer),
                        new SqlParameter("@Description", product.Description),
                        new SqlParameter("@DescriptionLong", product.DescriptionLong),
                        new SqlParameter("@Availability", product.Availability),
                        new SqlParameter("@StorageCount", Null(product.StorageCount)),
                        new SqlParameter("@Price", product.Price),
                        new SqlParameter("@Discount", product.Discount),
                        new SqlParameter("@DiscountTypeId", (int)product.DiscountTypeId),
                        new SqlParameter("@VATId", product.VATId),
                        new SqlParameter("@UrlAliasId", Null(product.UrlAliasId)),
                        new SqlParameter("@Locale", product.Locale));
            }

            UpdateProductCategories(product);
        }

        public override void Delete(Product product) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter productId = new SqlParameter("@ProductId", product.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpProductDelete", result, historyAccount, productId);
            }
        }
    }
}
