ALTER PROCEDURE pShpVATModify
	@HistoryAccount INT,
	@VATId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Percent DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpVAT WHERE VATId = @VATId AND HistoryId IS NULL) 
		RAISERROR('Invalid VATId %d', 16, 1, @VATId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpVAT ( Locale, [Name], [Notes], Code, [Percent], Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Name], [Notes], Code, [Percent], Icon,
			HistoryStamp, HistoryType, HistoryAccount, @VATId
		FROM cShpVAT
		WHERE VATId = @VATId

		UPDATE cShpVAT
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, [Percent] = @Percent, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE VATId = @VATId

		SET @Result = @VATId

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
