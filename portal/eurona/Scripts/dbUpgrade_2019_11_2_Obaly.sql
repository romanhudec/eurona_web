ALTER TABLE tShpProduct ADD PozadujObal BIT NULL DEFAULT(0), Obal BIT NULL DEFAULT(0), ObalOrder INT NULL
GO 
-------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pShpProductModifyEx]
	@HistoryAccount INT,
	@ProductId INT,
	@MaximalniPocetVBaleni INT = NULL,
	@MinimalniPocetVBaleni INT = NULL,
	@VamiNejviceNakupovane INT = NULL,
	@InternalStorageCount INT = -1,
	@LimitDate DATETIME = NULL,
	@DarkovySet INT = NULL,
	@BSR BIT = NULL,
	@Order INT = 0,
	@PozadujObal BIT = NULL,
	@Obal BIT = NULL,
	@ObalOrder INT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProduct
		SET
			MaximalniPocetVBaleni=@MaximalniPocetVBaleni, MinimalniPocetVBaleni=@MinimalniPocetVBaleni, VamiNejviceNakupovane=@VamiNejviceNakupovane, 
			InternalStorageCount=@InternalStorageCount, LimitDate=@LimitDate, DarkovySet=@DarkovySet, BSR=@BSR, [Order]=@Order,
			PozadujObal=@PozadujObal, Obal=@Obal, ObalOrder=@ObalOrder,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductId = @ProductId

		SET @Result = @ProductId

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
----------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vShpProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, BeznaCena = ISNULL(cp.BeznaCena, 0 ), MarzePovolena = ISNULL( cp.MarzePovolena, 0 ), MarzePovolenaMinimalni = ISNULL( cp.MarzePovolenaMinimalni, 0 ),
	cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , cp.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], 
	p.[Megasleva],  p.[Supercena], p.[CLHit], p.[Action], p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], 
	p.Parfumacia, p.Discount, cp.DynamickaSleva, cp.StatickaSleva, p.VamiNejviceNakupovane, p.DarkovySet,
	p.VAT, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WITH (NOLOCK) WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WITH (NOLOCK) WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale, p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, BonusovyKredit = cp.[CenaBK],
	p.ZadniEtiketa, p.ZobrazovatZadniEtiketu, p.BSR, [Order]=ISNULL(p.[Order], 999999),
	p.PozadujObal, p.Obal, ObalOrder=ISNULL(p.[ObalOrder], 999999)
FROM
	tShpProduct p WITH (NOLOCK)
	LEFT JOIN tShpProductLocalization pl WITH (NOLOCK) ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp WITH (NOLOCK) ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a WITH (NOLOCK) ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL
GO

----------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpZavozoveMisto ADD Kod INT NULL, [Popis] [nvarchar](500) NULL, [Psc] [nvarchar](10) NULL, [OsobniOdberVSidleSpolecnosti] [bit] default(0) null, [OsobniOdberPovoleneCasy] NVARCHAR(1000) NULL, [OsobniOdberAdresaSidlaSpolecnosti] NVARCHAR(250) NULL
GO
UPDATE tShpZavozoveMisto SET OsobniOdberVSidleSpolecnosti=0, Kod=CHECKSUM(Mesto)
GO
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [dbo].[tShpOrder] ADD ZavozoveMisto_Popis NVARCHAR(500) NULL, ZavozoveMisto_Psc NVARCHAR(10) NULL, ZavozoveMisto_OsobniOdberVSidleSpolecnosti [bit] default(0) null
GO
ALTER VIEW [dbo].[vShpOrders]
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
	ChildsCount = 0,--(SELECT Count(*) FROM tShpOrder co WITH (NOLOCK) WHERE co.HistoryId IS NULL AND co.ParentId=o.OrderId )
	o.ZavozoveMisto_Mesto, o.ZavozoveMisto_Psc, o.ZavozoveMisto_Popis, o.ZavozoveMisto_DatumACas, o.ZavozoveMisto_OsobniOdberVSidleSpolecnosti
