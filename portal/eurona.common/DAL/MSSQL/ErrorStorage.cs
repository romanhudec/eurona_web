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
    public sealed class ErrorStorage : MSSQLStorage<Error> {
        private string entitySelect = @"SELECT e.*, Name = ISNULL(o.Name, a.Login ), o.Code FROM tError e
			LEFT JOIN tAccount a ON a.AccountId = e.AccountId
			LEFT JOIN tOrganization o ON o.AccountId = e.AccountId";
        public ErrorStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Error GetError(DataRow record) {
            Error entiry = new Error();
            entiry.Id = Convert.ToInt32(record["Id"]);
            entiry.AccountId = Convert.ToInt32(record["AccountId"]);
            entiry.Stamp = Convert.ToDateTime(record["Stamp"]);
            entiry.InstanceId = Convert.ToInt32(record["InstanceId"]);
            entiry.Exception = Convert.ToString(record["Exception"]);
            entiry.StackTrace = Convert.ToString(record["StackTrace"]);
            entiry.Location = Convert.ToString(record["Location"]);
            entiry.Name = Convert.ToString(record["Name"]);
            entiry.Code = Convert.ToString(record["Code"]);
            return entiry;
        }

        public override List<Error> Read(object criteria) {

            if (criteria is Error.ReadById) return LoadById((criteria as Error.ReadById).Id);

            SqlParameter[] @params = null;
            @params = new SqlParameter[] { new SqlParameter("@InstanceId", this.InstanceId) };
            string sql = entitySelect + " WHERE e.InstanceId=0 OR e.InstanceId=@InstanceId ORDER BY e.Stamp DESC";
            List<Error> list = new List<Error>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));

            }
            return list;
        }

        public List<Error> LoadById(int id) {
            List<Error> list = new List<Error>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE e.Id = @Id";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Id", id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(Error entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "INSERT INTO tError (AccountId, Stamp, Location, InstanceId, Exception, StackTrace) VALUES (@AccountId, GETDATE(), @Location, @InstanceId, @Exception, @StackTrace)",
                        new SqlParameter("@AccountId", entity.AccountId),
                        new SqlParameter("@InstanceId", this.InstanceId),
                        new SqlParameter("@Exception", Null(entity.Exception)),
                        new SqlParameter("@StackTrace", Null(entity.StackTrace)),
                        new SqlParameter("@Location", Null(entity.Location))
                        );
            }
        }

        public override void Update(Error entity) {
            throw new NotImplementedException();
        }

        public override void Delete(Error entity) {
            throw new NotImplementedException();
        }
    }
}
