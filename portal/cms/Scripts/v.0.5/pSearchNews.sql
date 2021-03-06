ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Head, 
		Content = n.Description + ' ' + n.Content , UrlAlias = a.Alias 
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND
	(
		n.Head LIKE '%'+@Keywords+'%' OR 
		n.Description LIKE '%'+@Keywords+'%' OR 
		n.Content LIKE '%'+@Keywords+'%'
	)
	
END
GO