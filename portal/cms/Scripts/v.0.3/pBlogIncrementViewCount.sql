ALTER PROCEDURE pBlogIncrementViewCount
	@BlogId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE BlogId = @BlogId

END
GO
