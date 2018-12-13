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
    public sealed class OrderSettingsStorage : MSSQLStorage<OrderSettings> {
        private string entitySelect = @"SELECT * FROM tShpOrderSettings";
        public OrderSettingsStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static OrderSettings GetSettings(DataRow record) {
            OrderSettings entiry = new OrderSettings();
            entiry.Id = Convert.ToInt32(record["Id"]);
            entiry.Code = Convert.ToString(record["Code"]);
            entiry.Enabled = Convert.ToBoolean(record["Enabled"]);
            entiry.Locale = Convert.ToString(record["Locale"]);
            entiry.Value = Convert.ToDecimal(record["Value"]);

            return entiry;
        }

        public override List<OrderSettings> Read(object criteria) {

            if (criteria is OrderSettings.ReadById) return LoadById((criteria as OrderSettings.ReadById).Id);
            if (criteria is OrderSettings.ReadByCode) return LoadByCode((criteria as OrderSettings.ReadByCode).Code);

            SqlParameter[] @params = null;
            @params = new SqlParameter[] { new SqlParameter("@InstanceId", this.InstanceId) };
            string sql = entitySelect + " WHERE InstanceId=0 OR InstanceId=@InstanceId";
            List<OrderSettings> list = new List<OrderSettings>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetSettings(dr));

            }
            return list;
        }

        public List<OrderSettings> LoadById(int id) {
            List<OrderSettings> list = new List<OrderSettings>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Id = @Id";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Id", id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetSettings(dr));
            }
            return list;
        }

        public List<OrderSettings> LoadByCode(string code) {
            List<OrderSettings> list = new List<OrderSettings>();
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

        public override void Create(OrderSettings entity) {
            throw new NotImplementedException();
        }

        public override void Update(OrderSettings entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "UPDATE tShpOrderSettings SET [Value]=@Value, [Enabled]=@Enabled WHERE Id=@id",
                    new SqlParameter("@Id", entity.Id),
                    new SqlParameter("@Value", Null(entity.Value)),
                    new SqlParameter("@Enabled", Null(entity.Enabled))
                );
            }
        }

        public override void Delete(OrderSettings entity) {
            throw new NotImplementedException();
        }
    }
}
