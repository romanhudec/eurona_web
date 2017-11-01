ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tNavigationMenu
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu