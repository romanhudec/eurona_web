ALTER PROCEDURE pSearchForumPosts
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = f.ForumPostId, f.Title, Content = f.Content, UrlAlias = a.Alias, ImageUrl = NULL, RoleId=NULL, RoleString = ft.VisibleForRole
	FROM tForumPost f INNER JOIN
	tForum fo ON fo.ForumId = f.ForumId INNER JOIN
	tForumThread ft ON ft.ForumThreadId = fo.ForumThreadId INNER JOIN
	tUrlAlias a ON a.UrlAliasId = fo.UrlAliasId
	WHERE f.HistoryId IS NULL AND f.InstanceId = @InstanceId AND
	(
		f.Title LIKE '%'+@Keywords+'%' OR 
		f.Content LIKE '%'+@Keywords+'%'
	)
END
GO