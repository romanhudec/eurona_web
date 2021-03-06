ALTER PROCEDURE pCommentDelete
	@HistoryAccount INT,
	@CommentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CommentId IS NULL OR NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CommentId=%d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tComment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CommentId
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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
