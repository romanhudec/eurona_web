
ALTER VIEW vShpCurrencies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	CurrencyId, InstanceId, [Name], Notes, Code, Icon, Locale, Rate, Symbol
FROM
	cShpCurrency
WHERE
	HistoryId IS NULL
GO
