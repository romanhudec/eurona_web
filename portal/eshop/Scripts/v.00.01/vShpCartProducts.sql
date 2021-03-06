
ALTER VIEW vShpCartProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, cp.InstanceId, cp.CartId, cp.ProductId, cp.Quantity, c.AccountId,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = (p.Price - ( p.Price * ( p.Discount / 100 ) )), 
	PriceTotal = ((p.Price - ( p.Price * ( p.Discount / 100 ) )) * cp.Quantity),
	PriceTotalWVAT = ROUND(((p.Price - ( p.Price * ( p.Discount / 100 ) )) * cp.Quantity) * (1 + ISNULL(v.[Percent], 0)/100), 2),
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p ON p.ProductId = cp.ProductId
	INNER JOIN tShpCart c ON c.CartId = cp.CartId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
