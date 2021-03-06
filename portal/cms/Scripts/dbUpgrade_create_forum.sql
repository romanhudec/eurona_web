-- 05.11.2010
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT,  @MasterPageId INT, @UrlAliasId INT, @PageId INT
SET @InstanceId = 1
SET @MasterPageId = 1

SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 5, 'forum', 'forum', 'sk', 'alias prefix for forum aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF

---------------------------------------------------------------------------------------------------------
-- Home
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/forumThreads.aspx', @Locale='sk', @Alias = '~/forum-kategorie', @Name='Fórum kategórie',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='forum-kategorie', @Title='Fórum kategórie',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/forumThreads.aspx', @Locale='cs', @Alias = '~/forum-kategorie', @Name='Fórum kategorie',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='forum-kategorie', @Title='Fórum kategorie',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/forumThreads.aspx', @Locale='en', @Alias = '~/forum-categies', @Name='Forum Categories',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='forum-categies', @Title='Forum Categories',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT		
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/forumThreads.aspx', @Locale='de', @Alias = '~/forum-categies', @Name='Forum Categories',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='de', @Name='forum-categies', @Title='DE: Forum Categories',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT	

------------------------------------------------------------------------------------------------------------------------
-- FORUM tables TODO: 22.10.2010
------------------------------------------------------------------------------------------------------------------------
-- ForumThread
CREATE TABLE [dbo].[tForumThread](
	[ForumThreadId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(2000) NULL,
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tForumThread_Locale]  DEFAULT ('en'),
	[Locked] [bit] NULL, /*Priznak ci ma byt dane vlakno uzamknute*/
	[VisibleForRole] NVARCHAR(2000) NULL, /*Role pre ktore sa vlakno bude zobrazovat*/
	[EditableForRole] NVARCHAR(2000) NULL, /*Role pre ktore bude vlakno pristupne a vytvaranie prispevkov*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ForumThread] PRIMARY KEY CLUSTERED ([ForumThreadId] ASC)
)
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_UrlAliasId]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForumThread] ([ForumThreadId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_HistoryId]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForumThread_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [CK_tForumThread_HistoryType]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- Forum
CREATE TABLE [dbo].[tForum](
	[ForumId] [int] IDENTITY(1,1) NOT NULL,
	[ForumThreadId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[Icon] [nvarchar](255) NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(2000) NULL,
	[Pinned] [bit] NOT NULL,
	[Locked] [bit] NULL, /*Priznak ci ma byt dane vlakno uzamknute*/
	[ViewCount] [int] NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED ([ForumId] ASC)
)
GO
ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_ForumThreadId] FOREIGN KEY([ForumThreadId])
	REFERENCES [tForumThread] ([ForumThreadId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_ForumThreadId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_UrlAliasId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_HistoryId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForum_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [CK_tForum_HistoryType]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumPost
CREATE TABLE [dbo].[tForumPost](
	[ForumPostId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[ParentId] [int] NULL,
	[AccountId] [int] NOT NULL,
	[IPAddress] [nvarchar] (255) NULL,
	[Date] [datetime] NOT NULL,
	[Title] [nvarchar] (255) NULL,
	[Content] [nvarchar](MAX) NULL,
	[Votes] [int] NULL, /*Pocet hlasov, ktore post obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_ForumPostId] PRIMARY KEY CLUSTERED ([ForumPostId] ASC)
)
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_ParentId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_ForumId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_AccountId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_HistoryId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForumPost_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [CK_tForumPost_HistoryType]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumFavorites
CREATE TABLE [dbo].[tForumFavorites](
	[ForumFavoritesId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_ForumFavoritesId] PRIMARY KEY CLUSTERED ([ForumFavoritesId] ASC)
)
GO

ALTER TABLE [tForumFavorites]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumFavorites_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumFavorites] CHECK CONSTRAINT [FK_tForumFavorites_ForumId]
GO

ALTER TABLE [tForumFavorites]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumFavorites_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumFavorites] CHECK CONSTRAINT [FK_tForumFavorites_AccountId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumTracking
CREATE TABLE [dbo].[tForumTracking](
	[ForumTrackingId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_ForumTrackingId] PRIMARY KEY CLUSTERED ([ForumTrackingId] ASC)
)
GO

ALTER TABLE [tForumTracking]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumTracking_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumTracking] CHECK CONSTRAINT [FK_tForumTracking_ForumId]
GO

ALTER TABLE [tForumTracking]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumTracking_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumTracking] CHECK CONSTRAINT [FK_tForumTracking_AccountId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumPostAttachment
CREATE TABLE [dbo].[tForumPostAttachment](
	[ForumPostAttachmentId] [int] IDENTITY(1,1) NOT NULL,
	[ForumPostId] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](2000) NULL,
	[Type] [int] NULL,
	[Url] [nvarchar](255) NULL,
	[Size] [int] NULL,
	[Order] [int] NULL,
 CONSTRAINT [PK_ForumPostAttachmentId] PRIMARY KEY CLUSTERED ([ForumPostAttachmentId] ASC)
)
GO

ALTER TABLE [tForumPostAttachment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPostAttachment_ForumPostId] FOREIGN KEY([ForumPostId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPostAttachment] CHECK CONSTRAINT [FK_tForumPostAttachment_ForumPostId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Forum
CREATE VIEW vForumThreads AS SELECT A=1
GO
CREATE VIEW vForums AS SELECT A=1
GO
CREATE VIEW vForumPosts AS SELECT A=1
GO
CREATE VIEW vForumFavorites AS SELECT A=1
GO
CREATE VIEW vForumTrackings AS SELECT A=1
GO
CREATE VIEW vForumPostAttachments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForums
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumId, f.ForumThreadId, f.InstanceId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked, ViewCount = ISNULL(f.ViewCount,0),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId),
	LastPostDate = (SELECT MAX(fp.Date) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId),
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForum f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId

WHERE
	f.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumThreads
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumThreadId, f.InstanceId, f.Name, f.Description, f.Icon, f.Locale, f.Locked, f.VisibleForRole, f.EditableForRole,
	f.UrlAliasId, alias.Alias, alias.Url,
	ForumsCount = (SELECT Count(*) FROM tForum WHERE HistoryId IS NULL AND ForumThreadId=f.ForumThreadId),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp INNER JOIN tForum fo ON fo.ForumId=fp.ForumId AND fo.HistoryId IS NULL
		WHERE fp.HistoryId IS NULL AND fo.ForumThreadId=f.ForumThreadId)
FROM
	tForumThread f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId

WHERE
	f.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumPosts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	fp.ForumPostId, fp.ForumId, fp.InstanceId, fp.ParentId, fp.AccountId, fp.IPAddress, fp.Date, fp.Title, fp.Content,
	AccountName = a.Login,
	Votes = ISNULL(fp.Votes, 0), 
	TotalRating = ISNULL(fp.TotalRating, 0),
	RatingResult = CASE WHEN fp.TotalRating!= 0 AND fp.Votes!=0 THEN ISNULL(fp.TotalRating*1.0/fp.Votes*1.0, 0 ) ELSE 0 END
FROM
	tForumPost fp 
	INNER JOIN tAccount a ON a.AccountId = fp.AccountId
WHERE
	fp.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumFavorites
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ff.ForumFavoritesId, ff.ForumId, ff.AccountId,
	f.ForumThreadId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked,
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=ff.ForumId),
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForumFavorites ff
	INNER JOIN tForum f ON f.ForumId = ff.ForumId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumTrackings
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ft.ForumTrackingId, ft.ForumId, ft.AccountId, a.Email, AccountName = a.Login,
	f.ForumThreadId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked,
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=ft.ForumId),
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForumTracking ft
	INNER JOIN tAccount a ON a.AccountId = ft.AccountId
	INNER JOIN tForum f ON f.ForumId = ft.ForumId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
GO

------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumPostAttachments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	fpa.ForumPostAttachmentId, fpa.ForumPostId, fpa.Name, fpa.Description, fpa.Type, fpa.Url, fpa.Size, fpa.[Order]
FROM
	tForumPostAttachment fpa
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pForumThreadCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumThreadModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumThreadDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pForumCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pForumPostCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumPostModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumPostDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pForumPostIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumThreadCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(255),
	@Description NVARCHAR(2000) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@VisibleForRole  NVARCHAR(2000) = NULL, /*Role pre ktore sa vlakno bude zobrazovat*/
	@EditableForRole  NVARCHAR(2000) = NULL, /*Role pre ktore bude vlakno pristupne a vytvaranie prispevkov*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	
	INSERT INTO tForumThread ( InstanceId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Description, @Icon, @Locale, @Locked, @VisibleForRole, @EditableForRole, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumThreadId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumThreadDelete
	@HistoryAccount INT,
	@ForumThreadId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumThreadId IS NULL OR NOT EXISTS(SELECT * FROM tForumThread WHERE ForumThreadId = @ForumThreadId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumThreadId=%d', 16, 1, @ForumThreadId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumThread
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumThreadId
		WHERE ForumThreadId = @ForumThreadId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tForumThread WHERE ForumThreadId = @ForumThreadId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tForumThread SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ForumThreadId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumThreadModify
	@HistoryAccount INT,
	@ForumThreadId INT,
	@Name NVARCHAR(255),
	@Description NVARCHAR(2000) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@VisibleForRole  NVARCHAR(2000) = NULL, /*Role pre ktore sa vlakno bude zobrazovat*/
	@EditableForRole  NVARCHAR(2000) = NULL, /*Role pre ktore bude vlakno pristupne a vytvaranie prispevkov*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumThread WHERE ForumThreadId = @ForumThreadId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumThreadId %d', 16, 1, @ForumThreadId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForumThread ( InstanceId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @ForumThreadId
		FROM tForumThread
		WHERE ForumThreadId = @ForumThreadId

		UPDATE tForumThread
		SET
			[Name]=@Name, [Description]=@Description, Icon=@Icon, Locale=@Locale, Locked=@Locked, VisibleForRole=@VisibleForRole, EditableForRole=@EditableForRole, UrlAliasId=@UrlAliasId, 
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumThreadId = @ForumThreadId

		SET @Result = @ForumThreadId

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
ALTER PROCEDURE pForumCreate
	@HistoryAccount INT,
	@ForumThreadId INT,
	@InstanceId INT,
	@Icon NVARCHAR(255) = NULL,
	@Name NVARCHAR(255),
	@Description  NVARCHAR(2000) = NULL,
	@Pinned BIT = 0,
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	--ForumId, ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked,
	INSERT INTO tForum ( ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ForumThreadId, @InstanceId, @Icon, @Name, @Description, @Pinned, @Locked,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumModify
	@HistoryAccount INT,
	@ForumId INT,
	@ForumThreadId INT,
	@Icon NVARCHAR(255) = NULL,
	@Name NVARCHAR(255),
	@Description  NVARCHAR(2000) = NULL,
	@Pinned BIT = 0,
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumId %d', 16, 1, @ForumId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForum ( ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, @ForumId
		FROM tForum
		WHERE ForumId = @ForumId

		UPDATE tForum
		SET
			ForumThreadId=@ForumThreadId, Icon=@Icon, [Name]=@Name, [Description]=@Description, Pinned=@Pinned, Locked=@Locked, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumId = @ForumId

		SET @Result = @ForumId

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
ALTER PROCEDURE pForumDelete
	@HistoryAccount INT,
	@ForumId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumId IS NULL OR NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumId=%d', 16, 1, @ForumId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForum
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumId
		WHERE ForumId = @ForumId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tForum WHERE ForumId = @ForumId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tForum SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ForumId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostCreate
	@HistoryAccount INT,
	@ForumId INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Date DATETIME,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ForumId, @InstanceId, @ParentId, @AccountId, @IPAddress, @Date, @Title, @Content, 0, 0,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumPostId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostModify
	@HistoryAccount INT,
	@ForumPostId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumPostId %d', 16, 1, @ForumPostId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, @ForumPostId
		FROM tForumPost
		WHERE ForumPostId = @ForumPostId

		UPDATE tForumPost
		SET
			ParentId=@ParentId, AccountId=@AccountId, IPAddress=@IPAddress, Title=@Title, Content=@Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumPostId = @ForumPostId

		SET @Result = @ForumPostId

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
ALTER PROCEDURE pForumPostDelete
	@HistoryAccount INT,
	@ForumPostId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumPostId IS NULL OR NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ForumPostId=%d', 16, 1, @ForumPostId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumPost
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ForumPostId
		WHERE ForumPostId = @ForumPostId

		SET @Result = @ForumPostId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumIncrementViewCount
	@ForumId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumId %d', 16, 1, @ForumId);

	UPDATE tForum SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ForumId = @ForumId

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostIncrementVote
	@ForumPostId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumPostId %d', 16, 1, @ForumPostId);

	UPDATE tForumPost 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ForumPostId = @ForumPostId

END
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pForumTrackingCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumTrackingModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumTrackingDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumTrackingDelete
	@ForumTrackingId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumTrackingId IS NULL OR NOT EXISTS(SELECT * FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId) 
		RAISERROR('Invalid @ForumTrackingId=%d', 16, 1, @ForumTrackingId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId
		SET @Result = @ForumTrackingId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumTrackingCreate
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumTracking ( ForumId,  AccountId ) VALUES ( @ForumId, @AccountId )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumTrackingId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumTrackingModify
	@ForumTrackingId INT,
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumTracking WHERE ForumTrackingId = @ForumTrackingId ) 
		RAISERROR('Invalid ForumTrackingId %d', 16, 1, @ForumTrackingId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumTracking SET ForumId= @ForumId, AccountId=@AccountId
		WHERE ForumTrackingId = @ForumTrackingId

		SET @Result = @ForumTrackingId

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
CREATE PROCEDURE pForumFavoritesCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumFavoritesModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumFavoritesDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumFavoritesCreate
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumFavorites ( ForumId,  AccountId ) VALUES ( @ForumId, @AccountId )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumFavoritesId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumFavoritesModify
	@ForumFavoritesId INT,
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId ) 
		RAISERROR('Invalid ForumFavoritesId %d', 16, 1, @ForumFavoritesId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumFavorites SET ForumId= @ForumId, AccountId=@AccountId
		WHERE ForumFavoritesId = @ForumFavoritesId

		SET @Result = @ForumFavoritesId

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
ALTER PROCEDURE pForumFavoritesDelete
	@ForumFavoritesId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumFavoritesId IS NULL OR NOT EXISTS(SELECT * FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId) 
		RAISERROR('Invalid @ForumFavoritesId=%d', 16, 1, @ForumFavoritesId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId
		SET @Result = @ForumFavoritesId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pForumPostAttachmentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumPostAttachmentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pForumPostAttachmentDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostAttachmentCreate
	@ForumPostId INT,
	@Name NVARCHAR(255) = NULL,
	@Description NVARCHAR(2000) = NULL,
	@Type INT = 1,
	@Url NVARCHAR(255) = NULL,
	@Size INT = 0,
	@Order INT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumPostAttachment ( ForumPostId,  Name, [Description], [Type], Url, Size, [Order] ) VALUES ( @ForumPostId, @Name, @Description, @Type, @Url, @Size, @Order )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumPostAttachmentId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostAttachmentModify
	@ForumPostAttachmentId INT,
	@ForumPostId INT,
	@Name NVARCHAR(255) = NULL,
	@Description NVARCHAR(2000) = NULL,
	@Type INT = 1,
	@Url NVARCHAR(255) = NULL,
	@Size INT = 0,
	@Order INT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId ) 
		RAISERROR('Invalid ForumPostAttachmentId %d', 16, 1, @ForumPostAttachmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumPostAttachment SET ForumPostId= @ForumPostId, [Name]=@Name, [Description]=@Description, [Type]=@Type, Url=@Url, [Size]=@Size, [Order]=@Order
		WHERE ForumPostAttachmentId = @ForumPostAttachmentId

		SET @Result = @ForumPostAttachmentId

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
ALTER PROCEDURE pForumPostAttachmentDelete
	@ForumPostAttachmentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumPostAttachmentId IS NULL OR NOT EXISTS(SELECT * FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId) 
		RAISERROR('Invalid @ForumPostAttachmentId=%d', 16, 1, @ForumPostAttachmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId
		SET @Result = @ForumPostAttachmentId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO

------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------