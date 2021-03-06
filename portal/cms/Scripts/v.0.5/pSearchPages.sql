ALTER PROCEDURE pSearchPages
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.PageId, p.Title, Content = p.ContentKeywords, UrlAlias = a.Alias 
	FROM tPage p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO