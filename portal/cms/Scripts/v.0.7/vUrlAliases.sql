ALTER VIEW vUrlAliases
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[UrlAliasId], [InstanceId], [Url], [Locale], [Alias], [Name]
FROM tUrlAlias
GO

-- SELECT * FROM vUrlAliases