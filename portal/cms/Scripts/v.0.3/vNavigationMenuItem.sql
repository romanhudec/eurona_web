ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuItemId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenuItem m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenuItem