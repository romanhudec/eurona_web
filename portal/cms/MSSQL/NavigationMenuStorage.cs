using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class NavigationMenuStorage: MSSQLStorage<NavigationMenu>
		{
				public NavigationMenuStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static NavigationMenu GetMenu( DataRow record )
				{
						NavigationMenu menu = new NavigationMenu();
						menu.Id = Convert.ToInt32( record["NavigationMenuId"] );
						menu.InstanceId = Convert.ToInt32( record["InstanceId"] );
						menu.MenuId = Convert.ToInt32( record["MenuId"] );
						menu.Name = Convert.ToString( record["Name"] );
						menu.Locale = Convert.ToString( record["Locale"] );
						menu.Order = ConvertNullable.ToInt32( record["Order"] );
						menu.Icon = Convert.ToString( record["Icon"] );
						menu.RoleId = ConvertNullable.ToInt32( record["RoleId"] );
						menu.UrlAliasId = ConvertNullable.ToInt32( record["UrlAliasId"] );
						menu.Alias = Convert.ToString( record["Alias"] );

						return menu;
				}

				public override List<NavigationMenu> Read( object criteria )
				{
						if ( criteria is NavigationMenu.ReadById ) return LoadById( criteria as NavigationMenu.ReadById );
						if ( criteria is NavigationMenu.ReadByAlias ) return LoadByAlias( criteria as NavigationMenu.ReadByAlias );
						if ( criteria is NavigationMenu.ReadByMenuId ) return LoadForMenuId( criteria as NavigationMenu.ReadByMenuId );
						if ( criteria is NavigationMenu.ReadForCurrentAccount ) return LoadForCurrentAccount( criteria as NavigationMenu.ReadForCurrentAccount );
						List<NavigationMenu> list = new List<NavigationMenu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT NavigationMenuId, InstanceId, MenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenu
										WHERE Locale = @Locale AND InstanceId = @InstanceId
										ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				private List<NavigationMenu> LoadById( NavigationMenu.ReadById byMenuId )
				{
						List<NavigationMenu> list = new List<NavigationMenu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT NavigationMenuId, InstanceId, MenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenu
										WHERE NavigationMenuId = @NavigationMenuId
										ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@NavigationMenuId", byMenuId.NavigationMenuId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}
				private List<NavigationMenu> LoadByAlias( NavigationMenu.ReadByAlias by )
				{
						List<NavigationMenu> list = new List<NavigationMenu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT NavigationMenuId, InstanceId, MenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenu
										WHERE UrlAliasId = @UrlAliasId
										ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@UrlAliasId", by.UrlAliasId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}
				private List<NavigationMenu> LoadForMenuId( NavigationMenu.ReadByMenuId by )
				{
						List<NavigationMenu> list = new List<NavigationMenu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
										SELECT NavigationMenuId, InstanceId, MenuId, Locale, [Order], [Name], Icon, RoleId, UrlAliasId, Alias, Url
										FROM vNavigationMenu
										WHERE MenuId = @MenuId
										ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@MenuId", by.MenuId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				private List<NavigationMenu> LoadForCurrentAccount( NavigationMenu.ReadForCurrentAccount by )
				{
						List<NavigationMenu> list = new List<NavigationMenu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @AccountId IS NULL
										SELECT 
										m.NavigationMenuId, m.InstanceId, m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, m.Alias, m.Url
										FROM vNavigationMenu m (NOLOCK)
										INNER JOIN vMenu hm (NOLOCK) ON hm.MenuId=m.MenuId
										WHERE m.RoleId IS NULL AND m.Locale = @Locale AND m.InstanceId = @InstanceId AND hm.Code=@Code
										ORDER BY m.[Order] ASC
								ELSE
										SELECT DISTINCT
										m.NavigationMenuId, m.InstanceId, m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, m.Alias, m.Url
										FROM vNavigationMenu m (NOLOCK)
										INNER JOIN vMenu hm (NOLOCK) ON hm.MenuId=m.MenuId
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(m.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId)
										WHERE ar.AccountId = @AccountId AND m.Locale = @Locale AND m.InstanceId = @InstanceId AND
										hm.Code=@Code
										ORDER BY m.[Order] ASC";
								DataTable table = Query<DataTable>( connection, sql,
									new SqlParameter( "@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
									new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ), new SqlParameter( "@Code", by.Code ),
									new SqlParameter("@AdministratorRoleName", Role.ADMINISTRATOR)
								);
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				public override void Create( NavigationMenu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pNavigationMenuCreate",
									new SqlParameter( "@HistoryAccount", AccountId ),
									new SqlParameter( "@InstanceId", InstanceId ),
									new SqlParameter( "@MenuId", menu.MenuId ),
									new SqlParameter( "@Name", Null( menu.Name ) ),
									new SqlParameter( "@Locale", String.IsNullOrEmpty( menu.Locale ) ? Locale : menu.Locale ),
									new SqlParameter( "@Icon", Null( menu.Icon ) ),
									new SqlParameter( "@Order", Null( menu.Order ) ),
									new SqlParameter( "@RoleId", Null( menu.RoleId ) ),
									new SqlParameter( "@UrlAliasId", menu.UrlAliasId ) );
						}
				}

				public override void Update( NavigationMenu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pNavigationMenuModify",
									new SqlParameter( "@HistoryAccount", AccountId ),
									new SqlParameter( "@NavigationMenuId", menu.Id ),
									new SqlParameter( "@Name", Null( menu.Name ) ),
									new SqlParameter( "@Locale", menu.Locale ),
									new SqlParameter( "@Icon", Null( menu.Icon ) ),
									new SqlParameter( "@Order", Null( menu.Order ) ),
									new SqlParameter( "@RoleId", Null( menu.RoleId ) ),
									new SqlParameter( "@UrlAliasId", menu.UrlAliasId ) );
						}
				}

				public override void Delete( NavigationMenu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter pageId = new SqlParameter( "@NavigationMenuId", menu.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pNavigationMenuDelete", result, historyAccount, pageId );
						}
				}

		}
}
