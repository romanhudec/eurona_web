ALTER PROCEDURE pSupportedLocaleDelete
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @SupportedLocaleId IS NULL OR NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @SupportedLocaleId=%d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cSupportedLocale
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @SupportedLocaleId
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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
