
------------------------------------------------------------------------------------------------------------------------
-- UPGRADE ESHOP version 0.0 to 0.1
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tShpUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tShpUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO
------------------------------------------------------------------------------------
-- TABLES
------------------------------------------------------------------------------------
ALTER TABLE tShpCategory ADD InstanceId INT NULL
GO
ALTER TABLE tShpProduct ADD InstanceId INT NULL
GO
ALTER TABLE tShpProductComment ADD InstanceId INT NULL
GO
ALTER TABLE tShpProductCategories ADD InstanceId INT NULL
GO
ALTER TABLE tShpAttribute ADD InstanceId INT NULL
GO
ALTER TABLE tShpProductValue ADD InstanceId INT NULL
GO
ALTER TABLE tShpProductHighlights ADD InstanceId INT NULL
GO
ALTER TABLE tShpAddress ADD InstanceId INT NULL
GO
ALTER TABLE tShpCart ADD InstanceId INT NULL
GO
ALTER TABLE tShpCartProduct ADD InstanceId INT NULL
GO
ALTER TABLE tShpOrder ADD InstanceId INT NULL
GO

--Classifiers
ALTER TABLE cShpOrderStatus ADD InstanceId INT NULL
GO
ALTER TABLE cShpVAT ADD InstanceId INT NULL
GO
ALTER TABLE cShpHighlight ADD InstanceId INT NULL
GO
ALTER TABLE cShpShipment ADD InstanceId INT NULL
GO
ALTER TABLE cShpPayment ADD InstanceId INT NULL
GO
ALTER TABLE cShpCurrency ADD InstanceId INT NULL
GO
------------------------------------------------------------------------------------
-- Update tables
------------------------------------------------------------------------------------
UPDATE tShpCategory SET InstanceId = 1
GO
UPDATE tShpProduct SET InstanceId = 1
GO
UPDATE tShpProductComment SET InstanceId = 1
GO
UPDATE tShpProductCategories SET InstanceId = 1
GO
UPDATE tShpAttribute SET InstanceId = 1
GO
UPDATE tShpProductValue SET InstanceId = 1
GO
UPDATE tShpProductHighlights SET InstanceId = 1
GO
UPDATE tShpAddress SET InstanceId = 1
GO
UPDATE tShpCart SET InstanceId = 1
GO
UPDATE tShpCartProduct SET InstanceId = 1
GO
UPDATE tShpOrder SET InstanceId = 1
GO

--Classifiers
UPDATE cShpOrderStatus SET InstanceId = 1
GO
UPDATE cShpVAT SET InstanceId = 1
GO
UPDATE cShpHighlight SET InstanceId = 1
GO
UPDATE cShpShipment SET InstanceId = 1
GO
UPDATE cShpPayment SET InstanceId = 1
GO
UPDATE cShpCurrency SET InstanceId = 1
GO
------------------------------------------------------------------------------------
-- VIEWS
------------------------------------------------------------------------------------
ALTER VIEW vShpCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CategoryId, c.InstanceId, c.ParentId, c.[Name], c.Locale,
	a.UrlAliasId, a.Url, a.Alias
FROM
	tShpCategory c LEFT JOIN tUrlAlias a ON a.UrlAliasId = c.UrlAliasId
WHERE
	c.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, p.[Name], p.[Manufacturer], p.[Description], p.[DescriptionLong], p.Availability, 
	p.StorageCount, p.Price, p.Discount, 
	p.VATId, VAT = ISNULL(v.[Percent], 0),
	p.Locale, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 )
FROM
	tShpProduct p 
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpProductComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pc.ProductCommentId, pc.InstanceId, pc.ProductId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tShpProductComment pc 
	INNER JOIN vComments c ON c.CommentId = pc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpAttributes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AttributeId, a.InstanceId, a.CategoryId, a.[Name], a.Description, a.DefaultValue, a.Type, a.TypeLimit, a.Locale
FROM
	tShpAttribute a
WHERE
	a.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpProductValues
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.ProductValueId, v.InstanceId, v.ProductId, v.AttributeId, v.Value
FROM
	tShpProductValue v
WHERE
	v.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [InstanceId], [FirstName], [LastName], [Organization], [Id1], [Id2], [Id3],
	[City], [Street], [Zip], [State],
	[Phone], [Email], [Notes]
FROM
	tShpAddress
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vShpAddresses
------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------
ALTER VIEW vShpCartProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, cp.InstanceId, cp.CartId, cp.ProductId, cp.Quantity, c.AccountId,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = (p.Price - ( p.Price * ( p.Discount / 100 ) )), 
	PriceTotal = ((p.Price - ( p.Price * ( p.Discount / 100 ) )) * cp.Quantity),
	PriceTotalWVAT = ROUND(((p.Price - ( p.Price * ( p.Discount / 100 ) )) * cp.Quantity) * (1 + ISNULL(v.[Percent], 0)/100), 2),
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p ON p.ProductId = cp.ProductId
	INNER JOIN tShpCart c ON c.CartId = cp.CartId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
------------------------------------------------------------------------------------
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
	tShpOrder o INNER JOIN
	vShpCarts c ON c.CartId = o.CartId INNER JOIN
	tAccount a ON a.AccountId = c.AccountId INNER JOIN
	vShpShipments s ON s.ShipmentId = o.ShipmentId INNER JOIN
	cShpPayment p ON p.PaymentId = o.PaymentId LEFT JOIN
	cShpOrderStatus os ON os.CodeId = o.OrderStatusId AND os.Locale = a.Locale
