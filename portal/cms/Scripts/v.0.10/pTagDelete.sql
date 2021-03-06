ALTER PROCEDURE pTagDelete
	@HistoryAccount INT,
	@TagId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @TagId IS NULL OR NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid @TagId=%d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tTag
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @TagId
		WHERE TagId = @TagId

		SET @Result = @TagId

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
