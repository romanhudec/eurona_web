
ALTER VIEW vShpPiktogramyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpPiktogramyProduktu 
GO
