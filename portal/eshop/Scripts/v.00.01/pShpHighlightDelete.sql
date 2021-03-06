ALTER PROCEDURE pShpHighlightDelete
	@HistoryAccount INT,
	@HighlightId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @HighlightId IS NULL OR NOT EXISTS(SELECT * FROM tShpHighlight WHERE HighlightId = @HighlightId AND HistoryId IS NULL) 
		RAISERROR('Invalid @HighlightId=%d', 16, 1, @HighlightId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpHighlight
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @HighlightId
		WHERE HighlightId = @HighlightId

		SET @Result = @HighlightId

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
