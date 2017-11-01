
ALTER VIEW vArticleTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleTagId, a.ArticleId, t.TagId, t.Tag
FROM
	tArticleTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO
