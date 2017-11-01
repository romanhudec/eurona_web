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
		public sealed class VlastnostiProduktuStorage: MSSQLStorage<VlastnostiProduktu>
		{
				private string entitySelect = "SELECT ProductId, [Name], Locale, ImageUrl FROM vShpVlastnostiProduktu WHERE Locale=@Locale ";
				public VlastnostiProduktuStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static VlastnostiProduktu GetVlastnostiProduktu( DataRow record )
				{
						VlastnostiProduktu entiry = new VlastnostiProduktu();
						entiry.Id = Convert.ToInt32( record["ProductId"] );
						entiry.Name = Convert.ToString( record["Name"] );
						entiry.Locale = Convert.ToString( record["Locale"] );
						entiry.ImageUrl = Convert.ToString( record["ImageUrl"] );

						return entiry;
				}

				public override List<VlastnostiProduktu> Read( object criteria )
				{
						List<VlastnostiProduktu> list = new List<VlastnostiProduktu>();
						
						using ( SqlConnection connection = Connect() )
						{
								DataTable table = null;
								if ( criteria is VlastnostiProduktu.ReadByProduct )
								{
										string sql =  entitySelect + " AND ProductId=@ProductId";
										table = Query<DataTable>( connection, sql, 
												new SqlParameter( "@Locale", Locale ),
												new SqlParameter( "@ProductId", (criteria as VlastnostiProduktu.ReadByProduct).ProductId ) );

										foreach ( DataRow dr in table.Rows )
												list.Add( GetVlastnostiProduktu( dr ) );
								}
								else
								{

										table = Query<DataTable>( connection, entitySelect, new SqlParameter( "@Locale", Locale ) );
										foreach ( DataRow dr in table.Rows )
												list.Add( GetVlastnostiProduktu( dr ) );
								}
								
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override void Create( VlastnostiProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Update( VlastnostiProduktu entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( VlastnostiProduktu entity )
				{
						throw new NotImplementedException();
				}
		}
}
