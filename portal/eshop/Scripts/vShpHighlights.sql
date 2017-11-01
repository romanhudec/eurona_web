
ALTER VIEW vShpHighlights
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	HighlightId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpHighlight
WHERE
	HistoryId IS NULL
GO
