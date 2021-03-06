
ALTER VIEW vShpCenyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	c.ProductId, c.Locale, c.CurrencyId, c.Cena, BeznaCena = ISNULL(c.BeznaCena, 0 ), SurrencySymbol = cur.Symbol, SurrencyCode = cur.Code,
	MarzePovolena = ISNULL( c.MarzePovolena, 0 ),
	MarzePovolenaMinimalni = ISNULL( c.MarzePovolenaMinimalni, 0 ),
	DynamickaSleva, StatickaSleva, CenaBK
FROM tShpCenyProduktu c INNER JOIN
cShpCurrency cur ON cur.CurrencyId=c.CurrencyId
GO
