ALTER PROCEDURE pPollDelete
	@HistoryAccount INT,
	@PollId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PollId IS NULL OR NOT EXISTS(SELECT * FROM tPoll WHERE PollId = @PollId AND HistoryId IS NULL) 
		RAISERROR('Invalid @PollId=%d', 16, 1, @PollId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tPoll
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PollId
		WHERE PollId = @PollId

		SET @Result = @PollId

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
