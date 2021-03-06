ALTER PROCEDURE pSearchForums
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = f.ForumId, Title=f.Name, Content = f.Description, UrlAlias = a.Alias, ImageUrl = NULL, RoleId=NULL, RoleString = ft.VisibleForRole
	FROM tForum f INNER JOIN
	tForumThread ft ON ft.ForumThreadId = f.ForumThreadId INNER JOIN
	tUrlAlias a ON a.UrlAliasId = f.UrlAliasId
	WHERE f.HistoryId IS NULL AND f.InstanceId = @InstanceId AND
	(
		f.Name LIKE '%'+@Keywords+'%' OR 
		f.Description LIKE '%'+@Keywords+'%'
	)
END
GO