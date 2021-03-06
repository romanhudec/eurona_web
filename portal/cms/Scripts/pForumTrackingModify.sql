ALTER PROCEDURE pForumTrackingModify
	@ForumTrackingId INT,
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId ) 
		RAISERROR('Invalid ForumTrackingId %d', 16, 1, @ForumTrackingId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumTracking SET ForumId= @ForumId, AccountId=@AccountId
		WHERE ForumTrackingId = @ForumTrackingId

		SET @Result = @ForumTrackingId

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
