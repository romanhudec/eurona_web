using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Cron.Eurona.Import {
    public static class EuronaDAL {
        public enum BonusovyKreditTyp : int {
            None = 0,
            RucneZadany = 1,
            OdeslanaObjednavka = 2,
            Eurosap = 10
        }

        /// <summary>
        /// Vráti true, ak produkt s ID uz je v databayi Eurona zaevidovany pod nejakou inštanciou
        /// </summary>
        public static bool ExistProduct(CMS.Pump.MSSQLStorage mssqStorageDst, int productId) {
            string sql = "SELECT Count(ProductId) FROM tShpProduct WITH (NOLOCK) WHERE ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@ProductId", productId));
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count != 0;
        }

        #region Product Instance methods
        /// <summary>
        /// Vráti inštanciu existujúceho produktu
        /// </summary>
        public static int GetProductInstance(CMS.Pump.MSSQLStorage mssqStorageDst, int productId) {
            string sql = "SELECT InstanceId FROM tShpProduct WITH (NOLOCK) WHERE ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@ProductId", productId));
            int instance = Convert.ToInt32(dt.Rows[0][0]);
            return instance;
        }

        /// <summary>
        /// Zmeni instanciu produktu.
        /// </summary>
        public static void UpdateProductInstance(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, int instanceId) {
            string sql = "UPDATE tShpProduct SET InstanceId=@InstanceId WHERE ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@InstanceId", instanceId));
        }
        #endregion

        #region Product Highlights methods
        public static int? GetHighlightId(CMS.Pump.MSSQLStorage mssqStorageDst, string code, int instanceId, string locale) {
            string sql = "SELECT HighlightId FROM vShpHighlights WITH (NOLOCK)  WHERE Code=@Code AND InstanceId=@InstanceId AND Locale=@Locale";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@Code", code),
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@Locale", locale));

            if (dt.Rows.Count == 0) return null;

            int highlightId = Convert.ToInt32(dt.Rows[0][0]);
            return highlightId;
        }
        public static void RemoveProductHighlights(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int productId) {
            string sql = @"DELETE FROM tShpProductHighlights WHERE InstanceId=@InstanceId AND ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@ProductId", productId));
        }
        public static void InsertProductHighlight(CMS.Pump.MSSQLStorage mssqStorageDst, string highlightCode, int instanceId, int productId, string locale) {
            int? highLightId = GetHighlightId(mssqStorageDst, highlightCode, instanceId, locale);
            if (!highLightId.HasValue) return;

            string sql = @"INSERT INTO tShpProductHighlights (InstanceId, ProductId, HighlightId )
										VALUES( @InstanceId, @ProductId, @HighlightId )";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@HighlightId", highLightId.Value));
        }
        #endregion

        #region Product Categories methods
        public static bool ExistCategory(CMS.Pump.MSSQLStorage mssqStorageDst, int categoryId) {
            string sql = "SELECT CategoryId FROM tShpCategory  WITH (NOLOCK) WHERE CategoryId=@CategoryId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@CategoryId", categoryId));

            return dt.Rows.Count != 0;
        }
        public static void RemoveProductCategories(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int productId) {
            string sql = @"DELETE FROM tShpProductCategories WHERE InstanceId=@InstanceId AND ProductId=@ProductId";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@ProductId", productId));
        }

        public static void InsertProductCategory(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int categoryId, int productId) {
            if (instanceId == 1) { categoryId = 1000 + categoryId; }
            if (instanceId == 2) { categoryId = 2000 + categoryId; }
            if (instanceId == 3) { categoryId = 3000 + categoryId; }
            if (!ExistCategory(mssqStorageDst, categoryId)) return;

            string sql = @"INSERT INTO tShpProductCategories (InstanceId, ProductId, CategoryId )
										VALUES( @InstanceId, @ProductId, @CategoryId )";
            DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@CategoryId", categoryId));
        }
        #endregion

        /// <summary>
        /// Trieda na zosynchronizovanie kategorii
        /// </summary>
        public static class Classifiers {
            #region Categories
            public static void SyncCategory(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int categoryId, int? parentId) {
                if (instanceId == 1) { categoryId = 1000 + categoryId; parentId = 1000 + parentId; }
                if (instanceId == 2) { categoryId = 2000 + categoryId; parentId = 2000 + parentId; }
                if (instanceId == 3) { categoryId = 3000 + categoryId; parentId = 3000 + parentId; }
                string sql = @"SELECT CategoryId FROM tShpCategory WITH (NOLOCK) WHERE CategoryId=@CategoryId AND InstanceId=@InstanceId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@InstanceId", instanceId));

                //INSERT
                if (dt.Rows.Count == 0) {
                    sql = @"
												SET IDENTITY_INSERT tShpCategory ON
												INSERT INTO tShpCategory ( InstanceId, CategoryId, ParentId, HistoryType, HistoryStamp, HistoryAccount ) VALUES
												(@InstanceId, @CategoryId, @ParentId, 'C', GETDATE(), 1)
												SET IDENTITY_INSERT tShpCategory OFF";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@CategoryId", categoryId),
                            new SqlParameter("@ParentId", Null(parentId)));
                } else {
                    sql = @"
												UPDATE tShpCategory SET ParentId=@ParentId, InstanceId=@InstanceId WHERE CategoryId=@CategoryId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@CategoryId", categoryId),
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@ParentId", Null(parentId)));
                }
            }

            public static void SyncCategoryLocale(CMS.Pump.MSSQLStorage mssqStorageDst, int categoryId, string name, string nameParent, int instanceId, string locale, bool isLivienneCategory = false) {
                if (instanceId == 1) categoryId = 1000 + categoryId;
                if (instanceId == 2) categoryId = 2000 + categoryId;
                if (instanceId == 3) categoryId = 3000 + categoryId;
                string sql = @"SELECT UrlAliasId, CategoryId FROM vShpCategories WHERE CategoryId=@CategoryId AND InstanceId=@InstanceId AND Locale=@Locale";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@Locale", locale));

                //INSERT
                if (dt.Rows.Count == 0) {

                    string parent = string.Empty;
                    if (nameParent != null) {
                        parent = GetAliasName(nameParent);
                        if (!string.IsNullOrEmpty(parent)) parent += "/";
                        else parent = string.Empty;
                    }

                    ////Ak je CLLivienne ma inu url
                    string url = string.Format("~/eshop/category.aspx?id={0}", categoryId);
                    /*Zruse ne p.Cernym 26.9.2012*/
                    //if (instanceId == 3 /*CL*/ && isLivienneCategory)
                    //    url = string.Format("~/eshop/categoryLiv.aspx?id={0}", categoryId);

                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;
                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pUrlAliasCreate",
                    new SqlParameter("@InstanceId", instanceId),
                    new SqlParameter("@Url", url),
                    new SqlParameter("@Locale", locale),
                    new SqlParameter("@Name", string.Format("kategorie-{0}", categoryId)),
                    new SqlParameter("@Alias", "~/eshop/" + parent + GetAliasName(name)),
                    result);

                    sql = @"INSERT INTO tShpCategoryLocalization ( CategoryId, [Name], Locale, UrlAliasId ) VALUES
					( @CategoryId, @Name, @Locale, @UrlAliasId)";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@CategoryId", categoryId),
                            new SqlParameter("@Name", name),
                            new SqlParameter("@Locale", locale),
                            new SqlParameter("@UrlAliasId", Convert.ToInt32(result.Value)));
                } else
                //UPDATE
				{
                    foreach (DataRow row in dt.Rows) {
                        int urlAliasId = Convert.ToInt32(row["UrlAliasId"]);
                        #region Check LIV Category
                        //string url = string.Format("~/eshop/category.aspx?id={0}", categoryId);
                        //int aliasExistsId = 0;
                        //if (instanceId == 3 /*CL*/ && isLivienneCategory)
                        //{
                        //    url = string.Format("~/eshop/categoryLiv.aspx?id={0}", categoryId);
                        //    sql = @"SELECT * FROM tUrlAlias WHERE UrlAliasId=@UrlAliasId";
                        //    DataTable dtAlias = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        //    new SqlParameter("@UrlAliasId", urlAliasId)
                        //    );

                        //    if (dtAlias.Rows.Count != 0)
                        //        aliasExistsId = Convert.ToInt32(dtAlias.Rows[0]["UrlAliasId"]);
                        //}
                        //SqlParameter result = new SqlParameter("@Result", -1);
                        //if (aliasExistsId != 0)
                        //{
                        //    string parent = string.Empty;
                        //    if (nameParent != null)
                        //    {
                        //        parent = GetAliasName(nameParent);
                        //        if (!string.IsNullOrEmpty(parent)) parent += "/";
                        //        else parent = string.Empty;
                        //    }

                        //    result.Direction = ParameterDirection.Output;
                        //    mssqStorageDst.Exec(mssqStorageDst.Connection, "UPDATE tUrlAlias SET Url=@Url, Alias=@Alias WHERE UrlAliasId=@UrlAliasId",
                        //            new SqlParameter("@UrlAliasId", aliasExistsId),
                        //            new SqlParameter("@Url", url),
                        //            new SqlParameter("@Alias", "~/eshop/" + parent + GetAliasName(name))
                        //            );
                        //    result.Value = aliasExistsId;
                        //}
                        #endregion

                        sql = @"UPDATE tShpCategoryLocalization SET Name=@Name
						WHERE CategoryId=@CategoryId AND Locale=@Locale";
                        mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                                new SqlParameter("@CategoryId", categoryId),
                                new SqlParameter("@Name", name),
                                new SqlParameter("@Locale", locale));
                    }
                }
            }
            #endregion
        }

        public static class Product {
            public static void SyncProduct(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, int instanceId, string kod, decimal? vat, int? body, int? parfumacia, int? top, bool novinka, bool inovace, bool doprodej, bool vyprodano, bool prodejUkoncen, int storageCount, bool megasleva, bool supercena, bool clHit, bool action, bool vyprodej, bool onWeb, bool bsrProdukt, string zadni_etiketa = "", bool zobrazovat_zadni_etiketu = false) {
                string sql = string.Empty;
                //INSERT
                if (!EuronaDAL.ExistProduct(mssqStorageDst, productId)) {
                    sql = @"
					SET IDENTITY_INSERT tShpProduct ON
					INSERT INTO tShpProduct ([InstanceId], [ProductId], [StorageCount], [Code], [VAT], [Body], [Parfumacia], [Discount], [Top], [Novinka], [Inovace], [Doprodej], [Vyprodano], [ProdejUkoncen],
							[Megasleva], [Supercena], [CLHit], [Action], [Vyprodej], [OnWeb], [ZadniEtiketa], [ZobrazovatZadniEtiketu], [BSR],
							[HistoryType], [HistoryStamp], [HistoryAccount])
					VALUES( @InstanceId, @ProductId, @StorageCount, @Code, @VAT, @Body, @Parfumacia, 0, @Top, @Novinka, @Inovace, @Doprodej, @Vyprodano, @ProdejUkoncen, 
							@Megasleva, @Supercena,@CLHit, @Action, @Vyprodej, @OnWeb, @ZadniEtiketa, @ZobrazovatZadniEtiketu, @BSR, 
							'C', GETDATE(), 1 )
					SET IDENTITY_INSERT tShpProduct OFF";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@StorageCount", storageCount),
                            new SqlParameter("@Code", Null(kod)),
                            new SqlParameter("@VAT", Null(vat, 0)),
                            new SqlParameter("@Body", Null(body, 0)),
                            new SqlParameter("@Parfumacia", Null(parfumacia)),
                            new SqlParameter("@Top", Null(top)),
                            new SqlParameter("@Novinka", Null(novinka)),
                            new SqlParameter("@Inovace", Null(inovace)),
                            new SqlParameter("@Doprodej", Null(doprodej)),
                            new SqlParameter("@Vyprodano", Null(vyprodano)),
                            new SqlParameter("@ProdejUkoncen", Null(prodejUkoncen)),
                            new SqlParameter("@Megasleva", Null(megasleva)),
                            new SqlParameter("@Supercena", Null(supercena)),
                            new SqlParameter("@CLHit", Null(clHit)),
                            new SqlParameter("@Action", Null(action)),
                            new SqlParameter("@Vyprodej", Null(vyprodej)),
                            new SqlParameter("@OnWeb", Null(onWeb)),
                            new SqlParameter("@ZadniEtiketa", Null(zadni_etiketa)),
                            new SqlParameter("@ZobrazovatZadniEtiketu", Null(zobrazovat_zadni_etiketu)),
                            new SqlParameter("@BSR", Null(bsrProdukt))
                            );
                } else {
                    sql = @"
					UPDATE tShpProduct SET InstanceId=@InstanceId, StorageCount=@StorageCount, Code=@Code, VAT=@VAT, Body=@Body, Parfumacia=@Parfumacia, [Top]=@Top, Novinka=@Novinka, Inovace=@Inovace, Doprodej=@Doprodej, Vyprodano=@Vyprodano, ProdejUkoncen=@ProdejUkoncen,
					[Megasleva]=@Megasleva, [Supercena]=@Supercena, [CLHit]=@CLHit, [Action]=@Action, [Vyprodej]=@Vyprodej, [OnWeb]=@OnWeb, [ZadniEtiketa]=@ZadniEtiketa, [ZobrazovatZadniEtiketu]=@ZobrazovatZadniEtiketu, [BSR]=@BSR
					WHERE ProductId=@ProductId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@StorageCount", storageCount),
                            new SqlParameter("@Code", Null(kod)),
                            new SqlParameter("@VAT", Null(vat, 0)),
                            new SqlParameter("@Body", Null(body, 0)),
                            new SqlParameter("@Parfumacia", Null(parfumacia)),
                            new SqlParameter("@Top", Null(top)),
                            new SqlParameter("@Novinka", Null(novinka)),
                            new SqlParameter("@Inovace", Null(inovace)),
                            new SqlParameter("@Doprodej", Null(doprodej)),
                            new SqlParameter("@Vyprodano", Null(vyprodano)),
                            new SqlParameter("@ProdejUkoncen", Null(prodejUkoncen)),
                            new SqlParameter("@Megasleva", Null(megasleva)),
                            new SqlParameter("@Supercena", Null(supercena)),
                            new SqlParameter("@CLHit", Null(clHit)),
                            new SqlParameter("@Action", Null(action)),
                            new SqlParameter("@Vyprodej", Null(vyprodej)),
                            new SqlParameter("@OnWeb", Null(onWeb)),
                            new SqlParameter("@ZadniEtiketa", Null(zadni_etiketa)),
                            new SqlParameter("@ZobrazovatZadniEtiketu", Null(zobrazovat_zadni_etiketu)),
                            new SqlParameter("@BSR", Null(bsrProdukt))
                            );
                }
            }

            public static void SyncProductStock(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, int instanceId, int storageCount) {
                string sql = @"
				UPDATE tShpProduct SET StorageCount=@StorageCount WHERE ProductId=@ProductId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@StorageCount", storageCount)
                );
            }

            public static void SyncProductLocale(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, int instanceId, string locale,
                    string code, string name, string description, string descriptionLong, string instructionsForUse, string additionalInformation, bool isLivienne = false) {
                string sql = @"SELECT ProductId, UrlAliasId FROM vShpProducts WHERE ProductId=@ProductId AND InstanceId=@InstanceId AND Locale=@Locale";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@Locale", locale));

                //INSERT
                if (dt.Rows.Count == 0) {
                    #region Check Liv Product
                    string url = string.Format("~/eshop/product.aspx?id={0}", productId);

                    int aliasExistsId = 0;
                    /*
					if (instanceId == 3 /*CL*/
                    /* && isLivienne)
{
url = string.Format("~/eshop/productLiv.aspx?id={0}", productId);
sql = @"SELECT * FROM tUrlAlias WHERE [Name]=@Name AND InstanceId=@InstanceId AND Locale=@Locale";
DataTable dtAlias = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
new SqlParameter("@InstanceId", instanceId),
new SqlParameter("@Locale", locale),
new SqlParameter("@Name", string.Format("produkt-{0}", productId))
);

if (dtAlias.Rows.Count != 0)
  aliasExistsId = Convert.ToInt32(dtAlias.Rows[0]["UrlAliasId"]);
}
*/
                    SqlParameter result = new SqlParameter("@Result", -1);
                    if (aliasExistsId == 0) {
                        result.Direction = ParameterDirection.Output;
                        mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pUrlAliasCreate",
                                new SqlParameter("@InstanceId", instanceId),
                                new SqlParameter("@Url", url),
                                new SqlParameter("@Locale", locale),
                                new SqlParameter("@Name", string.Format("produkt-{0}", productId)),
                                new SqlParameter("@Alias", "~/eshop/" + EuronaDAL.GetAliasName(string.Format("{0}-{1}", code, name))),
                                result);
                    } else {
                        result.Direction = ParameterDirection.Output;
                        mssqStorageDst.Exec(mssqStorageDst.Connection, "UPDATE tUrlAlias SET Url=@Url, Alias=@Alias WHERE UrlAliasId=@UrlAliasId",
                                new SqlParameter("@UrlAliasId", aliasExistsId),
                                new SqlParameter("@Url", url),
                                new SqlParameter("@Alias", "~/eshop/" + EuronaDAL.GetAliasName(string.Format("{0}-{1}", code, name)))
                                );
                        result.Value = aliasExistsId;
                    }
                    #endregion

                    sql = @"
					INSERT INTO tShpProductLocalization  ([ProductId], [Locale], [Name], [Description], [DescriptionLong], [InstructionsForUse], [AdditionalInformation], [UrlAliasId], [FiullText] )
					VALUES( @ProductId, @Locale, @Name, @Description, @DescriptionLong, @InstructionsForUse, @AdditionalInformation, @UrlAliasId,  LOWER(dbo.fMakeAnsi(@Name + ' ' + @Description + ' ' + '" + code + @"')) )";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                    new SqlParameter("@ProductId", productId),
                            new SqlParameter("@Locale", locale),
                            new SqlParameter("@Name", Null(name)),
                            new SqlParameter("@Description", Null(description)),
                            new SqlParameter("@DescriptionLong", Null(descriptionLong)),
                            new SqlParameter("@InstructionsForUse", Null(instructionsForUse)),
                            new SqlParameter("@AdditionalInformation", Null(additionalInformation)),
                            new SqlParameter("@UrlAliasId", Convert.ToInt32(result.Value)));
                } else
                //UPDATE
				{
                    foreach (DataRow row in dt.Rows) {
                        #region Check LIV Product
                        string url = string.Format("~/eshop/product.aspx?id={0}", productId);
                        int aliasExistsId = 0;
                        /*
						if (instanceId == 3 /*CL*/
                        /* && isLivienne)
{
  url = string.Format("~/eshop/productLiv.aspx?id={0}", productId);
  sql = @"SELECT * FROM tUrlAlias WHERE [Name]=@Name AND InstanceId=@InstanceId AND Locale=@Locale";
  DataTable dtAlias = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
  new SqlParameter("@InstanceId", instanceId),
  new SqlParameter("@Locale", locale),
  new SqlParameter("@Name", string.Format("produkt-{0}", productId))
  );

  if (dtAlias.Rows.Count != 0)
      aliasExistsId = Convert.ToInt32(dtAlias.Rows[0]["UrlAliasId"]);
}
*/
                        SqlParameter result = new SqlParameter("@Result", -1);
                        if (aliasExistsId != 0) {
                            result.Direction = ParameterDirection.Output;
                            mssqStorageDst.Exec(mssqStorageDst.Connection, "UPDATE tUrlAlias SET Url=@Url, Alias=@Alias WHERE UrlAliasId=@UrlAliasId",
                                    new SqlParameter("@UrlAliasId", aliasExistsId),
                                    new SqlParameter("@Url", url),
                                    new SqlParameter("@Alias", "~/eshop/" + EuronaDAL.GetAliasName(string.Format("{0}-{1}", code, name)))
                                    );
                            result.Value = aliasExistsId;
                        }
                        #endregion

                        sql = @"
						UPDATE tShpProductLocalization SET [Name]=@Name, [Description]=@Description, [DescriptionLong]=@DescriptionLong, [InstructionsForUse]=@InstructionsForUse, [AdditionalInformation]=@AdditionalInformation,
                        [FiullText] = LOWER(dbo.fMakeAnsi(@Name + ' ' + @Description + ' ' +  + ' ' + '" + code + @"'))
						WHERE ProductId=@ProductId AND Locale=@Locale";
                        mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ProductId", productId),
                                new SqlParameter("@Locale", locale),
                                new SqlParameter("@Name", Null(name)),
                                new SqlParameter("@Description", Null(description)),
                                new SqlParameter("@DescriptionLong", Null(descriptionLong)),
                                new SqlParameter("@InstructionsForUse", Null(instructionsForUse)),
                                new SqlParameter("@AdditionalInformation", Null(additionalInformation)));

                        sql = @"
						UPDATE tUrlAlias SET InstanceId=@InstanceId
						WHERE UrlAliasId=@UrlAliasId";
                        mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@UrlAliasId", Convert.ToInt32(row["UrlAliasId"])),
                        new SqlParameter("@InstanceId", instanceId));
                    }
                }
            }

