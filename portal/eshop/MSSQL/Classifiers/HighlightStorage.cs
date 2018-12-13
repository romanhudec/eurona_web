using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities.Classifiers;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;

namespace SHP.MSSQL.Classifiers {
    [Serializable]
    public sealed class HighlightStorage : MSSQLStorage<Highlight> {
        public HighlightStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Highlight GetHighlight(DataRow record) {
            Highlight highlight = new Highlight();
            highlight.Id = Convert.ToInt32(record["HighlightId"]);
            highlight.InstanceId = Convert.ToInt32(record["InstanceId"]);
            highlight.Name = Convert.ToString(record["Name"]);
            highlight.Code = Convert.ToString(record["Code"]);
            highlight.Icon = Convert.ToString(record["Icon"]);
            highlight.Locale = Convert.ToString(record["Locale"]);
            highlight.Notes = Convert.ToString(record["Notes"]);

            return highlight;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<Highlight> Read(object criteria) {
            if (criteria is Highlight.ReadById) return LoadById(criteria as Highlight.ReadById);
            if (criteria is Highlight.ReadByCode) return LoadByCode(criteria as Highlight.ReadByCode);
            List<Highlight> blogs = new List<Highlight>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT HighlightId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpHighlights
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    blogs.Add(GetHighlight(dr));
            }
            return blogs;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Highlight> LoadById(Highlight.ReadById byHighlightId) {
            List<Highlight> blogs = new List<Highlight>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT HighlightId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpHighlights
								WHERE HighlightId = @HighlightId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@HighlightId", byHighlightId.Id));
                foreach (DataRow dr in table.Rows)
                    blogs.Add(GetHighlight(dr));
            }
            return blogs;
        }

        private List<Highlight> LoadByCode(Highlight.ReadByCode by) {
            List<Highlight> list = new List<Highlight>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT HighlightId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes]
								FROM vShpHighlights
								WHERE Code = @Code AND Locale=@Locale AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Code", by.Code),
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));

                foreach (DataRow dr in table.Rows)
                    list.Add(GetHighlight(dr));
            }
            return list;
        }


        public override void Create(Highlight highlight) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpHighlightCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Name", highlight.Name),
                        new SqlParameter("@Code", highlight.Code),
                        new SqlParameter("@Icon", highlight.Icon),
                        new SqlParameter("@Notes", highlight.Notes),
                        new SqlParameter("@Locale", String.IsNullOrEmpty(highlight.Locale) ? Locale : highlight.Locale),
                        result);

                highlight.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(Highlight highlight) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpHighlightModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@HighlightId", highlight.Id),
                        new SqlParameter("@Name", highlight.Name),
                        new SqlParameter("@Code", highlight.Code),
                        new SqlParameter("@Icon", highlight.Icon),
                        new SqlParameter("@Notes", highlight.Notes),
                        new SqlParameter("@Locale", highlight.Locale));
            }
        }

        public override void Delete(Highlight highlight) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter blogId = new SqlParameter("@HighlightId", highlight.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpHighlightDelete", result, historyAccount, blogId);
            }
        }
    }
}
