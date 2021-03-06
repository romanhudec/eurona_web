ALTER PROCEDURE pSearchImageGalleryComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS,
	ImageUrl NVARCHAR(500)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias,
		ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = g.ImageGalleryId ORDER BY gi.Position ASC) 
		FROM vImageGalleryComments gc INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gc.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO