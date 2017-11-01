using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMSAccount = CMS.Entities.Account;

namespace SHP.MSSQL
{
		public class SearchEngineStorage: MSSQLStorage<CMS.Entities.SearchEngineBase>
		{
				public SearchEngineStorage( int instanceId, CMSAccount account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static CMS.Entities.SearchEngineBase GetResult( DataRow record )
				{
						CMS.Entities.SearchEngineBase result = new CMS.Entities.SearchEngineBase();
						result.Id = Convert.ToInt32( record["Id"] );
						result.Title = Convert.ToString( record["Title"] );
						result.Content = Convert.ToString( record["Content"] );
						result.UrlAlias = Convert.ToString( record["UrlAlias"] );
						result.ImageUrl = Convert.ToString( record["ImageUrl"] );

						return result;
				}

				public override List<CMS.Entities.SearchEngineBase> Read( object criteria )
				{
						List<CMS.Entities.SearchEngineBase> list = new List<CMS.Entities.SearchEngineBase>();

						if ( ( criteria is ShpSearchEngineEntity.ISearch ) )
						{
								ShpSearchEngineEntity.ISearch search = criteria as ShpSearchEngineEntity.ISearch;
								using ( SqlConnection connection = Connect() )
								{
										string searchProcedure = string.Empty;
										if ( search is ShpSearchEngineEntity.SearchProducts )
												searchProcedure = "pShpSearchProducts";
										
										if ( !string.IsNullOrEmpty( searchProcedure ) )
										{
												DataTable table = QueryProc<DataTable>( connection, searchProcedure,
														new SqlParameter( "@Keywords", search.Keywords ),
														new SqlParameter( "@Locale", Locale ),
														new SqlParameter( "@InstanceId", InstanceId ) );
												foreach ( DataRow dr in table.Rows )
														list.Add( GetResult( dr ) );
										}
								}
						}
						else
								throw new InvalidCastException( "Criteria mus implement interface SearchEngineEntity.ISearch" );

						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}
				public override void Create( CMS.Entities.SearchEngineBase entity )
				{
						throw new NotImplementedException();
				}
				public override void Delete( CMS.Entities.SearchEngineBase entity )
				{
						throw new NotImplementedException();
				}
				public override void Update( CMS.Entities.SearchEngineBase entity )
				{
						throw new NotImplementedException();
				}

		}
}
