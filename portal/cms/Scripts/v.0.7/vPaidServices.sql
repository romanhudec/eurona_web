
ALTER VIEW vPaidServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	PaidServiceId, [InstanceId], [Name], [Notes], [CreditCost]
FROM
	cPaidService
WHERE
	HistoryId IS NULL
GO
