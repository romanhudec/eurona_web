ALTER PROCEDURE pArticleIncrementViewCount
	@ArticleId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ArticleId = @ArticleId

END
GO
