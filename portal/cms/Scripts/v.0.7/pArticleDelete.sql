ALTER PROCEDURE pArticleDelete
	@HistoryAccount INT,
	@ArticleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleId IS NULL OR NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleId=%d', 16, 1, @ArticleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tArticle
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleId
		WHERE ArticleId = @ArticleId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tArticle WHERE ArticleId = @ArticleId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tArticle SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ArticleId

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
