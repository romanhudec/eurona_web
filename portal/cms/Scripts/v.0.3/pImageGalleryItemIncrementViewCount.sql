ALTER PROCEDURE pImageGalleryItemIncrementViewCount
	@ImageGalleryItemId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO
