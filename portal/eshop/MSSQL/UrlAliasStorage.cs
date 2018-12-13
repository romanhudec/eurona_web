using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMSUrlAlias = CMS.Entities.UrlAlias;

namespace SHP.MSSQL {
    [Serializable]
    internal sealed class UrlAliasStorage : CMS.MSSQL.UrlAliasStorage, CMS.IStorage<UrlAlias> {
        public UrlAliasStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        public static new UrlAlias GetUrlAlias(DataRow record) {
            UrlAlias urlAlias = new UrlAlias();
            urlAlias.Id = Convert.ToInt32(record["UrlAliasId"]);
            urlAlias.InstanceId = Convert.ToInt32(record["InstanceId"]);
            urlAlias.Url = Convert.ToString(record["Url"]);
            urlAlias.Alias = Convert.ToString(record["Alias"]);
            urlAlias.Name = Convert.ToString(record["Name"]);
            urlAlias.Locale = Convert.ToString(record["Locale"]);
            return urlAlias;
        }

        public new List<UrlAlias> Read(object criteria) {
            if ((criteria is UrlAlias.ReadByAliasType.Categories) == false &&
                     (criteria is UrlAlias.ReadByAliasType.Products) == false)
                return new List<UrlAlias>();

            List<UrlAlias> list = new List<UrlAlias>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
												SELECT a.UrlAliasId, a.InstanceId, a.Url, a.Alias, a.Locale, a.[Name]
												FROM vUrlAliases a";
                SqlParameter[] @params = null;

                @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                if (criteria is UrlAlias.ReadByAliasType.Categories)
                    sql += " INNER JOIN vShpCategories x ON x.UrlAliasId = a.UrlAliasId";
                else if (criteria is UrlAlias.ReadByAliasType.Products)
                    sql += " INNER JOIN vShpProducts x ON x.UrlAliasId = a.UrlAliasId";

                sql += " WHERE a.Locale = @Locale AND a.InstanceId=@InstanceId";

                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetUrlAlias(dr));
            }
            return list;
        }
        #region IStorage<UrlAlias> Members


        public void Create(UrlAlias entity) {
            base.Create((CMSUrlAlias)entity);
        }

        public void Update(UrlAlias entity) {
            base.Update((CMSUrlAlias)entity);
        }

        public void Delete(UrlAlias entity) {
            base.Delete((CMSUrlAlias)entity);
        }

        #endregion
    }
}
