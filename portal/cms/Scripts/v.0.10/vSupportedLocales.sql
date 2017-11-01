
ALTER VIEW vSupportedLocales
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	SupportedLocaleId, InstanceId, [Name], Notes, Code, Icon
FROM
	cSupportedLocale
WHERE
	HistoryId IS NULL
GO
