
ALTER VIEW vShpCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CategoryId, c.[Order], c.InstanceId, c.ParentId, c.[Name], c.Locale,
	c.Icon, a.UrlAliasId, a.Url, a.Alias
FROM
	tShpCategory c LEFT JOIN tUrlAlias a ON a.UrlAliasId = c.UrlAliasId
WHERE
	c.HistoryId IS NULL
GO
