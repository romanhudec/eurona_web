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
		public sealed class UcinkyProduktuStorage: MSSQLStorage<UcinkyProduktu>
		{
				private string entitySelect = "SELECT ProductId, [Name], Locale, ImageUrl FROM vShpUcinkyProduktu WHERE Locale=@Locale ";
				public UcinkyProduktuStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static UcinkyProduktu GetUcinkyProduktu( DataRow record )
				{
						UcinkyProduktu entiry = new UcinkyProduktu();
						entiry.Id = Convert.ToInt32( record["ProductId"] );
						entiry.Name = Convert.ToString( record["Name"] );
						entiry.Locale = Convert.ToString( record["Locale"] );
						entiry.ImageUrl = Convert.ToString( record["ImageUrl"] );

						return entiry;
				}

				public override List<UcinkyProduktu> Read( object criteria )
				{
						List<UcinkyProduktu> list = new List<UcinkyProduktu>();

						using ( SqlConnection connection = Connect() )
						{
								DataTable table = null;
								if ( criteria is UcinkyProduktu.ReadByProduct )
								{
										string sql = entitySelect + " AND ProductId=@ProductId";
										table = Query<DataTable>( connection, sql,
												new SqlParameter( "@Locale", Locale ),
												new SqlParameter( "@ProductId", ( criteria as UcinkyProduktu.ReadByProduct ).ProductId ) );
										
										foreach ( DataRow dr in table.Rows )
												list.Add( GetUcinkyProduktu( dr ) );
								}
								else
								{

										table = Query<DataTable>( connection, entitySelect, new SqlParameter( "@Locale", Locale ) );
										foreach ( DataRow dr in table.Rows )
												list.Add( GetUcinkyProduktu( dr ) );
								}

						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override void Create( UcinkyProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( UcinkyProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( UcinkyProduktu entity )
				{
						throw new NotImplementedException();
				}
		}
}
