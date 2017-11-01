using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;

namespace CMS.MSSQL{
    public sealed class EmailLogStorage : MSSQLStorage<EmailLog> {
        private string entitySelect = "SELECT * FROM tEmailLog";
        public EmailLogStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static EmailLog GetEmailLog(DataRow record) {
            EmailLog entiry = new EmailLog();
            entiry.Id = Convert.ToInt32(record["Id"]);
            entiry.Email = Convert.ToString(record["Email"]);
            entiry.Subject = Convert.ToString(record["Subject"]);
            entiry.Message = Convert.ToString(record["Message"]);

            entiry.Status = Convert.ToBoolean(record["Status"]);
            entiry.Error = Convert.ToString(record["Error"]);
            entiry.Timestamp = Convert.ToDateTime(record["Timestamp"]);

            return entiry;
        }

        public override List<EmailLog> Read(object criteria) {
            List<EmailLog> list = new List<EmailLog>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                if (criteria is EmailLog.ReadById) {
                    string sql = entitySelect + " WHERE Id=@Id";
                    table = Query<DataTable>(connection, sql,
                            new SqlParameter("@Id", (criteria as EmailLog.ReadById).Id));

                    foreach (DataRow dr in table.Rows)
                        list.Add(GetEmailLog(dr));
                } else {

                    table = Query<DataTable>(connection, entitySelect);
                    foreach (DataRow dr in table.Rows)
                        list.Add(GetEmailLog(dr));
                }

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(EmailLog entity) {
            try {
                string sql = @"INSERT INTO tEmailLog (Email, Subject, Message, Status, Error, Timestamp) 
            VALUES
            (@Email, @Subject, @Message, @Status, @Error, GETDATE())";
                using (SqlConnection connection = Connect()) {
                    Exec(connection, sql,
                            new SqlParameter("@Email", Null(entity.Email)),
                            new SqlParameter("@Subject", Null(entity.Subject)),
                            new SqlParameter("@Message", Null(entity.Message)),
                            new SqlParameter("@Status", Null(entity.Status)),
                            new SqlParameter("@Error", Null(entity.Error))
                            );
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
            }
        }

        public override void Update(EmailLog entity) {
            throw new NotImplementedException();
        }

        public override void Delete(EmailLog entity) {
            throw new NotImplementedException();
        }
    }
}
