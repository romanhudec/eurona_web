ALTER PROCEDURE pArticleCategoryDelete
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleCategoryId IS NULL OR NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleCategoryId=%d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cArticleCategory
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleCategoryId
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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
