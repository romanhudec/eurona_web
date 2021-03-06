
ALTER VIEW vShpCarts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed,
	c.ShipmentId, ShipmentName = s.Name, ShipmentPrice = s.Price,
	c.PaymentId, PaymentName = p.Name,
	c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
	PriceTotal = (SELECT SUM(PriceTotal) FROM vShpCartProducts WHERE CartId=c.CartId),
	PriceTotalWVAT = (SELECT SUM(PriceTotalWVAT) FROM vShpCartProducts WHERE CartId=c.CartId)
FROM
	tShpCart c LEFT JOIN
	cShpShipment s ON s.ShipmentId = c.ShipmentId LEFT JOIN
	cShpPayment p ON p.PaymentId = c.PaymentId
GO
