ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = (p.Price - ( p.Price * ( p.Discount / 100 ) )), 
	PriceTotal = (p.Price - ( p.Price * ( p.Discount / 100 ))),
	PriceTotalWVAT = ROUND((p.Price - ( p.Price * ( p.Discount / 100 ) )) * (1 + ISNULL(v.[Percent], 0)/100), 2),
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
