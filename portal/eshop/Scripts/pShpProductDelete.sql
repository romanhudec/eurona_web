ALTER PROCEDURE pShpProductDelete
	@HistoryAccount INT,
	@ProductId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductId IS NULL OR NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ProductId=%d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProduct
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ProductId
		WHERE ProductId = @ProductId

		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tShpProduct WHERE ProductId = @ProductId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tShpProduct SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END	
		
		SET @Result = @ProductId

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
