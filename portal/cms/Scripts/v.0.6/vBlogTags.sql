
ALTER VIEW vBlogTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.BlogTagId, a.BlogId, t.TagId, t.Tag
FROM
	tBlogTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO
