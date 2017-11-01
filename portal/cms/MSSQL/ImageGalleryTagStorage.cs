using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ImageGalleryTagStorage: MSSQLStorage<ImageGalleryTag>
		{
				public ImageGalleryTagStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ImageGalleryTag GetImageGalleryTag( DataRow record )
				{
						ImageGalleryTag imageGalleryTag = new ImageGalleryTag();
						imageGalleryTag.Id = Convert.ToInt32( record["ImageGalleryTagId"] );
						imageGalleryTag.TagId = Convert.ToInt32( record["TagId"] );
						imageGalleryTag.ImageGalleryId = Convert.ToInt32( record["ImageGalleryId"] );
						imageGalleryTag.Name = Convert.ToString( record["Tag"] );
						return imageGalleryTag;
				}

				public override List<ImageGalleryTag> Read( object criteria )
				{
						if ( criteria is ImageGalleryTag.ReadByTagId ) return LoadByTagId( criteria as ImageGalleryTag.ReadByTagId );
						if ( criteria is ImageGalleryTag.ReadByImageGalleryId ) return LoadByImageGalleryId( criteria as ImageGalleryTag.ReadByImageGalleryId );
						List<ImageGalleryTag> list = new List<ImageGalleryTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryTagId, TagId, ImageGalleryTag
								FROM vImageGalleryTags";
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetImageGalleryTag( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<ImageGalleryTag> LoadByTagId( ImageGalleryTag.ReadByTagId byTagId )
				{
						List<ImageGalleryTag> list = new List<ImageGalleryTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryTagId, TagId, ImageGalleryId, Tag
								FROM vImageGalleryTags
								WHERE TagId = @TagId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@TagId", byTagId.TagId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetImageGalleryTag( dr ) );
						}
						return list;
				}

				private List<ImageGalleryTag> LoadByImageGalleryId( ImageGalleryTag.ReadByImageGalleryId byImageGalleryId )
				{
						List<ImageGalleryTag> list = new List<ImageGalleryTag>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ImageGalleryTagId, TagId, ImageGalleryId, Tag
								FROM vImageGalleryTags
								WHERE ImageGalleryId = @ImageGalleryId";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@ImageGalleryId", byImageGalleryId.ImageGalleryId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetImageGalleryTag( dr ) );
						}
						return list;
				}

				public override void Update( ImageGalleryTag entity )
				{
						throw new NotImplementedException();
				}

				public override void Create( ImageGalleryTag entity )
				{
						if ( string.IsNullOrEmpty( entity.Name ) )
								return;

						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pImageGalleryTagCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@ImageGalleryId", entity.ImageGalleryId ),
										new SqlParameter( "@Tag", entity.Name ) );
						}
				}

				public override void Delete( ImageGalleryTag entity )
				{
						throw new NotImplementedException();
				}
		}
}
