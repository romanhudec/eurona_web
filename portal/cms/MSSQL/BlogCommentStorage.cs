using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class BlogCommentStorage: MSSQLStorage<BlogComment>
		{
				public BlogCommentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static BlogComment GetComment( DataRow record )
				{
						BlogComment comment = new BlogComment();
						comment.Id = Convert.ToInt32( record["BlogCommentId"] );
						comment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						comment.CommentId = Convert.ToInt32( record["CommentId"] );
						comment.BlogId = Convert.ToInt32( record["BlogId"] );
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

				public override List<BlogComment> Read( object criteria )
				{
						if ( criteria is BlogComment.ReadByBlogId ) return LoadByBlogId( criteria as BlogComment.ReadByBlogId );
						if ( criteria is BlogComment.ReadByCommentId ) return LoadByCommentId( criteria as BlogComment.ReadByCommentId );
						List<BlogComment> list = new List<BlogComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [BlogCommentId], InstanceId, [BlogId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vBlogComments WHERE InstanceId=@InstanceId
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


				private List<BlogComment> LoadByBlogId( BlogComment.ReadByBlogId byBlogId )
				{
						List<BlogComment> list = new List<BlogComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [BlogCommentId], InstanceId, [BlogId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vBlogComments
								WHERE BlogId = @BlogId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@BlogId", byBlogId.BlogId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				private List<BlogComment> LoadByCommentId( BlogComment.ReadByCommentId byCommentId )
				{
						List<BlogComment> list = new List<BlogComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [BlogCommentId], InstanceId, [BlogId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vBlogComments
								WHERE CommentId = @CommentId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CommentId", byCommentId.CommentId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				public override void Create( BlogComment comment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pBlogCommentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@BlogId", comment.BlogId ),
										new SqlParameter( "@AccountId", comment.AccountId ),
										new SqlParameter( "@ParentId", Null(comment.ParentId) ),
										new SqlParameter( "@Title", comment.Title ),
										new SqlParameter( "@Content", comment.Content ) );
						}
				}

				public override void Update( BlogComment comment )
				{
						throw new NotImplementedException();
				}

				public override void Delete( BlogComment comment )
				{
						throw new NotImplementedException();
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( BlogComment.IncrementVoteCommand ) )
								return IncrementVote( command as BlogComment.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private BlogComment.IncrementVoteCommand IncrementVote( BlogComment.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								Comment.IncrementVoteCommand cmdC = new Comment.IncrementVoteCommand();
								cmdC.AccountId = cmd.AccountId;
								cmdC.CommentId = cmd.CommentId;
								cmdC.ObjectTypeId = (int)BlogComment.AccountVoteType;
								cmdC.Rating = cmd.Rating;
								Storage<Comment>.Execute( cmdC );
						}

						return cmd;
				}


		}
}
