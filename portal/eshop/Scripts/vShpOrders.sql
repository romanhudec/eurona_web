
ALTER VIEW vShpOrders
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.[Login], o.PaydDate,
	o.OrderStatusCode, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentCode, ShipmentName = s.Name, ShipmentIcon = s.Icon, ShipmentPrice = s.Price, ShipmentPriceWVAT = s.PriceWVAT,
	o.PaymentCode, PaymentName = p.Name, PaymentIcon = p.Icon,
	o.Price, o.PriceWVAT,
	o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.[Notes],
	o.Notified, o.Exported
FROM
	tShpOrder o WITH (NOLOCK)
	INNER JOIN vShpCarts c  WITH (NOLOCK) ON c.CartId = o.CartId
	INNER JOIN tAccount a  WITH (NOLOCK) ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s  WITH (NOLOCK) ON s.Code = o.ShipmentCode AND s.Locale=a.Locale
	LEFT JOIN cShpPayment p  WITH (NOLOCK) ON p.Code = o.PaymentCode AND s.Locale=a.Locale AND p.HistoryId IS NULL
	LEFT JOIN cShpOrderStatus os  WITH (NOLOCK) ON os.Code = o.OrderStatusCode  AND os.HistoryId IS NULL AND os.Locale=a.Locale
WHERE o.HistoryId IS NULL
GO
