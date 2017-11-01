using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class RoleStorage: MSSQLStorage<Role>
		{
				public RoleStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Role GetRole( DataRow record )
				{
						Role role = new Role();
						role.Id = Convert.ToInt32( record["RoleId"] );
						role.InstanceId = Convert.ToInt32( record["InstanceId"] );
						role.Name = Convert.ToString( record["Name"] );
						role.Notes = Convert.ToString( record["Notes"] );
						return role;
				}

				public override List<Role> Read( object criteria )
				{
						if ( criteria is Role.ReadById ) return LoadById( criteria as Role.ReadById );
						List<Role> list = new List<Role>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	RoleId, InstanceId, [Name], [Notes]
								FROM vRoles WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetRole( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Role> LoadById( Role.ReadById byRoleId )
				{
						List<Role> list = new List<Role>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	RoleId, InstanceId, [Name], [Notes]
								FROM vRoles
								WHERE RoleId = @RoleId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@RoleId", byRoleId.RoleId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetRole( dr ) );
						}
						return list;
				}

				public override void Create( Role role )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pRoleCreate",
									new SqlParameter( "@InstanceId", InstanceId ),
									new SqlParameter( "@Name", role.Name ),
									new SqlParameter( "@Notes", role.Notes ) );
						}
				}

				public override void Update( Role role )
				{
						throw new NotImplementedException();
				}

				public override void Delete( Role entity )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter roleId = new SqlParameter( "@RoleId", entity.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pRoleDelete", result, roleId );
						}
				}

		}
}
