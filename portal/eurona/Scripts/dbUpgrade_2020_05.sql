ALTER TABLE tShpCenyProduktu ADD [PlatnostOd] DATETIME NULL, [PlatnostDo] DATETIME NULL
GO 

-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vShpProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, BeznaCena = ISNULL(cp.BeznaCena, 0 ), MarzePovolena = ISNULL( cp.MarzePovolena, 0 ), MarzePovolenaMinimalni = ISNULL( cp.MarzePovolenaMinimalni, 0 ),
	cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , cp.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], 
	p.[Megasleva],  p.[Supercena], p.[CLHit], p.[Action], p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount], p.[LimitDate], 
	p.Parfumacia, p.Discount, cp.DynamickaSleva, cp.StatickaSleva, p.VamiNejviceNakupovane, p.DarkovySet,
	p.VAT, a.UrlAliasId, a.Url, a.Alias,pl.FiullText,
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
	--, expr = LOWER(dbo.fMakeAnsi(pl.[Name] + ' ' + p.Code + ' ' + pl.Description))
FROM
	tShpProduct p WITH (NOLOCK)
	LEFT JOIN tShpProductLocalization pl WITH (NOLOCK) ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp WITH (NOLOCK) ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale and ((cp.PlatnostOd is null or cp.PlatnostOd <= GETDATE()) and (cp.PlatnostDo is null or cp.PlatnostDo >= GETDATE()))
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a WITH (NOLOCK) ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL

GO
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
	LEFT JOIN tShpCenyProduktu cenyP WITH (NOLOCK) ON cenyP.ProductId = p.ProductId AND cenyP.Locale = pl.Locale and ((cenyP.PlatnostOd is null or cenyP.PlatnostOd <= GETDATE()) and (cenyP.PlatnostDo is null or cenyP.PlatnostDo >= GETDATE()))
	INNER JOIN tShpCart c WITH (NOLOCK) ON c.CartId = cp.CartId
	LEFT JOIN tUrlAlias a WITH (NOLOCK) ON a.UrlAliasId = pl.UrlAliasId
	--LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.CurrencyId = cp.CurrencyId
	LEFT JOIN cShpCurrency cur WITH (NOLOCK) ON cur.Locale = pl.Locale
ORDER BY cp.POrderD ASC

GO

