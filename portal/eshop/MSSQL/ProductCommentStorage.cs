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
		public sealed class ProductCommentStorage: MSSQLStorage<ProductComment>
		{
				public ProductCommentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ProductComment GetComment( DataRow record )
				{
						ProductComment comment = new ProductComment();
						comment.Id = Convert.ToInt32( record["ProductCommentId"] );
						comment.InstanceId = Convert.ToInt32( record["InstanceId"] );
						comment.CommentId = Convert.ToInt32( record["CommentId"] );
						comment.ProductId = Convert.ToInt32( record["ProductId"] );
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

				public override List<ProductComment> Read( object criteria )
				{
						if ( criteria is ProductComment.ReadById ) return LoadById( criteria as ProductComment.ReadById );
						if ( criteria is ProductComment.ReadByProductId ) return LoadByProductId( criteria as ProductComment.ReadByProductId );
						if ( criteria is ProductComment.ReadByCommentId ) return LoadByCommentId( criteria as ProductComment.ReadByCommentId );
						List<ProductComment> list = new List<ProductComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ProductCommentId], InstanceId, [ProductId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vShpProductComments 
								WHERE InstanceId=@InstanceId
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

				private List<ProductComment> LoadById( ProductComment.ReadById by )
				{
						List<ProductComment> list = new List<ProductComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ProductCommentId], InstanceId, [ProductId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vShpProductComments
								WHERE ProductCommentId = @ProductCommentId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ProductCommentId", by.ProductCommentId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}
				private List<ProductComment> LoadByProductId( ProductComment.ReadByProductId byProductId )
				{
						List<ProductComment> list = new List<ProductComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ProductCommentId], InstanceId, [ProductId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vShpProductComments
								WHERE ProductId = @ProductId AND InstanceId=@InstanceId
								ORDER BY [Date] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ProductId", byProductId.ProductId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetComment( dr ) );
						}
						return list;
				}

				private List<ProductComment> LoadByCommentId( ProductComment.ReadByCommentId byCommentId )
				{
						List<ProductComment> list = new List<ProductComment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT [ProductCommentId], InstanceId, [ProductId], [CommentId], [ParentId], [AccountId], [AccountName], [Date], [Title], [Content], 
										Votes , TotalRating, RatingResult
								FROM vShpProductComments
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

				public override void Create( ProductComment comment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpProductCommentCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@ProductId", comment.ProductId ),
										new SqlParameter( "@AccountId", comment.AccountId ),
										new SqlParameter( "@ParentId", Null( comment.ParentId ) ),
										new SqlParameter( "@Title", comment.Title ),
										new SqlParameter( "@Content", comment.Content ) );
						}
				}

				public override void Update( ProductComment comment )
				{
						throw new NotImplementedException();
				}

				public override void Delete( ProductComment comment )
				{
						throw new NotImplementedException();
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( ProductComment.IncrementVoteCommand ) )
								return IncrementVote( command as ProductComment.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private ProductComment.IncrementVoteCommand IncrementVote( ProductComment.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								Comment.IncrementVoteCommand cmdC = new Comment.IncrementVoteCommand();
								cmdC.AccountId = cmd.AccountId;
								cmdC.CommentId = cmd.CommentId;
								cmdC.ObjectTypeId = Convert.ToInt32( ProductComment.AccountVoteType );
								cmdC.Rating = cmd.Rating;
								Storage<Comment>.Execute( cmdC );
						}

						return cmd;
				}


		}
}
