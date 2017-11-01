using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Eurona.DAL.Entities;
using CMS.MSSQL;

namespace Eurona.DAL.MSSQL {
    internal sealed class SingleUserCookieLinkActivityStorage : MSSQLStorage<SingleUserCookieLinkActivity> {
        public SingleUserCookieLinkActivityStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private SingleUserCookieLinkActivity GetItem(DataRow record) {
            SingleUserCookieLinkActivity entity = new SingleUserCookieLinkActivity();
            entity.Id = Convert.ToInt32(record["Id"]);
            entity.Url = record["Url"].ToString();
            entity.UrlTimestamp = Convert.ToDateTime(record["UrlTimestamp"]);
            entity.IPAddress = record["IPAddress"].ToString();
            entity.CookieAccountId = Convert.ToInt32(record["CookieAccountId"]);
            entity.RegistrationAccountId = ConvertNullable.ToInt32(record["RegistrationAccountId"]);
            entity.RegistrationTimestamp = ConvertNullable.ToDateTime(record["RegistrationTimestamp"]);
            return entity;
        }

        public override List<SingleUserCookieLinkActivity> Read(object criteria) {
            List<SingleUserCookieLinkActivity> list = new List<SingleUserCookieLinkActivity>();
            if (criteria is SingleUserCookieLinkActivity.ReadBy) {
                return LoadBy((SingleUserCookieLinkActivity.ReadBy)criteria);
            }
            
            string sql = @"SELECT * from tSingleUserCookieLinkActivity";
            using (SqlConnection connection = Connect()) {
                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows) list.Add(GetItem(dr));
            }           
            return list;
        }

        private List<SingleUserCookieLinkActivity> LoadBy(SingleUserCookieLinkActivity.ReadBy by) {
            List<SingleUserCookieLinkActivity> list = new List<SingleUserCookieLinkActivity>();
            using (SqlConnection connection = Connect()) {
                string sql = @"SELECT * from tSingleUserCookieLinkActivity";
                sql += " WHERE (CookieAccountId IS NULL OR CookieAccountId=@CookieAccountId) AND IPAddress = @IPAddress";
                sql += " ORDER BY UrlTimestamp DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CookieAccountId", Null(by.CookieAccountId)),
                        new SqlParameter("@IPAddress", by.IPAddress));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetItem(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        public override void Create(SingleUserCookieLinkActivity entity) {
            using (SqlConnection connection = Connect()) {
                string cmd = @"INSERT INTO tSingleUserCookieLinkActivity 
                (Url, IPAddress, UrlTimestamp, CookieAccountId, RegistrationAccountId, RegistrationTimestamp, Timestamp) 
                VALUES 
                (@Url, @IPAddress, @UrlTimestamp, @CookieAccountId, @RegistrationAccountId, @RegistrationTimestamp, GETDATE())";
                Exec(connection, cmd,
                    new SqlParameter("@Url", entity.Url),
                    new SqlParameter("@IPAddress", entity.IPAddress),
                    new SqlParameter("@UrlTimestamp", entity.UrlTimestamp),
                    new SqlParameter("@CookieAccountId", entity.CookieAccountId),
                    new SqlParameter("@RegistrationAccountId", Null(entity.RegistrationAccountId)),
                    new SqlParameter("@RegistrationTimestamp", Null(entity.RegistrationTimestamp)));
            }
        }

        public override void Update(SingleUserCookieLinkActivity entity) {
            using (SqlConnection connection = Connect()) {
                string cmd = @"UPDATE tSingleUserCookieLinkActivity 
                SET Url=@Url, IPAddress=@IPAddress, UrlTimestamp=@UrlTimestamp, CookieAccountId=@CookieAccountId, RegistrationAccountId=@RegistrationAccountId, RegistrationTimestamp=@RegistrationTimestamp, Timestamp=GETDATE() WHERE Id=@Id";
                Exec(connection, cmd,
                    new SqlParameter("@Id", entity.Id),
                    new SqlParameter("@Url", entity.Url),
                    new SqlParameter("@IPAddress", entity.IPAddress),
                    new SqlParameter("@UrlTimestamp", entity.UrlTimestamp),
                    new SqlParameter("@CookieAccountId", entity.CookieAccountId),
                    new SqlParameter("@RegistrationAccountId", Null(entity.RegistrationAccountId)),
                    new SqlParameter("@RegistrationTimestamp", Null(entity.RegistrationTimestamp)));
            }
        }

        public override void Delete(SingleUserCookieLinkActivity entity) {
            using (SqlConnection connection = Connect()) {
                string cmd = "DELETE FROM tSingleUserCookieLinkActivity WHERE Id=@Id";
                Exec(connection, cmd, new SqlParameter("@Id", entity.Id));
            }
        }

    }
}
