ALTER PROCEDURE pShpCurrencyModify
	@HistoryAccount INT,
	@CurrencyId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Rate DECIMAL(19,2) = 0,
	@Symbol VARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpCurrency WHERE CurrencyId = @CurrencyId AND HistoryId IS NULL) 
		RAISERROR('Invalid CurrencyId %d', 16, 1, @CurrencyId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpCurrency ( Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @CurrencyId
		FROM cShpCurrency
		WHERE CurrencyId = @CurrencyId

		UPDATE cShpCurrency
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Rate = @Rate, Symbol = @Symbol, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CurrencyId = @CurrencyId

		SET @Result = @CurrencyId

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