WHERE o.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpOrderStatuses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrderStatusId, InstanceId, CodeId, [Name], Notes, Code, Icon, Locale
FROM
	cShpOrderStatus
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpVATs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	VATId, InstanceId, [Name], Notes, Code, Icon, Locale, [Percent]
FROM
	cShpVAT
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpHighlights
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	HighlightId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpHighlight
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpShipments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	s.ShipmentId, s.InstanceId, s.[Name], s.Notes, s.Code, s.Icon, s.Locale, s.Price, s.VATId, VAT = v.[Percent],
	PriceWVAT = ROUND(s.Price * (1 + v.[Percent]/100 ), 2)
FROM
	cShpShipment s LEFT JOIN
	cShpVAT v ON v.VATId = s.VATId
WHERE
	s.HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpPayments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	PaymentId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpPayment
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
ALTER VIEW vShpCurrencies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	CurrencyId, InstanceId, [Name], Notes, Code, Icon, Locale, Rate, Symbol
FROM
	cShpCurrency
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------
-- PROCEDURES
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCategory ( InstanceId, ParentId, [Name], Locale, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @Name, @Locale, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CategoryId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Manufacturer NVARCHAR(500),
	@Code NVARCHAR(500) = NULL,
	@Name NVARCHAR(500),
	@Description NVARCHAR(1000) = NULL,
	@DescriptionLong NVARCHAR(MAX) = NULL,
	@Availability NVARCHAR(500)  = NULL, /*dostupnost ('na objednanie', '24Ks', ...)*/
	@StorageCount INT = NULL, /*Pocet KS na sklade*/
	@Price DECIMAL(19,2), /*Cena BEZ DPH*/	
	@VATId INT = NULL, /*DPH%*/	
	@Discount DECIMAL(19,2) = 0, /*Zlava %*/	
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProduct ( InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, Locale, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Code, @Name, @Manufacturer, @Description, @DescriptionLong, @Availability, @StorageCount, @Price, @VATId, @Discount, @Locale, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ProductId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tShpProductComment ( InstanceId, CommentId, ProductId ) VALUES ( @InstanceId, @CommentId, @ProductId )

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductCategoriesCreate
	@InstanceId INT,
	@ProductId INT = NULL,
	@CategoryId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductCategories ( InstanceId, ProductId, CategoryId ) 
	VALUES ( @InstanceId, @ProductId, @CategoryId)

	SET @Result = @ProductId

	SELECT ProductId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpAttributeCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CategoryId INT,
	@Name NVARCHAR(500) = NULL,
	@Description NVARCHAR(1000)  = NULL,
	@DefaultValue NVARCHAR(1000)  = NULL,
	@Type INT,
	@TypeLimit NVARCHAR(MAX) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpAttribute ( InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @CategoryId, @Name, @Description, @DefaultValue, @Type, @TypeLimit, @Locale,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AttributeId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductValueCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ProductId INT,
	@AttributeId INT,
	@Value NVARCHAR(1000)  = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductValue ( InstanceId, ProductId, AttributeId, Value,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ProductId, @AttributeId, @Value,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductValueId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpHighlightCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpHighlight ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT HighlightId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpAddressCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@FirstName NVARCHAR(200) = NULL,
	@LastName NVARCHAR(200) = NULL,
	@Organization NVARCHAR(200) = NULL,
	@Id1 NVARCHAR(100) = NULL,
	@Id2 NVARCHAR(100) = NULL,
	@Id3 NVARCHAR(100) = NULL,	
	@City NVARCHAR(100) = '',
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@State NVARCHAR(100)= '',
	@Phone NVARCHAR(100) = NULL,
	@Email NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpAddress ( InstanceId, FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @FirstName, @LastName, @Organization, @Id1, @Id2, @Id3, @City, @Street, @Zip, @State, @Phone, @Email, @Notes,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AddressId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCartCreate
	@InstanceId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentId INT = NULL,		
	@PaymentId INT = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentId, @PaymentId, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCartProductCreate
	@InstanceId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CartId INT,
	@OrderStatusId INT = NULL,								
	@ShipmentId INT,		
	@PaymentId INT,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
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
	SET @number = replicate( 0, 5- LEN(@number) ) + @number

	SET @OrderNumber = 'OBJ-' + @year + @month + '-' + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, PaydDate, Notes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, GETDATE(), @OrderStatusId, @ShipmentId,  @PaymentId, @DeliveryAddressId, @InvoiceAddressId, @PaydDate, @Notes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpOrderStatusCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpOrderStatus ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderStatusId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpVATCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Percent DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpVAT ( InstanceId, Locale, [Name], [Notes], Code, [Percent], Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Percent, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpShipmentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Price DECIMAL(19,2) = 0,
	@VATId INT = NULL, /*DPH%*/	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpShipment ( InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Price, @VATId, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ShipmentId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpPaymentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpPayment ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PaymentId = @Result

END
GO
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpCurrencyCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Rate DECIMAL(19,2) = 0,
	@Symbol VARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpCurrency ( InstanceId, Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Rate, @Symbol, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CurrencyId = @Result

END
GO
------------------------------------------------------------------------------------
-- SEARCH
------------------------------------------------------------------------------------
ALTER PROCEDURE pShpSearchProducts
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.ProductId, Title = p.Name, 
		Content = ISNULL(p.Manufacturer,'') + ISNULL( p.Description, '')/* + ISNULL(p.DescriptionLong, '')*/ , 
		UrlAlias = a.Alias, ImageUrl = NULL
	FROM tShpProduct p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND 
	(
		p.Name LIKE '%'+@Keywords+'%' OR 
		p.Description LIKE '%'+@Keywords+'%' OR
		--p.DescriptionLong LIKE '%'+@Keywords+'%' OR
		p.Manufacturer LIKE '%'+@Keywords+'%' 
	)
	
END
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade ESHOP db version
INSERT INTO tShpUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 1, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
