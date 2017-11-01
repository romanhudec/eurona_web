using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class MenuStorage: MSSQLStorage<Menu>
		{
				public MenuStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Menu GetMenu( DataRow record )
				{
						Menu menu = new Menu();
						menu.Id = Convert.ToInt32( record["MenuId"] );
						menu.InstanceId = Convert.ToInt32( record["InstanceId"] );
						menu.Name = Convert.ToString( record["Name"] );
						menu.Locale = Convert.ToString( record["Locale"] );
						menu.Code = Convert.ToString( record["Code"] );
						menu.RoleId = ConvertNullable.ToInt32( record["RoleId"] );

						return menu;
				}

				public override List<Menu> Read( object criteria )
				{
						if ( criteria is Menu.ReadById ) return LoadById( criteria as Menu.ReadById );
						if ( criteria is Menu.ReadForCurrentAccount ) return LoadForCurrentAccount();
						List<Menu> list = new List<Menu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT MenuId, InstanceId, Locale, [Code], [Name], RoleId
								FROM vMenu
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Code] ASC
								";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				private List<Menu> LoadById( Menu.ReadById byMenuId )
				{
						List<Menu> list = new List<Menu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT MenuId, InstanceId, Locale, [Code], [Name], RoleId
								FROM vMenu
								WHERE MenuId = @MenuId
								ORDER BY [Code] ASC
								";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@MenuId", byMenuId.MenuId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				private List<Menu> LoadForCurrentAccount()
				{
						List<Menu> list = new List<Menu>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								IF @AccountId IS NULL
										SELECT 
										m.MenuId, m.InstanceId, m.Locale, m.[Code], m.[Name], m.RoleId
										FROM vMenu m (NOLOCK)
										WHERE m.RoleId IS NULL AND m.Locale = @Locale AND m.InstanceId=@InstanceId
										ORDER BY m.[Code] ASC
								ELSE
										SELECT
										m.MenuId, m.InstanceId, m.Locale, m.[Code], m.[Name], m.RoleId
										FROM vMenu m (NOLOCK)
										WHERE m.Locale = @Locale AND m.InstanceId=@InstanceId and (m.RoleId in (select RoleId from vAccountRoles where AccountId =@AccountId) or m.RoleId is null )
										ORDER BY m.[Code] ASC";
								DataTable table = Query<DataTable>( connection, sql,
									new SqlParameter( "@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
									new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId )
								);
								foreach ( DataRow dr in table.Rows )
										list.Add( GetMenu( dr ) );
						}
						return list;
				}

				public override void Create( Menu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pMenuCreate",
									new SqlParameter( "@HistoryAccount", AccountId ),
									new SqlParameter( "@InstanceId", InstanceId ),
									new SqlParameter( "@Name", Null( menu.Name ) ),
									new SqlParameter( "@Code", menu.Code ),
									new SqlParameter( "@Locale", String.IsNullOrEmpty( menu.Locale ) ? Locale : menu.Locale ),
									new SqlParameter( "@RoleId", Null( menu.RoleId ) ) );
						}
				}

				public override void Update( Menu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pMenuModify",
									new SqlParameter( "@HistoryAccount", AccountId ),
									new SqlParameter( "@MenuId", menu.Id ),
									new SqlParameter( "@Name", Null( menu.Name ) ),
									new SqlParameter( "@Code", menu.Code ),
									new SqlParameter( "@Locale", menu.Locale ),
									new SqlParameter( "@RoleId", Null( menu.RoleId ) ) );
						}
				}

				public override void Delete( Menu menu )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter pageId = new SqlParameter( "@MenuId", menu.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pMenuDelete", result, historyAccount, pageId );
						}
				}

		}
}
