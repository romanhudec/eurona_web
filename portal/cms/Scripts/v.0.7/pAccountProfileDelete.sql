ALTER PROCEDURE pAccountProfileDelete
	@HistoryAccount INT,
	@AccountProfileId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AccountProfileId IS NULL OR NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AccountProfileId=%d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAccountProfile
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountProfileId
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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