FROM
	dbo.tShpOrder AS o WITH (NOLOCK) INNER JOIN
    dbo.tShpCart AS c WITH (NOLOCK) ON c.CartId = o.CartId INNER JOIN
    dbo.tAccount AS a WITH (NOLOCK) ON a.AccountId = c.AccountId LEFT OUTER JOIN
    dbo.cShpCurrency AS cur WITH (NOLOCK) ON o.CurrencyId = cur.CurrencyId AND ISNULL(NULLIF (cur.InstanceId, 0), o.InstanceId) = o.InstanceId LEFT OUTER JOIN
    dbo.vShpShipments AS s WITH (NOLOCK) ON s.Code = o.ShipmentCode AND s.Locale = ISNULL(cur.Locale, a.Locale) AND ISNULL(NULLIF (s.InstanceId, 0), o.InstanceId) = o.InstanceId LEFT OUTER JOIN
    dbo.cShpPayment AS p WITH (NOLOCK) ON p.Code = o.PaymentCode AND p.HistoryId IS NULL AND p.Locale = ISNULL(cur.Locale, a.Locale) AND ISNULL(NULLIF (p.InstanceId, 0), o.InstanceId) 
    = o.InstanceId LEFT OUTER JOIN
    dbo.vShpOrderStatuses AS os WITH (NOLOCK) ON os.Code = o.OrderStatusCode AND os.Locale = a.Locale AND ISNULL(NULLIF (os.InstanceId, 0), o.InstanceId) = o.InstanceId LEFT OUTER JOIN
    dbo.tOrganization AS org1 WITH (NOLOCK) ON org1.AccountId = a.AccountId AND org1.HistoryId IS NULL LEFT OUTER JOIN
    dbo.tOrganization AS org2 WITH (NOLOCK) ON org2.AccountId = o.CreatedByAccountId AND org2.HistoryId IS NULL
WHERE o.HistoryId IS NULL
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
USE [eurona]
GO

