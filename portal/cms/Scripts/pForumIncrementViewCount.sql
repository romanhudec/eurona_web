ALTER PROCEDURE pForumIncrementViewCount
	@ForumId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumId %d', 16, 1, @ForumId);

	UPDATE tForum SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ForumId = @ForumId

END
GO
