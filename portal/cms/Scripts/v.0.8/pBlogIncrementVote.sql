ALTER PROCEDURE pBlogIncrementVote
	@BlogId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE BlogId = @BlogId

END
GO
