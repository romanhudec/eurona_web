ALTER PROCEDURE pImageGalleryItemIncrementVote
	@ImageGalleryItemId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO
