ALTER TABLE tShpProduct ADD MinimalniPocetVBaleni INT NULL
GO
-------------------------------------------------------------------------------------------------------------
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
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale, p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, BonusovyKredit = cp.[CenaBK]
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL

GO
-------------------------------------------------------------------------------------------------------------
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
			MaximalniPocetVBaleni=@MaximalniPocetVBaleni, MinimalniPocetVBaleni=@MinimalniPocetVBaleni, VamiNejviceNakupovane=@VamiNejviceNakupovane, InternalStorageCount=@InternalStorageCount, LimitDate=@LimitDate, DarkovySet=@DarkovySet,
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
-------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='NedostatecnyMinimalniPocetObjednatelnehoPoctu', @Locale='sk', @Translation='Minimálny počet tohto výrobku, ktorý je možné objednať v jednej objednávke je {0}'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='NedostatecnyMinimalniPocetObjednatelnehoPoctu', @Locale='cs', @Translation='Minimální počet tohoto výrobku, který je možné objednat v jedné objednávce je {0}'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='NedostatecnyMinimalniPocetObjednatelnehoPoctu', @Locale='pl', @Translation='Min. liczba produktów, które mogą być zamówione w jednym zamówieniu to {0}'

