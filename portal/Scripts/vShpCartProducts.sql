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