ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (p.Price - ( p.Price * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (p.Price - p.Discount )/*Zlava Suma*/
		ELSE p.Price
		END, 

	PriceTotal = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (p.Price - ( p.Price * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (p.Price - p.Discount )/*Zlava Suma*/
		ELSE p.Price
		END, 

	PriceTotalWVAT = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN ROUND((p.Price - ( p.Price * ( p.Discount / 100 ) )) * (1 + ISNULL(v.[Percent], 0)/100), 2 )/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN ROUND((p.Price - p.Discount ) * (1 + ISNULL(v.[Percent], 0)/100), 2 )/*Zlava Suma*/
		ELSE p.Price
		END, 

	ProductAvailability = p.Availability, a.Alias
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
