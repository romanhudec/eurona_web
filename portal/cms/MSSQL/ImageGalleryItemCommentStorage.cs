using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ImageGalleryItemCommentStorage: MSSQLStorage<ImageGalleryItemComment>
		{
				public ImageGalleryItemCommentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ImageGalleryItemComment GetComment( DataRow record )
				{
						ImageGalleryItemComment comment = new ImageGalleryItemComment();
						comment.Id = Convert.ToInt32( record["ImageGalleryItemCommentId"] );
						comment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						comment.CommentId = Convert.ToInt32( record["CommentId"] );
						comment.ImageGalleryItemId = Convert.ToInt32( record["ImageGalleryItemId"] );
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

				public override List<ImageGalleryItemComment> Read( object criteria )
				{
						if ( criteria is ImageGalleryItemComment.ReadByImageGalleryItemId ) return LoadByImageGalleryItemId( criteria as ImageGalleryItemComment.ReadByImageGalleryItemId );
						if ( criteria is ImageGalleryItemComment.ReadByCommentId ) return LoadByCommentId( criteria as ImageGalleryItemComment.ReadByCommentId );
						List<ImageGalleryItemComment> list = new List<ImageGalleryItemComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryItemCommentId], InstanceId, [ImageGalleryItemId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryItemComments WHERE InstanceId=@InstanceId
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


				private List<ImageGalleryItemComment> LoadByImageGalleryItemId( ImageGalleryItemComment.ReadByImageGalleryItemId byImageGalleryItemId )
				{
						List<ImageGalleryItemComment> list = new List<ImageGalleryItemComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryItemCommentId], InstanceId, [ImageGalleryItemId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryItemComments
								WHERE ImageGalleryItemId = @ImageGalleryItemId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ImageGalleryItemId", byImageGalleryItemId.ImageGalleryItemId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				private List<ImageGalleryItemComment> LoadByCommentId( ImageGalleryItemComment.ReadByCommentId byCommentId )
				{
						List<ImageGalleryItemComment> list = new List<ImageGalleryItemComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryItemCommentId], InstanceId, [ImageGalleryItemId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryItemComments
								WHERE CommentId = @CommentId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CommentId", byCommentId.CommentId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				public override void Create( ImageGalleryItemComment comment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pImageGalleryItemCommentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ImageGalleryItemId", comment.ImageGalleryItemId ),
										new SqlParameter( "@AccountId", comment.AccountId ),
										new SqlParameter( "@ParentId", Null(comment.ParentId) ),
										new SqlParameter( "@Title", comment.Title ),
										new SqlParameter( "@Content", comment.Content ) );
						}
				}

				public override void Update( ImageGalleryItemComment comment )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ImageGalleryItemComment comment )
				{
						throw new NotImplementedException();
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( ImageGalleryItemComment.IncrementVoteCommand ) )
								return IncrementVote( command as ImageGalleryItemComment.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private ImageGalleryItemComment.IncrementVoteCommand IncrementVote( ImageGalleryItemComment.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								Comment.IncrementVoteCommand cmdC = new Comment.IncrementVoteCommand();
								cmdC.AccountId = cmd.AccountId;
								cmdC.CommentId = cmd.CommentId;
								cmdC.ObjectTypeId = (int)ImageGalleryItemComment.AccountVoteType;
								cmdC.Rating = cmd.Rating;
								Storage<Comment>.Execute( cmdC );
						}

						return cmd;
				}


		}
}
