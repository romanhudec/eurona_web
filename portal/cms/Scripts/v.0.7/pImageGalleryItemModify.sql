ALTER PROCEDURE pImageGalleryItemModify
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGalleryItem ( ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryItemId
		FROM tImageGalleryItem
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		UPDATE tImageGalleryItem
		SET
			VirtualPath = ISNULL(@VirtualPath, VirtualPath), VirtualThumbnailPath = ISNULL(@VirtualThumbnailPath, VirtualThumbnailPath), Description = @Description, [Position] = @Position,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
