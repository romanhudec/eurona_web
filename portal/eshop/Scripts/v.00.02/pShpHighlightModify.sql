ALTER PROCEDURE pShpHighlightModify
	@HistoryAccount INT,
	@HighlightId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpHighlight WHERE HighlightId = @HighlightId AND HistoryId IS NULL) 
		RAISERROR('Invalid HighlightId %d', 16, 1, @HighlightId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpHighlight ( Locale, [Name], [Notes], Code, Icon, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @HighlightId
		FROM cShpHighlight
		WHERE HighlightId = @HighlightId

		UPDATE cShpHighlight
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Icon= @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE HighlightId = @HighlightId

		SET @Result = @HighlightId

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
