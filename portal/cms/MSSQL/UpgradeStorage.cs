using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class UpgradeStorage: MSSQLStorage<Upgrade>
		{
				public UpgradeStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Upgrade GetUpgrade( DataRow record )
				{
						Upgrade upgrade = new Upgrade();
						upgrade.Id = Convert.ToInt32( record["UpgradeId"] );
						upgrade.VersionMajor = Convert.ToInt32( record["VersionMajor"] );
						upgrade.VersionMinor = Convert.ToInt32( record["VersionMinor"] );
						upgrade.UpgradeDate = Convert.ToDateTime( record["UpgradeDate"] );
						return upgrade;
				}

				public override List<Upgrade> Read( object criteria )
				{
						List<Upgrade> list = new List<Upgrade>();
						if ( !( criteria is Upgrade.ReadCMSVersion ) && !( criteria is Upgrade.ReadSysVersion ) )
								return list;

						using ( SqlConnection connection = Connect() )
						{
								string sql = @" SELECT TOP 1 UpgradeId, VersionMajor, VersionMinor, UpgradeDate FROM";
								if ( criteria is Upgrade.ReadCMSVersion )
										sql += " tCMSUpgrade";
								else if ( criteria is Upgrade.ReadSysVersion )
										sql += " tSysUpgrade";

								sql += " ORDER BY UpgradeDate DESC";
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetUpgrade( dr ) );
						}
						
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override void Create( Upgrade entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( Upgrade entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( Upgrade entity )
				{
						throw new NotImplementedException();
				}
		}
}
