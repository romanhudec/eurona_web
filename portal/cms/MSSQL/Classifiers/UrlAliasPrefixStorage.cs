using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using CMS.Entities.Classifiers;

namespace CMS.MSSQL.Classifiers {
    [Serializable]
    internal sealed class UrlAliasPrefixStorage : MSSQLStorage<UrlAliasPrefix> {
        public UrlAliasPrefixStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static UrlAliasPrefix GetUrlAliasPrefix(DataRow record) {
            UrlAliasPrefix urlAliasPrefix = new UrlAliasPrefix();
            urlAliasPrefix.Id = Convert.ToInt32(record["UrlAliasPrefixId"]);
            urlAliasPrefix.InstanceId = Convert.ToInt32(record["InstanceId"]);
            urlAliasPrefix.Name = Convert.ToString(record["Name"]);
            urlAliasPrefix.Code = Convert.ToString(record["Code"]);
            urlAliasPrefix.Locale = Convert.ToString(record["Locale"]);
            urlAliasPrefix.Notes = Convert.ToString(record["Notes"]);

            return urlAliasPrefix;
        }

        public override List<UrlAliasPrefix> Read(object criteria) {
            if (criteria is UrlAliasPrefix.ReadById) return LoadById(criteria as UrlAliasPrefix.ReadById);
            List<UrlAliasPrefix> list = new List<UrlAliasPrefix>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT UrlAliasPrefixId, InstanceId, [Name], [Code], [Locale], [Notes]
								FROM vUrlAliasPrefixes
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetUrlAliasPrefix(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<UrlAliasPrefix> LoadById(UrlAliasPrefix.ReadById by) {
            List<UrlAliasPrefix> list = new List<UrlAliasPrefix>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT UrlAliasPrefixId, InstanceId, [Name], [Code], [Locale], [Notes]
								FROM vUrlAliasPrefixes
								WHERE UrlAliasPrefixId = @UrlAliasPrefixId
								ORDER BY [Name] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@UrlAliasPrefixId", by.Id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetUrlAliasPrefix(dr));
            }
            return list;
        }

        public override void Create(UrlAliasPrefix entity) {
            throw new NotImplementedException();
        }

        public override void Update(UrlAliasPrefix urlAliasPrefix) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pUrlAliasPrefixModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@UrlAliasPrefixId", urlAliasPrefix.Id),
                        new SqlParameter("@Name", urlAliasPrefix.Name),
                        new SqlParameter("@Notes", urlAliasPrefix.Notes));
            }
        }

        public override void Delete(UrlAliasPrefix entity) {
            throw new NotImplementedException();
        }
    }
}
