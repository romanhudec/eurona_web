------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.5 to 0.6
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tNews ADD [Title] [nvarchar](500) NULL, [Teaser] [nvarchar](1000) NULL, [ContentKeywords] [nvarchar](MAX) NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNewsCreate
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Date, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNewsModify
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@NewsId INT,
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid NewsId %d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNews ( Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Title = @Title, Teaser = @Teaser, Content = @Content, ContentKeywords = @ContentKeywords, UrlAliasId=@UrlAliasId, 
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NewsId = @NewsId

		SET @Result = @NewsId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	n.[NewsId], n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
	n.UrlAliasId, alias.Alias, alias.Url
FROM
	tNews n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId

WHERE
	n.HistoryId IS NULL 
GO
------------------------------------------------------------------------------------------------------------------------
UPDATE tNews SET Title=Head, Teaser=Description 
GO
ALTER TABLE tNews DROP COLUMN Head, Description
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Title, 
		Content = n.Teaser + n.ContentKeywords, UrlAlias = a.Alias 
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND
	(
		n.Title LIKE '%'+@Keywords+'%' OR 
		n.Teaser LIKE '%'+@Keywords+'%' OR 
		n.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO

------------------------------------------------------------------------------------------------------------------------
-- Comments
CREATE PROCEDURE pSearchArticleComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryItemComments AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleryComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, Title NVARCHAR(255), Content NVARCHAR(MAX), UrlAlias NVARCHAR(2000)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vImageGalleryComments gc INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gc.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR)
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchImageGalleryItemComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, Title NVARCHAR(255), Content NVARCHAR(MAX), UrlAlias NVARCHAR(2000)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias + '/' +  CAST(gc.ImageGalleryItemId AS NVARCHAR)
		FROM vImageGalleryItemComments gc INNER JOIN
		tImageGalleryItem gi ON gi.ImageGalleryItemId = gc.ImageGalleryItemId INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gi.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR)
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, Title NVARCHAR(255), Content NVARCHAR(MAX), UrlAlias NVARCHAR(2000)  )
	
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
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR)
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticleComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, Title NVARCHAR(255), Content NVARCHAR(MAX), UrlAlias NVARCHAR(2000)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vArticleComments gc INNER JOIN
		tArticle art ON art.ArticleId = gc.ArticleId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
		WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR)
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Upgrade CMS db version
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 6, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------