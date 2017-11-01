
ALTER VIEW vShpOrderStatuses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrderStatusId, InstanceId, CodeId, [Name], Notes, Code, Icon, Locale
FROM
	cShpOrderStatus
WHERE
	HistoryId IS NULL
GO
