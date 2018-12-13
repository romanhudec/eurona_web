using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class NewsletterStorage : MSSQLStorage<Newsletter> {
        public NewsletterStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Newsletter GetNews(DataRow record) {
            Newsletter newsletter = new Newsletter();
            newsletter.Id = Convert.ToInt32(record["NewsletterId"]);
            newsletter.InstanceId = Convert.ToInt32(record["InstanceId"]);
            newsletter.Locale = Convert.ToString(record["Locale"]);
            newsletter.Date = ConvertNullable.ToDateTime(record["Date"]);
            newsletter.Icon = Convert.ToString(record["Icon"]);
            newsletter.Subject = Convert.ToString(record["Subject"]);
            newsletter.Attachment = record["Attachment"] != DBNull.Value ? (byte[])record["Attachment"] : null;
            newsletter.Content = Convert.ToString(record["Content"]);
            newsletter.Roles = Convert.ToString(record["Roles"]);
            newsletter.SendDate = ConvertNullable.ToDateTime(record["SendDate"]);
            return newsletter;
        }

        public override List<Newsletter> Read(object criteria) {
            if (criteria is Newsletter.ReadById) return LoadById(criteria as Newsletter.ReadById);
            List<Newsletter> list = new List<Newsletter>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT [NewsletterId], InstanceId, [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles], [SendDate]
								FROM vNewsletter
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Date] DESC, NewsletterId DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetNews(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        private List<Newsletter> LoadById(Newsletter.ReadById byNewsId) {
            List<Newsletter> list = new List<Newsletter>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT [NewsletterId], InstanceId, [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles],[SendDate]
								FROM vNewsletter
								WHERE NewsletterId = @NewsletterId AND Locale = @Locale
								ORDER BY [Date] DESC, NewsletterId DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@NewsletterId", byNewsId.NewsletterId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetNews(dr));
            }
            return list;
        }

        public override void Create(Newsletter newsletter) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pNewsletterCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Date", Null(newsletter.Date)),
                        new SqlParameter("@Locale", newsletter.Locale),
                        new SqlParameter("@Icon", newsletter.Icon),
                        new SqlParameter("@Subject", newsletter.Subject),
                        new SqlParameter("@Attachment", newsletter.Attachment),
                        new SqlParameter("@SendDate", newsletter.SendDate),
                        new SqlParameter("@Roles", newsletter.Roles),
                        new SqlParameter("@Content", newsletter.Content));
            }
        }

        public override void Update(Newsletter newsletter) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pNewsletterModify",
                        new SqlParameter("@NewsletterId", newsletter.Id),
                        new SqlParameter("@Date", Null(newsletter.Date)),
                        new SqlParameter("@Icon", newsletter.Icon),
                        new SqlParameter("@Subject", newsletter.Subject),
                        new SqlParameter("@Attachment", newsletter.Attachment),
                        new SqlParameter("@SendDate", newsletter.SendDate),
                        new SqlParameter("@Roles", newsletter.Roles),
                        new SqlParameter("@Content", newsletter.Content)
                        );
            }
        }

        public override void Delete(Newsletter newsletter) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@NewsletterId", newsletter.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pNewsletterDelete", result, id);
            }
        }

    }
}
