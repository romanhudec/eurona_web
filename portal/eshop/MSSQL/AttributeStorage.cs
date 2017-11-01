using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHP.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.Entities;
using CMS.MSSQL;
using AttributeEntity = SHP.Entities.Attribute;

namespace SHP.MSSQL
{
		internal sealed class AttributeStorage: MSSQLStorage<AttributeEntity>
		{
				public AttributeStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static AttributeEntity GetAttribute( DataRow record )
				{
						AttributeEntity attribute = new AttributeEntity();
						attribute.Id = Convert.ToInt32( record["AttributeId"] );
						attribute.InstanceId = Convert.ToInt32( record["InstanceId"] );
						attribute.CategoryId = Convert.ToInt32( record["CategoryId"] );
						attribute.Name = Convert.ToString( record["Name"] );
						attribute.Locale = Convert.ToString( record["Locale"] );
						attribute.Description = Convert.ToString( record["Description"] );
						attribute.DefaultValue = Convert.ToString( record["DefaultValue"] );
						attribute.Type = (AttributeType.Type)Convert.ToInt32( record["Type"] );
						attribute.TypeLimit = Convert.ToString( record["TypeLimit"] );

						return attribute;
				}

				private static bool NullableDBToBool( object dbValue )
				{
						if ( dbValue == DBNull.Value ) return false;
						return Convert.ToInt32( dbValue ) == 1;
				}

				public override List<AttributeEntity> Read( object criteria )
				{
						if ( criteria is AttributeEntity.ReadById ) return LoadById( criteria as AttributeEntity.ReadById );
						if ( criteria is AttributeEntity.ReadByCategoryId ) return LoadByCategoryId( criteria as AttributeEntity.ReadByCategoryId );
						if ( criteria is AttributeEntity.ReadAllInherits4Category ) return LoadAllInherits4Category( criteria as AttributeEntity.ReadAllInherits4Category );
						List<AttributeEntity> attributes = new List<AttributeEntity>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AttributeId, InstanceId, CategoryId,  [Name], [Description],  DefaultValue, [Type], TypeLimit, Locale
								FROM vShpAttributes
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY Name ASC";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@Locale", Locale ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										attributes.Add( GetAttribute( dr ) );
						}
						return attributes;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<AttributeEntity> LoadById( AttributeEntity.ReadById byAttributeId )
				{
						List<AttributeEntity> attributes = new List<AttributeEntity>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AttributeId, InstanceId, CategoryId,  [Name], [Description],  DefaultValue, [Type], TypeLimit, Locale
								FROM vShpAttributes
								WHERE AttributeId = @AttributeId
								ORDER BY Name ASC";
								DataTable table = Query<DataTable>( connection, sql, 
										new SqlParameter( "@AttributeId", byAttributeId.AttributeId ) );
								foreach ( DataRow dr in table.Rows )
										attributes.Add( GetAttribute( dr ) );
						}
						return attributes;
				}

				private List<AttributeEntity> LoadByCategoryId( AttributeEntity.ReadByCategoryId by )
				{
						List<AttributeEntity> attributes = new List<AttributeEntity>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT AttributeId, InstanceId, CategoryId,  [Name], [Description],  DefaultValue, [Type], TypeLimit, Locale
								FROM vShpAttributes
								WHERE CategoryId = @CategoryId AND InstanceId=@InstanceId
								ORDER BY Name ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CategoryId", Null(by.CategoryId) ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										attributes.Add( GetAttribute( dr ) );
						}
						return attributes;
				}

				private List<AttributeEntity> LoadAllInherits4Category( AttributeEntity.ReadAllInherits4Category by )
				{
						List<AttributeEntity> attributes = new List<AttributeEntity>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT a.AttributeId, a.InstanceId, a.CategoryId,  a.[Name], a.[Description],  a.DefaultValue, a.[Type], a.TypeLimit, a.Locale
								FROM vShpAttributes a INNER JOIN
										dbo.fAllInheritCategoryAttributes(@categoryId) ca ON ca.AttributeId=a.AttributeId
								WHERE a.InstanceId=@InstanceId
								ORDER BY a.CategoryId ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@CategoryId", by.CategoryId ),
										new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										attributes.Add( GetAttribute( dr ) );
						}
						return attributes;
				}

				public override void Create( AttributeEntity attribute )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;

								ExecProc( connection, "pShpAttributeCreate",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@CategoryId", attribute.CategoryId ),
										new SqlParameter( "@Name", attribute.Name ),
										new SqlParameter( "@Type", attribute.Type ),
										new SqlParameter( "@TypeLimit", attribute.TypeLimit ),
										new SqlParameter( "@DefaultValue", attribute.DefaultValue ),
										new SqlParameter( "@Description", attribute.Description ),
										new SqlParameter( "@Locale", String.IsNullOrEmpty( attribute.Locale ) ? Locale : attribute.Locale ),
										result );

								attribute.Id = Convert.ToInt32( result.Value );
						}

				}

				public override void Update( AttributeEntity attribute )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pShpAttributeModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@AttributeId", attribute.Id ),
										new SqlParameter( "@CategoryId", attribute.CategoryId ),
										new SqlParameter( "@Name", attribute.Name ),
										new SqlParameter( "@Type", attribute.Type ),
										new SqlParameter( "@TypeLimit", attribute.TypeLimit ),
										new SqlParameter( "@DefaultValue", attribute.DefaultValue ),
										new SqlParameter( "@Description", attribute.Description ),
										new SqlParameter( "@Locale", attribute.Locale ) );
						}
				}

				public override void Delete( AttributeEntity attribute )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter blogId = new SqlParameter( "@AttributeId", attribute.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pShpAttributeDelete", result, historyAccount, blogId );
						}
				}
		}
}
