ALTER PROCEDURE pSearchImageGalleries
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = i.ImageGalleryId, Title = i.Name, 
		Content = NULL, 
		UrlAlias = a.Alias 
	FROM tImageGallery i INNER JOIN
	tUrlAlias a ON a.UrlAliasId = i.UrlAliasId
	WHERE i.HistoryId IS NULL AND
	(
		i.Name LIKE '%'+@Keywords+'%'
	)
	
END
GO