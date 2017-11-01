using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;

namespace SHP.MSSQL
{
		public sealed class ProductCategoriesStorage: MSSQLStorage<ProductCategories>
		{
				public ProductCategoriesStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ProductCategories GetProductCategories( DataRow record )
				{
						ProductCategories productCategory = new ProductCategories();
						productCategory.Id = 0;
						productCategory.InstanceId = Convert.ToInt32( record["InstanceId"] );
						productCategory.CategoryId = Convert.ToInt32( record["CategoryId"] );
						productCategory.ProductId = Convert.ToInt32( record["ProductId"] );
						return productCategory;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<ProductCategories> Read( object criteria )
				{
						if ( criteria is ProductCategories.ReadByCategoryId ) return LoadByCategoryId( criteria as ProductCategories.ReadByCategoryId );
						if ( criteria is ProductCategories.ReadByProductId ) return LoadByProductId( criteria as ProductCategories.ReadByProductId );
						List<ProductCategories> list = new List<ProductCategories>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT InstanceId, ProductId, CategoryId
								FROM tShpProductCategories 
								WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductCategories( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ProductCategories> LoadByCategoryId( ProductCategories.ReadByCategoryId by )
				{
						List<ProductCategories> list = new List<ProductCategories>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT InstanceId, ProductId, CategoryId
								FROM tShpProductCategories
								WHERE CategoryId = @CategoryId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CategoryId", by.CategoryId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductCategories( dr ) );
						}
						return list;
				}

				private List<ProductCategories> LoadByProductId( ProductCategories.ReadByProductId by )
				{
						List<ProductCategories> list = new List<ProductCategories>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT InstanceId, ProductId, CategoryId
								FROM tShpProductCategories
								WHERE ProductId = @ProductId AND InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ProductId", by.ProductId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductCategories( dr ) );
						}
						return list;
				}

				public override void Create( ProductCategories productCategory )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pShpProductCategoriesCreate",
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ProductId", productCategory.ProductId ),
										new SqlParameter( "@CategoryId", productCategory.CategoryId ),
										result );

								productCategory.Id = Convert.ToInt32( result.Value );
						}

				}

				public override void Update( ProductCategories productCategory )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ProductCategories productCategory )
				{
						throw new NotImplementedException();
				}
		}
}
