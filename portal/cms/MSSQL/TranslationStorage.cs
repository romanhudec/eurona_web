using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CMS.MSSQL
{
		public class TranslationStorage: MSSQLStorage<Translation>
		{
				public TranslationStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Translation GetTranslation( DataRow record )
				{
						Translation trans = new Translation()
						{
								Id = Convert.ToInt32( record["TranslationId"] ),
								InstanceId = Convert.ToInt32( record["InstanceId"] ),
								VocabularyId = Convert.ToInt32( record["VocabularyId"] ),
								Term = Convert.ToString( record["Term"] ),
								Trans = Convert.ToString( record["Translation"] ),
								Notes = Convert.ToString( record["Notes"] ),
						};
						return trans;
				}

				public override void Create( Translation entity )
				{
						throw new NotImplementedException();
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				public override List<Translation> Read( object criteria )
				{
						List<Translation> dict = new List<Translation>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"SELECT VocabularyId, InstanceId, TranslationId, Term, Translation, Notes FROM vTranslations";
								SqlParameter[] @params = null;
								if ( criteria is Translation.ReadById )
								{
										Translation.ReadById byId = criteria as Translation.ReadById;
										@params = new SqlParameter[] { new SqlParameter( "@TranslationId", byId.TranslationId ) };
										sql += " WHERE TranslationId = @TranslationId";
								}
								if ( criteria is Translation.ReadByVocabulary )
								{
										Translation.ReadByVocabulary byVocabulary = criteria as Translation.ReadByVocabulary;
										if ( byVocabulary.VocabularyId.HasValue )
										{
												@params = new SqlParameter[] {
														new SqlParameter("@VocabularyId", byVocabulary.VocabularyId.Value),
														new SqlParameter("@Locale", Locale),
														new SqlParameter("@InstanceId", InstanceId)};
												sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND VocabularyId = @VocabularyId";
										}
										else
										{
												@params = new SqlParameter[] {
														new SqlParameter("@VocabularyName", byVocabulary.Vocabulary),
														new SqlParameter("@Locale", Locale),
														new SqlParameter("@InstanceId", InstanceId)};
												sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId AND VocabularyName = @VocabularyName";
										}
								}
								sql += " ORDER BY Term";
								DataTable table = Query<DataTable>( connection, sql, @params );
								foreach ( DataRow dr in table.Rows )
										dict.Add( GetTranslation( dr ) );
						}
						return dict;
				}

				public override void Update( Translation entity )
				{
						using ( SqlConnection connection = Connect() )
						{
								SqlParameter historyAccount = new SqlParameter( "@HistoryAccount", Null( Account != null ? AccountId : (int?)null ) );
								SqlParameter id = new SqlParameter( "@TranslationId", entity.Id );
								SqlParameter translation = new SqlParameter( "@Translation", entity.Trans );
								SqlParameter notes = new SqlParameter( "@Notes", Null( entity.Notes ) );
								SqlParameter result = new SqlParameter( "@Result", -1 );
								result.Direction = ParameterDirection.Output;
								ExecProc( connection, "pTranslationModify", result, historyAccount, id, translation, notes );
						}
				}

				public override void Delete( Translation entity )
				{
						throw new NotImplementedException();
				}
		}
}
