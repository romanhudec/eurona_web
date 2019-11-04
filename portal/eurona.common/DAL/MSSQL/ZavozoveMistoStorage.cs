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
    public sealed class ZavozoveMistoStorage : MSSQLStorage<ZavozoveMisto> {
        private string entitySelect = @"SELECT * FROM tShpZavozoveMisto";
        public ZavozoveMistoStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ZavozoveMisto GetError(DataRow record) {
            ZavozoveMisto entiry = new ZavozoveMisto();
            entiry.Id = Convert.ToInt32(record["ZavozoveMistoId"]);
            entiry.Mesto = Convert.ToString(record["Mesto"]);
            entiry.DatumACas = Convert.ToDateTime(record["DatumACas"]);
            return entiry;
        }

        public override List<ZavozoveMisto> Read(object criteria) {
            if (criteria is ZavozoveMisto.ReadById) return LoadById((criteria as ZavozoveMisto.ReadById).Id);
            if (criteria is ZavozoveMisto.ReadByMesto) return LoadByMesto((criteria as ZavozoveMisto.ReadByMesto).Mesto);
            if (criteria is ZavozoveMisto.ReadJenAktualiByMesto) return LoadJenAktualiByMesto((criteria as ZavozoveMisto.ReadJenAktualiByMesto).Mesto);
            if (criteria is ZavozoveMisto.ReadOnlyMestoDistinct) return LoadOnlyMestoDistinct();

            SqlParameter[] @params = null;
            string sql = entitySelect + " ORDER BY DatumACas DESC";
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));

            }
            return list;
        }

        public List<ZavozoveMisto> LoadById(int id) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ZavozoveMistoId = @Id";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Id", id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public List<ZavozoveMisto> LoadOnlyMestoDistinct() {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = "SELECT DISTINCT  Mesto, DatumACas=GETDATE(), ZavozoveMistoId=0 FROM tShpZavozoveMisto ORDER BY Mesto ASC";
                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }
        public List<ZavozoveMisto> LoadByMesto(string mesto) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Mesto = @Mesto";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Mesto", mesto));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }
        public List<ZavozoveMisto> LoadJenAktualiByMesto(string mesto) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Mesto = @Mesto AND DatumACas > GETDATE()";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Mesto", mesto));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(ZavozoveMisto entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "INSERT INTO tShpZavozoveMisto (Mesto, DatumACas) VALUES (@Mesto, @DatumACas)",
                        new SqlParameter("@Mesto", entity.Mesto),
                        new SqlParameter("@DatumACas", entity.DatumACas)
                        );
            }
        }

        public override void Update(ZavozoveMisto entity) {
            throw new NotImplementedException();
        }

        public override void Delete(ZavozoveMisto entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "DELETE FROM tShpZavozoveMisto WHERE ZavozoveMistoId=@Id", new SqlParameter("@Id", entity.Id));
            }
        }
    }
}
