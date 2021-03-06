ALTER PROCEDURE pFaqModify
	@HistoryAccount INT,
	@FaqId INT,
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tFaq WHERE FaqId = @FaqId AND HistoryId IS NULL) 
		RAISERROR('Invalid FaqId %d', 16, 1, @FaqId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tFaq ( InstanceId, Locale, [Order], Question, Answer,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Order], Question, Answer,
			HistoryStamp, HistoryType, HistoryAccount, @FaqId
		FROM tFaq
		WHERE FaqId = @FaqId

		UPDATE tFaq
		SET
			[Order] = @Order, Question = @Question, Answer = @Answer,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE FaqId = @FaqId

		SET @Result = @FaqId

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
