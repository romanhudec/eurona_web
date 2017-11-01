
ALTER VIEW vShpOrderStatuses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrderStatusId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpOrderStatus
WHERE
	HistoryId IS NULL
GO
