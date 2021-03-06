ALTER PROCEDURE pPollModify
	@HistoryAccount INT,
	@PollId INT,
	@Closed BIT = 0,
	@Question NVARCHAR(1000),
	@DateFrom DATETIME,
	@DateTo DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPoll WHERE PollId = @PollId AND HistoryId IS NULL) 
		RAISERROR('Invalid PollId %d', 16, 1, @PollId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tPoll ( InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @PollId
		FROM tPoll
		WHERE PollId = @PollId

		UPDATE tPoll
		SET
			Closed = @Closed, Question = @Question, DateFrom = @DateFrom, DateTo = @DateTo, Icon = @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PollId = @PollId

		SET @Result = @PollId

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
