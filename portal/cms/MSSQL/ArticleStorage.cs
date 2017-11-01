using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ArticleStorage: MSSQLStorage<Article>
		{
				public ArticleStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Article GetArticle( DataRow record )
				{
						Article article = new Article();
						article.Id = Convert.ToInt32( record["ArticleId"] );
						article.InstanceId = Convert.ToInt32( record["InstanceId"] );
						article.Approved = NullableDBToBool( record["Approved"] );
						article.ArticleCategoryId = Convert.ToInt32( record["ArticleCategoryId"] );
						article.ArticleCategoryName = Convert.ToString( record["ArticleCategoryName"] );
						article.City = Convert.ToString( record["City"] );
						article.Content = Convert.ToString( record["Content"] );
						article.Country = Convert.ToString( record["Country"] );
						article.EnableComments = NullableDBToBool( record["EnableComments"] );
						article.CommentsCount = Convert.ToInt32( record["CommentsCount"] );
						article.ExpiredDate = ConvertNullable.ToDateTime( record["ExpiredDate"] );
						article.Icon = Convert.ToString( record["Icon"] );
						article.Locale = Convert.ToString( record["Locale"] );
						article.ReleaseDate = Convert.ToDateTime( record["ReleaseDate"] );
						article.RoleId = ConvertNullable.ToInt32( record["RoleId"] );
						article.Teaser = Convert.ToString( record["Teaser"] );
						article.Title = Convert.ToString( record["Title"] );
						article.Visible = NullableDBToBool( record["Visible"] );

						article.UrlAliasId = ConvertNullable.ToInt32( record["UrlAliasId"] );
						article.Alias = Convert.ToString( record["Alias"] );

						article.ViewCount = Convert.ToInt32( record["ViewCount"] );
						article.Votes = Convert.ToInt32( record["Votes"] );
						article.TotalRating = Convert.ToInt32( record["TotalRating"] );
						article.RatingResult = Convert.ToDouble( record["RatingResult"] );

						return article;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<Article> Read( object criteria )
				{
						if ( criteria is Article.ReadById ) return LoadById( criteria as Article.ReadById );
						if ( criteria is Article.ReadLatest ) return LoadLatest( criteria as Article.ReadLatest );
						if ( criteria is Article.ReadReleased ) return LoadReleased( criteria as Article.ReadReleased );
						List<Article> articles = new List<Article>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias,
										ArticleCategoryId, ArticleCategoryName, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
										Visible, ViewCount, Votes , TotalRating, RatingResult
								FROM vArticles
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										articles.Add( GetArticle( dr ) );
						}
						return articles;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Article> LoadById( Article.ReadById byArticleId )
				{
						List<Article> articles = new List<Article>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ArticleId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
										ArticleCategoryId, ArticleCategoryName, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
										Visible, ViewCount, Votes, TotalRating, RatingResult
								FROM vArticles 
								WHERE ArticleId = @ArticleId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ArticleId", byArticleId.ArticleId ) );
								foreach ( DataRow dr in table.Rows )
										articles.Add( GetArticle( dr ) );
						}
						return articles;
				}

				private List<Article> LoadReleased( Article.ReadReleased by )
				{
						List<Article> articles = new List<Article>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @AccountId IS NULL
										SELECT ArticleId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
												ArticleCategoryId, ArticleCategoryName, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vArticles
										WHERE Locale = @Locale AND InstanceId=@InstanceId AND RoleId IS NULL AND ArticleCategoryId = ISNULL(@CategoryId, ArticleCategoryId ) AND
												(@TagId IS NULL OR ArticleId IN ( SELECT ArticleId FROM vArticleTags WHERE TagId = @TagId )) AND
												ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC
								ELSE
										SELECT DISTINCT a.ArticleId, a.InstanceId, a.Locale, Icon, Title, Teaser, Content, a.RoleId, Country, [UrlAliasId], Alias, 
												ArticleCategoryId, ArticleCategoryName, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vArticles a
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(a.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @AccountId
										WHERE a.Locale = @Locale AND a.InstanceId=@InstanceId AND ArticleCategoryId = ISNULL(@CategoryId, ArticleCategoryId ) AND
												(@TagId IS NULL OR ArticleId IN ( SELECT ArticleId FROM vArticleTags WHERE TagId = @TagId )) AND
												ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@CategoryId", Null( by.CategoryId ) ),
										new SqlParameter( "@TagId", Null( by.TagId ) ),
										new SqlParameter( "@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
										new SqlParameter( "@AdministratorRoleName", Role.ADMINISTRATOR ),
										new SqlParameter( "@InstanceId", InstanceId )
										);

								foreach ( DataRow dr in table.Rows )
										articles.Add( GetArticle( dr ) );
						}
						return articles;
				}
				private List<Article> LoadLatest( Article.ReadLatest latest )
				{
						List<Article> articles = new List<Article>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = string.Format( @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @AccountId IS NULL
										SELECT TOP {0} InstanceId, ArticleId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
												ArticleCategoryId, ArticleCategoryName, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vArticles 
										WHERE Locale = @Locale AND InstanceId=@InstanceId AND RoleId IS NULL AND ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC
								ELSE
										SELECT TOP {0} a.InstanceId, a.ArticleId, a.Locale, a.Icon, a.Title, a.Teaser, a.Content, a.RoleId, a.Country, a.[UrlAliasId], a.Alias, 
												a.ArticleCategoryId, a.ArticleCategoryName, a.City, a.Approved, a.ReleaseDate, a.ExpiredDate, a.EnableComments, a.CommentsCount,
												a.Visible, a.ViewCount, a.Votes, a.TotalRating, a.RatingResult
										FROM vArticles a
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(a.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @AccountId
										WHERE a.Locale = @Locale AND a.InstanceId=@InstanceId AND a.ReleaseDate <= GETDATE() AND a.Visible = 1
										ORDER BY a.ReleaseDate DESC", latest.Count );
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@AccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
										new SqlParameter( "@AdministratorRoleName", Role.ADMINISTRATOR ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										articles.Add( GetArticle( dr ) );
						}
						return articles;
				}

				private void DeleteArticleTags( Article article )
				{
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"DELETE FROM tArticleTag WHERE ArticleId=@ArticleId";
								SqlParameter articleId = new SqlParameter( "@ArticleId", article.Id );
								Exec( connection, sql, articleId );
						}
				}

				private void UpdateArticleTags( Article article )
				{
						//Nacitanie povodnych hodnot
						List<ArticleTag> tags = article.ArticleTags;

						//Vymazanie starych hodnot
						DeleteArticleTags( article );

						using ( SqlConnection connection = Connect() )
						{
								foreach ( ArticleTag at in tags )
								{
										at.ArticleId = article.Id;
										Storage<ArticleTag>.Create( at );
								}
						}
				}

				public override void Create( Article article )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pArticleCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Approved", article.Approved ),
										new SqlParameter( "@ArticleCategoryId", article.ArticleCategoryId ),
										new SqlParameter( "@City", article.City ),
										new SqlParameter( "@Content", article.Content ),
										new SqlParameter( "@ContentKeywords", Null( article.ContentKeywords, String.Empty ) ),
										new SqlParameter( "@Country", article.Country ),
										new SqlParameter( "@EnableComments", article.EnableComments ),
										new SqlParameter( "@ExpiredDate", article.ExpiredDate ),
										new SqlParameter( "@Icon", article.Icon ),
										new SqlParameter( "@Locale", article.Locale ),
										new SqlParameter( "@ReleaseDate", article.ReleaseDate ),
										new SqlParameter( "@RoleId", article.RoleId ),
										new SqlParameter( "@Teaser", article.Teaser ),
										new SqlParameter( "@Title", article.Title ),
										new SqlParameter( "@UrlAliasId", article.UrlAliasId ),
										new SqlParameter( "@Visible", article.Visible ),
										result );

								article.Id = Convert.ToInt32( result.Value ); ;
						}

						UpdateArticleTags( article );
				}

				public override void Update( Article article )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pArticleModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@ArticleId", article.Id ),
										new SqlParameter( "@Approved", article.Approved ),
										new SqlParameter( "@ArticleCategoryId", article.ArticleCategoryId ),
										new SqlParameter( "@City", article.City ),
										new SqlParameter( "@Content", article.Content ),
										new SqlParameter( "@ContentKeywords", Null( article.ContentKeywords, String.Empty ) ),
										new SqlParameter( "@Country", article.Country ),
										new SqlParameter( "@EnableComments", article.EnableComments ),
										new SqlParameter( "@ExpiredDate", article.ExpiredDate ),
										new SqlParameter( "@Icon", article.Icon ),
										new SqlParameter( "@Locale", article.Locale ),
										new SqlParameter( "@ReleaseDate", article.ReleaseDate ),
										new SqlParameter( "@RoleId", article.RoleId ),
										new SqlParameter( "@Teaser", article.Teaser ),
										new SqlParameter( "@Title", article.Title ),
										new SqlParameter( "@UrlAliasId", article.UrlAliasId ),
										new SqlParameter( "@Visible", article.Visible ) );
						}

						UpdateArticleTags( article );
				}

				public override void Delete( Article article )
				{
						DeleteArticleTags( article );

						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter articleId = new SqlParameter( "@ArticleId", article.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pArticleDelete", result, historyAccount, articleId );
						}
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( Article.IncrementViewCountCommand ) )
								return IncrementViewCount( command as Article.IncrementViewCountCommand ) as R;

						if ( t == typeof( Article.IncrementVoteCommand ) )
								return IncrementVote( command as Article.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private Article.IncrementViewCountCommand IncrementViewCount( Article.IncrementViewCountCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter articleId = new SqlParameter( "@ArticleId", cmd.ArticleId );
								ExecProc( connection, "pArticleIncrementViewCount", articleId );
						}

						return cmd;
				}

				private Article.IncrementVoteCommand IncrementVote( Article.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter articleId = new SqlParameter( "@ArticleId", cmd.ArticleId );
								SqlParameter rating = new SqlParameter( "@Rating", cmd.Rating );
								ExecProc( connection, "pArticleIncrementVote", articleId, rating );

								AccountVote accountVote = new AccountVote();
								accountVote.AccountId = cmd.AccountId;
								accountVote.ObjectId = cmd.ArticleId;
								accountVote.ObjectTypeId = (int)Article.AccountVoteType;
								accountVote.Rating = cmd.Rating;
								Storage<AccountVote>.Create( accountVote );
						}

						return cmd;
				}

		}
}
