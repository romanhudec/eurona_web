ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [InstanceId], [Name], [Description], [Url]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages
