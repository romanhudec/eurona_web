ALTER PROCEDURE pAccountCreditModify
	@HistoryAccount INT,
	@AccountCreditId INT,
	@Credit DECIMAL(19,2), 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountCredit WHERE AccountCreditId = @AccountCreditId AND HistoryId IS NULL) 
		RAISERROR('Invalid AccountCreditId %d', 16, 1, @AccountCreditId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccountCredit ( InstanceId, AccountId, Credit,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountId, Credit,
			HistoryStamp, HistoryType, HistoryAccount, @AccountCreditId
		FROM tAccountCredit
		WHERE AccountCreditId = @AccountCreditId

		UPDATE tAccountCredit
		SET
			Credit = @Credit,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountCreditId = @AccountCreditId

		SET @Result = @AccountCreditId

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
