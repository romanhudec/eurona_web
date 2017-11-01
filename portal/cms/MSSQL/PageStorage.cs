using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    internal sealed class PageStorage : MSSQLStorage<Page> {
        public PageStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Page GetPage(DataRow record) {
            Page page = new Page();
            page.Id = Convert.ToInt32(record["PageId"]);
            page.InstanceId = Convert.ToInt32(record["InstanceId"]);
            page.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            page.MasterPageId = Convert.ToInt32(record["MasterPageId"]);
            page.Name = Convert.ToString(record["Name"]);
            page.Title = Convert.ToString(record["Title"]);
            page.Locale = Convert.ToString(record["Locale"]);
            page.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            page.Alias = Convert.ToString(record["Alias"]);
            page.Content = Convert.ToString(record["Content"]);
            page.RoleId = ConvertNullable.ToInt32(record["RoleId"]);
            return page;
        }

        public override List<Page> Read(object criteria) {
            if (criteria is Page.ReadForCurrentAccount) return ReadForCurrentAccount(criteria as Page.ReadForCurrentAccount);
            List<Page> list = new List<Page>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT PageId, InstanceId, ParentId, MasterPageId, [Name], Title, Locale, [UrlAliasId], Alias, [Content], RoleId
								FROM vPages";
                SqlParameter[] @params = null;
                if (criteria is Page.ReadById) {
                    @params = new SqlParameter[] { new SqlParameter("@PageId", (criteria as Page.ReadById).PageId) };
                    sql += " WHERE PageId = @PageId";
                } else if (criteria is Page.ReadByParent) {
                    @params = new SqlParameter[] { new SqlParameter("@ParentId", (criteria as Page.ReadByParent).ParentId) };
                    sql += " WHERE ParentId = @ParentId";
                } else if (criteria is Page.ReadByName) {
                    @params = new SqlParameter[] { 
												new SqlParameter("@Name", (criteria as Page.ReadByName).Name), 
												new SqlParameter("@Locale", string.IsNullOrEmpty((criteria as Page.ReadByName).Locale) ? Locale : (criteria as Page.ReadByName).Locale ),
												new SqlParameter( "@InstanceId", InstanceId )};
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND [Name] = @Name";
                } else if (criteria is Page.ReadContentPages) {
                    @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND Content IS NOT NULL"; // AND LEN(ISNULL(Content, '')) != 0";
                } else {
                    @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId";
                }
                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPage(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        private List<Page> ReadForCurrentAccount(Page.ReadForCurrentAccount by) {
            List<Page> list = new List<Page>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								DECLARE @AdministratorRoleId INT, @PageRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								SELECT @PageRoleId = p.RoleId FROM vPages p (NOLOCK)
								WHERE Locale = @Locale AND p.InstanceId=@InstanceId AND [Name] = @Name

								IF @AccountId IS NULL OR @PageRoleId IS NULL
										SELECT p.PageId, p.ParentId, p.InstanceId, p.MasterPageId, p.[Name], p.Title, p.Locale, p.[UrlAliasId], p.Alias, p.[Content], p.RoleId
										FROM vPages p (NOLOCK)
										WHERE p.RoleId IS NULL AND Locale = @Locale AND p.InstanceId=@InstanceId AND [Name] = @Name
								ELSE
										SELECT DISTINCT p.PageId, p.ParentId, p.InstanceId, p.MasterPageId, p.[Name], p.Title, p.Locale, p.[UrlAliasId], p.Alias, p.[Content], p.RoleId
										FROM vPages p (NOLOCK)
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(p.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId)
										AND ar.AccountId = @AccountId
										WHERE Locale = @Locale AND p.InstanceId=@InstanceId AND [Name] = @Name
										";
                SqlParameter[] @params = null;
                @params = new SqlParameter[] {
								new SqlParameter("@Name", by.Name),
								new SqlParameter("@Locale", string.IsNullOrEmpty(by.Locale) ? Locale : by.Locale),
								new SqlParameter("@InstanceId", InstanceId),
								new SqlParameter("@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value),
								new SqlParameter("@AdministratorRoleName", Role.ADMINISTRATOR)};

                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPage(dr));
            }
            return list;
        }

        public override void Create(Page page) {

            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pPageCreate",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@ParentId", Null(page.ParentId)),
                    new SqlParameter("@InstanceId", InstanceId),
                    new SqlParameter("@MasterPageId", page.MasterPageId),
                    new SqlParameter("@Name", page.Name),
                    new SqlParameter("@Title", page.Title),
                    new SqlParameter("@Locale", String.IsNullOrEmpty(page.Locale) ? Locale : page.Locale),
                    new SqlParameter("@UrlAliasId", page.UrlAliasId),
                    new SqlParameter("@Content", Null(page.Content, String.Empty)),
                    new SqlParameter("@ContentKeywords", Null(page.ContentKeywords, String.Empty)),
                    new SqlParameter("@RoleId", Null(page.RoleId)),
                    new SqlParameter("@SubPageCreateContents", 1),
                    result);

                page.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(Page page) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pPageModify",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@PageId", page.Id),
                    new SqlParameter("@MasterPageId", page.MasterPageId),
                    new SqlParameter("@Name", page.Name),
                    new SqlParameter("@Title", page.Title),
                    new SqlParameter("@Locale", page.Locale),
                    new SqlParameter("@UrlAliasId", page.UrlAliasId),
                    new SqlParameter("@Content", page.Content),
                    new SqlParameter("@ContentKeywords", Null(page.ContentKeywords, String.Empty)),
                    new SqlParameter("@RoleId", Null(page.RoleId)));
            }
        }

        public override void Delete(Page page) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter pageId = new SqlParameter("@PageId", page.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pPageDelete", result, historyAccount, pageId);
            }
        }

    }
}
