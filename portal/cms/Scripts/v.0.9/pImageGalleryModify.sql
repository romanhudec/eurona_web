ALTER PROCEDURE pImageGalleryModify
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGallery ( InstanceId, [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryId
		FROM tImageGallery
		WHERE ImageGalleryId = @ImageGalleryId

		UPDATE tImageGallery
		SET
			[Name]=@Name, [Date]=@Date, RoleId=@RoleId, Visible=@Visible, UrlAliasId=@UrlAliasId, EnableComments=@EnableComments, EnableVotes=@EnableVotes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryId = @ImageGalleryId

		SET @Result = @ImageGalleryId

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
