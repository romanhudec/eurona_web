ALTER PROCEDURE pPollOptionModify
	@PollOptionId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPollOption WHERE PollOptionId = @PollOptionId ) 
		RAISERROR('Invalid PollOptionId %d', 16, 1, @PollOptionId);

	BEGIN TRANSACTION;

	BEGIN TRY


		UPDATE tPollOption
		SET [Order] = @Order, [Name] = @Name
		WHERE PollOptionId = @PollOptionId

		SET @Result = @PollOptionId

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
