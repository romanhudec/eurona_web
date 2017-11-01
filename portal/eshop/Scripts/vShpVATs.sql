
ALTER VIEW vShpVATs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	VATId, InstanceId, [Name], Notes, Code, Icon, Locale, [Percent]
FROM
	cShpVAT
WHERE
	HistoryId IS NULL
GO
