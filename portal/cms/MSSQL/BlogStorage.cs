using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class BlogStorage: MSSQLStorage<Blog>
		{
				public BlogStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Blog GetBlog( DataRow record )
				{
						Blog blog = new Blog();
						blog.Id = Convert.ToInt32( record["BlogId"] );
						blog.InstanceId = Convert.ToInt32( record["InstanceId"] );
						blog.Approved = NullableDBToBool( record["Approved"] );
						blog.AccountId = Convert.ToInt32( record["AccountId"] );
						blog.UserName = Convert.ToString( record["Login"] );
						blog.City = Convert.ToString( record["City"] );
						blog.Content = Convert.ToString( record["Content"] );
						blog.Country = Convert.ToString( record["Country"] );
						blog.EnableComments = NullableDBToBool( record["EnableComments"] );
						blog.CommentsCount = Convert.ToInt32( record["CommentsCount"] );
						blog.ExpiredDate = ConvertNullable.ToDateTime( record["ExpiredDate"] );
						blog.Icon = Convert.ToString( record["Icon"] );
						blog.Locale = Convert.ToString( record["Locale"] );
						blog.ReleaseDate = Convert.ToDateTime( record["ReleaseDate"] );
						blog.RoleId = ConvertNullable.ToInt32( record["RoleId"] );
						blog.Teaser = Convert.ToString( record["Teaser"] );
						blog.Title = Convert.ToString( record["Title"] );
						blog.Visible = NullableDBToBool( record["Visible"] );

						blog.UrlAliasId = ConvertNullable.ToInt32( record["UrlAliasId"] );
						blog.Alias = Convert.ToString( record["Alias"] );

						blog.ViewCount = Convert.ToInt32( record["ViewCount"] );
						blog.Votes = Convert.ToInt32( record["Votes"] );
						blog.TotalRating = Convert.ToInt32( record["TotalRating"] );
						blog.RatingResult = Convert.ToDouble( record["RatingResult"] );

						return blog;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<Blog> Read( object criteria )
				{
						if ( criteria is Blog.ReadById ) return LoadById( criteria as Blog.ReadById );
						if ( criteria is Blog.ReadByAccountId ) return LoadByAccountId( criteria as Blog.ReadByAccountId );
						if ( criteria is Blog.ReadLatest ) return LoadLatest( criteria as Blog.ReadLatest );
						if ( criteria is Blog.ReadReleased ) return LoadReleased( criteria as Blog.ReadReleased );
						List<Blog> blogs = new List<Blog>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT BlogId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
										AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
										Visible, ViewCount, Votes , TotalRating, RatingResult
								FROM vBlogs
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@Locale", Locale ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										blogs.Add( GetBlog( dr ) );
						}
						return blogs;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Blog> LoadById( Blog.ReadById byBlogId )
				{
						List<Blog> blogs = new List<Blog>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	BlogId, InstanceId,Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
										AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
										Visible, ViewCount, Votes, TotalRating, RatingResult
								FROM vBlogs
								WHERE BlogId = @BlogId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@BlogId", byBlogId.BlogId ) );
								foreach ( DataRow dr in table.Rows )
										blogs.Add( GetBlog( dr ) );
						}
						return blogs;
				}

				private List<Blog> LoadByAccountId( Blog.ReadByAccountId byAccountId )
				{
						List<Blog> blogs = new List<Blog>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT	BlogId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
										AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
										Visible, ViewCount, Votes, TotalRating, RatingResult
								FROM vBlogs
								WHERE Locale = @Locale AND AccountId = @AccountId AND InstanceId=@InstanceId
								ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@AccountId", byAccountId.AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										blogs.Add( GetBlog( dr ) );
						}
						return blogs;
				}

				/// <summary>
				/// Vrati zoznam blogov, ktore maju povolene zverejnenie pre prihlaseneho pouzivatela v roli.
				/// </summary>
				private List<Blog> LoadReleased( Blog.ReadReleased by )
				{
						List<Blog> blogs = new List<Blog>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @LogedAccountId IS NULL
										SELECT BlogId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
												AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vBlogs 
										WHERE Locale = @Locale AND InstanceId=@InstanceId AND RoleId IS NULL AND AccountId = ISNULL(@AccountId, AccountId ) AND
												(@TagId IS NULL OR BlogId IN ( SELECT BlogId FROM vBlogTags WHERE TagId = @TagId )) AND
												ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC
								ELSE
										SELECT DISTINCT BlogId, b.InstanceId, Locale, Icon, Title, Teaser, Content, b.RoleId, Country, [UrlAliasId], Alias, 
												b.AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vBlogs b
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(b.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @LogedAccountId
										WHERE b.Locale = @Locale AND b.InstanceId=@InstanceId AND  b.AccountId = ISNULL(@AccountId, b.AccountId ) AND
												(@TagId IS NULL OR BlogId IN ( SELECT BlogId FROM vBlogTags WHERE TagId = @TagId )) AND
												ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@AccountId", Null( by.AccountId ) ),
										new SqlParameter( "@TagId", Null( by.TagId ) ),
										new SqlParameter( "@LogedAccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
										new SqlParameter( "@AdministratorRoleName", Role.ADMINISTRATOR ) );

								foreach ( DataRow dr in table.Rows )
										blogs.Add( GetBlog( dr ) );
						}
						return blogs;
				}

				/// <summary>
				/// Vrati zoznam poslednych blogov, ktore maju povolene zverejnenie.
				/// </summary>
				private List<Blog> LoadLatest( Blog.ReadLatest latest )
				{
						List<Blog> blogs = new List<Blog>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = string.Format( @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @LogedAccountId IS NULL
										SELECT TOP {0}	BlogId, InstanceId, Locale, Icon, Title, Teaser, Content, RoleId, Country, [UrlAliasId], Alias, 
												AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vBlogs 
										WHERE Locale = @Locale AND InstanceId=@InstanceId AND RoleId IS NULL AND ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC
								ELSE
										SELECT TOP {0}	BlogId, b.InstanceId, b.Locale, Icon, Title, Teaser, Content, b.RoleId, Country, b.[UrlAliasId], b.Alias, 
												b.AccountId, Login, City, Approved, ReleaseDate, ExpiredDate, EnableComments, CommentsCount,
												Visible, ViewCount, Votes, TotalRating, RatingResult
										FROM vBlogs b
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(b.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @LogedAccountId
										WHERE b.Locale = @Locale AND b.InstanceId=@InstanceId AND ReleaseDate <= GETDATE() AND Visible = 1
										ORDER BY ReleaseDate DESC", latest.Count );
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@LogedAccountId", Account != null ? (object)Account.Id : (object)DBNull.Value ),
										new SqlParameter( "@AdministratorRoleName", Role.ADMINISTRATOR ) );
								foreach ( DataRow dr in table.Rows )
										blogs.Add( GetBlog( dr ) );
						}
						return blogs;
				}

				private void DeleteBlogTags( Blog blog )
				{
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"DELETE FROM tBlogTag WHERE BlogId=@BlogId";
								SqlParameter blogId = new SqlParameter( "@BlogId", blog.Id );
								Exec( connection, sql, blogId );
						}
				}

				private void UpdateBlogTags( Blog blog )
				{
						//Nacitanie povodnych hodnot
						List<BlogTag> tags = blog.BlogTags;

						//Vymazanie starych hodnot
						DeleteBlogTags( blog );

						using ( SqlConnection connection = Connect() )
						{
								foreach ( BlogTag at in tags )
								{
										at.BlogId = blog.Id;
										Storage<BlogTag>.Create( at );
								}
						}
				}

				public override void Create( Blog blog )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pBlogCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Approved", blog.Approved ),
										new SqlParameter( "@AccountId", blog.AccountId ),
										new SqlParameter( "@City", blog.City ),
										new SqlParameter( "@Content", blog.Content ),
										new SqlParameter( "@ContentKeywords", Null( blog.ContentKeywords, String.Empty ) ),
										new SqlParameter( "@Country", blog.Country ),
										new SqlParameter( "@EnableComments", blog.EnableComments ),
										new SqlParameter( "@ExpiredDate", blog.ExpiredDate ),
										new SqlParameter( "@Icon", blog.Icon ),
										new SqlParameter( "@Locale", blog.Locale ),
										new SqlParameter( "@ReleaseDate", blog.ReleaseDate ),
										new SqlParameter( "@RoleId", blog.RoleId ),
										new SqlParameter( "@UrlAliasId", blog.UrlAliasId ),
										new SqlParameter( "@Teaser", blog.Teaser ),
										new SqlParameter( "@Title", blog.Title ),
										new SqlParameter( "@Visible", blog.Visible ),
										result );

								blog.Id = Convert.ToInt32( result.Value );
						}

						UpdateBlogTags( blog );
				}

				public override void Update( Blog blog )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pBlogModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@BlogId", blog.Id ),
										new SqlParameter( "@Approved", blog.Approved ),
										new SqlParameter( "@AccountId", blog.AccountId ),
										new SqlParameter( "@City", blog.City ),
										new SqlParameter( "@Content", blog.Content ),
										new SqlParameter( "@ContentKeywords", Null( blog.ContentKeywords, String.Empty ) ),
										new SqlParameter( "@Country", blog.Country ),
										new SqlParameter( "@EnableComments", blog.EnableComments ),
										new SqlParameter( "@ExpiredDate", blog.ExpiredDate ),
										new SqlParameter( "@Icon", blog.Icon ),
										new SqlParameter( "@Locale", blog.Locale ),
										new SqlParameter( "@ReleaseDate", blog.ReleaseDate ),
										new SqlParameter( "@RoleId", blog.RoleId ),
										new SqlParameter( "@UrlAliasId", blog.UrlAliasId ),
										new SqlParameter( "@Teaser", blog.Teaser ),
										new SqlParameter( "@Title", blog.Title ),
										new SqlParameter( "@Visible", blog.Visible ) );
						}

						UpdateBlogTags( blog );
				}

				public override void Delete( Blog blog )
				{
						DeleteBlogTags( blog );

						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter blogId = new SqlParameter( "@BlogId", blog.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pBlogDelete", result, historyAccount, blogId );
						}
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( Blog.IncrementViewCountCommand ) )
								return IncrementViewCount( command as Blog.IncrementViewCountCommand ) as R;

						if ( t == typeof( Blog.IncrementVoteCommand ) )
								return IncrementVote( command as Blog.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private Blog.IncrementViewCountCommand IncrementViewCount( Blog.IncrementViewCountCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter blogId = new SqlParameter( "@BlogId", cmd.BlogId );
								ExecProc( connection, "pBlogIncrementViewCount", blogId );
						}

						return cmd;
				}

				private Blog.IncrementVoteCommand IncrementVote( Blog.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter blogId = new SqlParameter( "@BlogId", cmd.BlogId );
								SqlParameter rating = new SqlParameter( "@Rating", cmd.Rating );
								ExecProc( connection, "pBlogIncrementVote", blogId, rating );

								AccountVote accountVote = new AccountVote();
								accountVote.AccountId = cmd.AccountId;
								accountVote.ObjectId = cmd.BlogId;
								accountVote.ObjectTypeId = (int)Blog.AccountVoteType;
								accountVote.Rating = cmd.Rating;
								Storage<AccountVote>.Create( accountVote );
						}

						return cmd;
				}

		}
}
