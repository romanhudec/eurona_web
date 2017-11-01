ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[PageId], [MasterPageId], [Locale], [Title], [Name], [Url], [Content], [RoleId]
FROM
	tPage
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vPages
