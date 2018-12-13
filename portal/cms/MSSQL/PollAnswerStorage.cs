using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class PollAnswerStorage : MSSQLStorage<PollAnswer> {
        public PollAnswerStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static PollAnswer GetPollAnswer(DataRow record) {
            PollAnswer answer = new PollAnswer();
            answer.Id = Convert.ToInt32(record["PollAnswerId"]);
            answer.InstanceId = Convert.ToInt32(record["InstanceId"]);
            answer.PollOptionId = Convert.ToInt32(record["PollOptionId"]);
            answer.IP = Convert.ToString(record["IP"]);

            return answer;
        }

        public override List<PollAnswer> Read(object criteria) {
            if (criteria is PollAnswer.ReadById) return LoadById(criteria as PollAnswer.ReadById);
            if (criteria is PollAnswer.ReadByPollOptionId) return LoadByPollOptionId(criteria as PollAnswer.ReadByPollOptionId);
            if (criteria is PollAnswer.ReadByPollAndIP) return LoadByPollAndIP(criteria as PollAnswer.ReadByPollAndIP);
            List<PollAnswer> list = new List<PollAnswer>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollAnswerId,InstanceId, PollOptionId, IP
								FROM vPollAnswers WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPollAnswer(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<PollAnswer> LoadById(PollAnswer.ReadById byId) {
            List<PollAnswer> list = new List<PollAnswer>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollAnswerId, InstanceId, PollOptionId, IP
								FROM vPollAnswers
								WHERE PollAnswerId = @PollAnswerId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@PollAnswerId", byId.PollAnswerId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPollAnswer(dr));
            }
            return list;
        }

        private List<PollAnswer> LoadByPollOptionId(PollAnswer.ReadByPollOptionId byPollOptionId) {
            List<PollAnswer> list = new List<PollAnswer>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollAnswerId, InstanceId, PollOptionId, IP
								FROM vPollAnswers
								WHERE PollOptionId = @PollOptionId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@PollOptionId", byPollOptionId.PollOptionId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPollAnswer(dr));
            }
            return list;
        }

        private List<PollAnswer> LoadByPollAndIP(PollAnswer.ReadByPollAndIP byPollIP) {
            List<PollAnswer> list = new List<PollAnswer>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT a.PollAnswerId, a.InstanceId, a.PollOptionId, a.IP
								FROM vPollAnswers a INNER JOIN
								vPollOptions o ON o.PollOptionId = a.PollOptionId
								WHERE o.PollId = @PollId AND a.IP = @IP AND a.InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@PollId", byPollIP.PollId),
                        new SqlParameter("@IP", byPollIP.IP),
                        new SqlParameter("@InstanceId", InstanceId));

                foreach (DataRow dr in table.Rows)
                    list.Add(GetPollAnswer(dr));
            }
            return list;
        }

        public override void Create(PollAnswer answer) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pPollAnswerCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@PollOptionId", answer.PollOptionId),
                        new SqlParameter("@IP", answer.IP));
            }
        }

        public override void Update(PollAnswer answer) {
            throw new NotImplementedException();
        }

        public override void Delete(PollAnswer entity) {
            throw new NotImplementedException();
        }

    }
}
