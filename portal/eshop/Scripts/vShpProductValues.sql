
ALTER VIEW vShpProductValues
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.ProductValueId, v.InstanceId, v.ProductId, v.AttributeId, v.Value
FROM
	tShpProductValue v
WHERE
	v.HistoryId IS NULL
GO
