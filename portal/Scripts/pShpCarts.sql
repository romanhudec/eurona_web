ALTER PROCEDURE pShpCarts
	@Locale CHAR(2),
	@InstanceId INT,
	@CartId INT = NULL,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@Closed BIT = NULL
--%%WITH ENCRYPTION%%
AS
BEGIN
	SELECT
		c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed, Locale = @Locale,
		c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
		c.PaymentCode, PaymentName = p.Name,
		c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
		PriceTotal = c.Price, PriceTotalWVAT = c.PriceWVAT, c.Discount, c.[Status],
		c.[BodyEurosapTotal], c.[KatalogovaCenaCelkemByEurosap]
	FROM
		tShpCart c
		LEFT JOIN tAccount a ON a.AccountId = c.AccountId
		LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode
		LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode
	WHERE 
		(@InstanceId IS NULL OR c.InstanceId = @InstanceId) AND 
		c.CartId = ISNULL( @CartId, c.CartId ) AND
		( @AccountId IS NULL OR c.AccountId = @AccountId ) AND
		( @SessionId IS NULL OR c.SessionId = @SessionId  ) AND
		( @Closed IS NULL OR ( @Closed =1 AND c.Closed IS NULL ) )
END
GO
