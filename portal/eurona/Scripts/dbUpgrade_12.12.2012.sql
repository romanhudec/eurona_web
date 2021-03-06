
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT, @ProductsFBMasterPageId INT
SET @MasterPageId = 1

DECLARE @UrlAliasId INT
DECLARE @PageId INT
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)


--Error content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1801, @MasterPageId, '', 'sk', 'anonymous-cart-rotator-content', 'BANNER Produktu v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1802, @MasterPageId, '', 'cs', 'anonymous-cart-rotator-content', 'BANNER Produktu v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1803, @MasterPageId, '', 'en', 'anonymous-cart-rotator-content', 'BANNER Produktu v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -18044, @MasterPageId, '', 'pl', 'anonymous-cart-rotator-content', 'BANNER Produktu v košiku anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

----------------------------------------------------------------------------------------------------------------------------------------------------------
-- DARKOVE SETY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Dárkové sety'
SET @pageUrl = '~/eshop/products.aspx?id=ds'
SET @pageAlias = '~/eshop/darkove-sety'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Dárkové sety'
SET @pageUrl = '~/eshop/products.aspx?id=ds'
SET @pageAlias = '~/eshop/darkove-sety'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		

-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpProduct ADD LimitDate DATETIME NULL, DarkovySet INT NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductModifyEx
	@HistoryAccount INT,
	@ProductId INT,
	@MaximalniPocetVBaleni INT = NULL,
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
			MaximalniPocetVBaleni=@MaximalniPocetVBaleni, VamiNejviceNakupovane=@VamiNejviceNakupovane, InternalStorageCount=@InternalStorageCount, LimitDate=@LimitDate, DarkovySet=@DarkovySet,
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

------------------------------------------------------------------------------------------------------------------------
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
	pl.Locale, p.MaximalniPocetVBaleni, BonusovyKredit = cp.[CenaBK]
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='DoPostovnehoZdarmaChybiObjednatJesteZa', @Locale='sk', @Translation='Aby ste mali poštovné zadarmo, musíte ešte objednať produkty za {0} €.'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='DoPostovnehoZdarmaChybiObjednatJesteZa', @Locale='cs', @Translation='Aby jste měli poštovné zdarma musíte ješte objednat výrobky za {0} Kč.'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='DoPostovnehoZdarmaChybiObjednatJesteZa', @Locale='pl', @Translation='Tak, że masz darmowe wysyłki nadal zamawiać produkty dla {0} zł.'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='GratulujemeMatePostovneZdarma', @Locale='sk', @Translation='Gratulujeme, na tejto objednávke už poštovné neplatíte!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='GratulujemeMatePostovneZdarma', @Locale='cs', @Translation='Gratulujeme, na této objednávce již poštovné neplatíte!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='GratulujemeMatePostovneZdarma', @Locale='pl', @Translation='Gratulujemy tej kolejności już płacić opłatę pocztową!'

EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceUkoncena', @Locale='sk', @Translation='Limitovaná akcia bola ukončená! Tento produkt sa už objednať nedá!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceUkoncena', @Locale='cs', @Translation='Limitovaná akce byla ukončena! Tento produkt se už objednat nedá!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceUkoncena', @Locale='pl', @Translation='Ograniczone działanie zostało zakończone! Produkt nie jest już nie kazał!'

EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceInfo', @Locale='sk', @Translation='Limitovaná akcia do {0}, v ponuke posledných {1} ks!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceInfo', @Locale='cs', @Translation='Limitovaná akce do {0}, v nabídce posledních {1} ks!'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='TextoveHlaseni', @Term='LimitovanaAkceInfo', @Locale='pl', @Translation='Ograniczone działania {0}, ostatnie oferowanych {1} sztuk!'
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------