ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	n.[NewsId], n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
	n.UrlAliasId, alias.Alias, alias.Url
FROM
	tNews n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId

WHERE
	n.HistoryId IS NULL 
GO

-- SELECT * FROM vNews
