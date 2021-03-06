ALTER PROCEDURE pUrlAliasPrefixModify
	@HistoryAccount INT,
	@UrlAliasPrefixId INT,
	@Name NVARCHAR(100) = '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cUrlAliasPrefix WHERE UrlAliasPrefixId = @UrlAliasPrefixId AND HistoryId IS NULL) 
		RAISERROR('Invalid UrlAliasPrefixId %d', 16, 1, @UrlAliasPrefixId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cUrlAliasPrefix ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @UrlAliasPrefixId
		FROM cUrlAliasPrefix
		WHERE UrlAliasPrefixId = @UrlAliasPrefixId

		UPDATE cUrlAliasPrefix
		SET
			[Name] = @Name, [Notes] = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE UrlAliasPrefixId = @UrlAliasPrefixId

		SET @Result = @UrlAliasPrefixId

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
