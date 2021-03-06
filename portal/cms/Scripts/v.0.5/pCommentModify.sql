ALTER PROCEDURE pCommentModify
	@HistoryAccount INT,
	@CommentId INT,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tComment ( Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, @CommentId
		FROM tComment
		WHERE CommentId = @CommentId

		UPDATE tComment
		SET
			Title = @Title, Content = @Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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
