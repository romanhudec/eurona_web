using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class IPNFStorage: MSSQLStorage<IPNF>
		{
				public IPNFStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static IPNF GetIPNF( DataRow record )
				{
						IPNF ipnf = new IPNF();
						ipnf.Id = Convert.ToInt32( record["IPNFId"] );
						ipnf.InstanceId = Convert.ToInt32( record["InstanceId"] );
						ipnf.Locale = Convert.ToString( record["Locale"] );
						ipnf.Type = (IPNF.IPNFType)Convert.ToInt32( record["Type"] );
						ipnf.IPF = Convert.ToString( record["IPF"] );
						ipnf.Notes = Convert.ToString( record["Notes"] );
						return ipnf;
				}

				public override List<IPNF> Read( object criteria )
				{
						if ( criteria is IPNF.ReadById ) return LoadById( criteria as IPNF.ReadById );
						if ( criteria is IPNF.ReadByType ) return LoadByType( criteria as IPNF.ReadByType );
						List<IPNF> list = new List<IPNF>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [IPNFId], InstanceId ,[Type] ,[Locale] ,[IPF] ,[Notes]
								FROM vIPNF WHERE Locale=@Locale AND InstanceId=@InstanceId
								ORDER BY IPF";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetIPNF( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<IPNF> LoadByType( IPNF.ReadByType by )
				{
						List<IPNF> list = new List<IPNF>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [IPNFId], InstanceId ,[Type] ,[Locale] ,[IPF] ,[Notes]
								FROM vIPNFs
								WHERE Type = @Type AND Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY IPF";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Type", by.Type ),
										new SqlParameter( "@Locale", by.Locale ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetIPNF( dr ) );
						}
						return list;
				}

				private List<IPNF> LoadById( IPNF.ReadById by )
				{
						List<IPNF> list = new List<IPNF>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [IPNFId], InstanceId ,[Type] ,[Locale] ,[IPF] ,[Notes]
								FROM vIPNFs
								WHERE IPNFId = @IPNFId
								ORDER BY [Date] DESC, IPNFId DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@IPNFId", by.IPNFId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetIPNF( dr ) );
						}
						return list;
				}

				public override void Create( IPNF entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( IPNF entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( IPNF entity )
				{
						throw new NotImplementedException();
				}

		}
}
