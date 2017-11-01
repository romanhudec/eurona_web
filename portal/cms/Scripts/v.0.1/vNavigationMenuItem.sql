ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	NavigationMenuItemId, NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tNavigationMenuItem
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenuItem