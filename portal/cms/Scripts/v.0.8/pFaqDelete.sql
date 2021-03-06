ALTER PROCEDURE pFaqDelete
	@HistoryAccount INT,
	@FaqId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @FaqId IS NULL OR NOT EXISTS(SELECT * FROM tFaq WHERE FaqId = @FaqId AND HistoryId IS NULL) 
		RAISERROR('Invalid @FaqId=%d', 16, 1, @FaqId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tFaq
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @FaqId
		WHERE FaqId = @FaqId

		SET @Result = @FaqId

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
