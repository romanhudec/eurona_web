ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [Default], [InstanceId], [Name], [Description], [Url], [Contents], [PageUrl]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages
