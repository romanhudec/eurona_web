ALTER PROCEDURE pPollOptionDelete
	@PollOptionId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PollOptionId IS NULL OR NOT EXISTS(SELECT * FROM tPollOption WHERE PollOptionId = @PollOptionId ) 
		RAISERROR('Invalid @PollOptionId=%d', 16, 1, @PollOptionId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tPollAnswer WHERE PollOptionId = @PollOptionId
		DELETE FROM tPollOption WHERE PollOptionId = @PollOptionId

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
