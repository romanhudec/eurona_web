
ALTER VIEW vShpVlastnostiProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpVlastnostiProduktu 
GO
