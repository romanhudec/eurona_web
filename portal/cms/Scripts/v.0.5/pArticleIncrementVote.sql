ALTER PROCEDURE pArticleIncrementVote
	@ArticleId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ArticleId = @ArticleId

END
GO
