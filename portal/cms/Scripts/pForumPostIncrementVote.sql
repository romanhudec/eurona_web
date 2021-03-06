ALTER PROCEDURE pForumPostIncrementVote
	@ForumPostId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumPostId %d', 16, 1, @ForumPostId);

	UPDATE tForumPost 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ForumPostId = @ForumPostId

END
GO
