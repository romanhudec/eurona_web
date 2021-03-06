
ALTER VIEW vImageGalleryItems
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ImageGalleryItemId, ImageGalleryId, [VirtualPath], [VirtualThumbnailPath], [Position], [Date], Description,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryItemComments WHERE ImageGalleryItemId = g.ImageGalleryItemId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	Votes = ISNULL(Votes, 0), 
	TotalRating = ISNULL(TotalRating, 0),
	RatingResult =  ISNULL(TotalRating*1.0/Votes*1.0, 0 )	
FROM tImageGalleryItem g
WHERE HistoryId IS NULL
GO
