using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;

namespace Eurona.Common.DAL.MSSQL
{
    public sealed class CategoryStorage : MSSQLStorage<Category>
    {
        public CategoryStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private static Category GetCategory(DataRow record)
        {
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

        private static bool NullableDBToBool(object dbValue)
        {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<Category> Read(object criteria)
        {
            if (criteria is Category.ReadByInstance) return LoadByInstance(criteria as Category.ReadByInstance);
            if (criteria is Category.ReadById) return LoadById(criteria as Category.ReadById);
            if (criteria is Category.ReadByParentId) return LoadByParentId(criteria as Category.ReadByParentId);
            if (criteria is Category.ReadByProductId) return LoadByProductId(criteria as Category.ReadByProductId);
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
					SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
					FROM vShpCategories
					WHERE Locale = @Locale AND ( InstanceId=0 OR InstanceId=@InstanceId )
					ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                new SqlParameter("@Locale", Locale),
                new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }

        private List<Category> LoadByInstance(Category.ReadByInstance by)
        {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
				FROM vShpCategories
				WHERE  ( InstanceId=0 OR InstanceId=@InstanceId ) AND Locale=@Locale
				ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                new SqlParameter("@InstanceId", by.InstanceId),
                new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        private List<Category> LoadById(Category.ReadById byCategoryId)
        {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
				FROM vShpCategories
				WHERE CategoryId = @CategoryId AND Locale=@Locale
				ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                new SqlParameter("@CategoryId", byCategoryId.CategoryId),
                new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        private List<Category> LoadByParentId(Category.ReadByParentId byParentId)
        {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				SELECT CategoryId, [Order], InstanceId, ParentId, [Name], Locale, Icon, UrlAliasId, Alias
				FROM vShpCategories
				WHERE ( InstanceId=0 OR InstanceId=@InstanceId ) AND ( ( @ParentId IS NULL AND ParentId IS NULL ) OR ( ParentId = @ParentId ) ) AND Locale=@Locale
				ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                new SqlParameter("@ParentId", Null(byParentId.ParentId)),
                new SqlParameter("@InstanceId", byParentId.InstanceId.HasValue ? byParentId.InstanceId.Value : InstanceId),
                new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        private List<Category> LoadByProductId(Category.ReadByProductId by)
        {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				SELECT c.CategoryId, c.[Order], c.InstanceId, c.ParentId, c.[Name], c.Locale, c.Icon, c.UrlAliasId, c.Alias
				FROM vShpCategories c 
				INNER JOIN tShpProductCategories pc ON pc.CategoryId=c.CategoryId
				WHERE ( c.InstanceId=0 OR c.InstanceId=@InstanceId ) AND pc.ProductId=ProductId AND Locale=@Locale
				ORDER BY c.[Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                new SqlParameter("@ProductId", Null(by.ProductId)),
                new SqlParameter("@InstanceId", InstanceId),
                new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetCategory(dr));
            }
            return list;
        }

        public override void Create(Category category)
        {
            throw new NotImplementedException();
        }

        public override void Update(Category category)
        {
            //throw new NotImplementedException();
            using (SqlConnection connection = Connect())
            {
                Exec(connection, "UPDATE tShpCategory SET [Order]=@Order WHERE CategoryId=@CategoryId",
                new SqlParameter("@CategoryId", category.Id),
                new SqlParameter("@Order", Null(category.Order)));
            }
        }

        public override void Delete(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
