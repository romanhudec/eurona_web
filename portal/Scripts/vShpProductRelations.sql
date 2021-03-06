ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = pl.Name, ProductPrice= cp.Cena, cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code, ProductDiscount= p.Discount,
	ProductPriceWDiscount = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (cp.Cena - ( cp.Cena * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (cp.Cena - p.Discount )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	PriceTotal = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (cp.Cena - ( cp.Cena * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (cp.Cena - p.Discount )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	PriceTotalWVAT = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN ROUND((cp.Cena - ( cp.Cena * ( p.Discount / 100 ) )) * (1 + ISNULL(p.VAT, 0)/100), 2 )/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN ROUND((cp.Cena - p.Discount ) * (1 + ISNULL(p.VAT, 0)/100), 2 )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	ProductAvailability = p.Availability, a.Alias,
	pl.Locale
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
GO
