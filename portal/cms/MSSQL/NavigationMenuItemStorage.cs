using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class NavigationMenuItemStorage : MSSQLStorage<NavigationMenuItem> {
        public NavigationMenuItemStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static NavigationMenuItem GetMenu(DataRow record) {
            NavigationMenuItem menu = new NavigationMenuItem();
            menu.Id = Convert.ToInt32(record["NavigationMenuItemId"]);
            menu.InstanceId = Convert.ToInt32(record["InstanceId"]);
            menu.NavigationMenuId = Convert.ToInt32(record["NavigationMenuId"]);
            menu.Name = Convert.ToString(record["Name"]);
            menu.Locale = Convert.ToString(record["Locale"]);
            menu.Order = ConvertNullable.ToInt32(record["Order"]);
            menu.Icon = Convert.ToString(record["Icon"]);
            menu.RoleId = ConvertNullable.ToInt32(record["RoleId"]);
            menu.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            menu.Alias = Convert.ToString(record["Alias"]);

            return menu;
        }

        public override List<NavigationMenuItem> Read(object criteria) {
            if (criteria is NavigationMenuItem.ReadById) return LoadById(criteria as NavigationMenuItem.ReadById);
            if (criteria is NavigationMenuItem.ReadForCurrentAccount) return LoadForCurrentAccount(criteria as NavigationMenuItem.ReadForCurrentAccount);
            if (criteria is NavigationMenuItem.ReadByNavigationMenuId) return LoadForNavigationMenuId(criteria as NavigationMenuItem.ReadByNavigationMenuId);
            List<NavigationMenuItem> list = new List<NavigationMenuItem>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT NavigationMenuItemId, InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenuItem
										WHERE Locale = @Locale AND InstanceId=@InstanceId
										ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMenu(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        private List<NavigationMenuItem> LoadById(NavigationMenuItem.ReadById byMenuId) {
            List<NavigationMenuItem> list = new List<NavigationMenuItem>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT NavigationMenuItemId, InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenuItem
										WHERE NavigationMenuItemId = @NavigationMenuItemId
										ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@NavigationMenuItemId", byMenuId.NavigationMenuItemId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMenu(dr));
            }
            return list;
        }

        private List<NavigationMenuItem> LoadForNavigationMenuId(NavigationMenuItem.ReadByNavigationMenuId by) {
            List<NavigationMenuItem> list = new List<NavigationMenuItem>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT NavigationMenuItemId, InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenuItem
										WHERE NavigationMenuId = @NavigationMenuId AND InstanceId=@InstanceId
										ORDER BY [Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@NavigationMenuId", by.NavigationMenuId),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMenu(dr));
            }
            return list;
        }

        private List<NavigationMenuItem> LoadForCurrentAccount(NavigationMenuItem.ReadForCurrentAccount by) {
            List<NavigationMenuItem> list = new List<NavigationMenuItem>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								IF @AccountId IS NULL
										SELECT 
										m.NavigationMenuItemId, m.InstanceId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, m.Alias, m.Url
										FROM vNavigationMenuItem m (NOLOCK)
										WHERE m.RoleId IS NULL AND m.Locale = @Locale AND m.InstanceId = @InstanceId AND m.NavigationMenuId = @NavigationMenuId
										ORDER BY m.[Order] ASC
								ELSE
										SELECT
										m.NavigationMenuItemId, m.InstanceId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, m.Alias, m.Url
										FROM vNavigationMenuItem m (NOLOCK)
										WHERE m.Locale = @Locale AND m.InstanceId = @InstanceId and (m.RoleId in (select RoleId from vAccountRoles where AccountId =@AccountId) or m.RoleId is null ) and
												m.NavigationMenuId = @NavigationMenuId
										ORDER BY m.[Order] ASC";
                DataTable table = Query<DataTable>(connection, sql,
                    new SqlParameter("@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value),
                    new SqlParameter("@NavigationMenuId", by.NavigationMenuId),
                    new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId)
                );
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMenu(dr));
            }
            return list;
        }

        public override void Create(NavigationMenuItem menu) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pNavigationMenuItemCreate",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@InstanceId", InstanceId),
                    new SqlParameter("@NavigationMenuId", menu.NavigationMenuId),
                    new SqlParameter("@Name", Null(menu.Name)),
                    new SqlParameter("@Locale", String.IsNullOrEmpty(menu.Locale) ? Locale : menu.Locale),
                    new SqlParameter("@Icon", menu.Icon),
                    new SqlParameter("@Order", Null(menu.Order)),
                    new SqlParameter("@RoleId", Null(menu.RoleId)),
                    new SqlParameter("@UrlAliasId", menu.UrlAliasId));
            }
        }

        public override void Update(NavigationMenuItem menu) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pNavigationMenuItemModify",
                    new SqlParameter("@HistoryAccount", AccountId),
                    new SqlParameter("@NavigationMenuItemId", menu.Id),
                    new SqlParameter("@Name", Null(menu.Name)),
                    new SqlParameter("@Locale", menu.Locale),
                    new SqlParameter("@Icon", menu.Icon),
                    new SqlParameter("@Order", Null(menu.Order)),
                    new SqlParameter("@RoleId", Null(menu.RoleId)),
                    new SqlParameter("@UrlAliasId", menu.UrlAliasId));
            }
        }

        public override void Delete(NavigationMenuItem menu) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter pageId = new SqlParameter("@NavigationMenuItemId", menu.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pNavigationMenuItemDelete", result, historyAccount, pageId);
            }
        }

    }
}
