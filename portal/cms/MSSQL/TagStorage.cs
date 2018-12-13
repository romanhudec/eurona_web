using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class TagStorage : MSSQLStorage<Tag> {
        public TagStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Tag GetTag(DataRow record) {
            Tag tag = new Tag();
            tag.Id = Convert.ToInt32(record["TagId"]);
            tag.InstanceId = Convert.ToInt32(record["InstanceId"]);
            tag.Name = Convert.ToString(record["Tag"]);
            return tag;
        }

        public override List<Tag> Read(object criteria) {
            if (criteria is Tag.ReadById) return LoadById(criteria as Tag.ReadById);
            List<Tag> list = new List<Tag>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT TagId, InstanceId, Tag
								FROM vTags WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetTag(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Tag> LoadById(Tag.ReadById byTagId) {
            List<Tag> list = new List<Tag>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT TagId, InstanceId, Tag
								FROM vTags
								WHERE TagId = @TagId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@TagId", byTagId.TagId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetTag(dr));
            }
            return list;
        }

        public override void Create(Tag tag) {
            if (string.IsNullOrEmpty(tag.Name))
                return;

            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pTagCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Tag", Null(tag.Name)));
            }
        }

        public override void Update(Tag tag) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pTagModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@TagId", tag.Id),
                        new SqlParameter("@Tag", Null(tag.Name))
                        );
            }
        }

        public override void Delete(Tag tag) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter id = new SqlParameter("@TagId", tag.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pTagDelete", result, historyAccount, id);
            }
        }

    }
}
