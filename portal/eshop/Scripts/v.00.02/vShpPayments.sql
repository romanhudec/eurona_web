
ALTER VIEW vShpPayments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	PaymentId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpPayment
WHERE
	HistoryId IS NULL
GO
