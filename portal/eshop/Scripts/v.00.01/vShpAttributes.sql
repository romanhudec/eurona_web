
ALTER VIEW vShpAttributes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AttributeId, a.InstanceId, a.CategoryId, a.[Name], a.Description, a.DefaultValue, a.Type, a.TypeLimit, a.Locale
FROM
	tShpAttribute a
WHERE
	a.HistoryId IS NULL
GO