//            public static void SyncProductCeny(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, int jazyk, int body, decimal? cena, decimal? beznaCena, bool? marzePovolena, bool? marzePovolenaMinimalni, decimal? cenaBk, DateTime? platnostOd, DateTime? platnostDo) {
//                string sql = @"SELECT * FROM  tShpCenyProduktu WITH (NOLOCK) WHERE ProductId=@ProductId AND Locale=@Locale";
//                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@ProductId", productId),
//                        new SqlParameter("@Locale", locale));

//                //INSERT
//                if (dt.Rows.Count == 0) {
//                    sql = @"
//					INSERT INTO tShpCenyProduktu ([ProductId], [Locale], [CurrencyId], [Body], [Cena], [BeznaCena], [MarzePovolena], [MarzePovolenaMinimalni], [CenaBK])
//					VALUES( @ProductId, @Locale, @CurrencyId, @Body, @Cena, @Bezna_cena, @MarzePovolena, @MarzePovolenaMinimalni, @CenaBK  )";
//                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
//                            new SqlParameter("@ProductId", productId),
//                            new SqlParameter("@Locale", locale),
//                            new SqlParameter("@CurrencyId", jazyk),
//                            new SqlParameter("@Body", Null(body)),
//                            new SqlParameter("@Cena", Null(cena)),
//                            new SqlParameter("@Bezna_cena", Null(beznaCena)),
//                            new SqlParameter("@MarzePovolena", Null(marzePovolena)),
//                            new SqlParameter("@MarzePovolenaMinimalni", Null(marzePovolenaMinimalni)),
//                            new SqlParameter("@CenaBK", Null(cenaBk)));
//                } else {
//                    sql = @"
//					UPDATE tShpCenyProduktu SET Body=@Body, Cena=@Cena, BeznaCena=@Bezna_cena, MarzePovolena=@MarzePovolena, MarzePovolenaMinimalni=@MarzePovolenaMinimalni, CenaBK=@CenaBK WHERE ProductId=@ProductId AND Locale=@Locale";
//                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
//                            new SqlParameter("@ProductId", productId),
//                            new SqlParameter("@Locale", locale),
//                            new SqlParameter("@Body", Null(body)),
//                            new SqlParameter("@Cena", Null(cena)),
//                            new SqlParameter("@Bezna_cena", Null(beznaCena)),
//                            new SqlParameter("@MarzePovolena", Null(marzePovolena)),
//                            new SqlParameter("@MarzePovolenaMinimalni", Null(marzePovolenaMinimalni)),
//                            new SqlParameter("@CenaBK", Null(cenaBk)));
//                }
//            }

            public static void SyncDeleteProductCeny(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale) {
                string sql = @"DELETE FROM  tShpCenyProduktu WITH (NOLOCK) WHERE ProductId=@ProductId AND Locale=@Locale";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale));
            }
            public static void SyncProductCeny(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, int jazyk, int body, decimal? cena, decimal? beznaCena, bool? marzePovolena, bool? marzePovolenaMinimalni, decimal? cenaBk, DateTime? platnostOd, DateTime? platnostDo) {
               string sql = @"
					INSERT INTO tShpCenyProduktu ([ProductId], [Locale], [CurrencyId], [Body], [Cena], [BeznaCena], [MarzePovolena], [MarzePovolenaMinimalni], [CenaBK], [PlatnostOd], [PlatnostDo])
					VALUES( @ProductId, @Locale, @CurrencyId, @Body, @Cena, @Bezna_cena, @MarzePovolena, @MarzePovolenaMinimalni, @CenaBK, @PlatnostOd, @PlatnostDo )";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale),
                        new SqlParameter("@CurrencyId", jazyk),
                        new SqlParameter("@Body", Null(body)),
                        new SqlParameter("@Cena", Null(cena)),
                        new SqlParameter("@Bezna_cena", Null(beznaCena)),
                        new SqlParameter("@MarzePovolena", Null(marzePovolena)),
                        new SqlParameter("@MarzePovolenaMinimalni", Null(marzePovolenaMinimalni)),
                        new SqlParameter("@CenaBK", Null(cenaBk)),
                        new SqlParameter("@PlatnostOd", Null(platnostOd)),
                        new SqlParameter("@PlatnostDo", Null(platnostDo)));
            }
            public static void SyncProductCenyCL(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, int jazyk, int body, decimal? cena, decimal? beznaCena, bool? dynamickaSleva, decimal? statickaSleva, decimal? cenaBk) {
                string sql = @"SELECT * FROM  tShpCenyProduktu WITH (NOLOCK) WHERE ProductId=@ProductId AND Locale=@Locale";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale));

                //INSERT
                if (dt.Rows.Count == 0) {
                    sql = @"
					INSERT INTO tShpCenyProduktu ([ProductId], [Locale], [CurrencyId], [Body], [Cena], [BeznaCena], [DynamickaSleva], [StatickaSleva], [CenaBK])
					VALUES( @ProductId, @Locale, @CurrencyId, @Body, @Cena, @Bezna_cena, @DynamickaSleva, @StatickaSleva, @CenaBK  )";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@Locale", locale),
                            new SqlParameter("@CurrencyId", jazyk),
                            new SqlParameter("@Body", Null(body)),
                            new SqlParameter("@Cena", Null(cena)),
                            new SqlParameter("@Bezna_cena", Null(beznaCena)),
                            new SqlParameter("@DynamickaSleva", Null(dynamickaSleva)),
                            new SqlParameter("@StatickaSleva", Null(statickaSleva)),
                            new SqlParameter("@CenaBK", Null(cenaBk)));
                } else {
                    sql = @"
					UPDATE tShpCenyProduktu SET Body=@Body, Cena=@Cena, BeznaCena=@Bezna_cena, StatickaSleva=@StatickaSleva, DynamickaSleva=@DynamickaSleva, CenaBK=@CenaBK WHERE ProductId=@ProductId AND Locale=@Locale";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@Locale", locale),
                            new SqlParameter("@Body", Null(body)),
                            new SqlParameter("@Cena", Null(cena)),
                            new SqlParameter("@Bezna_cena", Null(beznaCena)),
                            new SqlParameter("@DynamickaSleva", Null(dynamickaSleva)),
                            new SqlParameter("@StatickaSleva", Null(statickaSleva)),
                            new SqlParameter("@CenaBK", Null(cenaBk)));
                }
            }
            public static void RemoveVlastnostiProduktu(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale) {
                string sql = @"DELETE FROM tShpVlastnostiProduktu WHERE ProductId=@ProductId AND Locale=@Locale";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@Locale", locale));
            }
            public static void SyncProductVlastnosti(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, string name, string imageUrl) {
                string sql = @"
				INSERT INTO tShpVlastnostiProduktu ([ProductId], [Locale], [Name], [ImageUrl] )
				VALUES( @ProductId, @Locale, @Name, @ImageUrl )";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale),
                        new SqlParameter("@Name", Null(name)),
                        new SqlParameter("@ImageUrl", Null(imageUrl)));
            }

            public static void RemovePiktogramyProduktu(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale) {
                string sql = @"DELETE FROM tShpPiktogramyProduktu WHERE ProductId=@ProductId AND Locale=@Locale";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@Locale", locale));
            }
            public static void SyncProductPiktogramy(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, string name, string imageUrl) {
                string sql = @"
				INSERT INTO tShpPiktogramyProduktu ([ProductId], [Locale], [Name], [ImageUrl] )
				VALUES( @ProductId, @Locale, @Name, @ImageUrl )";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale),
                        new SqlParameter("@Name", Null(name)),
                        new SqlParameter("@ImageUrl", Null(imageUrl)));
            }

            public static void RemoveUcinkyProduktu(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale) {
                string sql = @"DELETE FROM tShpUcinkyProduktu WHERE ProductId=@ProductId AND Locale=@Locale";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@Locale", locale));
            }
            public static void SyncProductUcinky(CMS.Pump.MSSQLStorage mssqStorageDst, int productId, string locale, string name, string imageUrl) {
                string sql = @"
				INSERT INTO tShpUcinkyProduktu ([ProductId], [Locale], [Name], [ImageUrl] )
				VALUES( @ProductId, @Locale, @Name, @ImageUrl )";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Locale", locale),
                        new SqlParameter("@Name", Null(name)),
                        new SqlParameter("@ImageUrl", Null(imageUrl)));
            }

            public static void RemoveProductRelations(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int productId) {
                string sql = @"DELETE FROM tShpProductRelation WHERE ParentProductId=@ParentProductId AND InstanceId=@InstanceId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                new SqlParameter("@ParentProductId", productId),
                new SqlParameter("@InstanceId", instanceId));
            }
            public static void SyncProductAlternativni(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId, int parentId, int productId) {
                string sql = @"SELECT * FROM  tShpProductRelation WITH (NOLOCK) WHERE ParentProductId=@ParentProductId AND ProductId=@ProductId AND InstanceId=@InstanceId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ParentProductId", parentId),
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@InstanceId", instanceId));

                sql = @"SELECT DISTINCT CODE FROM vShpProducts WITH (NOLOCK) WHERE ProductId IN (@ParentProductId , @ProductId )";
                DataTable dtProduct = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@ParentProductId", parentId),
                        new SqlParameter("@ProductId", productId));

                if (dtProduct.Rows.Count < 2) return;

                //INSERT
                if (dt.Rows.Count == 0) {
                    //None = 0,
                    //Related = 1, /*Suvisiace produkty*/
                    //Alternate = 2, /*Alternativne produkty*/ 

                    sql = @"
					INSERT INTO tShpProductRelation (ParentProductId, ProductId, InstanceId, RelationType )
					VALUES( @ParentProductId, @ProductId, @InstanceId, @RelationType )";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@ParentProductId", parentId),
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@RelationType", 2));
                }
            }
        }

        public static class Order {
            public enum OrderStatus : int {
                None = 0,
                WaitingForProccess = -1,
                InProccess = -2,
                Proccessed = -3,
                Storno = -4
            }

            /// <summary>
            /// Vrati ID meny podla kodu
            /// </summary>
            private static int GetCurrencyId(CMS.Pump.MSSQLStorage mssqStorageDst, string currencyCode) {
                string sql = @"
				SELECT c.CurrencyId FROM cShpCurrency c 
				INNER JOIN cSupportedLocale l ON l.Code = c.Locale
				WHERE c.Code=@Code";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@Code", currencyCode));
                if (dt.Rows.Count == 0) return 0;
                else return Convert.ToInt32(dt.Rows[0]["CurrencyId"]);
            }
            public static void SyncEuronaFinalOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, int orderStatusCode, string currencyCode, string notes) {
                if (orderStatusCode == 2) orderStatusCode = (int)OrderStatus.Storno;
                if (orderStatusCode == 4) orderStatusCode = (int)OrderStatus.Proccessed;
                if (orderStatusCode != (int)OrderStatus.Storno && orderStatusCode != (int)OrderStatus.Proccessed) orderStatusCode = (int)OrderStatus.InProccess;

                int currencyId = GetCurrencyId(mssqStorageDst, currencyCode);
                string sql = @"
				UPDATE tShpOrder SET OrderStatusCode=@OrderStatusCode, CurrencyId=@CurrencyId, Notes=@Notes
				WHERE OrderId=@OrderId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@OrderStatusCode", orderStatusCode.ToString()),
                        new SqlParameter("@CurrencyId", currencyId),
                        new SqlParameter("@Notes", notes));
            }
            public static void SyncEuronaFakturyOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, string currencyCode, string notes) {
                int currencyId = GetCurrencyId(mssqStorageDst, currencyCode);
                string sql = @"
				UPDATE tShpOrder SET CurrencyId=@CurrencyId, Notes=@Notes
				WHERE OrderId=@OrderId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@CurrencyId", currencyId),
                        new SqlParameter("@Notes", notes));
            }
            public static void SyncOrder(CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, string orderNumber, string orderStatusCode, DateTime? paydDate, string notes, decimal? price, decimal? priceWVAT) {
                string sql = @"
				UPDATE tShpOrder SET OrderNumber=@OrderNumber, OrderStatusCode=@OrderStatusCode, PaydDate=@PaydDate, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT
				WHERE OrderId=@OrderId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@OrderNumber", orderNumber),
                        new SqlParameter("@OrderStatusCode", orderStatusCode),
                        new SqlParameter("@PaydDate", Null(paydDate)),
                        new SqlParameter("@Notes", Null(notes)),
                        new SqlParameter("@Price", Null(price)),
                        new SqlParameter("@PriceWVAT", Null(priceWVAT)));
            }
            /*
            public static DataTable GetOrders(CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId) {
                string sql = @"SELECT OrderId ,InstanceId ,OrderNumber ,OrderDate ,CartId ,OrderStatusCode ,PaydDate ,ShipmentCode ,PaymentCode ,Notes ,Price ,PriceWVAT ,CurrencyId, AssociatedOrderId, CreatedByAccountId
				FROM tShpOrder WITH (NOLOCK) WHERE HistoryId IS NULL AND InstanceId=@InstanceId AND OrderStatusCode=@OrderStatusCode";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql,
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@OrderStatusCode", OrderStatus.InProccess));
                return dt;
            }
            public static DataRow GetCart(CMS.Pump.MSSQLStorage mssqStorageDst, int cartId) {
                string sql = @"SELECT c.CartId, c.InstanceId, c.SessionId, c.AccountId, c.Created,c.ShipmentCode,c.PaymentCode,c.DeliveryAddressId,c.InvoiceAddressId,c.Notes,c.Closed
				FROM tShpCart c WITH (NOLOCK)
				WHERE c.CartId=@CartId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@CartId", cartId));
                if (dt.Rows.Count == 0) return null;
                return dt.Rows[0];
            }
            public static DataTable GetCartProducts(CMS.Pump.MSSQLStorage mssqStorageDst, int cartId) {
                string sql = @"SELECT CartProductId,InstanceId,CartId,ProductId,Quantity ,Price ,PriceWVAT ,VAT ,Discount ,PriceTotal ,PriceTotalWVAT ,CurrencyId
				FROM tShpCartProduct WITH (NOLOCK) WHERE CartId=@CartId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@CartId", cartId));
                return dt;
            }
            */

        }

        public static class Account {
            public enum AddressType {
                Register,
                Delivery
            }

            /// <summary>
            /// Vráti TVD_ID (ID odberatela z TVD) podla AccountId v CMS
            /// </summary>
            public static int GetTVDAccountId(CMS.Pump.MSSQLStorage mssqStorageDst, int accountId) {
                string sql = "SELECT TVD_Id FROM vAccounts WHERE AccountId=@AccountId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@AccountId", accountId));
                if (dt.Rows.Count == 0) return 0;
                else return Convert.ToInt32(dt.Rows[0]["TVD_Id"]);
            }

            /// <summary>
            /// Vráti ID accountu z CMS podla jeho TVD_ID
            /// </summary>
            public static int GetAccountId(CMS.Pump.MSSQLStorage mssqStorageDst, int tvd_id) {
                string sql = "SELECT AccountId FROM vAccounts WHERE TVD_Id=@TVD_Id";
                using (SqlConnection connectionDst = mssqStorageDst.Connect()) {
                    DataTable dt = mssqStorageDst.Query(connectionDst, sql, new SqlParameter("@TVD_Id", tvd_id));
                    if (dt.Rows.Count == 0) return 0;
                    else return Convert.ToInt32(dt.Rows[0]["AccountId"]);
                }
            }

            public static int SyncAccount(CMS.Pump.MSSQLStorage mssqStorageDst, int accountTVDId, int instanceId, string login, string password, string email, bool enabled, string roles, bool canAccessIntensa, DateTime? registerDate) {
                string pwd = CMS.Utilities.Cryptographer.MD5Hash(password);

                string sql = "SELECT AccountId FROM vAccounts WHERE TVD_Id=@TVD_Id AND InstanceId = @InstanceId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@TVD_Id", accountTVDId), new SqlParameter("@InstanceId", instanceId));
                bool accountExist = dt.Rows.Count != 0;
                //INSERT
                if (!accountExist) {
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;

                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pAccountCreate",
                            new SqlParameter("@HistoryAccount", 1),
                            new SqlParameter("@InstanceId", instanceId),
                            new SqlParameter("@Login", login),
                            new SqlParameter("@Password", pwd),
                            new SqlParameter("@Enabled", enabled),
                            new SqlParameter("@Verified", 1),
                            new SqlParameter("@Roles", roles),
                            new SqlParameter("@Email", email),
                            result);

                    if (result.Value == DBNull.Value) return 0;


                    int accountId = Convert.ToInt32(result.Value);
                    sql = @"UPDATE tAccount SET TVD_Id=@TVD_Id, CanAccessIntensa=@CanAccessIntensa, HistoryStamp=ISNULL(@registerDate, HistoryStamp) WHERE AccountId=@AccountId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@AccountId", accountId),
                            new SqlParameter("@TVD_Id", accountTVDId),
                            new SqlParameter("@CanAccessIntensa", canAccessIntensa),
                            new SqlParameter("@registerDate", Null(registerDate)));

                    return accountId;
                } else {
                    int accountId = Convert.ToInt32(dt.Rows[0][0]);
                    sql = @"UPDATE tAccount SET Enabled=@Enabled, Email=@Email, CanAccessIntensa=@CanAccessIntensa/*, Password=@Password*/ WHERE AccountId=@AccountId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@Enabled", enabled),
                            new SqlParameter("@AccountId", accountId),
                            new SqlParameter("@Email", email),
                            new SqlParameter("@CanAccessIntensa", canAccessIntensa));
                    //new SqlParameter( "@Password", pwd ) );

                    return accountId;
                }
            }
            public static int SyncOrganization(CMS.Pump.MSSQLStorage mssqStorageDst, int accountId, int instanceId, string code, string email, string name, string ico, string dic, int parentId, bool platceDPH,
                    string street, string city, string psc, string state, string phone, string mobil,
                    string street_d, string city_d, string psc_d, string state_d,
                    string bankCode, string bankAccountNumber, int topManager, string fax, string icq, string skype, string workPhone, string personalCardId, string pf, DateTime? birthDay, string regionCode, decimal marze, int omezenyPristup, string statut,
                    bool angelTeamClen, bool angelTeamManager, int angelTeamManagerTyp) {

                string sql = "SELECT OrganizationId FROM tOrganization WHERE AccountId=@AccountId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@AccountId", accountId));
                bool orgExist = dt.Rows.Count != 0;
                //INSERT
                if (!orgExist) {
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;

                    mssqStorageDst.ExecProc(mssqStorageDst.Connection, "pOrganizationCreate",
                                    new SqlParameter("@HistoryAccount", 1),
                                    new SqlParameter("@InstanceId", instanceId),
                                    new SqlParameter("@AccountId", accountId),
                                    new SqlParameter("@Id1", ico),
                                    new SqlParameter("@Id2", dic),
                                    new SqlParameter("@Id3", string.Empty),
                                    new SqlParameter("@Name", name),
                                    new SqlParameter("@Notes", string.Empty),
                                    new SqlParameter("@Web", string.Empty),
                                    new SqlParameter("@ContactEmail", email),
                                    new SqlParameter("@ContactPhone", phone),
                                    new SqlParameter("@ContactMobile", mobil),
                                    new SqlParameter("@ParentId", parentId),
                                    new SqlParameter("@VATPayment", platceDPH),
                                    new SqlParameter("@Code", code),
                                    new SqlParameter("@TopManager", topManager),
                                    new SqlParameter("@FAX", Null(fax)),
                                    new SqlParameter("@Skype", Null(skype)),
                                    new SqlParameter("@ICQ", Null(icq)),
                                    new SqlParameter("@ContactBirthDay", Null(birthDay)),
                                    new SqlParameter("@ContactCardId", Null(personalCardId)),
                                    new SqlParameter("@ContactWorkPhone", Null(workPhone)),
                                    new SqlParameter("@PF", Null(pf)),
                                    new SqlParameter("@RegionCode", Null(regionCode)),
                                    new SqlParameter("@UserMargin", Null(marze)),
                                    new SqlParameter("@Statut", Null(statut)),
                                    result
                            );

                    int orgId = Convert.ToInt32(result.Value);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Register, street, city, psc, state);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Delivery, street_d, city_d, psc_d, state_d);
                    SyncBank(mssqStorageDst, orgId, bankCode, bankAccountNumber);

                    return orgId;
                } else {
                    int orgId = Convert.ToInt32(dt.Rows[0][0]);
                    sql = @"UPDATE tOrganization SET Code=@Code, Id1=@Id1, Id2=@Id2, Name=@Name, ContactEmail=@ContactEmail, ContactPhone=@ContactPhone, ContactMobile=@ContactMobile , ParentId=@ParentId, VATPayment=@VATPayment, 
					TopManager=@TopManager, UserMargin=@UserMargin, RestrictedAccess=@RestrictedAccess, Statut=@Statut, RegionCode=@RegionCode,
					Angel_team_clen=@Angel_team_clen, Angel_team_manager=@Angel_team_manager, Angel_team_manager_typ=@Angel_team_manager_typ
					WHERE OrganizationId=@OrganizationId";
                    mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                            new SqlParameter("@Id1", ico),
                            new SqlParameter("@Id2", dic),
                            new SqlParameter("@Name", name),
                            new SqlParameter("@ContactEmail", email),
                            new SqlParameter("@ContactPhone", phone),
                            new SqlParameter("@ContactMobile", mobil),
                            new SqlParameter("@ParentId", parentId),
                            new SqlParameter("@VATPayment", platceDPH),
                            new SqlParameter("@Code", code),
                            new SqlParameter("@OrganizationId", orgId),
                            new SqlParameter("@TopManager", topManager),
                            new SqlParameter("@UserMargin", Null(marze)),
                            new SqlParameter("@RestrictedAccess", Null(omezenyPristup)),
                            new SqlParameter("@Statut", Null(statut)),
                            new SqlParameter("@RegionCode", Null(regionCode)),
                            new SqlParameter("@Angel_team_clen", Null(angelTeamClen)),
                            new SqlParameter("@Angel_team_manager", Null(angelTeamManager)),
                            new SqlParameter("@Angel_team_manager_typ", Null(angelTeamManagerTyp))
                            );

                    SyncAddress(mssqStorageDst, orgId, AddressType.Register, street, city, psc, state);
                    SyncAddress(mssqStorageDst, orgId, AddressType.Delivery, street_d, city_d, psc_d, state_d);
                    SyncBank(mssqStorageDst, orgId, bankCode, bankAccountNumber);

                    return orgId;
                }
            }
            private static void SyncAddress(CMS.Pump.MSSQLStorage mssqStorageDst, int orgId, AddressType addressType, string street, string city, string psc, string state) {
                string sql = "SELECT RegisteredAddress, CorrespondenceAddress FROM tOrganization WHERE OrganizationId=@OrganizationId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@OrganizationId", orgId));

                int addressHomeId = Convert.ToInt32(dt.Rows[0]["RegisteredAddress"]);
                int addressTempId = Convert.ToInt32(dt.Rows[0]["CorrespondenceAddress"]);

                sql = @"UPDATE tAddress SET City=@City, Street=@Street, Zip=@Zip, State=@State
				WHERE AddressId=@AddressId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@City", city),
                        new SqlParameter("@Street", street),
                        new SqlParameter("@Zip", psc),
                        new SqlParameter("@State", state),
                        new SqlParameter("@AddressId", addressType == AddressType.Register ? addressHomeId : addressTempId));
            }

            private static void SyncBank(CMS.Pump.MSSQLStorage mssqStorageDst, int orgId, string kod, string cisloUctu) {
                string sql = "SELECT BankContact FROM tOrganization WHERE OrganizationId=@OrganizationId";
                DataTable dt = mssqStorageDst.Query(mssqStorageDst.Connection, sql, new SqlParameter("@OrganizationId", orgId));

                int bankContact = Convert.ToInt32(dt.Rows[0]["BankContact"]);

                sql = @"UPDATE tBankContact SET BankCode=@BankCode, AccountNumber=@AccountNumber
				WHERE BankContactId=@BankContactId";
                mssqStorageDst.Exec(mssqStorageDst.Connection, sql,
                        new SqlParameter("@BankCode", kod),
                        new SqlParameter("@AccountNumber", cisloUctu),
                        new SqlParameter("@BankContactId", bankContact));
            }
        }

        public static class BonusoveKredityUzivatele {
            /// <summary>
            /// Vráti ID accountu z CMS podla jeho TVD_ID
            /// </summary>
            public static DataTable GetBonusoveKredityUzivatele(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId) {
                string sql = @"SELECT YYYYMM = (YEAR(bk.Datum)*100+MONTH(bk.Datum)), Kredit=SUM(bk.Hodnota), bk.TVD_Id
								FROM vBonusoveKredityUzivatele bk
								INNER JOIN tAccount a ON a.AccountId = bk.AccountId
								WHERE a.InstanceId = @instanceId AND bk.TVD_Id IS NOT NULL
								GROUP BY bk.TVD_Id, (YEAR(bk.Datum)*100+MONTH(bk.Datum))";
                DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql, new SqlParameter("@instanceId", instanceId));
                return dt;
            }

            public static int GetBonusovyKredit(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId, int bkTyp) {
                string sql = @"SELECT TOP 1 [BonusovyKreditId] FROM [tBonusovyKredit] WHERE InstanceId=@instanceId AND [Typ]=@bkTyp";
                using (SqlConnection connectionSrc = mssqStorageSrc.Connect()) {
                    DataTable dt = mssqStorageSrc.Query(connectionSrc, sql, new SqlParameter("@instanceId", instanceId), new SqlParameter("@bkTyp", bkTyp));
                    if (dt == null || dt.Rows.Count == 0) return 0;
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
            }

            public static void InsertBonusovyKredityUzivatele(CMS.Pump.MSSQLStorage mssqStorageSrc, int bonusovyKreditId, int accountId, DateTime platnostOd, DateTime platnostDo, int narok) {
                string sql = @"
				IF EXISTS(SELECT AccountId, BonusovyKreditId  FROM tBonusovyKreditUzivatele WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo ) BEGIN
					UPDATE tBonusovyKreditUzivatele SET Hodnota=@narok WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo AND [Hodnota] <> @narok
				END
				ELSE
				BEGIN
					INSERT INTO tBonusovyKreditUzivatele ( [AccountId] ,[BonusovyKreditId] ,[Datum] ,[PlatnostOd] ,[PLatnostDo] ,[Kod] ,[Hodnota] ,[Poznamka]) 
                    VALUES 
                    (@accountId, @bonusovyKreditId, GETDATE(), @platnostOd, @platnostDo, '', @narok, '')
				END";
                using (SqlConnection connectionDst = mssqStorageSrc.Connect()) {
                    mssqStorageSrc.Exec(connectionDst, sql,
                    new SqlParameter("@accountId", accountId),
                    new SqlParameter("@bonusovyKreditId", bonusovyKreditId),
                    new SqlParameter("@platnostOd", platnostOd),
                    new SqlParameter("@platnostDo", platnostDo),
                    new SqlParameter("@narok", narok));
                }

                /*
                string sqlDelete = @"DELETE FROM tBonusovyKreditUzivatele WHERE [AccountId]=@accountId AND [BonusovyKreditId]=@bonusovyKreditId AND [PlatnostOd]=@platnostOd AND [PLatnostDo]=@platnostDo";
                mssqStorageSrc.Exec(mssqStorageSrc.Connection, sqlDelete);

                string sql = @"INSERT INTO tBonusovyKreditUzivatele ( [AccountId] ,[BonusovyKreditId] ,[Datum] ,[PlatnostOd] ,[PLatnostDo] ,[Kod] ,[Hodnota] ,[Poznamka]) 
                VALUES 
                (@accountId, @bonusovyKreditId, GETDATE(), @platnostOd, @platnostDo, '', @narok, '')";
                DataTable dt = mssqStorageSrc.Query(mssqStorageSrc.Connection, sql,
                    new SqlParameter("@accountId", accountId),
                    new SqlParameter("@bonusovyKreditId", bonusovyKreditId),
                    new SqlParameter("@platnostOd", platnostOd),
                    new SqlParameter("@platnostDo", platnostDo),
                    new SqlParameter("@narok", narok));
                return dt;
                 * */
            }
        }

        #region Helpers methods
        public static string GetAliasName(string name) {
            name = name.ToLower().Trim();
            string alias = CMS.Utilities.StringUtilities.RemoveDiacritics(name);
            alias = alias.Replace("\n", "");
            alias = alias.Replace("\r", "");
            alias = alias.Replace("%", "");
            alias = alias.Replace(",", ""); alias = alias.Replace("?", "");
            alias = alias.Replace(";", ""); alias = alias.Replace("&", "-and-");
            alias = alias.Replace(" ", "-");
            alias = alias.Replace("----", "-");
            alias = alias.Replace("---", "-");
            alias = alias.Replace("--", "-");
            alias = alias.Replace("+", "plus");
            return alias;
        }
        private static object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        private static object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        private static object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        private static object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }
        #endregion
    }
}
