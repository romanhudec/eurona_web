using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ImageGalleryItemStorage: MSSQLStorage<ImageGalleryItem>
		{
				public ImageGalleryItemStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ImageGalleryItem GetImageGalleryItem( DataRow record )
				{
						ImageGalleryItem imageGalleryItem = new ImageGalleryItem();
						imageGalleryItem.Id = Convert.ToInt32( record["ImageGalleryItemId"] );
						imageGalleryItem.InstanceId = Convert.ToInt32( record["InstanceId"] );
						imageGalleryItem.ImageGalleryId = Convert.ToInt32( record["ImageGalleryId"] );
						imageGalleryItem.Date = Convert.ToDateTime( record["Date"] );
						imageGalleryItem.VirtualPath = Convert.ToString( record["VirtualPath"] );
						imageGalleryItem.VirtualThumbnailPath = Convert.ToString( record["VirtualThumbnailPath"] );
						imageGalleryItem.Position = Convert.ToInt32( record["Position"] );
						imageGalleryItem.Description = Convert.ToString( record["Description"] );

						imageGalleryItem.CommentsCount = Convert.ToInt32( record["CommentsCount"] );
						imageGalleryItem.ViewCount = Convert.ToInt32( record["ViewCount"] );
						imageGalleryItem.Votes = Convert.ToInt32( record["Votes"] );
						imageGalleryItem.TotalRating = Convert.ToInt32( record["TotalRating"] );
						imageGalleryItem.RatingResult = Convert.ToDouble( record["RatingResult"] );

						return imageGalleryItem;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<ImageGalleryItem> Read( object criteria )
				{
						if ( criteria is ImageGalleryItem.ReadById ) return LoadById( criteria as ImageGalleryItem.ReadById );
						if ( criteria is ImageGalleryItem.ReadByImageGalleryId ) return LoadByImageGalleryId( criteria as ImageGalleryItem.ReadByImageGalleryId );
						if ( criteria is ImageGalleryItem.ReadTopByImageGalleryId ) return LoadTopByImageGalleryId( criteria as ImageGalleryItem.ReadTopByImageGalleryId );
						if ( criteria is ImageGalleryItem.ReadByImageGalleryAndPosition ) return LoadByImageGalleryAndPosition( criteria as ImageGalleryItem.ReadByImageGalleryAndPosition );
						List<ImageGalleryItem> items = new List<ImageGalleryItem>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryItemId, InstanceId, ImageGalleryId,VirtualPath, VirtualThumbnailPath,Position,Date,Description,CommentsCount,ViewCount,Votes,TotalRating,RatingResult
								FROM vImageGalleryItems WHERE InstanceId=@InstanceId
								ORDER BY Position ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										items.Add( GetImageGalleryItem( dr ) );
						}
						return items;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ImageGalleryItem> LoadById( ImageGalleryItem.ReadById byImageGalleryItemId )
				{
						List<ImageGalleryItem> items = new List<ImageGalleryItem>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryItemId,InstanceId,ImageGalleryId,VirtualPath, VirtualThumbnailPath,Position,Date,Description,CommentsCount,ViewCount,Votes,TotalRating,RatingResult
								FROM vImageGalleryItems
								WHERE ImageGalleryItemId = @ImageGalleryItemId
								ORDER BY Position ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ImageGalleryItemId", byImageGalleryItemId.ImageGalleryItemId ) );
								foreach ( DataRow dr in table.Rows )
										items.Add( GetImageGalleryItem( dr ) );
						}
						return items;
				}

				private List<ImageGalleryItem> LoadByImageGalleryId( ImageGalleryItem.ReadByImageGalleryId by )
				{
						List<ImageGalleryItem> items = new List<ImageGalleryItem>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryItemId,InstanceId, ImageGalleryId,VirtualPath, VirtualThumbnailPath,Position,Date,Description,CommentsCount,ViewCount,Votes,TotalRating,RatingResult
								FROM vImageGalleryItems
								WHERE ImageGalleryId = @ImageGalleryId
								ORDER BY Position ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ImageGalleryId", by.ImageGalleryId ) );
								foreach ( DataRow dr in table.Rows )
										items.Add( GetImageGalleryItem( dr ) );
						}
						return items;
				}

				private List<ImageGalleryItem> LoadTopByImageGalleryId( ImageGalleryItem.ReadTopByImageGalleryId by )
				{
						List<ImageGalleryItem> items = new List<ImageGalleryItem>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = string.Format( @"
								SELECT TOP({0}) ImageGalleryItemId,InstanceId, ImageGalleryId,VirtualPath, VirtualThumbnailPath,Position,Date,Description,CommentsCount,ViewCount,Votes,TotalRating,RatingResult
								FROM vImageGalleryItems
								WHERE ImageGalleryId = @ImageGalleryId
								ORDER BY Position ASC", by.Top );
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ImageGalleryId", by.ImageGalleryId ) );
								foreach ( DataRow dr in table.Rows )
										items.Add( GetImageGalleryItem( dr ) );
						}
						return items;
				}

				private List<ImageGalleryItem> LoadByImageGalleryAndPosition( ImageGalleryItem.ReadByImageGalleryAndPosition by )
				{
						List<ImageGalleryItem> items = new List<ImageGalleryItem>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryItemId,InstanceId, ImageGalleryId,VirtualPath, VirtualThumbnailPath,Position,Date,Description,CommentsCount,ViewCount,Votes,TotalRating,RatingResult
								FROM vImageGalleryItems
								WHERE ImageGalleryId = @ImageGalleryId AND Position = @Position
								ORDER BY Position ASC";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@ImageGalleryId", by.ImageGalleryId ),
										new SqlParameter( "@Position", by.Position ) );
								foreach ( DataRow dr in table.Rows )
										items.Add( GetImageGalleryItem( dr ) );
						}
						return items;
				}

				public override void Create( ImageGalleryItem imageGalleryItem )
				{
						using ( SqlConnection connection = Connect() )
						{
								DataSet ds = QueryProc<DataSet>( connection, "pImageGalleryItemCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@Description", Null(imageGalleryItem.Description) ),
										new SqlParameter( "@ImageGalleryId", imageGalleryItem.ImageGalleryId ),
										new SqlParameter( "@VirtualPath", imageGalleryItem.VirtualPath ),
										new SqlParameter( "@VirtualThumbnailPath", imageGalleryItem.VirtualThumbnailPath ),
										new SqlParameter( "@Position", imageGalleryItem.Position ) );

								int imageGalleryItemId = Convert.ToInt32( ds.Tables[0].Rows[0]["ImageGalleryItemId"] );
								imageGalleryItem.Id = imageGalleryItemId;
						}
				}

				public override void Update( ImageGalleryItem imageGalleryItem )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pImageGalleryItemModify",
										new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) ),
										new SqlParameter( "@ImageGalleryItemId", imageGalleryItem.Id ),
										new SqlParameter( "@Description", Null(imageGalleryItem.Description) ),
										new SqlParameter( "@ImageGalleryId", imageGalleryItem.ImageGalleryId ),
										new SqlParameter( "@VirtualPath", imageGalleryItem.VirtualPath ),
										new SqlParameter( "@VirtualThumbnailPath", imageGalleryItem.VirtualThumbnailPath ),
										new SqlParameter( "@Position", imageGalleryItem.Position ) );
						}
				}

				public override void Delete( ImageGalleryItem imageGalleryItem )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter imageGalleryItemId = new SqlParameter( "@ImageGalleryItemId", imageGalleryItem.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pImageGalleryItemDelete", result, historyAccount, imageGalleryItemId );
						}
				}

				public override R Execute<R>( R command )
				{
						Type t = typeof( R );
						if ( t == typeof( ImageGalleryItem.IncrementViewCountCommand ) )
								return IncrementViewCount( command as ImageGalleryItem.IncrementViewCountCommand ) as R;

						if ( t == typeof( ImageGalleryItem.IncrementVoteCommand ) )
								return IncrementVote( command as ImageGalleryItem.IncrementVoteCommand ) as R;

						return base.Execute<R>( command );
				}

				private ImageGalleryItem.IncrementViewCountCommand IncrementViewCount( ImageGalleryItem.IncrementViewCountCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter imageGalleryItemId = new SqlParameter( "@ImageGalleryItemId", cmd.ImageGalleryItemId );
								ExecProc( connection, "pImageGalleryItemIncrementViewCount", imageGalleryItemId );
						}

						return cmd;
				}

				private ImageGalleryItem.IncrementVoteCommand IncrementVote( ImageGalleryItem.IncrementVoteCommand cmd )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter imageGalleryItemId = new SqlParameter( "@ImageGalleryItemId", cmd.ImageGalleryItemId );
								SqlParameter rating = new SqlParameter( "@Rating", cmd.Rating );
								ExecProc( connection, "pImageGalleryItemIncrementVote", imageGalleryItemId, rating );

								AccountVote accountVote = new AccountVote();
								accountVote.AccountId = cmd.AccountId;
								accountVote.ObjectId = cmd.ImageGalleryItemId;
								accountVote.ObjectTypeId = (int)ImageGalleryItem.AccountVoteType;
								accountVote.Rating = cmd.Rating;
								Storage<AccountVote>.Create( accountVote );
						}

						return cmd;
				}

		}
}
