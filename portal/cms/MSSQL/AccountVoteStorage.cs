using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class AccountVoteStorage : MSSQLStorage<AccountVote> {
        public AccountVoteStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private AccountVote GetAccountVote(DataRow record) {
            AccountVote accountVote = new AccountVote();
            accountVote.Id = Convert.ToInt32(record["AccountVoteId"]);
            accountVote.InstanceId = Convert.ToInt32(record["InstanceId"]);
            accountVote.AccountId = Convert.ToInt32(record["AccountId"]);
            accountVote.ObjectTypeId = Convert.ToInt32(record["ObjectType"]);
            accountVote.ObjectId = Convert.ToInt32(record["ObjectId"]);
            accountVote.Rating = Convert.ToInt32(record["Rating"]);
            accountVote.Date = Convert.ToDateTime(record["Date"]);
            return accountVote;
        }

        public override List<AccountVote> Read(object criteria) {
            if (criteria is AccountVote.ReadById) return LoadById((criteria as AccountVote.ReadById).AccountVoteId);
            if (criteria is AccountVote.ReadBy) return LoadBy((criteria as AccountVote.ReadBy));
            List<AccountVote> accountVotes = new List<AccountVote>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT AccountVoteId, InstanceId, AcountId, ObjectType, ObjectId, Rating, [Date]
										FROM vAccountVotes WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) accountVotes.Add(GetAccountVote(dr));
            }
            return accountVotes;
        }


        public override int Count(object criteria) {
            if (criteria is AccountVote.CountBy) {
                AccountVote.CountBy by = criteria as AccountVote.CountBy;
                using (SqlConnection connection = Connect()) {
                    string sql = @"
										SELECT Count(*)
										FROM vAccountVotes
										WHERE AccountId = @AccountId AND ObjectId = @ObjectId AND ObjectType = @ObjectType AND InstanceId=@InstanceId";

                    DataTable table = Query<DataTable>(connection, sql,
                            new SqlParameter("@AccountId", by.AccountId),
                            new SqlParameter("@ObjectId", by.ObjectId),
                            new SqlParameter("@ObjectType", by.ObjectTypeId),
                            new SqlParameter("@InstanceId", InstanceId));

                    return Convert.ToInt32(table.Rows[0][0]);
                }

            }
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT Count(*)
								FROM vAccountVotes WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));

                return Convert.ToInt32(table.Rows[0][0]);
            }
        }


        private List<AccountVote> LoadById(int id) {
            List<AccountVote> accountVotes = new List<AccountVote>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT AccountVoteId, InstanceId, AcountId, ObjectType, ObjectId, Rating, [Date]
										FROM vAccountVotes
										WHERE AccountVoteId = @AccountVoteId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AccountVoteId", id));
                if (table.Rows.Count > 0)
                    accountVotes.Add(GetAccountVote(table.Rows[0]));
            }
            return accountVotes;
        }

        private List<AccountVote> LoadBy(AccountVote.ReadBy by) {
            List<AccountVote> accountVotes = new List<AccountVote>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT AccountVoteId, InstanceId, AcountId, ObjectType, ObjectId, Rating, [Date]
										FROM vAccountVotes
										WHERE AccountId = @AccountId AND ObjectId = @ObjectId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", by.AccountId),
                        new SqlParameter("@ObjectId", by.ObjectId),
                        new SqlParameter("@InstanceId", InstanceId));

                foreach (DataRow dr in table.Rows) accountVotes.Add(GetAccountVote(dr));
            }
            return accountVotes;
        }

        public override void Create(AccountVote accountVote) {
            using (SqlConnection connection = Connect()) {
                SqlParameter instanceId = new SqlParameter("@InstanceId", InstanceId);
                SqlParameter account = new SqlParameter("@AccountId", Null(accountVote.AccountId));
                SqlParameter objectId = new SqlParameter("@ObjectId", accountVote.ObjectId);
                SqlParameter objectType = new SqlParameter("@ObjectType", accountVote.ObjectTypeId);
                SqlParameter rating = new SqlParameter("@Rating", accountVote.Rating);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountVoteCreate", result, instanceId, account, objectType, objectId, rating);
                accountVote.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(AccountVote entity) {
            throw new NotImplementedException();
        }

        public override void Delete(AccountVote entity) {
            throw new NotImplementedException();
        }


    }
}
