
ALTER VIEW vImageGalleryTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryTagId, g.ImageGalleryId, t.TagId, t.Tag
FROM
	tImageGalleryTag g 
	INNER JOIN vTags t ON t.TagId = g.TagId
GO
