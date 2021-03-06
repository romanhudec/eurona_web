ALTER PROCEDURE pForumTrackingDelete
	@ForumTrackingId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumTrackingId IS NULL OR NOT EXISTS(SELECT * FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId) 
		RAISERROR('Invalid @ForumTrackingId=%d', 16, 1, @ForumTrackingId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId
		SET @Result = @ForumTrackingId

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
