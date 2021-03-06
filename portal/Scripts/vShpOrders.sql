ALTER VIEW vShpOrders
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.[Login], o.PaydDate,
	o.OrderStatusCode, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentCode, ShipmentName = s.Name, ShipmentIcon = s.Icon, o.ShipmentPrice, o.ShipmentPriceWVAT,
	o.PaymentCode, PaymentName = p.Name, PaymentIcon = p.Icon,
	o.Price, o.PriceWVAT,
	o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.[Notes],
	o.Notified, o.Exported,
	o.CurrencyId, CurrencySymbol = cur.Symbol, CurrencyCode = cur.Code,
	o.ParentId/*Parent objednavka*/, 
	o.AssociationAccountId/*Pridruzienie tejto objednavky k objednavke pouzivatela*/, 
	o.AssociationRequestStatus, /*Status poziadavky na pridruzenie*/
	o.CreatedByAccountId/*Pouzivatel, ktory objednavku vytvoril*/,
	o.ShipmentFrom,o.ShipmentTo,
	OwnerName = org1.Name,
	CreatedByName = org2.Name,
	a.TVD_Id,
	NoPostage = ISNULL(o.NoPostage, 0 ),
	ChildsCount = (SELECT Count(*) FROM tShpOrder co WHERE co.HistoryId IS NULL AND co.ParentId=o.OrderId )
FROM
	tShpOrder o WITH (NOLOCK)
	INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	INNER JOIN tAccount a WITH (NOLOCK) ON a.AccountId = c.AccountId
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON o.CurrencyId = cur.CurrencyId AND ( cur.InstanceId = 0 OR cur.InstanceId = o.InstanceId )
	LEFT JOIN vShpShipments s WITH (NOLOCK) ON s.Code = o.ShipmentCode AND s.Locale=ISNULL(cur.Locale, a.Locale) AND ( s.InstanceId = 0 OR s.InstanceId = o.InstanceId )
	LEFT JOIN cShpPayment p WITH (NOLOCK) ON p.Code = o.PaymentCode AND p.HistoryId IS NULL AND p.Locale=ISNULL(cur.Locale, a.Locale) AND ( p.InstanceId = 0 OR p.InstanceId = o.InstanceId )
	LEFT JOIN vShpOrderStatuses os WITH (NOLOCK) ON os.Code = o.OrderStatusCode AND os.Locale=a.Locale AND ( os.InstanceId = 0 OR os.InstanceId = o.InstanceId )
	LEFT JOIN tOrganization org1 WITH (NOLOCK) ON org1.AccountId = a.AccountId AND org1.HistoryId IS NULL
	LEFT JOIN tOrganization org2 WITH (NOLOCK) ON org2.AccountId = o.CreatedByAccountId AND org2.HistoryId IS NULL
WHERE o.HistoryId IS NULL
GO