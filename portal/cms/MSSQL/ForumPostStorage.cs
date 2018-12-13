using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class ForumPostStorage : MSSQLStorage<ForumPost> {
        private const string entitySelect = @"SELECT ForumPostId ,ForumId ,InstanceId ,ParentId ,AccountId, AccountName ,IPAddress ,[Date] ,Title ,Content ,Votes ,TotalRating ,RatingResult
						FROM vForumPosts";

        public ForumPostStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ForumPost GetForumPost(DataRow record) {
            ForumPost forumPost = new ForumPost();
            forumPost.Id = Convert.ToInt32(record["ForumPostId"]);
            forumPost.ForumId = Convert.ToInt32(record["ForumId"]);
            forumPost.InstanceId = Convert.ToInt32(record["InstanceId"]);
            forumPost.AccountId = Convert.ToInt32(record["AccountId"]);
            forumPost.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            forumPost.Date = Convert.ToDateTime(record["Date"]);
            forumPost.Title = Convert.ToString(record["Title"]);
            forumPost.Content = Convert.ToString(record["Content"]);

            forumPost.AccountName = Convert.ToString(record["AccountName"]);
            forumPost.Votes = Convert.ToInt32(record["Votes"]);
            forumPost.TotalRating = Convert.ToInt32(record["TotalRating"]);
            forumPost.RatingResult = Convert.ToDouble(record["RatingResult"]);

            return forumPost;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<ForumPost> Read(object criteria) {
            if (criteria is ForumPost.ReadById) return LoadById(criteria as ForumPost.ReadById);
            if (criteria is ForumPost.ReadByForumId) return LoadByForumId(criteria as ForumPost.ReadByForumId);
            List<ForumPost> list = new List<ForumPost>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetForumPost(dr));
            }
            return list;
        }

        private List<ForumPost> LoadById(ForumPost.ReadById by) {
            List<ForumPost> list = new List<ForumPost>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ForumPostId = @ForumPostId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ForumPostId", by.ForumPostId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetForumPost(dr));
            }
            return list;
        }

        private List<ForumPost> LoadByForumId(ForumPost.ReadByForumId by) {
            List<ForumPost> list = new List<ForumPost>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ForumId = @ForumId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ForumId", by.ForumId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetForumPost(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        public override void Create(ForumPost forumPost) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pForumPostCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Date", forumPost.Date),
                        new SqlParameter("@ForumId", forumPost.ForumId),
                        new SqlParameter("@ParentId", forumPost.ParentId),
                        new SqlParameter("@AccountId", forumPost.AccountId),
                        new SqlParameter("@IPAddress", forumPost.IPAddress),
                        new SqlParameter("@Title", forumPost.Title),
                        new SqlParameter("@Content", forumPost.Content),

                        result);

                forumPost.Id = Convert.ToInt32(result.Value); ;
            }
        }

        public override void Update(ForumPost forumPost) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pForumPostModify",
                        new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null)),
                        new SqlParameter("@ForumPostId", forumPost.Id),
                        new SqlParameter("@ParentId", forumPost.ParentId),
                        new SqlParameter("@AccountId", forumPost.AccountId),
                        new SqlParameter("@IPAddress", forumPost.IPAddress),
                        new SqlParameter("@Title", forumPost.Title),
                        new SqlParameter("@Content", forumPost.Content));
            }
        }

        public override void Delete(ForumPost forumPost) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter articleId = new SqlParameter("@ForumPostId", forumPost.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pForumPostDelete", result, historyAccount, articleId);
            }
        }

        public override R Execute<R>(R command) {
            Type t = typeof(R);

            if (t == typeof(ForumPost.IncrementVoteCommand))
                return IncrementVote(command as ForumPost.IncrementVoteCommand) as R;

            return base.Execute<R>(command);
        }

        private ForumPost.IncrementVoteCommand IncrementVote(ForumPost.IncrementVoteCommand cmd) {
            using (SqlConnection connection = Connect()) {
                SqlParameter articleId = new SqlParameter("@ForumPostId", cmd.ForumPostId);
                SqlParameter rating = new SqlParameter("@Rating", cmd.Rating);
                ExecProc(connection, "pForumPostIncrementVote", articleId, rating);

                AccountVote accountVote = new AccountVote();
                accountVote.AccountId = cmd.AccountId;
                accountVote.ObjectId = cmd.ForumPostId;
                accountVote.ObjectTypeId = (int)ForumPost.AccountVoteType;
                accountVote.Rating = cmd.Rating;
                Storage<AccountVote>.Create(accountVote);
            }

            return cmd;
        }
    }
}
