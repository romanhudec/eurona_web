ALTER PROCEDURE pSupportedLocaleModify
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid SupportedLocaleId %d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @SupportedLocaleId
		FROM cSupportedLocale
		WHERE SupportedLocaleId = @SupportedLocaleId

		UPDATE cSupportedLocale
		SET
			[Name] = @Name, [Notes] = @Notes, Code = @Code, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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
