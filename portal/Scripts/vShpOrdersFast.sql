ALTER VIEW vShpOrdersFast
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.[Login], o.PaydDate, o.ParentId,
	o.OrderStatusCode, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentCode, ShipmentName = s.Name, ShipmentIcon = s.Icon, o.ShipmentPrice, o.ShipmentPriceWVAT,
	o.AssociationAccountId/*Pridruzienie tejto objednavky k objednavke pouzivatela*/, 
	o.AssociationRequestStatus, /*Status poziadavky na pridruzenie*/
	o.CreatedByAccountId/*Pouzivatel, ktory objednavku vytvoril*/,
	o.Price, o.PriceWVAT,
	OwnerName = org1.Name,
	a.TVD_Id
FROM
	tShpOrder o WITH (NOLOCK)
	INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = o.CartId
	INNER JOIN tAccount a WITH (NOLOCK) ON a.AccountId = c.AccountId
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON o.CurrencyId = cur.CurrencyId AND ( cur.InstanceId = 0 OR cur.InstanceId = o.InstanceId )
	LEFT JOIN vShpShipments s WITH (NOLOCK) ON s.Code = o.ShipmentCode AND s.Locale=ISNULL(cur.Locale, a.Locale) AND ( s.InstanceId = 0 OR s.InstanceId = o.InstanceId )
	LEFT JOIN vShpOrderStatuses os WITH (NOLOCK) ON os.Code = o.OrderStatusCode AND os.Locale=a.Locale AND ( os.InstanceId = 0 OR os.InstanceId = o.InstanceId )
	LEFT JOIN tOrganization org1 WITH (NOLOCK) ON org1.AccountId = a.AccountId AND org1.HistoryId IS NULL
WHERE o.HistoryId IS NULL