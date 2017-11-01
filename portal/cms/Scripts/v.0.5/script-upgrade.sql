------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.4 to 0.5
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tPage ADD [ContentKeywords] [nvarchar](MAX) NULL
GO
ALTER TABLE tArticle ADD [ContentKeywords] [nvarchar](MAX) NULL
GO
ALTER TABLE tBlog ADD [ContentKeywords] [nvarchar](MAX) NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pPageModify
	@HistoryAccount INT,
	@PageId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),	
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPage WHERE PageId = @PageId AND HistoryId IS NULL)  BEGIN
		RAISERROR('Invalid PageId %d', 16, 1, @PageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
	
		-- Normalizacia nazvu
		SET @Name = dbo.fMakeAnsi(@Name)

		INSERT INTO tPage (MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @PageId
		FROM tPage
		WHERE PageId = @PageId

		UPDATE tPage
		SET
			MasterPageId = @MasterPageId, RoleId = @RoleId,
			Locale = @Locale, [Name] = @Name, Title = @Title, UrlAliasId = @UrlAliasId, Content = @Content, ContentKeywords = @ContentKeywords,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PageId = @PageId

		SET @Result = @PageId

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
ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleModify
	@HistoryAccount INT,
	@ArticleId INT,
	@ArticleCategoryId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId,
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @ArticleId
		FROM tArticle
		WHERE ArticleId = @ArticleId

		UPDATE tArticle
		SET
			ArticleCategoryId=ISNULL(@ArticleCategoryId, ArticleCategoryId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, ContentKeywords=@ContentKeywords,
			RoleId=@RoleId, UrlAliasId=@UrlAliasId, Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), 
			ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ArticleId = @ArticleId

		SET @Result = @ArticleId

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
ALTER PROCEDURE pArticleCreate
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ArticleCategoryId, @Locale, @Icon, @Title, @Teaser, @ContentKeywords, @Content, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogModify
	@HistoryAccount INT,
	@BlogId INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @BlogId
		FROM tBlog
		WHERE BlogId = @BlogId

		UPDATE tBlog
		SET
			AccountId=ISNULL(@AccountId, AccountId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, ContentKeywords=@ContentKeywords, RoleId=@RoleId, UrlAliasId=@UrlAliasId,
			Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE BlogId = @BlogId

		SET @Result = @BlogId

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
ALTER PROCEDURE pBlogCreate
	@HistoryAccount INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @Locale, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BlogId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pSearchPages AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchArticles AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogs AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchNews AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleries AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
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
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchArticles
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = art.ArticleId, art.Title, 
		Content = art.Teaser + art.ContentKeywords, 
		UrlAlias = a.Alias 
	FROM tArticle art INNER JOIN
	tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
	WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND
	(
		art.Title LIKE '%'+@Keywords+'%' OR 
		art.Teaser LIKE '%'+@Keywords+'%' OR 
		art.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSearchBlogs
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = b.BlogId, b.Title, 
		Content = b.Teaser + b.ContentKeywords, 
		UrlAlias = a.Alias 
	FROM tBlog b INNER JOIN
	tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
	WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND
	(
		b.Title LIKE '%'+@Keywords+'%' OR 
		b.Teaser LIKE '%'+@Keywords+'%' OR 
		b.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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

------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tCMSUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tCMSUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade CMS db version
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 5, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
