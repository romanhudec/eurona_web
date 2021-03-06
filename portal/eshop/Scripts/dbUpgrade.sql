
--======================================================================================================================
-- UPGRADE ESHOP version 0.2 to 0.3
--======================================================================================================================
DELETE FROM tShpCartProduct
DELETE FROM tShpOrder
DELETE FROM tShpCart

DROP TABLE tShpCartProduct
DROP TABLE tShpOrder
DROP TABLE tShpCart

------------------------------------------------------------------------------------------------------------------------
-- tShpCart
CREATE TABLE [tShpCart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[SessionId] [int] NULL,
	[AccountId] [int] NULL,
	[Created] [DateTime] NOT NULL,
	[ShipmentCode] [varchar](100) NULL,
	[PaymentCode] [varchar](100) NULL,
	[DeliveryAddressId] [int] NULL,
	[InvoiceAddressId] [int] NULL,	
	[Notes] [nvarchar](2000) NULL,
	[Closed] [DateTime] NULL,
	CONSTRAINT [PK_tShpCart] PRIMARY KEY CLUSTERED( [CartId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_Account] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_Account]
GO

-- address
ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_DeliveryAddress]
GO
ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_InvoiceAddress] FOREIGN KEY([InvoiceAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_InvoiceAddress]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpCartProduct
CREATE TABLE [tShpCartProduct](
	[CartProductId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[PriceWVAT] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[VAT] [DECIMAL](19,2) NOT NULL, /*DPH*/
	[Discount][DECIMAL](19,2) NULL, /*Zlava*/
	[PriceTotal] [DECIMAL](19,2) NULL, /*Cena spolu BEZ DPH*/
	[PriceTotalWVAT] [DECIMAL](19,2) NULL, /*Cena spolu BEZ DPH*/

	CONSTRAINT [PK_tShpCartProduct] PRIMARY KEY CLUSTERED( [CartProductId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpCartProduct]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCartProduct_Cart] FOREIGN KEY([CartId])
	REFERENCES [tShpCart] ([CartId])
GO
ALTER TABLE [tShpCartProduct] CHECK CONSTRAINT [CK_tShpCartProduct_Cart]
GO

ALTER TABLE [tShpCartProduct]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCartProduct_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpCartProduct] CHECK CONSTRAINT [CK_tShpCartProduct_Product]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpOrder
CREATE TABLE [tShpOrder](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[OrderNumber] [nvarchar](100) NOT NULL,
	[OrderDate][datetime] NOT NULL,
	[CartId] [int] NOT NULL,
	[OrderStatusCode] [varchar](100) NULL,
	[PaydDate] [datetime] NULL,
	[ShipmentCode] [varchar](100) NULL,
	[PaymentCode] [varchar](100) NULL,
	[DeliveryAddressId] [int] NULL,
	[InvoiceAddressId] [int] NULL,
	[InvoiceUrl] [nvarchar](500) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Price] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[PriceWVAT] [DECIMAL](19,2) NULL, /*Cena S DPH*/
	[Notified] [bit] NULL CONSTRAINT [DF_tShpOrder_Notified]  DEFAULT (0),
	[Exported] [bit] NULL CONSTRAINT [DF_tShpOrder_Exported]  DEFAULT (0),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,		
	CONSTRAINT [PK_tShpOrder] PRIMARY KEY CLUSTERED( [OrderId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_Cart] FOREIGN KEY([CartId])
	REFERENCES [tShpCart] ([CartId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_Cart]
GO

-- address
ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_DeliveryAddress]
GO
ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_InvoiceAddress] FOREIGN KEY([InvoiceAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_InvoiceAddress]
GO

-- history
ALTER TABLE [tShpOrder] WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_HistoryType]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpOrder_tShpOrder] FOREIGN KEY([HistoryId])
	REFERENCES [tShpOrder] ([OrderId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [FK_tShpOrder_tShpOrder]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpOrder_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [FK_tShpOrder_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vShpCartProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, cp.InstanceId, cp.CartId, c.AccountId, cp.ProductId, ProductCode = p.Code,ProductName = p.Name, 
	cp.Quantity,
	cp.Price,/*Cena BEZ DPH*/
	cp.PriceWVAT,  /*Cena S DPH*/
	cp.VAT,/*DPH*/
	cp.Discount, /*Zlava*/
	cp.PriceTotal, /*Cena spolu BEZ DPH*/
	PriceTotalWVAT,/*Cena spolu S DPH*/
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p ON p.ProductId = cp.ProductId
	INNER JOIN tShpCart c ON c.CartId = cp.CartId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
------------------------------------------------------------------------------------------------------------------------

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
	tShpCart c
	LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode AND s.HistoryId IS NULL
	LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode AND p.HistoryId IS NULL
GO

------------------------------------------------------------------------------------------------------------------------
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
	tShpOrder o
	INNER JOIN vShpCarts c ON c.CartId = o.CartId
	INNER JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s ON s.Code = o.ShipmentCode
	LEFT JOIN cShpPayment p ON p.Code = o.PaymentCode AND p.HistoryId IS NULL
	LEFT JOIN cShpOrderStatus os ON os.Code = o.OrderStatusCode AND os.Locale=a.Locale
WHERE o.HistoryId IS NULL
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCartProductCreate
	@InstanceId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity, Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity, @Price, @PriceWVAT, @VAT, @Discount, @PriceTotal, @PriceTotalWVAT )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCartProductModify
	@CartProductId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId) 
		RAISERROR('Invalid CartProductId %d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCartProduct
		SET CartId = @CartId, ProductId = @ProductId, Quantity = @Quantity,
		Price=@Price, PriceWVAT=@PriceWVAT, VAT=@VAT, Discount=@Discount, PriceTotal=@PriceTotal, PriceTotalWVAT=@PriceTotalWVAT
		WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(5)

	SET @year =  CAST( YEAR(GETDATE()) as nvarchar(4) )
	SET @month = CAST( MONTH(GETDATE()) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month

	SELECT @number = COUNT(*) + 1 from tShpOrder 
	SET @number = replicate( 0, 4- LEN(@number) ) + @number

	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, GETDATE(), @OrderStatusCode, @ShipmentCode,  @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, @InvoiceUrl, @PaydDate, @Notes, @Price, @PriceWVAT, @Notified, @Exported, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpOrderModify
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100),	
	@PaymentCode VARCHAR(100),		
	@PaydDate DATETIME = NULL,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder
		SET
			CartId=@CartId, OrderStatusCode=@OrderStatusCode, ShipmentCode=@ShipmentCode, PaymentCode=@PaymentCode, PaydDate=@PaydDate, InvoiceUrl=@InvoiceUrl, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Notified=@Notified, Exported=@Exported,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderId = @OrderId

		SET @Result = @OrderId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO

------------------------------------------------------------------------------------------------------------------------
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
	tShpOrder o
	INNER JOIN vShpCarts c ON c.CartId = o.CartId
	INNER JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s ON s.Code = o.ShipmentCode AND s.Locale=a.Locale
	LEFT JOIN cShpPayment p ON p.Code = o.PaymentCode AND p.HistoryId IS NULL
	LEFT JOIN cShpOrderStatus os ON os.Code = o.OrderStatusCode  AND os.HistoryId IS NULL AND os.Locale=a.Locale
WHERE o.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
--======================================================================================================================
-- Upgrade ESHOP db version
INSERT INTO tShpUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 3, GETDATE() )
GO
--======================================================================================================================
-- Upgrade
--======================================================================================================================