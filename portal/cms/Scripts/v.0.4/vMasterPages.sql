ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [Name], [Description], [Url]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages
