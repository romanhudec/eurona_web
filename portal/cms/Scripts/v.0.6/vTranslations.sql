ALTER VIEW vTranslations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.VocabularyId, VocabularyName = v.Name, v.Locale, t.TranslationId, t.Term, t.Translation, t.Notes
FROM tTranslation t (NOLOCK)
INNER JOIN tVocabulary v (NOLOCK) ON t.VocabularyId = v.VocabularyId
WHERE t.HistoryId IS NULL
GO

-- SELECT * FROM vTranslations
