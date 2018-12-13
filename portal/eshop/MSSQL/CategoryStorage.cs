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
    public sealed class CategoryStorage : MSSQLStorage<Category> {
        public CategoryStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Category GetCategory(DataRow record) {
            Category category = new Category();
            category.Id = Convert.ToInt32(record["CategoryId"]);
            category.Order = ConvertNullable.ToInt32(record["Order"]);
            category.InstanceId = Convert.ToInt32(record["InstanceId"]);
            category.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            category.Name = Convert.ToString(record["Name"]);
            category.Locale = Convert.ToString(record["Locale"]);
            category.Icon = Convert.ToString(record["Icon"]);
            category.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            category.Alias = Convert.ToString(record["Alias"]);

            return category;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<Category> Read(object criteria) {
            if (criteria is Category.ReadById) return LoadById(criteria as Category.ReadById);
            if (criteria is Category.ReadByParentId) return LoadByParentId(criteria as Category.ReadByParentId);
            if (criteria is Category.ReadByProductId) return LoadByProductId(criteria as Category.ReadByProductId);
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
								FROM vShpCategories
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Category> LoadById(Category.ReadById byCategoryId) {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
								FROM vShpCategories
								WHERE CategoryId = @CategoryId
								ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CategoryId", byCategoryId.CategoryId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        private List<Category> LoadByParentId(Category.ReadByParentId byParentId) {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
								FROM vShpCategories
								WHERE InstanceId=@InstanceId AND Locale = @Locale AND ( ( @ParentId IS NULL AND ParentId IS NULL ) OR ( ParentId = @ParentId ) )
								ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ParentId", Null(byParentId.ParentId)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        private List<Category> LoadByProductId(Category.ReadByProductId by) {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT c.CategoryId, c.[Order], c.InstanceId, c.ParentId, c.[Name], c.Locale, c.Icon, c.UrlAliasId, c.Alias
								FROM vShpCategories c 
								INNER JOIN tShpProductCategories pc ON pc.CategoryId=c.CategoryId
								WHERE c.InstanceId=@InstanceId AND pc.ProductId=ProductId
								ORDER BY c.[Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ProductId", Null(by.ProductId)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        public override void Create(Category category) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpCategoryCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Order", Null(category.Order)),
                        new SqlParameter("@ParentId", Null(category.ParentId)),
                        new SqlParameter("@Name", category.Name),
                        new SqlParameter("@UrlAliasId", Null(category.UrlAliasId)),
                        new SqlParameter("@Locale", String.IsNullOrEmpty(category.Locale) ? Locale : category.Locale),
                        new SqlParameter("@Icon", Null(category.Icon)),
                        result);

                category.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(Category category) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpCategoryModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@CategoryId", category.Id),
                        new SqlParameter("@Order", Null(category.Order)),
                        new SqlParameter("@ParentId", Null(category.ParentId)),
                        new SqlParameter("@Name", category.Name),
                        new SqlParameter("@UrlAliasId", Null(category.UrlAliasId)),
                        new SqlParameter("@Locale", category.Locale),
                        new SqlParameter("@Icon", Null(category.Icon)));
            }
        }

        public override void Delete(Category category) {
            using (SqlConnection connection = Connect()) {
                DeleteChilds(category.Id);
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter blogId = new SqlParameter("@CategoryId", category.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpCategoryDelete", result, historyAccount, blogId);
            }
        }

        private void DeleteChilds(int parentId) {
            List<Category> list = LoadByParentId(new Category.ReadByParentId { ParentId = parentId });
            foreach (Category category in list) {
                DeleteChilds(category.Id);
                using (SqlConnection connection = Connect()) {
                    SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                    SqlParameter blogId = new SqlParameter("@CategoryId", category.Id);
                    SqlParameter result = new SqlParameter("@Result", -1);
                    result.Direction = ParameterDirection.Output;
                    ExecProc(connection, "pShpCategoryDelete", result, historyAccount, blogId);
                }
            }
        }
    }
}
