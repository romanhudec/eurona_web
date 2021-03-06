ALTER PROCEDURE pForumDelete
	@HistoryAccount INT,
	@ForumId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumId IS NULL OR NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumId=%d', 16, 1, @ForumId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForum
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumId
		WHERE ForumId = @ForumId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tForum WHERE ForumId = @ForumId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tForum SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ForumId

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
