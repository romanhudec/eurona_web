ALTER PROCEDURE pAccountProfileModify
	@HistoryAccount INT,
	@AccountProfileId INT,
	@AccountId INT = NULL,
	@ProfileId INT = NULL,
	@Value NVARCHAR(MAX),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid AccountProfileId %d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccountProfile ( AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, @AccountProfileId
		FROM tAccountProfile
		WHERE AccountProfileId = @AccountProfileId

		UPDATE tAccountProfile
		SET
			AccountId=ISNULL(@AccountId, AccountId), ProfileId=ISNULL(@ProfileId, ProfileId), Value=@Value,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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
