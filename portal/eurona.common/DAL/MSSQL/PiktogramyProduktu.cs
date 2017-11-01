using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL
{
		public sealed class PiktogramyProduktuStorage: MSSQLStorage<PiktogramyProduktu>
		{
				private string entitySelect = "SELECT ProductId, [Name], Locale, ImageUrl FROM vShpPiktogramyProduktu WHERE Locale=@Locale ";
				public PiktogramyProduktuStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static PiktogramyProduktu GetPiktogramyProduktu( DataRow record )
				{
						PiktogramyProduktu entiry = new PiktogramyProduktu();
						entiry.Id = Convert.ToInt32( record["ProductId"] );
						entiry.Name = Convert.ToString( record["Name"] );
						entiry.Locale = Convert.ToString( record["Locale"] );
						entiry.ImageUrl = Convert.ToString( record["ImageUrl"] );

						return entiry;
				}

				public override List<PiktogramyProduktu> Read( object criteria )
				{
						List<PiktogramyProduktu> list = new List<PiktogramyProduktu>();
						
						using ( SqlConnection connection = Connect() )
						{
								DataTable table = null;
								if ( criteria is PiktogramyProduktu.ReadByProduct )
								{
										string sql = entitySelect + " AND ProductId=@ProductId";
										table = Query<DataTable>( connection, sql, 
												new SqlParameter( "@Locale", Locale ),
												new SqlParameter( "@ProductId", (criteria as PiktogramyProduktu.ReadByProduct).ProductId ) );

										foreach ( DataRow dr in table.Rows )
												list.Add( GetPiktogramyProduktu( dr ) );
								}
								else
								{

										table = Query<DataTable>( connection, entitySelect, new SqlParameter( "@Locale", Locale ) );
										foreach ( DataRow dr in table.Rows )
												list.Add( GetPiktogramyProduktu( dr ) );
								}
								
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override void Create( PiktogramyProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( PiktogramyProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( PiktogramyProduktu entity )
				{
						throw new NotImplementedException();
				}
		}
}
