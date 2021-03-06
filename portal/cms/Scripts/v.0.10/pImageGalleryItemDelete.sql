ALTER PROCEDURE pImageGalleryItemDelete
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryItemId IS NULL OR NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryItemId=%d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGalleryItem
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryItemId
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
