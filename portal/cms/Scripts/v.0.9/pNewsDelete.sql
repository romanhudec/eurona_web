ALTER PROCEDURE pNewsDelete
	@HistoryAccount INT,
	@NewsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NewsId IS NULL OR NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NewsId=%d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY
	
		UPDATE tNews
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NewsId
		WHERE NewsId = @NewsId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tNews WHERE NewsId = @NewsId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tNews SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END
		
		SET @Result = @NewsId

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
