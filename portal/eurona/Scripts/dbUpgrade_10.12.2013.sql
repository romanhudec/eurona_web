------------------------------------------------------------------------------------------------------------------------
-- UPGRADE Eurona
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

ALTER TABLE tShpCartProduct ADD POrderD [INT] NOT NULL DEFAULT(0)
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vShpCartProducts
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
	p.MaximalniPocetVBaleni, CerpatBK = ISNULL(cp.CerpatBK, 0),
	POrder = cp.POrderD
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p WITH (NOLOCK) ON p.ProductId = cp.ProductId
	LEFT JOIN tShpProductLocalization pl WITH (NOLOCK) ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cenyP WITH (NOLOCK) ON cenyP.ProductId = p.ProductId AND cenyP.Locale = pl.Locale
	INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = cp.CartId
	LEFT JOIN tUrlAlias a WITH (NOLOCK) ON a.UrlAliasId = pl.UrlAliasId
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.CurrencyId = cp.CurrencyId
ORDER BY cp.POrderD ASC
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
	@CurrencyId INT = NULL,
	@CerpatBK BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity, Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, CurrencyId, CerpatBK, POrderD ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity, @Price, @PriceWVAT, @VAT, @Discount, @PriceTotal, @PriceTotalWVAT, @CurrencyId, @CerpatBK, 
	(select convert(decimal(12,6),getdate()))  )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
--INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Eshop products form with share on Facebook', 'Default MasterPage for products with Share on FB Froms', '~/eshop/default.master', '~/eshop/pageFB.aspx?name=')
--SET @ProductsFBMasterPageId = SCOPE_IDENTITY()
------------------------------------------------------------------------------------------------------------------------