using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class ForumThreadStorage : MSSQLStorage<ForumThread> {
        private const string entitySelect = @"SELECT ForumThreadId ,InstanceId, ObjectId ,Name ,Description ,Icon ,Locale ,Locked ,VisibleForRole ,EditableForRole ,
						UrlAliasId ,Alias ,Url ,ForumsCount ,ForumPostCount 
						FROM vForumThreads";

        public ForumThreadStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ForumThread GetForumThread(DataRow record) {
            ForumThread forumThread = new ForumThread();
            forumThread.Id = Convert.ToInt32(record["ForumThreadId"]);
            forumThread.InstanceId = Convert.ToInt32(record["InstanceId"]);
            forumThread.ObjectId = ConvertNullable.ToInt32(record["ObjectId"]);
            forumThread.Name = Convert.ToString(record["Name"]);
            forumThread.Description = Convert.ToString(record["Description"]);
            forumThread.Icon = Convert.ToString(record["Icon"]);
            forumThread.Locale = Convert.ToString(record["Locale"]);

            forumThread.Locked = Convert.ToBoolean(record["Locked"]);
            forumThread.VisibleForRole = Convert.ToString(record["VisibleForRole"]);
            forumThread.EditableForRole = Convert.ToString(record["EditableForRole"]);

            forumThread.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            forumThread.Alias = Convert.ToString(record["Alias"]);

            forumThread.ForumsCount = Convert.ToInt32(record["ForumsCount"]);
            forumThread.ForumPostCount = Convert.ToInt32(record["ForumPostCount"]);

            return forumThread;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        private bool IsVisibleForRole(ForumThread ft) {
            if (string.IsNullOrEmpty(ft.VisibleForRole)) return true;
            else {
                if (!Security.IsLogged(false)) return false;
                if (Security.IsInRole(Role.ADMINISTRATOR)) return true;

                foreach (string role in Account.RoleArray) {
                    if (ft.VisibleForRole.Contains(role))
                        return true;
                }

                return false;
            }
        }
        public override List<ForumThread> Read(object criteria) {
            if (criteria is ForumThread.ReadById) return LoadById(criteria as ForumThread.ReadById);
            if (criteria is ForumThread.ReadByObjectId) return LoadByObjectId(criteria as ForumThread.ReadByObjectId);
            List<ForumThread> list = new List<ForumThread>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE InstanceId = @InstanceId AND Locale=@Locale";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId), new SqlParameter("@Locale", Locale));
                foreach (DataRow dr in table.Rows) {
                    ForumThread ft = GetForumThread(dr);
                    if (IsVisibleForRole(ft)) list.Add(ft);
                }
            }
            return list;
        }

        private List<ForumThread> LoadById(ForumThread.ReadById by) {
            List<ForumThread> list = new List<ForumThread>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ForumThreadId = @ForumThreadId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ForumThreadId", by.ForumThreadId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetForumThread(dr));
            }
            return list;
        }

        private List<ForumThread> LoadByObjectId(ForumThread.ReadByObjectId by) {
            List<ForumThread> list = new List<ForumThread>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ObjectId = @ObjectId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ObjectId", by.ObjectId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetForumThread(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        public override void Create(ForumThread forumThread) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pForumThreadCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@ObjectId", Null(forumThread.ObjectId)),
                        new SqlParameter("@Name", forumThread.Name),
                        new SqlParameter("@Description", forumThread.Description),
                        new SqlParameter("@Icon", Null(forumThread.Icon)),
                        new SqlParameter("@Locale", forumThread.Locale),
                        new SqlParameter("@Locked", forumThread.Locked),
                        new SqlParameter("@VisibleForRole", forumThread.VisibleForRole),
                        new SqlParameter("@EditableForRole", forumThread.EditableForRole),
                        new SqlParameter("@UrlAliasId", Null(forumThread.UrlAliasId)),
                        result);

                forumThread.Id = Convert.ToInt32(result.Value); ;
            }
        }

        public override void Update(ForumThread forumThread) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pForumThreadModify",
                        new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null)),
                        new SqlParameter("@ForumThreadId", forumThread.Id),
                        new SqlParameter("@ObjectId", Null(forumThread.ObjectId)),
                        new SqlParameter("@Name", forumThread.Name),
                        new SqlParameter("@Description", forumThread.Description),
                        new SqlParameter("@Icon", forumThread.Icon),
                        new SqlParameter("@Locale", forumThread.Locale),
                        new SqlParameter("@Locked", forumThread.Locked),
                        new SqlParameter("@VisibleForRole", forumThread.VisibleForRole),
                        new SqlParameter("@EditableForRole", forumThread.EditableForRole),
                        new SqlParameter("@UrlAliasId", forumThread.UrlAliasId));
            }
        }

        public override void Delete(ForumThread forumThread) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter articleId = new SqlParameter("@ForumThreadId", forumThread.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pForumThreadDelete", result, historyAccount, articleId);
            }
        }
    }
}
