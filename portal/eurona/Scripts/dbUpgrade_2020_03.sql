ALTER TABLE cShpShipment ADD [PlatbaKartou] INT NOT NULL DEFAULT(1), [PlatbaDobirkou] INT NOT NULL DEFAULT(1)
GO 

update cShpShipment set PlatbaKartou=1, PlatbaDobirkou=1
GO
----------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vShpShipments]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	s.ShipmentId, s.InstanceId, s.[Name], s.Notes, s.Code, s.Icon, s.Locale, s.Price, s.VATId, VAT = v.[Percent], [Default], [Order], [Hide], [PlatbaKartou], [PlatbaDobirkou], 
	PriceWVAT = ROUND(s.Price * (1 + v.[Percent]/100 ), 2)
FROM
	cShpShipment s LEFT JOIN
	cShpVAT v ON v.VATId = s.VATId
WHERE
	s.HistoryId IS NULL


GO