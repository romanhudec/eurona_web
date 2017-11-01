using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using Eurona.DAL.Entities;

namespace Eurona.DAL.MSSQL
{
    internal sealed class AdvisorPageStorage : MSSQLStorage<AdvisorPage>
    {
        public AdvisorPageStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private static AdvisorPage GetPage(DataRow record)
        {
            AdvisorPage page = new AdvisorPage();
            page.Id = Convert.ToInt32(record["AdvisorPageId"]);
            page.InstanceId = Convert.ToInt32(record["InstanceId"]);
            page.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            page.MasterPageId = Convert.ToInt32(record["MasterPageId"]);
            page.Blocked = Convert.ToBoolean(record["Blocked"]);
            page.AdvisorAccountId = Convert.ToInt32(record["AdvisorAccountId"]);
            page.OrganizationId = Convert.ToInt32(record["OrganizationId"]);
            page.OrganizationCode = Convert.ToString(record["OrganizationCode"]);
            page.OrganizationName = Convert.ToString(record["OrganizationName"]);
            page.Email = Convert.ToString(record["Email"]);
            page.Name = Convert.ToString(record["Name"]);
            page.Title = Convert.ToString(record["Title"]);
            page.Locale = Convert.ToString(record["Locale"]);
            page.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            page.Alias = Convert.ToString(record["Alias"]);
            page.Content = Convert.ToString(record["Content"]);
            page.RoleId = ConvertNullable.ToInt32(record["RoleId"]);
            return page;
        }

        public override List<AdvisorPage> Read(object criteria)
        {
            if (criteria is AdvisorPage.ReadForCurrentAccount) return ReadForCurrentAccount(criteria as AdvisorPage.ReadForCurrentAccount);
            List<AdvisorPage> list = new List<AdvisorPage>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
						SELECT AdvisorPageId, InstanceId, ParentId, MasterPageId, [Name], [Email], [AdvisorAccountId], [Blocked], [OrganizationId], [OrganizationName], [OrganizationCode], Title, Locale, [UrlAliasId], Alias, [Content], RoleId
						FROM vAdvisorPages";
                SqlParameter[] @params = null;
                if (criteria is AdvisorPage.ReadById)
                {
                    @params = new SqlParameter[] { new SqlParameter("@AdvisorPageId", (criteria as AdvisorPage.ReadById).AdvisorPageId) };
                    sql += " WHERE AdvisorPageId = @AdvisorPageId";
                }
                else if (criteria is AdvisorPage.ReadByAdvisorAccountId)
                {
                    @params = new SqlParameter[] { new SqlParameter("@AdvisorAccountId", (criteria as AdvisorPage.ReadByAdvisorAccountId).AdvisorAccountId) };
                    sql += " WHERE AdvisorAccountId = @AdvisorAccountId";
                }
                else if (criteria is AdvisorPage.ReadByParent)
                {
                    @params = new SqlParameter[] { new SqlParameter("@ParentId", (criteria as AdvisorPage.ReadByParent).ParentId) };
                    sql += " WHERE ParentId = @ParentId";
                }
                else if (criteria is AdvisorPage.ReadByName)
                {
                    @params = new SqlParameter[] { 
						new SqlParameter("@Name", (criteria as AdvisorPage.ReadByName).Name), 
						new SqlParameter("@Locale", string.IsNullOrEmpty((criteria as AdvisorPage.ReadByName).Locale) ? Locale : (criteria as AdvisorPage.ReadByName).Locale ),
						new SqlParameter( "@InstanceId", InstanceId )};
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND [Name] = @Name";
                }
                else if (criteria is AdvisorPage.ReadContentPages)
                {
                    @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND Content IS NOT NULL"; // AND LEN(ISNULL(Content, '')) != 0";
                }
                else
                {
                    @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                    sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId";
                }
                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPage(dr));
            }
            return list;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }


        private List<AdvisorPage> ReadForCurrentAccount(AdvisorPage.ReadForCurrentAccount by)
        {
            List<AdvisorPage> list = new List<AdvisorPage>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				DECLARE @AdministratorRoleId INT, @PageRoleId INT
				SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

				SELECT @PageRoleId = p.RoleId FROM vAdvisorPages p (NOLOCK)
				WHERE p.InstanceId=@InstanceId AND [Name] = @Name

				IF @AccountId IS NULL OR @PageRoleId IS NULL
					SELECT p.AdvisorPageId, p.ParentId, p.InstanceId, p.MasterPageId, p.[Name], p.[Email], p.[AdvisorAccountId], p.[Blocked], p.[OrganizationId], p.[OrganizationName], p.[OrganizationCode], p.Title, p.Locale, p.[UrlAliasId], p.Alias, p.[Content], p.RoleId
					FROM vAdvisorPages p (NOLOCK)
					WHERE p.RoleId IS NULL AND p.InstanceId=@InstanceId AND [Name] = @Name
				ELSE
					SELECT DISTINCT p.AdvisorPageId, p.ParentId, p.InstanceId, p.MasterPageId, p.[Name], p.[Email], p.[AdvisorAccountId], p.[Blocked], p.[OrganizationId], p.[OrganizationName], p.[OrganizationCode], p.Title, p.Locale, p.[UrlAliasId], p.Alias, p.[Content], p.RoleId
					FROM vAdvisorPages p (NOLOCK)
					INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(p.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId)
					AND ar.AccountId = @AccountId
					WHERE p.InstanceId=@InstanceId AND [Name] = @Name
					";
                SqlParameter[] @params = null;
                @params = new SqlParameter[] {
				new SqlParameter("@Name", by.Name),
				new SqlParameter("@InstanceId", InstanceId),
				new SqlParameter("@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value),
				new SqlParameter("@AdministratorRoleName", CMS.Entities.Role.ADMINISTRATOR)};

                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetPage(dr));
            }
            return list;
        }

        public override void Create(AdvisorPage page)
        {

            using (SqlConnection connection = Connect())
            {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pAdvisorPageCreate",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@ParentId", Null(page.ParentId)),
                    new SqlParameter("@InstanceId", InstanceId),
                    new SqlParameter("@MasterPageId", page.MasterPageId),
                    new SqlParameter("@Name", page.Name),
                    new SqlParameter("@AdvisorAccountId", page.AdvisorAccountId),
                    new SqlParameter("@Blocked", page.Blocked),
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

        public override void Update(AdvisorPage page)
        {
            using (SqlConnection connection = Connect())
            {
                ExecProc(connection, "pAdvisorPageModify",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@AdvisorPageId", page.Id),
                    new SqlParameter("@MasterPageId", page.MasterPageId),
                    new SqlParameter("@Name", page.Name),
                    new SqlParameter("@Blocked", page.Blocked),
                    new SqlParameter("@Title", page.Title),
                    new SqlParameter("@Locale", page.Locale),
                    new SqlParameter("@UrlAliasId", page.UrlAliasId),
                    new SqlParameter("@Content", page.Content),
                    new SqlParameter("@ContentKeywords", Null(page.ContentKeywords, String.Empty)),
                    new SqlParameter("@RoleId", Null(page.RoleId)));
            }
        }

        public override void Delete(AdvisorPage page)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter pageId = new SqlParameter("@AdvisorPageId", page.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAdvisorPageDelete", result, historyAccount, pageId);
            }
        }

    }
}
