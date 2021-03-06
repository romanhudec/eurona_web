ALTER VIEW vShpProductReviews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductReviewsId, pr.InstanceId, pr.ProductId, pr.ArticleId,
	a.Icon, a.Title, a.Teaser, a.RoleId, a.Country, a.City, a.ReleaseDate, a.Visible, 
	alias.Alias
FROM
	tShpProductReviews pr
	INNER JOIN vArticles a ON a.ArticleId = pr.ArticleId
	INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = a.UrlAliasId
GO
