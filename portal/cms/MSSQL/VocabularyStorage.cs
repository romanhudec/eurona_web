using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CMS.MSSQL
{
		public class VocabularyStorage: MSSQLStorage<Vocabulary>
		{
				public VocabularyStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Vocabulary GetVocabulary( DataRow record )
				{
						Vocabulary voc = new Vocabulary()
						{
								Id = Convert.ToInt32( record["VocabularyId"] ),
								InstanceId = Convert.ToInt32( record["InstanceId"] ),
								Name = Convert.ToString( record["Name"] ),
								Locale = Convert.ToString( record["Locale"] ),
						};
						return voc;
				}
				public override void Create( Vocabulary entity )
				{
						throw new NotImplementedException();
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override List<Vocabulary> Read( object criteria )
				{
						List<Vocabulary> dict = new List<Vocabulary>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"SELECT VocabularyId, InstanceId, Name, Locale FROM vVocabularies";
								SqlParameter[] @params = new SqlParameter[] {
								new SqlParameter("@Locale", Locale),
								new SqlParameter("@InstanceId", InstanceId)};
								sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId";
								sql += " ORDER BY Name";
								DataTable table = Query<DataTable>( connection, sql, @params );
								foreach ( DataRow dr in table.Rows )
										dict.Add( GetVocabulary( dr ) );
						}
						return dict;
				}

				public override void Update( Vocabulary entity )
				{
						throw new NotImplementedException();
				}

				public override void Delete( Vocabulary entity )
				{
						throw new NotImplementedException();
				}
		}
}
