ALTER PROCEDURE pAccountCreditDelete
	@HistoryAccount INT,
	@AccountCreditId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AccountCreditId IS NULL OR NOT EXISTS(SELECT * FROM tAccountCredit WHERE AccountCreditId = @AccountCreditId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AccountCreditId=%d', 16, 1, @AccountCreditId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAccountCredit
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountCreditId
		WHERE AccountCreditId = @AccountCreditId

		SET @Result = @AccountCreditId

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
