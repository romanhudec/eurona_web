ALTER PROCEDURE pImageGalleryDelete
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryId IS NULL OR NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryId=%d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGallery
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryId
		WHERE ImageGalleryId = @ImageGalleryId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tImageGallery SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ImageGalleryId

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
