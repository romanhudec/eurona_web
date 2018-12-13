using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class FAQStorage : MSSQLStorage<FAQ> {
        public FAQStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static FAQ GetFaq(DataRow record) {
            FAQ faq = new FAQ();
            faq.Id = Convert.ToInt32(record["FaqId"]);
            faq.InstanceId = Convert.ToInt32(record["InstanceId"]);
            faq.Question = Convert.ToString(record["Question"]);
            faq.Answer = Convert.ToString(record["Answer"]);
            faq.Locale = Convert.ToString(record["Locale"]);
            faq.Order = ConvertNullable.ToInt32(record["Order"]);

            return faq;
        }

        public override List<FAQ> Read(object criteria) {
            if (criteria is FAQ.ReadById) return LoadById(criteria as FAQ.ReadById);
            List<FAQ> list = new List<FAQ>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT FaqId, InstanceId, Locale, [Order], Question, Answer
								FROM vFaqs
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetFaq(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<FAQ> LoadById(FAQ.ReadById byFaqId) {
            List<FAQ> list = new List<FAQ>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT FaqId, InstanceId, Locale, [Order], Question, Answer
								FROM vFaqs
								WHERE FaqId = @FaqId AND Locale = @Locale
								ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@FaqId", byFaqId.FAQId),
                        new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetFaq(dr));
            }
            return list;
        }

        public override void Create(FAQ faq) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pFaqCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Order", Null(faq.Order)),
                        new SqlParameter("@Locale", faq.Locale),
                        new SqlParameter("@Question", faq.Question),
                        new SqlParameter("@Answer", faq.Answer));
            }
        }

        public override void Update(FAQ faq) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pFaqModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@FaqId", faq.Id),
                        new SqlParameter("@Order", Null(faq.Order)),
                        new SqlParameter("@Question", faq.Question),
                        new SqlParameter("@Answer", faq.Answer)
                        );
            }
        }

        public override void Delete(FAQ faq) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter id = new SqlParameter("@FaqId", faq.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pFaqDelete", result, historyAccount, id);
            }
        }

    }
}
