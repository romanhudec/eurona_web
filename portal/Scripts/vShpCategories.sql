
ALTER VIEW vShpCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CategoryId, c.[Order], c.InstanceId, c.ParentId, cl.[Name], cl.Locale,
	c.Icon, a.UrlAliasId, a.Url, a.Alias
FROM
	tShpCategory c 
	LEFT JOIN tShpCategoryLocalization cl ON cl.CategoryId = c.CategoryId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = cl.UrlAliasId AND a.Locale = cl.Locale 
WHERE
	c.HistoryId IS NULL
GO