/****** Object:  StoredProcedure [dbo].[pShpOrderCreate]    Script Date: 12/17/2019 1:42:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[pShpOrderCreate]
	@HistoryAccount INT,
	@InstanceId INT,
	@OrderDate DATETIME,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100) = NULL,	
	@ShipmentPrice DECIMAL(19,2) = 0,
	@ShipmentPriceWVAT DECIMAL(19,2) = 0,			
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
	@CurrencyId INT = NULL,
	@ParentId INT = NULL,/*Parent objednavka*/
	@AssociationAccountId INT = NULL,/*Pridruzienie tejto objednavky k objednavke pouzivatela*/
	@AssociationRequestStatus INT = 0,/*Status poziadavky na pridruzenie*/
	@CreatedByAccountId INT = NULL,/*Pouzivatel, ktory objednavku vytvoril*/
	@ShipmentFrom DATETIME = NULL,
	@ShipmentTo DATETIME = NULL,
	@NoPostage BIT = 0,
	@ZavozoveMisto_Mesto VARCHAR(100) = null,
	@ZavozoveMisto_Psc VARCHAR(10) = null,
	@ZavozoveMisto_DatumACas VARCHAR(100) = null,
	@ZavozoveMisto_Popis VARCHAR(500) = null,
	@ZavozoveMisto_OsobniOdberVSidleSpolecnosti BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;	
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @CounterId INT, @Count INT, @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(6)

	-- TODO : 15.03.2011
	SELECT @CounterId = [CounterId], @Count = ISNULL([Counter], 0) + 1 FROM tShpOrderCounter WITH (TABLOCK) WHERE InstanceId=@InstanceId AND [Year] = YEAR(@OrderDate) AND [Month] = MONTH(@OrderDate)
	SET @Count = ISNULL(@Count, 1 )

	IF @CounterId IS NULL
	BEGIN
		INSERT INTO tShpOrderCounter WITH (TABLOCK) ( InstanceId, [Year], [Month], [Counter] ) VALUES ( @InstanceId, YEAR(@OrderDate), MONTH(@OrderDate), @Count )
	END
	ELSE
	BEGIN
		UPDATE tShpOrderCounter WITH (TABLOCK) SET [Counter] = @Count WHERE [CounterId] = @CounterId
	END

	-- Vytvorenie cisla objednavky
	SET @year =  CAST( YEAR(@OrderDate) as nvarchar(4) )
	SET @month = CAST( MONTH(@OrderDate) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month
	SET @number = replicate( 0, 5- LEN(CAST(@Count as nvarchar(5) ) ) ) + CAST(@Count as nvarchar(5) )
	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------------------------

	INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, ShipmentPrice, ShipmentPriceWVAT, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo, NoPostage, ZavozoveMisto_Mesto, ZavozoveMisto_DatumACas, ZavozoveMisto_Popis, ZavozoveMisto_Psc, ZavozoveMisto_OsobniOdberVSidleSpolecnosti,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, @OrderDate, @OrderStatusCode, @ShipmentCode, @ShipmentPrice, @ShipmentPriceWVAT,  @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, @InvoiceUrl, @PaydDate, @Notes, @Price, @PriceWVAT, @Notified, @Exported, @CurrencyId, @ParentId, @AssociationAccountId,@AssociationRequestStatus, @CreatedByAccountId, @ShipmentFrom,  @ShipmentTo, @NoPostage, @ZavozoveMisto_Mesto, @ZavozoveMisto_DatumACas, @ZavozoveMisto_Popis, @ZavozoveMisto_Psc, @ZavozoveMisto_OsobniOdberVSidleSpolecnosti,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	-------------------------------------------------------------------------------------
	UPDATE tShpCart SET Closed=GETDATE() WHERE CartId=@CartId

	SELECT OrderId = @Result

END

GO

-----------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pShpOrderModify]
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderDate DATETIME,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100),
	@ShipmentPrice DECIMAL(19,2) = 0,
	@ShipmentPriceWVAT DECIMAL(19,2) = 0,		
	@PaymentCode VARCHAR(100),		
	@PaydDate DATETIME = NULL,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@CurrencyId INT = NULL,
	@ParentId INT = NULL,/*Parent objednavka*/
	@AssociationAccountId INT = NULL,/*Pridruzienie tejto objednavky k objednavke pouzivatela*/
	@AssociationRequestStatus INT = 0,/*Status poziadavky na pridruzenie*/
	@CreatedByAccountId INT = NULL,/*Pouzivatel, ktory objednavku vytvoril*/
	@ShipmentFrom DATETIME = NULL,
	@ShipmentTo DATETIME = NULL,
	@NoPostage BIT = 0,
	@ZavozoveMisto_Mesto VARCHAR(100) = null,
	@ZavozoveMisto_Psc VARCHAR(10) = null,
	@ZavozoveMisto_DatumACas VARCHAR(100) = null,
	@ZavozoveMisto_Popis VARCHAR(500) = null,
	@ZavozoveMisto_OsobniOdberVSidleSpolecnosti BIT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WITH (NOLOCK) WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, ShipmentPrice, ShipmentPriceWVAT, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo, NoPostage, ZavozoveMisto_Mesto, ZavozoveMisto_DatumACas, ZavozoveMisto_Popis, ZavozoveMisto_Psc, ZavozoveMisto_OsobniOdberVSidleSpolecnosti, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, ShipmentPrice, ShipmentPriceWVAT, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo, NoPostage, ZavozoveMisto_Mesto, ZavozoveMisto_DatumACas, ZavozoveMisto_Popis, ZavozoveMisto_Psc, ZavozoveMisto_OsobniOdberVSidleSpolecnosti, 
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder WITH (ROWLOCK)
		SET
			CartId=@CartId, OrderDate=@OrderDate, OrderStatusCode=@OrderStatusCode, ShipmentCode=@ShipmentCode, ShipmentPrice=@ShipmentPrice, ShipmentPriceWVAT=@ShipmentPriceWVAT, PaymentCode=@PaymentCode, PaydDate=@PaydDate, InvoiceUrl=@InvoiceUrl, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Notified=@Notified, Exported=@Exported, CurrencyId=@CurrencyId,
			ParentId=@ParentId, AssociationAccountId=@AssociationAccountId, AssociationRequestStatus=@AssociationRequestStatus, CreatedByAccountId=@CreatedByAccountId, ShipmentFrom=@ShipmentFrom, ShipmentTo=@ShipmentTo, NoPostage=@NoPostage, ZavozoveMisto_Mesto=@ZavozoveMisto_Mesto, ZavozoveMisto_DatumACas=@ZavozoveMisto_DatumACas, ZavozoveMisto_Popis=@ZavozoveMisto_Popis, ZavozoveMisto_Psc=@ZavozoveMisto_Psc, ZavozoveMisto_OsobniOdberVSidleSpolecnosti=@ZavozoveMisto_OsobniOdberVSidleSpolecnosti,
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

