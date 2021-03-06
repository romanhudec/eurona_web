ALTER PROCEDURE pSearchPages
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT Id = ISNULL(p.ParentId, p.PageId), p.Title, Content = ISNULL(p.ContentKeywords, ''), UrlAlias = a.Alias, ImageUrl = NULL, p.RoleId, RoleString = NULL
	FROM tPage p LEFT JOIN
	tPage pParent ON pParent.PageId=p.ParentId INNER JOIN
	tUrlAlias a ON a.UrlAliasId = ISNULL(pParent.UrlAliasId, p.UrlAliasId )
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO