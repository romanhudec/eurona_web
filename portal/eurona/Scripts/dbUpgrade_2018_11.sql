ALTER TABLE cShpShipment ADD [Default] BIT NULL
GO
update cShpShipment set [Default]=1 WHERE Code=3
GO

ALTER VIEW [dbo].[vShpShipments]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	s.ShipmentId, s.InstanceId, s.[Name], s.Notes, s.Code, s.Icon, s.Locale, s.Price, s.VATId, VAT = v.[Percent], [Default],
	PriceWVAT = ROUND(s.Price * (1 + v.[Percent]/100 ), 2)
FROM
	cShpShipment s LEFT JOIN
	cShpVAT v ON v.VATId = s.VATId
WHERE
	s.HistoryId IS NULL
GO