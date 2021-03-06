
ALTER VIEW vShpCarts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed,
	c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
	c.PaymentCode, PaymentName = p.Name,
	c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
	PriceTotal = (SELECT SUM(PriceTotal) FROM vShpCartProducts WHERE CartId=c.CartId),
	PriceTotalWVAT = (SELECT SUM(PriceTotalWVAT) FROM vShpCartProducts WHERE CartId=c.CartId)
FROM
	tShpCart c  WITH (NOLOCK)
	LEFT JOIN cShpShipment s   WITH (NOLOCK) ON s.Code = c.ShipmentCode AND s.HistoryId IS NULL
	LEFT JOIN cShpPayment p  WITH (NOLOCK) ON p.Code = c.PaymentCode AND p.HistoryId IS NULL
GO
