ALTER VIEW vVocabularies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[VocabularyId], [InstanceId], [Locale], [Name], [Notes]
FROM tVocabulary
GO

-- SELECT * FROM vVocabularies
