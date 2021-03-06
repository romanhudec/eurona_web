
ALTER VIEW vShpOrders
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.Login, o.PaydDate,
	o.OrderStatusId, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentId, ShipmentName = s.Name, ShipmentIcon = s.Icon, ShipmentPrice = s.Price, ShipmentPriceWVAT = s.PriceWVAT,
	o.PaymentId, PaymentName = p.Name, PaymentIcon = p.Icon,
	ProductsPrice = c.PriceTotal, ProductsPriceWVAT = c.PriceTotalWVAT,
	o.DeliveryAddressId, o.InvoiceAddressId, o.[Notes]
FROM
	tShpOrder o
	INNER JOIN vShpCarts c ON c.CartId = o.CartId
	INNER JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s ON s.ShipmentId = o.ShipmentId
	LEFT JOIN cShpPayment p ON p.PaymentId = o.PaymentId
	LEFT JOIN cShpOrderStatus os ON os.CodeId = o.OrderStatusId AND os.Locale = a.Locale
WHERE o.HistoryId IS NULL
GO
