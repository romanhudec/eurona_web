using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class PollStorage : MSSQLStorage<Poll> {
        public PollStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Poll GetPoll(DataRow record) {
            Poll poll = new Poll();
            poll.Id = Convert.ToInt32(record["PollId"]);
            poll.InstanceId = Convert.ToInt32(record["InstanceId"]);
            poll.Closed = record["Closed"] == DBNull.Value ? false : Convert.ToBoolean(record["Closed"]);
            poll.Locale = Convert.ToString(record["Locale"]);
            poll.Question = Convert.ToString(record["Question"]);
            poll.Icon = Convert.ToString(record["Icon"]);
            poll.DateFrom = Convert.ToDateTime(record["DateFrom"]);
            poll.DateTo = ConvertNullable.ToDateTime(record["DateTo"]);
            poll.VotesTotal = record["VotesTotal"] != DBNull.Value ? Convert.ToInt32(record["VotesTotal"]) : 0;
            return poll;
        }

        public override List<Poll> Read(object criteria) {
            if (criteria is Poll.ReadById) return LoadById(criteria as Poll.ReadById);
            if (criteria is Poll.ReadActivePoll) return LoadActivePoll();
            List<Poll> list = new List<Poll>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollId, InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon, VotesTotal
								FROM vPolls
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY DateFrom DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPoll(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Poll> LoadById(Poll.ReadById byPollId) {
            List<Poll> list = new List<Poll>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollId, InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon, VotesTotal
								FROM vPolls
								WHERE PollId = @PollId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@PollId", byPollId.PollId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPoll(dr));
            }
            return list;
        }

        private List<Poll> LoadActivePoll() {
            List<Poll> list = new List<Poll>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PollId, InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon, VotesTotal
								FROM vPolls
								WHERE Locale = @Locale AND InstanceId=@InstanceId AND ( Closed IS NULL OR Closed = 0 ) AND
								( DateFrom <= GETDATE() AND ( DateTo IS NULL OR DateTo >= GETDATE()) )";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPoll(dr));
            }
            return list;
        }

        public override void Create(Poll poll) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pPollCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Closed", Null(poll.Closed)),
                        new SqlParameter("@Locale", poll.Locale),
                        new SqlParameter("@Icon", poll.Icon),
                        new SqlParameter("@Question", poll.Question),
                        new SqlParameter("@DateFrom", poll.DateFrom),
                        new SqlParameter("@DateTo", Null(poll.DateTo)));
            }
        }

        public override void Update(Poll poll) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pPollModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@PollId", poll.Id),
                        new SqlParameter("@Closed", Null(poll.Closed)),
                        new SqlParameter("@Icon", poll.Icon),
                        new SqlParameter("@Question", poll.Question),
                        new SqlParameter("@DateFrom", poll.DateFrom),
                        new SqlParameter("@DateTo", Null(poll.DateTo)));
            }
        }

        public override void Delete(Poll poll) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter pageId = new SqlParameter("@PollId", poll.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pPollDelete", result, historyAccount, pageId);
            }
        }

    }
}
