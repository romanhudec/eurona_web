ALTER PROCEDURE pForumPostDelete
	@HistoryAccount INT,
	@ForumPostId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumPostId IS NULL OR NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumPostId=%d', 16, 1, @ForumPostId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumPost
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumPostId
		WHERE ForumPostId = @ForumPostId

		SET @Result = @ForumPostId

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
