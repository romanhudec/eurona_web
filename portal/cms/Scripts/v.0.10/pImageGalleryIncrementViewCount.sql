ALTER PROCEDURE pImageGalleryIncrementViewCount
	@ImageGalleryId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	UPDATE tImageGallery SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryId = @ImageGalleryId

END
GO
