using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CMS.MSSQL {
    [Serializable]
    internal class MasterPageStorage : MSSQLStorage<MasterPage> {
        public MasterPageStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static MasterPage GetMasterPage(DataRow record) {
            MasterPage page = new MasterPage();
            page.Id = Convert.ToInt32(record["MasterPageId"]);
            page.Default = Convert.ToBoolean(record["Default"]);
            page.InstanceId = Convert.ToInt32(record["InstanceId"]);
            page.Name = Convert.ToString(record["Name"]);
            page.Description = Convert.ToString(record["Description"]);
            page.Url = Convert.ToString(record["Url"]);
            page.Contents = Convert.ToInt32(record["Contents"]);
            page.PageUrl = Convert.ToString(record["PageUrl"]);
            return page;
        }

        public override void Create(MasterPage entity) {
            throw new NotImplementedException();
        }

        public override List<MasterPage> Read(object criteria) {
            List<MasterPage> list = new List<MasterPage>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT [MasterPageId], [Default], InstanceId, [Name], [Description], [Url], [Contents], [PageUrl]
								FROM vMasterPages";
                SqlParameter[] @params = null;
                if (criteria is MasterPage.ReadById) {
                    @params = new SqlParameter[] { new SqlParameter("@MasterPageId", (criteria as MasterPage.ReadById).MasterPageId),
										new SqlParameter("@InstanceId", InstanceId)};
                    sql += " WHERE MasterPageId = @MasterPageId";
                } else {
                    @params = new SqlParameter[] { new SqlParameter("@InstanceId", InstanceId) };
                    sql += " WHERE InstanceId=@InstanceId";
                }
                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMasterPage(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Update(MasterPage entity) {
            throw new NotImplementedException();
        }

        public override void Delete(MasterPage entity) {
            throw new NotImplementedException();
        }
    }
}
