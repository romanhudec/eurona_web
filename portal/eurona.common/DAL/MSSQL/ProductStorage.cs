using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL {
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
            product.AdditionalInformation = Convert.ToString(record["AdditionalInformation"]);
            product.InstructionsForUse = Convert.ToString(record["InstructionsForUse"]);
            product.Availability = Convert.ToString(record["Availability"]);
            product.StorageCount = ConvertNullable.ToInt32(record["StorageCount"]);
            product.Price = Convert.ToDecimal(record["Price"]);
            product.BeznaCena = Convert.ToDecimal(record["BeznaCena"]);
            product.CurrencyId = ConvertNullable.ToInt32(record["CurrencyId"]);
            product.CurrencySymbol = Convert.ToString(record["CurrencySymbol"]);
            product.CurrencyCode = Convert.ToString(record["CurrencyCode"]);
            product.Parfumacia = ConvertNullable.ToInt32(record["Parfumacia"]);
            product.Body = ConvertNullable.ToDecimal(record["Body"]);
            product.Top = ConvertNullable.ToInt32(record["Top"]);
            product.Discount = Convert.ToDecimal(record["Discount"]);
            product.VAT = Convert.ToDecimal(record["VAT"]);
            product.Locale = Convert.ToString(record["Locale"]);

            product.Novinka = Convert.ToBoolean(record["Novinka"]);
            product.Inovace = Convert.ToBoolean(record["Inovace"]);
            product.Doprodej = Convert.ToBoolean(record["Doprodej"]);
            product.Vyprodano = Convert.ToBoolean(record["Vyprodano"]);
            product.ProdejUkoncen = Convert.ToBoolean(record["ProdejUkoncen"]);
            product.MaximalniPocetVBaleni = ConvertNullable.ToInt32(record["MaximalniPocetVBaleni"]);
            product.MinimalniPocetVBaleni = ConvertNullable.ToInt32(record["MinimalniPocetVBaleni"]);
            product.BonusovyKredit = ConvertNullable.ToDecimal(record["BonusovyKredit"]);

            product.Megasleva = Convert.ToBoolean(record["Megasleva"]);
            product.Supercena = Convert.ToBoolean(record["Supercena"]);
            product.CLHit = Convert.ToBoolean(record["CLHit"]);
            product.Action = Convert.ToBoolean(record["Action"]);
            product.Vyprodej = Convert.ToBoolean(record["Vyprodej"]);
            product.OnWeb = Convert.ToBoolean(record["OnWeb"]);
            product.InternalStorageCount = Convert.ToInt32(record["InternalStorageCount"]);
            product.LimitDate = ConvertNullable.ToDateTime(record["LimitDate"]);

            product.VamiNejviceNakupovane = ConvertNullable.ToInt32(record["VamiNejviceNakupovane"]);
            product.DarkovySet = ConvertNullable.ToInt32(record["DarkovySet"]);

            product.MarzePovolena = Convert.ToBoolean(record["MarzePovolena"]);
            product.MarzePovolenaMinimalni = Convert.ToBoolean(record["MarzePovolenaMinimalni"]);

            product.DynamickaSleva = ConvertNullable.ToBool(record["DynamickaSleva"]);
            product.StatickaSleva = ConvertNullable.ToDecimal(record["StatickaSleva"]);

            product.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            product.Alias = Convert.ToString(record["Alias"]);

            product.CommentsCount = Convert.ToInt32(record["CommentsCount"]);
            product.ViewCount = Convert.ToInt32(record["ViewCount"]);
            product.Votes = Convert.ToInt32(record["Votes"]);
            product.TotalRating = Convert.ToInt32(record["TotalRating"]);
            product.RatingResult = Convert.ToDouble(record["RatingResult"]);

            product.ZadniEtiketa = Convert.ToString(record["ZadniEtiketa"]);
            product.ZobrazovatZadniEtiketu = Convert.ToBoolean(record["ZobrazovatZadniEtiketu"] == DBNull.Value ? false : record["ZobrazovatZadniEtiketu"]);
            product.BSR = Convert.ToBoolean(record["BSR"] == DBNull.Value ? false : record["BSR"]);
            product.Order = ConvertNullable.ToInt32(record["Order"]);
            return product;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<Product> Read(object criteria) {
            if (criteria is Product.ReadById) return LoadById(criteria as Product.ReadById);
            if (criteria is Product.ReadByCode) return LoadByCode(criteria as Product.ReadByCode);
            if (criteria is Product.ReadByFilter) return LoadByFilter(criteria as Product.ReadByFilter);
            if (criteria is Product.ReadByCategory) return LoadByCategory(criteria as Product.ReadByCategory);
            if (criteria is Product.ReadAllInCategory) return LoadAllInCategory(criteria as Product.ReadAllInCategory);
            if (criteria is Product.ReadHighlights) return LoadHighlights(criteria as Product.ReadHighlights);
            if (criteria is Product.ReadWithBK) return LoadWithBK(criteria as Product.ReadWithBK);
            if (criteria is Product.ReadVamiNejviceNakupovane) return LoadVamiNejviceNakupovane(criteria as Product.ReadVamiNejviceNakupovane);
            if (criteria is Product.ReadDarkoveSety) return LoadDarkoveSety(criteria as Product.ReadDarkoveSety);
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT ProductId, InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, AdditionalInformation, InstructionsForUse, Availability, StorageCount, Price, BeznaCena, DynamickaSleva, StatickaSleva, MarzePovolena, MarzePovolenaMinimalni, CurrencyId, CurrencySymbol, CurrencyCode, Body, [Top],  [Novinka],  [Inovace],  [Doprodej],  [Vyprodano], [ProdejUkoncen], MaximalniPocetVBaleni, MinimalniPocetVBaleni, BonusovyKredit, 
						[Megasleva], [Supercena] , [CLHit] , [Action] , [Vyprodej], [OnWeb], [InternalStorageCount], [LimitDate], [VamiNejviceNakupovane], [DarkovySet],
						Parfumacia, Discount,  VAT, Locale, UrlAliasId, Alias,
						CommentsCount, ViewCount, Votes, TotalRating, RatingResult,
                        ZadniEtiketa, ZobrazovatZadniEtiketu, BSR, [Order]
				FROM vShpProducts p
				WHERE p.OnWeb=1 AND Locale = @Locale AND (InstanceId = 0 OR InstanceId=@InstanceId ) AND (ProdejUkoncen IS NULL OR ProdejUkoncen=0)
				ORDER BY Novinka DESC, [Order] ASC, Name ASC";
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
					SELECT ProductId,InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, AdditionalInformation, InstructionsForUse, Availability, StorageCount, Price, BeznaCena, DynamickaSleva, StatickaSleva, MarzePovolena, MarzePovolenaMinimalni, CurrencyId, CurrencySymbol, CurrencyCode, Parfumacia, Body, [Top],  [Novinka], [Inovace], [Doprodej],  [Vyprodano],  [ProdejUkoncen], MaximalniPocetVBaleni, MinimalniPocetVBaleni, BonusovyKredit, 
							[Megasleva], [Supercena] , [CLHit] , [Action] , [Vyprodej], [OnWeb], [InternalStorageCount], [LimitDate], [VamiNejviceNakupovane], [DarkovySet],
							Discount,  VAT, Locale, UrlAliasId, Alias,
							CommentsCount, ViewCount, Votes, TotalRating, RatingResult,
                            ZadniEtiketa, ZobrazovatZadniEtiketu, BSR, [Order]
					FROM vShpProducts
					WHERE ProductId = @ProductId AND Locale=@Locale
					ORDER BY Novinka DESC, [Order] ASC, Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ProductId", byProductId.ProductId), new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadByCode(Product.ReadByCode by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductId,InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, AdditionalInformation, InstructionsForUse, Availability, StorageCount, Price, BeznaCena, DynamickaSleva, StatickaSleva, MarzePovolena, MarzePovolenaMinimalni, CurrencyId, CurrencySymbol, CurrencyCode, Parfumacia, Body, [Top],  [Novinka], [Inovace], [Doprodej],  [Vyprodano],  [ProdejUkoncen], MaximalniPocetVBaleni, MinimalniPocetVBaleni, BonusovyKredit, 
										[Megasleva], [Supercena] , [CLHit] , [Action] , [Vyprodej], [OnWeb], [InternalStorageCount], [LimitDate], [VamiNejviceNakupovane], [DarkovySet],
										Discount,  VAT, Locale, UrlAliasId, Alias,
										CommentsCount, ViewCount, Votes, TotalRating, RatingResult,
                                        ZadniEtiketa, ZobrazovatZadniEtiketu, BSR, [Order]
								FROM vShpProducts
								WHERE Code = @Code AND Locale=@Locale /*AND (InstanceId = 0 OR InstanceId=@InstanceId )*/ AND (ProdejUkoncen IS NULL OR ProdejUkoncen=0)
								ORDER BY Novinka DESC, [Order] ASC, Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Code", by.Code),
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadByCategory(Product.ReadByCategory by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT DISTINCT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price, p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
										p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
										p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                                        p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
								FROM vShpProducts p INNER JOIN
										tShpProductCategories pc ON pc.ProductId = p.ProductId
								WHERE p.OnWeb=1 AND pc.CategoryId=@CategoryId AND p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)
                                ORDER BY p.Novinka DESC, p.[Order] ASC, p.Name ASC";

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@CategoryId", Null(by.CategoryId)));
                sqlParams.Add(new SqlParameter("@Locale", Locale));

                DataTable table = Query<DataTable>(connection, sql, sqlParams.ToArray());
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }
        private List<Product> LoadAllInCategory(Product.ReadAllInCategory by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string top = "100 PERCENT";
                if (by.ByFilter != null && by.ByFilter.MaxCount.HasValue)
                    top = by.ByFilter.MaxCount.Value.ToString();

                string sql = string.Format(@"
				SELECT DISTINCT TOP {0} p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount,p.BeznaCena, p.Price, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Top], p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
						p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
						p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
						p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                        p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
				FROM vShpProducts p INNER JOIN
						tShpProductCategories pc ON pc.ProductId = p.ProductId INNER JOIN
						[dbo].fAllChildCategories(@CategoryId) c ON c.CategoryId = pc.CategoryId
				WHERE p.OnWeb=1 AND p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)", top);

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@CategoryId", Null(by.CategoryId)));
                sqlParams.Add(new SqlParameter("@Locale", Locale));

                string byFilterWhere = string.Empty;
                if (by.ByFilter != null) {
                    BuildByFilter(by.ByFilter, "p", ref byFilterWhere, ref sqlParams);
                    sql = sql + " AND " + byFilterWhere;
                    switch (by.ByFilter.SortBy) {
                        case Product.SortBy.Default:
                            sql += "ORDER BY p.Novinka DESC, p.[Order] ASC, p.[Name] ASC";
                            break;
                        case Product.SortBy.NameASC:
                            sql += "ORDER BY p.Novinka DESC, p.[Name] ASC";
                            break;
                        case Product.SortBy.PriceASC:
                            sql += "ORDER BY p.[Price] ASC";
                            break;
                        case Product.SortBy.PriceDESC:
                            sql += "ORDER BY p.[Price] DESC";
                            break;
                    }
                } else {
                    sql += "ORDER BY p.Novinka DESC, p.[Order] ASC, p.[Name] ASC";
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
								SELECT TOP {0} p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price, p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
										p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount],  p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
										p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                                        p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
								FROM vShpProducts p
								INNER JOIN vShpProductHighlights ph ON ph.ProductId = p.ProductId
								WHERE p.OnWeb=1 AND ph.HighlightId = @HighlightId AND /*(p.InstanceId = 0 OR p.InstanceId=@InstanceId ) AND*/ p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)
								ORDER BY Novinka DESC, [Order] ASC, Name ASC", by.MaxCount.HasValue ? by.MaxCount.Value.ToString() : "100 PERCENT");
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@HighlightId", by.HighlightId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private List<Product> LoadWithBK(Product.ReadWithBK by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price, p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
										p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
										p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
										p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                                        p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
								FROM vShpProducts p
								WHERE p.OnWeb=1 /*AND (p.InstanceId = 0 OR p.InstanceId=@InstanceId )*/ AND p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)
								AND p.BonusovyKredit IS NOT NULL AND p.BonusovyKredit > 0
								ORDER BY Novinka DESC, [Order] ASC, Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private List<Product> LoadVamiNejviceNakupovane(Product.ReadVamiNejviceNakupovane by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price, p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
							p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
							p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
							p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                            p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
					FROM vShpProducts p
					WHERE p.OnWeb=1 /*AND (p.InstanceId = 0 OR p.InstanceId=@InstanceId )*/ AND p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)
					AND p.[VamiNejviceNakupovane] IS NOT NULL AND p.[VamiNejviceNakupovane] > 0
					ORDER BY p.[VamiNejviceNakupovane] ASC, p.[Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private List<Product> LoadDarkoveSety(Product.ReadDarkoveSety by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price, p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
					p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
					p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
					p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                    p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
					FROM vShpProducts p
					WHERE p.OnWeb=1 /*AND (p.InstanceId = 0 OR p.InstanceId=@InstanceId ) AND*/ p.Locale=@Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)
					AND p.[DarkovySet] IS NOT NULL AND p.[DarkovySet] > 0
					ORDER BY p.[DarkovySet] ASC, p.[Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProduct(dr));
            }
            return list;
        }

        private List<Product> LoadByFilter(Product.ReadByFilter by) {
            List<Product> list = new List<Product>();
            using (SqlConnection connection = Connect()) {
                string top = "100 PERCENT";
                if (by.MaxCount.HasValue)
                    top = by.MaxCount.Value.ToString();

                string sql = string.Format(@"
				SELECT TOP {0} p.ProductId, p.InstanceId, p.Code, p.[Name], p.Manufacturer, p.[Description], p.DescriptionLong, p.AdditionalInformation, p.InstructionsForUse, p.Availability, p.StorageCount, p.Price,p.BeznaCena, p.DynamickaSleva, p.StatickaSleva, p.MarzePovolena, p.MarzePovolenaMinimalni, p.CurrencyId, p.CurrencySymbol, p.CurrencyCode, p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, p.BonusovyKredit, 
				p.[Megasleva], p.[Supercena] , p.[CLHit] , p.[Action] , p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], p.[VamiNejviceNakupovane], p.[DarkovySet],
				p.[Top], p.Parfumacia, p.Discount, p.VAT, p.Locale, p.UrlAliasId, p.Alias,
				p.CommentsCount, p.ViewCount, p.Votes, p.TotalRating, p.RatingResult,
                p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, BSR, [Order]
				FROM vShpProducts p
				WHERE p.OnWeb=1 AND p.Locale = @Locale AND (p.ProdejUkoncen IS NULL OR p.ProdejUkoncen=0)", top);
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@Locale", Locale));
                string byFilterWhere = string.Empty;
                if (by != null) {
                    BuildByFilter(by, "p", ref byFilterWhere, ref sqlParams);
                    sql = sql + " AND " + byFilterWhere;
                    switch (by.SortBy) {
                        case Product.SortBy.Default: {
                                if (by.BestSellers.HasValue && by.BestSellers.Value == true)
                                    sql += " ORDER BY p.[SalesCount] DESC, p.[Name] ASC";
                                else
                                    sql += " ORDER BY p.[Order] ASC, p.[SalesCount] DESC, p.[Name] ASC";
                            }
                            break;
                        case Product.SortBy.NameASC:
                            sql += " ORDER BY p.Novinka DESC, p.[Order] ASC, p.[Name] ASC";
                            break;
                        case Product.SortBy.PriceASC:
                            sql += " ORDER BY p.[Price] ASC";
                            break;
                        case Product.SortBy.PriceDESC:
                            sql += " ORDER BY p.[Price] DESC";
                            break;
                        case Product.SortBy.DarkovySet:
                            sql += " ORDER BY p.[DarkovySet] ASC, p.[Order] ASC";
                            break;
                        case Product.SortBy.IdDESC:
                            sql += " ORDER BY p.ProductId DESC";
                            break;
                        case Product.SortBy.IdASC:
                            sql += " ORDER BY p.ProductId ASC";
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
			(@Expression IS NULL OR LOWER(dbo.fMakeAnsi({0}Name)) LIKE '%'+LOWER(dbo.fMakeAnsi(@Expression))+'%' OR LOWER(dbo.fMakeAnsi({0}Description)) LIKE '%'+LOWER(dbo.fMakeAnsi(@Expression))+'%' OR {0}Code LIKE '%'+@Expression)  AND
			(@PriceFrom IS NULL OR {0}Price >= @PriceFrom)  AND
			(@PriceTo IS NULL OR {0}Price <= @PriceTo) AND
			(@TOPProducts IS NULL OR {0}[Top] = @TOPProducts) AND
			(@DarkoveSety IS NULL OR {0}[DarkovySet] != 0) AND

			(@Novinka IS NULL OR {0}Novinka=@Novinka) AND
			(@Inovace IS NULL OR {0}Inovace=@Inovace) AND
			(@Doprodej IS NULL OR {0}Doprodej=@Doprodej) AND
			(@Vyprodano IS NULL OR {0}Vyprodano=@Vyprodano) AND
			(@ProdejUkoncen IS NULL OR {0}ProdejUkoncen=@ProdejUkoncen) AND
										
			(@Megasleva IS NULL OR {0}Megasleva=@Megasleva) AND
			(@Supercena IS NULL OR {0}Supercena=@Supercena) AND
			(@CLHit IS NULL OR {0}CLHit=@CLHit) AND
			(@Action IS NULL OR {0}Action=@Action) AND
			(@Vyprodej IS NULL OR {0}Vyprodej=@Vyprodej) AND
            (@BSR IS NULL OR {0}BSR=@BSR) AND
			(@OnWeb IS NULL OR {0}OnWeb=@OnWeb)",
            columnPrefix);

            byFilter.Manufacturer = string.IsNullOrEmpty(byFilter.Manufacturer) ? null : byFilter.Manufacturer;
            byFilter.Expression = string.IsNullOrEmpty(byFilter.Expression) ? null : byFilter.Expression;
            parameters.Add(new SqlParameter("@BestSellers", Null(byFilter.BestSellers)));
            parameters.Add(new SqlParameter("@Manufacturer", Null(byFilter.Manufacturer)));
            parameters.Add(new SqlParameter("@Expression", Null(byFilter.Expression)));
            parameters.Add(new SqlParameter("@PriceFrom", Null(byFilter.PriceFrom)));
            parameters.Add(new SqlParameter("@PriceTo", Null(byFilter.PriceTo)));
            parameters.Add(new SqlParameter("@TOPProducts", Null(byFilter.TOPProducts)));
            parameters.Add(new SqlParameter("@DarkoveSety", Null(byFilter.DarkoveSety)));

            parameters.Add(new SqlParameter("@Novinka", Null(byFilter.Novinka)));
            parameters.Add(new SqlParameter("@Inovace", Null(byFilter.Inovace)));
            parameters.Add(new SqlParameter("@Doprodej", Null(byFilter.Doprodej)));
            parameters.Add(new SqlParameter("@Vyprodano", Null(byFilter.Vyprodano)));
            parameters.Add(new SqlParameter("@ProdejUkoncen", Null(byFilter.ProdejUkoncen)));

            parameters.Add(new SqlParameter("@Megasleva", Null(byFilter.Megasleva)));
            parameters.Add(new SqlParameter("@Supercena", Null(byFilter.Supercena)));
            parameters.Add(new SqlParameter("@CLHit", Null(byFilter.CLHit)));
            parameters.Add(new SqlParameter("@Action", Null(byFilter.Action)));
            parameters.Add(new SqlParameter("@Vyprodej", Null(byFilter.Vyprodej)));
            parameters.Add(new SqlParameter("@OnWeb", Null(byFilter.OnWeb)));
            parameters.Add(new SqlParameter("@BSR", Null(byFilter.BSR)));
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
            List<SHP.Entities.ProductCategories> productCategories = product.ProductCategories;

            //Vymazanie starych hodnot
            RemoveProductFromCategories(product);

            using (SqlConnection connection = Connect()) {
                foreach (SHP.Entities.ProductCategories pc in productCategories) {
                    pc.ProductId = product.Id;
                    SHP.Storage<SHP.Entities.ProductCategories>.Create(pc);
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
                SHP.Storage<CMS.Entities.AccountVote>.Create(accountVote);
            }

            return cmd;
        }

        public override void Create(Product product) {
            throw new NotImplementedException();
        }

        public override void Update(Product product) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpProductModifyEx",
                    new SqlParameter("@HistoryAccount", 1),
                    new SqlParameter("@ProductId", product.Id),
                    new SqlParameter("@MaximalniPocetVBaleni", Null(product.MaximalniPocetVBaleni)),
                    new SqlParameter("@MinimalniPocetVBaleni", Null(product.MinimalniPocetVBaleni)),
                    new SqlParameter("@VamiNejviceNakupovane", Null(product.VamiNejviceNakupovane)),
                    new SqlParameter("@DarkovySet", Null(product.DarkovySet)),
                    new SqlParameter("@InternalStorageCount", product.InternalStorageCount),
                    new SqlParameter("@LimitDate", Null(product.LimitDate)),
                    new SqlParameter("@BSR", Null(product.BSR)),
                    new SqlParameter("@Order", Null(product.Order))
                    );
            }
        }

        public override void Delete(Product product) {
            throw new NotImplementedException();
        }
    }
}
