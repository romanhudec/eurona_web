
ALTER VIEW vShpCarts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed, a.Locale,
	c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
	c.PaymentCode, PaymentName = p.Name,
	c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
	PriceTotal = c.Price, PriceTotalWVAT = c.PriceWVAT, c.Discount, c.[Status],
	c.[BodyEurosapTotal], c.[KatalogovaCenaCelkemByEurosap]
FROM
	tShpCart c
	LEFT JOIN tAccount a WITH (NOLOCK) ON a.AccountId = c.AccountId
	LEFT JOIN cShpShipment s WITH (NOLOCK) ON s.Code = c.ShipmentCode
	LEFT JOIN cShpPayment p WITH (NOLOCK) ON p.Code = c.PaymentCode
GO
