ALTER PROCEDURE pSearchBlogComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vBlogComments gc INNER JOIN
		tBlog b ON b.BlogId = gc.BlogId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
		WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO