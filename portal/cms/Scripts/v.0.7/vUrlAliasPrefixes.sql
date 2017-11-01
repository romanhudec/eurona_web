
ALTER VIEW vUrlAliasPrefixes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	UrlAliasPrefixId, [InstanceId], [Name], [Code], [Locale], [Notes]
FROM cUrlAliasPrefix
WHERE HistoryId IS NULL
GO
