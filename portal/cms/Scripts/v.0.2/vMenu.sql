ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	MenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tMenu
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vMenu