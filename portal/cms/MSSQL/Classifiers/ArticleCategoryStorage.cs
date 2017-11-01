using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using CMS.Entities.Classifiers;

namespace CMS.MSSQL.Classifiers
{
		internal sealed class ArticleCategoryStorage: MSSQLStorage<ArticleCategory>
		{
				public ArticleCategoryStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ArticleCategory GetArticleCategory( DataRow record )
				{
						ArticleCategory articleCategory = new ArticleCategory();
						articleCategory.Id = Convert.ToInt32( record["ArticleCategoryId"] );
						articleCategory.InstanceId = Convert.ToInt32( record["InstanceId"] );
						articleCategory.Name = Convert.ToString( record["Name"] );
						articleCategory.Code = Convert.ToString( record["Code"] );
						articleCategory.Locale = Convert.ToString( record["Locale"] );
						articleCategory.Notes = Convert.ToString( record["Notes"] );
						articleCategory.ArticlesInCategory = Convert.ToInt32( record["ArticlesInCategory"] );

						return articleCategory;
				}

				public override List<ArticleCategory> Read( object criteria )
				{
						if ( criteria is ArticleCategory.ReadById ) return LoadById( criteria as ArticleCategory.ReadById );
						List<ArticleCategory> list = new List<ArticleCategory>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleCategoryId, InstanceId, [Name], [Code], [Locale], [Notes], [ArticlesInCategory]
								FROM vArticleCategories
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetArticleCategory( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ArticleCategory> LoadById( ArticleCategory.ReadById by )
				{
						List<ArticleCategory> list = new List<ArticleCategory>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleCategoryId, InstanceId, [Name], [Code], [Locale], [Notes], [ArticlesInCategory]
								FROM vArticleCategories
								WHERE ArticleCategoryId = @ArticleCategoryId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ArticleCategoryId", by.Id ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetArticleCategory( dr ) );
						}
						return list;
				}

				public override void Create( ArticleCategory articleCategory )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pArticleCategoryCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Name", articleCategory.Name ),
										new SqlParameter( "@Code", articleCategory.Code ),
										new SqlParameter( "@Locale", articleCategory.Locale ),
										new SqlParameter( "@Notes", articleCategory.Notes ) );
						}
				}

				public override void Update( ArticleCategory articleCategory )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pArticleCategoryModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@ArticleCategoryId", articleCategory.Id ),
										new SqlParameter( "@Name", articleCategory.Name ),
										new SqlParameter( "@Code", articleCategory.Code ),
										new SqlParameter( "@Notes", articleCategory.Notes ) );
						}
				}

				public override void Delete( ArticleCategory articleCategory )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter addressId = new SqlParameter( "@ArticleCategoryId", articleCategory.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pArticleCategoryDelete", result, historyAccount, addressId );
						}
				}
		}
}
