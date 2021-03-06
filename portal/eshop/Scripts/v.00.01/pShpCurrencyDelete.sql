ALTER PROCEDURE pShpCurrencyDelete
	@HistoryAccount INT,
	@CurrencyId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CurrencyId IS NULL OR NOT EXISTS(SELECT * FROM cShpCurrency WHERE CurrencyId = @CurrencyId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CurrencyId=%d', 16, 1, @CurrencyId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpCurrency
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CurrencyId
		WHERE CurrencyId = @CurrencyId

		SET @Result = @CurrencyId

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
