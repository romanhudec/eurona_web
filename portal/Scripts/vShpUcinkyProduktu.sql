
ALTER VIEW vShpUcinkyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpUcinkyProduktu 
GO
