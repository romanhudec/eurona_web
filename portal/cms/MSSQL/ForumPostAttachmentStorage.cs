using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ForumPostAttachmentStorage: MSSQLStorage<ForumPostAttachment>
		{
				private const string entitySelect = @"SELECT ForumPostAttachmentId, ForumPostId, Name, Description, Type, Url, Size, [Order]
						FROM vForumPostAttachments";

				public ForumPostAttachmentStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static ForumPostAttachment GetForumPostAttachment( DataRow record )
				{
						ForumPostAttachment attachment = new ForumPostAttachment();
						attachment.Id = Convert.ToInt32( record["ForumPostAttachmentId"] );
						attachment.ForumPostId = Convert.ToInt32( record["ForumPostId"] );
						attachment.Type = (ForumPostAttachment.AttachmentType)Convert.ToInt32( record["Type"] );

						attachment.Name = Convert.ToString( record["Name"] );
						attachment.Description = Convert.ToString( record["Description"] );
						attachment.Url = Convert.ToString( record["Url"] );
						attachment.Size = Convert.ToInt32( record["Size"] );
						attachment.Order = Convert.ToInt32( record["Order"] );
						return attachment;
				}

				public override List<ForumPostAttachment> Read( object criteria )
				{
						if ( criteria is ForumPostAttachment.ReadById ) return LoadById( criteria as ForumPostAttachment.ReadById );
						if ( criteria is ForumPostAttachment.ReadBy ) return LoadBy( criteria as ForumPostAttachment.ReadBy );
						List<ForumPostAttachment> list = new List<ForumPostAttachment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								DataTable table = Query<DataTable>( connection, sql );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumPostAttachment( dr ) );
						}
						return list;
				}

				private List<ForumPostAttachment> LoadById( ForumPostAttachment.ReadById by )
				{
						List<ForumPostAttachment> list = new List<ForumPostAttachment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE ForumPostAttachmentId = @ForumPostAttachmentId ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumPostAttachmentId", by.ForumPostAttachmentId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumPostAttachment( dr ) );
						}
						return list;
				}

				private List<ForumPostAttachment> LoadBy( ForumPostAttachment.ReadBy by )
				{
						List<ForumPostAttachment> list = new List<ForumPostAttachment>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = entitySelect;
								sql += " WHERE (@ForumPostId IS NULL OR ForumPostId = @ForumPostId) ORDER BY [Order] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ForumPostId", Null( by.ForumPostId ) ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetForumPostAttachment( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}


				public override void Create( ForumPostAttachment attachment )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pForumPostAttachmentCreate",
										new SqlParameter( "@ForumPostId", attachment.ForumPostId ),
										new SqlParameter( "@Type", (int)attachment.Type ),
										new SqlParameter( "@Name", attachment.Name ),
										new SqlParameter( "@Description", attachment.Description ),
										new SqlParameter( "@Url", attachment.Url ),
										new SqlParameter( "@Size", attachment.Size ),
										new SqlParameter( "@Order", attachment.Order ),
										result );

								attachment.Id = Convert.ToInt32( result.Value ); ;
						}
				}

				public override void Update( ForumPostAttachment attachment )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pForumPostAttachmentModify",
										new SqlParameter( "@ForumPostAttachmentId", attachment.Id ),
										new SqlParameter( "@ForumPostId", attachment.ForumPostId ),
										new SqlParameter( "@Type", (int)attachment.Type ),
										new SqlParameter( "@Name", attachment.Name ),
										new SqlParameter( "@Description", attachment.Description ),
										new SqlParameter( "@Url", attachment.Url ),
										new SqlParameter( "@Size", attachment.Size ),
										new SqlParameter( "@Order", attachment.Order ));
						}
				}

				public override void Delete( ForumPostAttachment attachment )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter pkId = new SqlParameter( "@ForumPostAttachmentId", attachment.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pForumPostAttachmentDelete", result, pkId );
						}
				}
		}
}
