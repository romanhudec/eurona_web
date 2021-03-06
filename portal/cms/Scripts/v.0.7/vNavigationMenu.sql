ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuId, m.InstanceId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu