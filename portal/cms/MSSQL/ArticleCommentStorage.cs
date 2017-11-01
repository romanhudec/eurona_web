using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ArticleCommentStorage: MSSQLStorage<ArticleComment>
		{
				public ArticleCommentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ArticleComment GetComment( DataRow record )
				{
						ArticleComment comment = new ArticleComment();
						comment.Id = Convert.ToInt32( record["ArticleCommentId"] );
						comment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						comment.CommentId = Convert.ToInt32( record["CommentId"] );
						comment.ArticleId = Convert.ToInt32( record["ArticleId"] );
						comment.ParentId = ConvertNullable.ToInt32( record["ParentId"] );
						comment.Date = Convert.ToDateTime( record["Date"] );
						comment.AccountId = Convert.ToInt32( record["AccountId"] );
						comment.AccountName = Convert.ToString( record["AccountName"] );
						comment.Title = Convert.ToString( record["Title"] );
						comment.Content = Convert.ToString( record["Content"] );
						
						comment.Votes = Convert.ToInt32( record["Votes"] );
						comment.TotalRating = Convert.ToInt32( record["TotalRating"] );
						comment.RatingResult = Convert.ToDouble( record["RatingResult"] );
						return comment;
				}

				public override List<ArticleComment> Read( object criteria )
				{
						if ( criteria is ArticleComment.ReadByArticleId ) return LoadByArticleId( criteria as ArticleComment.ReadByArticleId );
						if ( criteria is ArticleComment.ReadByCommentId ) return LoadByCommentId( criteria as ArticleComment.ReadByCommentId );
						List<ArticleComment> list = new List<ArticleComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ArticleCommentId], InstanceId, [ArticleId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vArticleComments WHERE InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				private List<ArticleComment> LoadByArticleId( ArticleComment.ReadByArticleId byArticleId )
				{
						List<ArticleComment> list = new List<ArticleComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ArticleCommentId], InstanceId, [ArticleId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vArticleComments
								WHERE ArticleId = @ArticleId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ArticleId", byArticleId.ArticleId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				private List<ArticleComment> LoadByCommentId( ArticleComment.ReadByCommentId byCommentId )
				{
						List<ArticleComment> list = new List<ArticleComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ArticleCommentId], InstanceId, [ArticleId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vArticleComments
								WHERE CommentId = @CommentId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CommentId", byCommentId.CommentId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				public override void Create( ArticleComment comment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pArticleCommentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ArticleId", comment.ArticleId ),
										new SqlParameter( "@AccountId", comment.AccountId ),
										new SqlParameter( "@ParentId", Null(comment.ParentId) ),
										new SqlParameter( "@Title", comment.Title ),
										new SqlParameter( "@Content", comment.Content ) );
						}
				}

				public override void Update( ArticleComment comment )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ArticleComment comment )
				{
						throw new NotImplementedException();
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( ArticleComment.IncrementVoteCommand ) )
								return IncrementVote( command as ArticleComment.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private ArticleComment.IncrementVoteCommand IncrementVote( ArticleComment.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								Comment.IncrementVoteCommand cmdC = new Comment.IncrementVoteCommand();
								cmdC.AccountId = cmd.AccountId;
								cmdC.CommentId = cmd.CommentId;
								cmdC.ObjectTypeId = (int)ArticleComment.AccountVoteType;
								cmdC.Rating = cmd.Rating;
								Storage<Comment>.Execute( cmdC );
						}

						return cmd;
				}


		}
}