-----------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vShpCartProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, p.InstanceId, CreatedInInstanceId = cp.InstanceId, cp.CartId, c.AccountId, cp.ProductId, ProductCode = p.Code,ProductName = pl.Name, 
	cp.Quantity,
	cp.Price,/*Cena BEZ DPH*/
	cp.PriceWVAT,  /*Cena S DPH*/
	cp.VAT,/*DPH*/
	cp.Discount, /*Zlava*/
	cp.PriceTotal, /*Cena spolu BEZ DPH*/
	PriceTotalWVAT,/*Cena spolu S DPH*/
	ProductAvailability = p.Availability, a.Alias, pl.Locale,
	cp.CurrencyId, CurrencySymbol = cur.Symbol, CurrencyCode = cur.Code,
	cenyP.Body, BodyCelkem = (cenyP.Body * cp.Quantity),
	KatalogPriceWVAT = cenyP.Cena, KatalogPriceWVATTotal = (cenyP.Cena * cp.Quantity),
	p.MaximalniPocetVBaleni,p.MinimalniPocetVBaleni , CerpatBK = ISNULL(cp.CerpatBK, 0),
	POrder = cp.POrderD, BSRProdukt=p.BSR, Obal=p.Obal, PozadujObal=p.PozadujObal
FROM
	tShpCartProduct cp WITH (NOLOCK)
	INNER JOIN tShpProduct p WITH (NOLOCK) ON p.ProductId = cp.ProductId
	LEFT JOIN tShpProductLocalization pl WITH (NOLOCK) ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cenyP WITH (NOLOCK) ON cenyP.ProductId = p.ProductId AND cenyP.Locale = pl.Locale
	INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = cp.CartId
	LEFT JOIN tUrlAlias a WITH (NOLOCK) ON a.UrlAliasId = pl.UrlAliasId
	--LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.CurrencyId = cp.CurrencyId
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.Locale = pl.Locale
ORDER BY cp.POrderD ASC
GO

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpZavozoveMisto ADD [Stat] [nvarchar](10) NULL
GO
UPDATE tShpZavozoveMisto SET [Stat]='CZ'
GO
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
AlTER TABLE cShpShipment ADD [Hide] [BIT] NULL
GO

UPDATE cShpShipment SET Hide=0
GO
----------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vShpShipments]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	s.ShipmentId, s.InstanceId, s.[Name], s.Notes, s.Code, s.Icon, s.Locale, s.Price, s.VATId, VAT = v.[Percent], [Default], [Order], [Hide],
	PriceWVAT = ROUND(s.Price * (1 + v.[Percent]/100 ), 2)
FROM
	cShpShipment s LEFT JOIN
	cShpVAT v ON v.VATId = s.VATId
WHERE
	s.HistoryId IS NULL

GO



