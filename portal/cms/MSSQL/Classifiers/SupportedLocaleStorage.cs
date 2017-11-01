using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using CMS.Entities.Classifiers;

namespace CMS.MSSQL.Classifiers
{
		internal sealed class SupportedLocaleStorage: MSSQLStorage<SupportedLocale>
		{
				public SupportedLocaleStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static SupportedLocale GetSupportedLocale( DataRow record )
				{
						SupportedLocale supportedLocale = new SupportedLocale();
						supportedLocale.Id = Convert.ToInt32( record["SupportedLocaleId"] );
						supportedLocale.InstanceId = Convert.ToInt32( record["InstanceId"] );
						supportedLocale.Name = Convert.ToString( record["Name"] );
						supportedLocale.Code = Convert.ToString( record["Code"] );
						supportedLocale.Notes = Convert.ToString( record["Notes"] );
						supportedLocale.Icon = Convert.ToString( record["Icon"] );
						supportedLocale.Locale = supportedLocale.Code;

						return supportedLocale;
				}

				public override List<SupportedLocale> Read( object criteria )
				{
						if ( criteria is SupportedLocale.ReadById ) return LoadById( criteria as SupportedLocale.ReadById );
						List<SupportedLocale> list = new List<SupportedLocale>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT SupportedLocaleId, InstanceId, [Name], [Code], [Notes], Icon
								FROM vSupportedLocales
								WHERE InstanceId=@InstanceId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetSupportedLocale( dr ) );
						}
						return list;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<SupportedLocale> LoadById( SupportedLocale.ReadById by )
				{
						List<SupportedLocale> list = new List<SupportedLocale>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT SupportedLocaleId, InstanceId, [Name], [Code], [Notes], Icon
								FROM vSupportedLocales
								WHERE SupportedLocaleId = @SupportedLocaleId
								ORDER BY [Name] ASC";
								DataTable table = Query<DataTable>( connection, sql,
										new SqlParameter( "@SupportedLocaleId", by.Id ) );
								foreach ( DataRow dr in table.Rows )
										list.Add( GetSupportedLocale( dr ) );
						}
						return list;
				}

				public override void Create( SupportedLocale entity )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pSupportedLocaleCreate",
										new SqlParameter( "@InstanceId", InstanceId ),
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@Name", entity.Name ),
										new SqlParameter( "@Code", entity.Code ),
										new SqlParameter( "@Notes", entity.Notes ),
										new SqlParameter( "@Icon", entity.Icon ) );
						}
				}

				public override void Update( SupportedLocale entity )
				{
						using ( SqlConnection connection = Connect() )
						{
								ExecProc( connection, "pSupportedLocaleModify",
										new SqlParameter( "@HistoryAccount", AccountId ),
										new SqlParameter( "@SupportedLocaleId", entity.Id ),
										new SqlParameter( "@Name", entity.Name ),
										new SqlParameter( "@Code", entity.Code ),
										new SqlParameter( "@Notes", entity.Notes ),
										new SqlParameter( "@Icon", entity.Icon ) );
						}
				}

				public override void Delete( SupportedLocale entity )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", AccountId );
								SqlParameter imageGalleryId = new SqlParameter( "@SupportedLocaleId", entity.Id );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pSupportedLocaleDelete", result, historyAccount, imageGalleryId );
						}
				}
		}
}
