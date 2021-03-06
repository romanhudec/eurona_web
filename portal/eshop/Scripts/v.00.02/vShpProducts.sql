
ALTER VIEW vShpProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, p.[Name], p.[Manufacturer], p.[Description], p.[DescriptionLong], p.Availability, 
	p.StorageCount, p.Price, p.Discount, 
	p.VATId, VAT = ISNULL(v.[Percent], 0),
	p.Locale, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 )
FROM
	tShpProduct p 
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO
