ALTER PROCEDURE pBlogDelete
	@HistoryAccount INT,
	@BlogId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @BlogId IS NULL OR NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid @BlogId=%d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tBlog
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @BlogId
		WHERE BlogId = @BlogId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tBlog WHERE BlogId = @BlogId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tBlog SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @BlogId

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
