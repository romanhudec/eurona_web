using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using SHP.Entities;

namespace SHP.MSSQL
{
		public sealed class ProductReviewsStorage: MSSQLStorage<ProductReviews>
		{
				public ProductReviewsStorage( int instanceId, CMS.Entities.Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ProductReviews GetProductReviews( DataRow record )
				{
						ProductReviews reviews = new ProductReviews();
						reviews.Id = Convert.ToInt32( record["ProductReviewsId"] );
						reviews.InstanceId = Convert.ToInt32( record["InstanceId"] );
						reviews.ProductId = Convert.ToInt32( record["ProductId"] );
						reviews.ArticleId = Convert.ToInt32( record["ArticleId"] );
						reviews.City = Convert.ToString( record["City"] );
						reviews.Country = Convert.ToString( record["Country"] );
						reviews.Icon = Convert.ToString( record["Icon"] );
						reviews.ReleaseDate = Convert.ToDateTime( record["ReleaseDate"] );
						reviews.RoleId = ConvertNullable.ToInt32( record["RoleId"] );
						reviews.Teaser = Convert.ToString( record["Teaser"] );
						reviews.Title = Convert.ToString( record["Title"] );
						reviews.Alias = Convert.ToString( record["Alias"] );

						return reviews;
				}


				public override List<ProductReviews> Read( object criteria )
				{
						if ( criteria is ProductReviews.ReadById ) return LoadById( criteria as ProductReviews.ReadById );
						if ( criteria is ProductReviews.ReadByProduct ) return LoadByProduct( criteria as ProductReviews.ReadByProduct );
						List<ProductReviews> list = new List<ProductReviews>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProductReviewsId, ProductId, ArticleId, InstanceId, Icon, Title, Teaser, City, Country, ReleaseDate, RoleId, Alias
								FROM vShpProductReviews
								WHERE InstanceId=@InstanceId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductReviews( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ProductReviews> LoadById( ProductReviews.ReadById by )
				{
						List<ProductReviews> list = new List<ProductReviews>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProductReviewsId, ProductId, ArticleId, InstanceId, Icon, Title, Teaser, City, Country, ReleaseDate, RoleId, Alias
								FROM vShpProductReviews 
								WHERE ProductReviewsId = @ProductReviewsId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ProductReviewsId", Null(by.ProductReviewsId) ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductReviews( dr ) );
						}
						return list;
				}

				private List<ProductReviews> LoadByProduct( ProductReviews.ReadByProduct by )
				{
						List<ProductReviews> list = new List<ProductReviews>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @AccountId IS NULL
										SELECT ProductReviewsId, ProductId, ArticleId, InstanceId, Icon, Title, Teaser, City, Country, ReleaseDate, RoleId, Alias
										FROM vShpProductReviews 
										WHERE InstanceId=@InstanceId AND ProductId=@ProductId AND RoleId IS NULL AND ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC
								ELSE
										SELECT a.ProductReviewsId, a.ProductId, a.ArticleId, a.InstanceId, a.Icon, a.Title, a.Teaser, a.City, a.Country, a.ReleaseDate, a.RoleId, a.Alias
										FROM vShpProductReviews a
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(a.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @AccountId
										WHERE a.InstanceId=@InstanceId AND a.ProductId=@ProductId AND a.ReleaseDate <= GETDATE() AND a.Visible = 1
										ORDER BY a.ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
										new SqlParameter( "@AdministratorRoleName", CMS.Entities.Role.ADMINISTRATOR ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ProductId", Null( by.ProductId ) ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetProductReviews( dr ) );
						}
						return list;
				}

				public override void Create( ProductReviews reviews )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pShpProductReviewsCreate",
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ProductId", reviews.ProductId ),
										new SqlParameter( "@ArticleId", reviews.ArticleId ),
										result );

								reviews.Id = Convert.ToInt32( result.Value ); ;
						}
				}

				public override void Update( ProductReviews entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ProductReviews reviews )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter id = new SqlParameter( "@ProductReviewsId", reviews.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pShpProductReviewsDelete", result, id );
						}
				}

		}
}
