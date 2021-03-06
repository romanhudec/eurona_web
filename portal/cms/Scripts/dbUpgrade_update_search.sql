-- 07.01.2011
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT,  @MasterPageId INT, @UrlAliasId INT, @PageId INT
SET @InstanceId = 1
SET @MasterPageId = 1
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pSearchForums AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchForumPosts AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticleComments
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
	RoleId INT NULL,
	RoleString NVARCHAR(255) NULL  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias, art.RoleId, RoleString = NULL
		FROM vArticleComments gc INNER JOIN
		tArticle art ON art.ArticleId = gc.ArticleId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
		WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL, RoleId, RoleString
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticles
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = art.ArticleId, art.Title, 
		Content = art.Teaser + art.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL, art.RoleId, RoleString = NULL
	FROM tArticle art INNER JOIN
	tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
	WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
	(
		art.Title LIKE '%'+@Keywords+'%' OR 
		art.Teaser LIKE '%'+@Keywords+'%' OR 
		art.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogComments
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
	RoleId INT NULL,
	RoleString NVARCHAR(255) NULL  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias, b.RoleId, RoleString = NULL
		FROM vBlogComments gc INNER JOIN
		tBlog b ON b.BlogId = gc.BlogId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
		WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL, RoleId, RoleString
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogs
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = b.BlogId, b.Title, 
		Content = b.Teaser + b.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL, b.RoleId, RoleString = NULL
	FROM tBlog b INNER JOIN
	tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
	WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
	(
		b.Title LIKE '%'+@Keywords+'%' OR 
		b.Teaser LIKE '%'+@Keywords+'%' OR 
		b.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleries
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = i.ImageGalleryId, Title = i.Name, Content = NULL, UrlAlias = a.Alias,
	ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = i.ImageGalleryId ORDER BY gi.Position ASC),
	i.RoleId, RoleString = NULL
	FROM tImageGallery i INNER JOIN
	tUrlAlias a ON a.UrlAliasId = i.UrlAliasId
	WHERE i.HistoryId IS NULL AND i.InstanceId = @InstanceId AND
	(
		i.Name LIKE '%'+@Keywords+'%'
	)
END
GO
------------------------------------------------------------------------------------------------------------------------
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
	ImageUrl NVARCHAR(500) ,
	RoleId INT NULL,
	RoleString NVARCHAR(255) NULL  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias,
		ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = g.ImageGalleryId ORDER BY gi.Position ASC),
		g.RoleId, RoleString = NULL
		FROM vImageGalleryComments gc INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gc.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl, RoleId, RoleString
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleryItemComments
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
	ImageUrl NVARCHAR(500),
	RoleId INT NULL,
	RoleString NVARCHAR(255) NULL  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias + '/' +  CAST(gc.ImageGalleryItemId AS NVARCHAR),
		ImageUrl = gi.VirtualThumbnailPath, g.RoleId, RoleString = NULL
		FROM vImageGalleryItemComments gc INNER JOIN
		tImageGalleryItem gi ON gi.ImageGalleryItemId = gc.ImageGalleryItemId INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gi.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl, RoleId, RoleString
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Title, 
		Content = n.Teaser + n.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL, RoleId=NULL, RoleString = NULL
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND n.InstanceId = @InstanceId AND
	(
		n.Title LIKE '%'+@Keywords+'%' OR 
		n.Teaser LIKE '%'+@Keywords+'%' OR 
		n.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------