ALTER PROCEDURE pCommentIncrementVote
	@CommentId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	UPDATE tComment 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE CommentId = @CommentId

END
GO
