ALTER PROCEDURE pShpCategoryDelete
	@HistoryAccount INT,
	@CategoryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CategoryId IS NULL OR NOT EXISTS(SELECT * FROM tShpCategory WHERE CategoryId = @CategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CategoryId=%d', 16, 1, @CategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCategory
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CategoryId
		WHERE CategoryId = @CategoryId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tShpCategory WHERE CategoryId = @CategoryId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tShpCategory SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END		

		SET @Result = @CategoryId

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
