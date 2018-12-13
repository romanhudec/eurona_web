using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    public sealed class CommentStorage : MSSQLStorage<Comment> {
        public CommentStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Comment GetComment(DataRow record) {
            Comment comment = new Comment();
            comment.Id = Convert.ToInt32(record["CommentId"]);
            comment.InstanceId = Convert.ToInt32(record["InstanceId"]);
            comment.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            comment.Date = Convert.ToDateTime(record["Date"]);
            comment.AccountId = Convert.ToInt32(record["AccountId"]);
            comment.Title = Convert.ToString(record["Title"]);
            comment.Content = Convert.ToString(record["Content"]);
            comment.Votes = Convert.ToInt32(record["Votes"] == DBNull.Value ? 0 : record["Votes"]);
            comment.TotalRating = Convert.ToInt32(record["TotalRating"] == DBNull.Value ? 0 : record["TotalRating"]);
            return comment;
        }

        public override List<Comment> Read(object criteria) {
            if (criteria is Comment.ReadById) return LoadById(criteria as Comment.ReadById);
            List<Comment> list = new List<Comment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT [CommentId], InstanceId, [ParentId], [AccountId], [Date], [Title], [Content], Votes , TotalRating
								FROM vComments WHERE InstanceId=@InstanceId
								ORDER BY [Date] DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetComment(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        private List<Comment> LoadById(Comment.ReadById byCommentId) {
            List<Comment> list = new List<Comment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT [CommentId], InstanceId, [ParentId], [AccountId], [Date], [Title], [Content], Votes , TotalRating
								FROM vComments
								WHERE CommentId = @CommentId
								ORDER BY [Date] DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@CommentId", byCommentId.CommentId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetComment(dr));
            }
            return list;
        }

        public override void Create(Comment comment) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pCommentCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Date", Null(comment.Date)),
                        new SqlParameter("@AccountId", comment.AccountId),
                        new SqlParameter("@ParentId", comment.ParentId),
                        new SqlParameter("@Title", comment.Title),
                        new SqlParameter("@Content", comment.Content));
            }
        }

        public override void Update(Comment comment) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pCommentModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@CommentId", comment.Id),
                        new SqlParameter("@Title", comment.Title),
                        new SqlParameter("@Content", comment.Content)
                        );
            }
        }

        public override void Delete(Comment comment) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter id = new SqlParameter("@CommentId", comment.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pCommentDelete", result, historyAccount, id);
            }
        }

        public override R Execute<R>(R command) {
            Type t = typeof(R);
            if (t == typeof(Comment.IncrementVoteCommand))
                return IncrementVote(command as Comment.IncrementVoteCommand) as R;

            return base.Execute<R>(command);
        }

        private Comment.IncrementVoteCommand IncrementVote(Comment.IncrementVoteCommand cmd) {
            using (SqlConnection connection = Connect()) {
                SqlParameter commentId = new SqlParameter("@CommentId", cmd.CommentId);
                SqlParameter rating = new SqlParameter("@Rating", cmd.Rating);
                ExecProc(connection, "pCommentIncrementVote", commentId, rating);

                AccountVote accountVote = new AccountVote();
                accountVote.AccountId = cmd.AccountId;
                accountVote.ObjectId = cmd.CommentId;
                accountVote.ObjectTypeId = (int)cmd.ObjectTypeId;
                accountVote.Rating = cmd.Rating;
                Storage<AccountVote>.Create(accountVote);
            }

            return cmd;
        }

    }
}
