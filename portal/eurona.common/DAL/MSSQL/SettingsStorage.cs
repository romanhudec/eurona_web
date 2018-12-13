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
    public sealed class SettingsStorage : MSSQLStorage<Settings> {
        private string entitySelect = @"SELECT * FROM tSettings";
        public SettingsStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Settings GetSettings(DataRow record) {
            Settings entiry = new Settings();
            entiry.Id = Convert.ToInt32(record["SettingsId"]);
            entiry.Name = Convert.ToString(record["Name"]);
            entiry.Code = Convert.ToString(record["Code"]);
            entiry.GroupName = Convert.ToString(record["GroupName"]);
            entiry.Value = Convert.ToString(record["Value"]);

            return entiry;
        }

        public override List<Settings> Read(object criteria) {

            if (criteria is Settings.ReadById) return LoadById((criteria as Settings.ReadById).Id);
            if (criteria is Settings.ReadByCode) return LoadByCode((criteria as Settings.ReadByCode).Code);

            SqlParameter[] @params = null;
            @params = new SqlParameter[] { new SqlParameter("@InstanceId", this.InstanceId) };
            string sql = entitySelect + " WHERE InstanceId=0 OR InstanceId=@InstanceId";
            List<Settings> list = new List<Settings>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetSettings(dr));

            }
            return list;
        }

        public List<Settings> LoadById(int id) {
            List<Settings> list = new List<Settings>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE SettingsId = @Id";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Id", id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetSettings(dr));
            }
            return list;
        }

        public List<Settings> LoadByCode(string code) {
            List<Settings> list = new List<Settings>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Code = @Code";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Code", code));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetSettings(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(Settings entity) {
            throw new NotImplementedException();
        }

        public override void Update(Settings entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "UPDATE tSettings SET [Value]=@Value WHERE SettingsId=@id",
                    new SqlParameter("@Id", entity.Id),
                    new SqlParameter("@Value", Null(entity.Value))
                );
            }
        }

        public override void Delete(Settings entity) {
            throw new NotImplementedException();
        }
    }
}
