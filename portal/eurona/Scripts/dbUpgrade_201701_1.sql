ALTER TABLE tShpProduct ADD ZadniEtiketa NVARCHAR(500) NULL, ZobrazovatZadniEtiketu BIT DEFAULT(0)
GO

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
	pl.Locale, p.MaximalniPocetVBaleni, p.MinimalniPocetVBaleni, BonusovyKredit = cp.[CenaBK],
	p.ZadniEtiketa, p.ZobrazovatZadniEtiketu
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL

GO

-- CRON: 
-- doplnit : dstZadniEtiketyImagePath do EURONA_IMPORT_PRODUCT
-- <add name="dstZadniEtiketyImagePath" value="d:\sk\Mothiva\artemis.cms\trunk\brand\eurona\eurona\images\ZadniEtikety"></add>