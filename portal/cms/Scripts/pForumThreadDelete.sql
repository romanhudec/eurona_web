ALTER PROCEDURE pForumThreadDelete
	@HistoryAccount INT,
	@ForumThreadId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumThreadId IS NULL OR NOT EXISTS(SELECT * FROM tForumThread WHERE ForumThreadId = @ForumThreadId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumThreadId=%d', 16, 1, @ForumThreadId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumThread
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumThreadId
		WHERE ForumThreadId = @ForumThreadId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tForumThread WHERE ForumThreadId = @ForumThreadId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tForumThread SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ForumThreadId

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
