ALTER PROCEDURE pPageDelete
	@HistoryAccount INT,
	@PageId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PageId IS NULL OR NOT EXISTS(SELECT * FROM tPage WHERE PageId = @PageId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @PageId=%d', 16, 1, @PageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tPage
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PageId
		WHERE PageId = @PageId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tPage WHERE PageId = @PageId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tPage SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END		

		SET @Result = @PageId

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
