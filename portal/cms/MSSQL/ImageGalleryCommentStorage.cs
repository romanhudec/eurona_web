using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ImageGalleryCommentStorage: MSSQLStorage<ImageGalleryComment>
		{
				public ImageGalleryCommentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ImageGalleryComment GetComment( DataRow record )
				{
						ImageGalleryComment comment = new ImageGalleryComment();
						comment.Id = Convert.ToInt32( record["ImageGalleryCommentId"] );
						comment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						comment.CommentId = Convert.ToInt32( record["CommentId"] );
						comment.ImageGalleryId = Convert.ToInt32( record["ImageGalleryId"] );
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

				public override List<ImageGalleryComment> Read( object criteria )
				{
						if ( criteria is ImageGalleryComment.ReadByImageGalleryId ) return LoadByImageGalleryId( criteria as ImageGalleryComment.ReadByImageGalleryId );
						if ( criteria is ImageGalleryComment.ReadByCommentId ) return LoadByCommentId( criteria as ImageGalleryComment.ReadByCommentId );
						List<ImageGalleryComment> list = new List<ImageGalleryComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryCommentId], InstanceId, [ImageGalleryId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryComments WHERE InstanceId=@InstanceId
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


				private List<ImageGalleryComment> LoadByImageGalleryId( ImageGalleryComment.ReadByImageGalleryId byImageGalleryId )
				{
						List<ImageGalleryComment> list = new List<ImageGalleryComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryCommentId], InstanceId, [ImageGalleryId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryComments
								WHERE ImageGalleryId = @ImageGalleryId AND InstanceId = @InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ImageGalleryId", byImageGalleryId.ImageGalleryId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				private List<ImageGalleryComment> LoadByCommentId( ImageGalleryComment.ReadByCommentId byCommentId )
				{
						List<ImageGalleryComment> list = new List<ImageGalleryComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ImageGalleryCommentId], InstanceId, [ImageGalleryId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vImageGalleryComments
								WHERE CommentId = @CommentId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CommentId", byCommentId.CommentId ), new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				public override void Create( ImageGalleryComment comment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pImageGalleryCommentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ImageGalleryId", comment.ImageGalleryId ),
										new SqlParameter( "@AccountId", comment.AccountId ),
										new SqlParameter( "@ParentId", Null(comment.ParentId) ),
										new SqlParameter( "@Title", comment.Title ),
										new SqlParameter( "@Content", comment.Content ) );
						}
				}

				public override void Update( ImageGalleryComment comment )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ImageGalleryComment comment )
				{
						throw new NotImplementedException();
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( ImageGalleryComment.IncrementVoteCommand ) )
								return IncrementVote( command as ImageGalleryComment.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private ImageGalleryComment.IncrementVoteCommand IncrementVote( ImageGalleryComment.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								Comment.IncrementVoteCommand cmdC = new Comment.IncrementVoteCommand();
								cmdC.AccountId = cmd.AccountId;
								cmdC.CommentId = cmd.CommentId;
								cmdC.ObjectTypeId = (int)ImageGalleryComment.AccountVoteType;
								cmdC.Rating = cmd.Rating;
								Storage<Comment>.Execute( cmdC );
						}

						return cmd;
				}


		}
}
