------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.2 to 0.3
------------------------------------------------------------------------------------------------------------------------
-- Upgrade

------------------------------------------------------------------------------------------------------------------------
-- ACCOUNT VOTE
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tAccountVote](
	[AccountVoteId] [int] IDENTITY(1,1) NOT NULL,
	[ObjectType] [int] NOT NULL,
	[ObjectId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Date] [DATETIME] NOT NULL,
 CONSTRAINT [PK_AccountVoteId] PRIMARY KEY CLUSTERED ([AccountVoteId] ASC)
)
GO

ALTER TABLE [tAccountVote]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountVote_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountVote] CHECK CONSTRAINT [FK_tAccountVote_AccountId]
GO

------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vAccountVotes AS SELECT A=1
GO


ALTER VIEW vAccountVotes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	AccountVoteId, ObjectType, ObjectId, AccountId, Rating, [Date]
FROM tAccountVote
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pAccountVoteCreate AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pAccountVoteCreate
	@AccountId INT,
	@ObjectType INT,
	@ObjectId INT,
	@Rating INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountVote ( AccountId, ObjectType, ObjectId, Rating, [Date]) 
	VALUES ( @AccountId, @ObjectType, @ObjectId, @Rating, GETDATE())

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountVoteId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
-- URL ALIAS
------------------------------------------------------------------------------------------------------------------------
-- UrlAlias
CREATE TABLE [dbo].[tUrlAlias](
	[UrlAliasId] [int] IDENTITY(1,1) NOT NULL,
	[Url] NVARCHAR(255) NOT NULL,
	[Locale] [char](2) NOT NULL CONSTRAINT [DF_tUrlAlias_Locale]  DEFAULT ('en'),
	[Alias] NVARCHAR(500) NOT NULL,
	[Name] NVARCHAR(500) NULL,
 CONSTRAINT [PK_UrlAlias] PRIMARY KEY CLUSTERED ([UrlAliasId] ASC)
)
GO
ALTER TABLE [tUrlAlias]  WITH CHECK 
	ADD CONSTRAINT [CK_tUrlAlias_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tUrlAlias] CHECK CONSTRAINT [CK_tUrlAlias_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [tPage] ADD UrlAliasId int NULL
GO
ALTER TABLE [tPage]  WITH CHECK 
	ADD CONSTRAINT [FK_tPage_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [tMenu] ADD UrlAliasId int NULL
GO
ALTER TABLE [tMenu]  WITH CHECK 
	ADD CONSTRAINT [FK_tMenu_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [tNavigationMenu] ADD UrlAliasId int NULL
GO
ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD CONSTRAINT [FK_tNavigationMenu_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [tNavigationMenuItem] ADD UrlAliasId int NULL
GO
ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD CONSTRAINT [FK_tNavigationMenuItem_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vUrlAliases AS SELECT A=1
GO

ALTER VIEW vUrlAliases
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[UrlAliasId], [Url], [Locale], [Alias], [Name]
FROM tUrlAlias
GO
-- SELECT * FROM vUrlAliases
------------------------------------------------------------------------------------------------------------------------
-- UrlAlias
CREATE PROCEDURE pUrlAliasCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasDelete
	@UrlAliasId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @UrlAliasId IS NULL OR NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId) BEGIN
		RAISERROR('Invalid @UrlAliasId=%d', 16, 1, @UrlAliasId);
		RETURN
	END

	UPDATE tPage SET UrlAliasId = NULL WHERE UrlAliasId = @UrlAliasId
	DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId

	SET @Result = @UrlAliasId

END	

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasCreate
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale)  BEGIN
		SET @Alias = @Alias + '-' + @Locale
	END	
	
	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')

	INSERT INTO tUrlAlias (Url, Locale, Alias, [Name] ) 
	VALUES ( @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasModify
	@UrlAliasId INT,
	@Url NVARCHAR(2000),
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId)  BEGIN
		RAISERROR('Invalid UrlAliasId %d', 16, 1, @UrlAliasId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
		SET @Alias = REPLACE( @Alias, '.', '-')
		SET @Alias = REPLACE( @Alias, '_', '-')
		SET @Alias = REPLACE( @Alias, ':', '-')

		UPDATE tUrlAlias
		SET Url = ISNULL(@Url, Url ), Alias = ISNULL(dbo.fMakeAnsi(@Alias), Alias), [Name] = ISNULL(@Name, [Name] )
		WHERE UrlAliasId = @UrlAliasId

		SET @Result = @UrlAliasId

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
ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PageId], p.[MasterPageId], p.[Locale], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
	a.Url, a.Alias
FROM
	tPage p LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO
-- SELECT * FROM vPages
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, UrlAliasId, Content, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
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

		INSERT INTO tPage (MasterPageId, Locale, Title, [Name], UrlAliasId, Content, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			MasterPageId, Locale, Title, [Name], UrlAliasId, Content, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @PageId
		FROM tPage
		WHERE PageId = @PageId

		UPDATE tPage
		SET
			MasterPageId = @MasterPageId, RoleId = @RoleId,
			Locale = @Locale, [Name] = @Name, Title = @Title, UrlAliasId = @UrlAliasId, Content = @Content,
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
ALTER PROCEDURE pPageDelete
	@HistoryAccount INT,
	@PageId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PageId IS NULL OR NOT EXISTS(SELECT * FROM tPage WHERE PageId = @PageId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @PageId=%d', 16, 1, @PageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tPage
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PageId
		WHERE PageId = @PageId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tPage WHERE PageId = @PageId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tPage SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END		

		SET @Result = @PageId

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
ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pMenuModify
	@HistoryAccount INT,
	@MenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tMenu WHERE MenuId = @MenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid MenuId %d', 16, 1, @MenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE MenuId = @MenuId

		SET @Result = @MenuId

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
ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO
-- SELECT * FROM vNavigationMenu
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuModify
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNavigationMenu WHERE NavigationMenuId = @NavigationMenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid NavigationMenuId %d', 16, 1, @NavigationMenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuId
		FROM tNavigationMenu
		WHERE NavigationMenuId = @NavigationMenuId

		UPDATE tNavigationMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NavigationMenuId = @NavigationMenuId

		SET @Result = @NavigationMenuId

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
ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuItemId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenuItem m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO
-- SELECT * FROM vNavigationMenuItem
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuItemCreate
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @NavigationMenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuItemModify
	@HistoryAccount INT,
	@NavigationMenuItemId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNavigationMenuItem WHERE NavigationMenuItemId = @NavigationMenuItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid NavigationMenuItemId %d', 16, 1, @NavigationMenuItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuItemId
		FROM tNavigationMenuItem
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		UPDATE tNavigationMenuItem
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		SET @Result = @NavigationMenuItemId

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
ALTER TABLE [tNews]  ADD UrlAliasId INT NULL
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNews_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [FK_tNews_UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	n.[NewsId], n.[Locale], n.[Date], n.[Icon], n.[Head], n.[Description], n.[Content],
	n.UrlAliasId, alias.Alias, alias.Url
FROM
	tNews n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId

WHERE
	n.HistoryId IS NULL 
GO
-- SELECT * FROM vNews
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNewsCreate
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Head NVARCHAR(255) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Date, @Icon, @Head, @Description, @Content, @UrlAliasId, 
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
	@Head NVARCHAR(255) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid NewsId %d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Date], Icon, Head, Description, Content, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Head = @Head, Description = @Description, Content = @Content, UrlAliasId=@UrlAliasId, 
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
ALTER PROCEDURE pNewsDelete
	@HistoryAccount INT,
	@NewsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NewsId IS NULL OR NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NewsId=%d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY
	
		UPDATE tNews
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NewsId
		WHERE NewsId = @NewsId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tNews WHERE NewsId = @NewsId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tNews SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END
		
		SET @Result = @NewsId

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
-- COMMENTS
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tComment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[AccountId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Title] [nvarchar] (255) NULL,
	[Content] [nvarchar](1000) NULL,
	[Votes] [int] NULL, /*Pocet hlasov, ktore komentar obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore komentar dostal pri hlasovani*/	
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_CommentId] PRIMARY KEY CLUSTERED ([CommentId] ASC)
)
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_ParentId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_AccountId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_HistoryId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [CK_tComment_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [CK_tComment_HistoryType]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vComments AS SELECT A=1
GO

ALTER VIEW vComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[CommentId], [ParentId], [AccountId], [Date], [Title], [Content], [Votes], [TotalRating]
FROM
	tComment
WHERE
	HistoryId IS NULL
GO


------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pCommentCreate
	@HistoryAccount INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@Date DATETIME,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tComment ( ParentId, AccountId, [Date], Title, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ParentId, @AccountId, @Date, @Title, @Content, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CommentId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pCommentModify
	@HistoryAccount INT,
	@CommentId INT,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tComment ( Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, @CommentId
		FROM tComment
		WHERE CommentId = @CommentId

		UPDATE tComment
		SET
			Title = @Title, Content = @Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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
ALTER PROCEDURE pCommentDelete
	@HistoryAccount INT,
	@CommentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CommentId IS NULL OR NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CommentId=%d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tComment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CommentId
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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
CREATE PROCEDURE pCommentIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pCommentIncrementVote
	@CommentId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	UPDATE tComment 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE CommentId = @CommentId

END
GO

------------------------------------------------------------------------------------------------------------------------
-- TAGS
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tTag](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](255) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_TagId] PRIMARY KEY CLUSTERED ([TagId] ASC)
)
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTag_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [FK_tTag_HistoryId]
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [CK_tTag_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [CK_tTag_HistoryType]
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTag_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [FK_tTag_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vTags AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	TagId, Tag
FROM tTag WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
-- Tags
CREATE PROCEDURE pTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pTagCreate
	@HistoryAccount INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tTag ( Tag, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Tag, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT TagId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pTagModify
	@HistoryAccount INT,
	@TagId INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid TagId %d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTag ( Tag,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Tag,
			HistoryStamp, HistoryType, HistoryAccount, @TagId
		FROM tTag
		WHERE TagId = @TagId

		UPDATE tTag
		SET
			Tag = ISNULL(@Tag, Tag ),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TagId = @TagId

		SET @Result = @TagId

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
ALTER PROCEDURE pTagDelete
	@HistoryAccount INT,
	@TagId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @TagId IS NULL OR NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid @TagId=%d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tTag
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @TagId
		WHERE TagId = @TagId

		SET @Result = @TagId

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
-- ARTICLES
------------------------------------------------------------------------------------------------------------------------
-- ArticleCategory
CREATE TABLE [cArticleCategory](
	[ArticleCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cArticleCategory_Locale]  DEFAULT ('sk'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cArticleCategory] PRIMARY KEY CLUSTERED ([ArticleCategoryId] ASC)
)
GO

ALTER TABLE [cArticleCategory]  WITH CHECK 
	ADD  CONSTRAINT [FK_cArticleCategory_cArticleCategory] FOREIGN KEY([HistoryId])
	REFERENCES [cArticleCategory] (ArticleCategoryId)
GO
ALTER TABLE [cArticleCategory] CHECK CONSTRAINT [FK_cArticleCategory_cArticleCategory]
GO

ALTER TABLE [cArticleCategory]  WITH CHECK 
	ADD  CONSTRAINT [CK_cArticleCategory_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cArticleCategory] CHECK CONSTRAINT [CK_cArticleCategory_HistoryType]
GO

------------------------------------------------------------------------------------------------------------------------
-- Articles
CREATE TABLE [dbo].[tArticle](
	[ArticleId] [int] IDENTITY(1,1) NOT NULL,
	[ArticleCategoryId] INT NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tArticle_Locale]  DEFAULT ('en'),
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	[Country] [nvarchar](255 ) NULL, /*Stat, ktoreho sa clanok tyka*/
	[City] [nvarchar](255 ) NULL /*Mesto, ktoreho sa clanok tyka*/,
	[Approved] [bit] NULL, /*Indikuje, ci je clanok schvaleny redaktorom*/
	[ReleaseDate] [datetime] NOT NULL, /*Datum a cas zverejnenia clanku*/
	[ExpiredDate] [datetime] NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	[EnableComments] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dany clanok viditelny*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni clanku*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore clanok obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_ArticleId] PRIMARY KEY CLUSTERED ([ArticleId] ASC)
)
GO
ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [FK_tArticle_cArticleCategory] FOREIGN KEY([ArticleCategoryId])
	REFERENCES [cArticleCategory] ([ArticleCategoryId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_cArticleCategory]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_HistoryId]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [CK_tArticle_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [CK_tArticle_HistoryType]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_HistoryAccount]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [CK_tArticle_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [CK_tArticle_Locale]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [FK_tArticle_tRole] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_tRole]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_UrlAliasId]
GO
-----------------------------------------------------------------------------
CREATE VIEW vArticleCategories AS SELECT A=1
GO

-----------------------------------------------------------------------------

ALTER VIEW vArticleCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	a.ArticleCategoryId, a.[Name], a.[Code], a.[Locale], a.[Notes], 
	ArticlesInCategory = (SELECT Count(*) FROM tArticle 
		WHERE HistoryId IS NULL AND
			  Visible=1 AND 
			  ReleaseDate<=GETDATE() AND 
			  ArticleCategoryId = a.ArticleCategoryId )
FROM
	cArticleCategory a
WHERE
	HistoryId IS NULL
GO

-- Procedures
CREATE PROCEDURE pArticleCategoryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryDelete AS BEGIN SET NOCOUNT ON; END
GO

-----------------------------------------------------------------------------
ALTER PROCEDURE pArticleCategoryCreate
	@HistoryAccount INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Locale [char](2) = 'en', 
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cArticleCategory ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Name, @Code, @Notes, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleCategoryId = @Result

END
GO
-----------------------------------------------------------------------------
ALTER PROCEDURE pArticleCategoryModify
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleCategoryId %d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cArticleCategory ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @ArticleCategoryId
		FROM cArticleCategory
		WHERE ArticleCategoryId = @ArticleCategoryId

		UPDATE cArticleCategory
		SET
			[Name] = @Name, [Code] = @Code, [Notes] = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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

-----------------------------------------------------------------------------

ALTER PROCEDURE pArticleCategoryDelete
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleCategoryId IS NULL OR NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleCategoryId=%d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cArticleCategory
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleCategoryId
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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
CREATE VIEW vArticleComments AS SELECT A=1
GO
CREATE VIEW vArticles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------

ALTER VIEW vArticles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleId, a.Locale, a.Icon, a.Title, a.Teaser, a.Content, a.RoleId, a.Country,
	a.ArticleCategoryId, ArticleCategoryName = c.Name,
	a.City, a.Approved, a.ReleaseDate, a.ExpiredDate, 
	a.EnableComments, a.Visible, 
	CommentsCount = ( SELECT Count(*) FROM vArticleComments WHERE ArticleId = a.ArticleId ),
	ViewCount = ISNULL(a.ViewCount, 0 ), 
	Votes = ISNULL(a.Votes, 0), 
	TotalRating = ISNULL(a.TotalRating, 0),
	RatingResult =  ISNULL(a.TotalRating*1.0/a.Votes*1.0, 0 ),
	a.UrlAliasId, alias.Alias, alias.Url

FROM
	tArticle a INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = a.UrlAliasId

WHERE
	HistoryId IS NULL
GO

------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pArticleCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleDelete AS BEGIN SET NOCOUNT ON; END
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

	INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ArticleCategoryId, @Locale, @Icon, @Title, @Teaser, @Content, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleId = @Result

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

		INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ArticleCategoryId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId,
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @ArticleId
		FROM tArticle
		WHERE ArticleId = @ArticleId

		UPDATE tArticle
		SET
			ArticleCategoryId=ISNULL(@ArticleCategoryId, ArticleCategoryId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, RoleId=@RoleId, UrlAliasId=@UrlAliasId,
			Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
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
ALTER PROCEDURE pArticleDelete
	@HistoryAccount INT,
	@ArticleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleId IS NULL OR NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleId=%d', 16, 1, @ArticleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tArticle
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleId
		WHERE ArticleId = @ArticleId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tArticle WHERE ArticleId = @ArticleId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tArticle SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ArticleId

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
CREATE PROCEDURE pArticleIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pArticleIncrementViewCount
	@ArticleId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle SET ViewCount = ViewCount + 1 WHERE ArticleId = @ArticleId

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleIncrementVote
	@ArticleId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle 
		SET ViewCount = ViewCount + 1,
		TotalRating = TotalRating + @Rating
	WHERE ArticleId = @ArticleId

END
GO

------------------------------------------------------------------------------------------------------------------------
-- ArticleTag
CREATE TABLE [dbo].[tArticleTag](
	[ArticleTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[ArticleId] INT NOT NULL,
 CONSTRAINT [PK_ArticleTagId] PRIMARY KEY CLUSTERED ([ArticleTagId] ASC)
)
GO

ALTER TABLE [tArticleTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tArticleTag] CHECK CONSTRAINT [FK_tArticleTag_TagId]
GO

ALTER TABLE [tArticleTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleTag_ArticleId] FOREIGN KEY([ArticleId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticleTag] CHECK CONSTRAINT [FK_tArticleTag_ArticleId]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vArticleTags AS SELECT A=1
GO

ALTER VIEW vArticleTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleTagId, a.ArticleId, t.TagId, t.Tag
FROM
	tArticleTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO

------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pArticleTagCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pArticleTagCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ArticleId FROM vArticleTags WHERE TagId=@TagId AND ArticleId=@ArticleId) BEGIN
		INSERT INTO tArticleTag ( TagId, ArticleId ) VALUES ( @TagId, @ArticleId )
	END

END
GO

------------------------------------------------------------------------------------------------------------------------
-- ArticleComment
CREATE TABLE [dbo].[tArticleComment](
	[ArticleCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ArticleId] INT NOT NULL,
 CONSTRAINT [PK_ArticleCommentId] PRIMARY KEY CLUSTERED ([ArticleCommentId] ASC)
)
GO

ALTER TABLE [tArticleComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tArticleComment] CHECK CONSTRAINT [FK_tArticleComment_CommentId]
GO

ALTER TABLE [tArticleComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleComment_ArticleId] FOREIGN KEY([ArticleId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticleComment] CHECK CONSTRAINT [FK_tArticleComment_ArticleId]
GO
------------------------------------------------------------------------------------------------------------------------

ALTER VIEW vArticleComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.ArticleCommentId, ac.ArticleId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tArticleComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pArticleCommentCreate AS BEGIN SET NOCOUNT ON; END
GO

ALTER PROCEDURE pArticleCommentCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tArticleComment ( CommentId, ArticleId ) VALUES ( @CommentId, @ArticleId )

END
GO
------------------------------------------------------------------------------------------------------------------------
-- Blogs
CREATE TABLE [dbo].[tBlog](
	[BlogId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tBlog_Locale]  DEFAULT ('en'),
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa blog bude zobrazovat*/
	[Country] [nvarchar](255 ) NULL, /*Stat, ktoreho sa blog tyka*/
	[City] [nvarchar](255 ) NULL /*Mesto, ktoreho sa blog tyka*/,
	[Approved] [bit] NULL, /*Indikuje, ci je blog schvaleny redaktorom*/
	[ReleaseDate] [datetime] NOT NULL, /*Datum a cas zverejnenia blogu*/
	[ExpiredDate] [datetime] NULL, /*Datum a cas stiahnutia blogu (uz nebude verejne dostupny)*/
	[EnableComments] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dany blog viditelny*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni blogu*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore blog obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore blog dostal pri hlasovani*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_BlogId] PRIMARY KEY CLUSTERED ([BlogId] ASC)
)
GO
ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [FK_tBlog_tAccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_tAccountId]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_HistoryId]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [CK_tBlog_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [CK_tBlog_HistoryType]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_HistoryAccount]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [CK_tBlog_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [CK_tBlog_Locale]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [FK_tBlog_tRole] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_tRole]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
-- BlogTag
CREATE TABLE [dbo].[tBlogTag](
	[BlogTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[BlogId] INT NOT NULL,
 CONSTRAINT [PK_BlogTagId] PRIMARY KEY CLUSTERED ([BlogTagId] ASC)
)
GO

ALTER TABLE [tBlogTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tBlogTag] CHECK CONSTRAINT [FK_tBlogTag_TagId]
GO

ALTER TABLE [tBlogTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogTag_BlogId] FOREIGN KEY([BlogId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlogTag] CHECK CONSTRAINT [FK_tBlogTag_BlogId]
GO
------------------------------------------------------------------------------------------------------------------------
-- BlogComment
CREATE TABLE [dbo].[tBlogComment](
	[BlogCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[BlogId] INT NOT NULL,
 CONSTRAINT [PK_BlogCommentId] PRIMARY KEY CLUSTERED ([BlogCommentId] ASC)
)
GO

ALTER TABLE [tBlogComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tBlogComment] CHECK CONSTRAINT [FK_tBlogComment_CommentId]
GO

ALTER TABLE [tBlogComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogComment_BlogId] FOREIGN KEY([BlogId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlogComment] CHECK CONSTRAINT [FK_tBlogComment_BlogId]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pBlogCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogCommentCreate AS BEGIN SET NOCOUNT ON; END
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

	INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @Locale, @Icon, @Title, @Teaser, @Content, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BlogId = @Result

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

		INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, Locale, Icon, Title, Teaser, Content, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @BlogId
		FROM tBlog
		WHERE BlogId = @BlogId

		UPDATE tBlog
		SET
			AccountId=ISNULL(@AccountId, AccountId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, RoleId=@RoleId, UrlAliasId=@UrlAliasId,
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
ALTER PROCEDURE pBlogDelete
	@HistoryAccount INT,
	@BlogId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @BlogId IS NULL OR NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid @BlogId=%d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tBlog
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @BlogId
		WHERE BlogId = @BlogId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tBlog WHERE BlogId = @BlogId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tBlog SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @BlogId

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
ALTER PROCEDURE pBlogIncrementVote
	@BlogId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE BlogId = @BlogId

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogIncrementViewCount
	@BlogId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE BlogId = @BlogId

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogTagCreate
	@HistoryAccount INT,
	@BlogId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, BlogId FROM vBlogTags WHERE TagId=@TagId AND BlogId=@BlogId) BEGIN
		INSERT INTO tBlogTag ( TagId, BlogId ) VALUES ( @TagId, @BlogId )
	END

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pBlogCommentCreate
	@HistoryAccount INT,
	@BlogId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tBlogComment ( CommentId, BlogId ) VALUES ( @CommentId, @BlogId )

END
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vBlogTags AS SELECT TagId=1, BlogId=1
GO
CREATE VIEW vBlogComments AS SELECT A=1
GO
CREATE VIEW vBlogs AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vBlogs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	b.BlogId, b.Locale, b.Icon, b.Title, b.Teaser, b.Content, b.RoleId, b.Country,
	b.AccountId, Login = a.Login,
	b.City, b.Approved, b.ReleaseDate, b.ExpiredDate, 
	b.EnableComments, b.Visible, 
	b.UrlAliasId, alias.Alias, alias.Url,
	CommentsCount = ( SELECT Count(*) FROM vBlogComments WHERE BlogId = b.BlogId ),
	ViewCount = ISNULL(b.ViewCount, 0 ), 
	Votes = ISNULL(b.Votes, 0), 
	TotalRating = ISNULL(b.TotalRating, 0),
	RatingResult =  ISNULL(b.TotalRating*1.0/b.Votes*1.0, 0 )
FROM
	tBlog b INNER JOIN vAccounts a ON a.AccountId = b.AccountId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = b.UrlAliasId
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vBlogComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.BlogCommentId, ac.BlogId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tBlogComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vBlogTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.BlogTagId, a.BlogId, t.TagId, t.Tag
FROM
	tBlogTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO
------------------------------------------------------------------------------------------------------------------------
-- PROFILE
------------------------------------------------------------------------------------------------------------------------
-- Profile
CREATE TABLE [dbo].[tProfile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NULL,
	[Type] INT NULL,
	[Description] NVARCHAR(1000) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([ProfileId] ASC)
)
GO

------------------------------------------------------------------------------------------------------------------------
-- Account Profile
CREATE TABLE [dbo].[tAccountProfile](
	[AccountProfileId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[ProfileId] INT NOT NULL,
	[Value] NVARCHAR(MAX) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_AccountProfile] PRIMARY KEY CLUSTERED ([AccountProfileId] ASC)
)
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_AccountId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_ProfileId] FOREIGN KEY([ProfileId])
	REFERENCES [tProfile] ([ProfileId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_ProfileId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tAccountProfile] ([AccountProfileId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_HistoryId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [CK_tAccountProfile_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [CK_tAccountProfile_HistoryType]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vProfiles AS SELECT A=1
GO
CREATE VIEW vAccountProfiles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ProfileId, [Name], [Type], [Description]
FROM tProfile
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vAccountProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ap.AccountProfileId, ap.AccountId, ap.ProfileId, ap.[Value], ProfileType = p.Type, ProfileName = p.Name
FROM tAccountProfile ap 
INNER JOIN tProfile p ON p.ProfileId = ap.ProfileId
WHERE ap.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pAccountProfileCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountProfileCreate
	@HistoryAccount INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @ProfileId, @Value, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountProfileId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountProfileModify
	@HistoryAccount INT,
	@AccountProfileId INT,
	@AccountId INT = NULL,
	@ProfileId INT = NULL,
	@Value NVARCHAR(MAX),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid AccountProfileId %d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccountProfile ( AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, @AccountProfileId
		FROM tAccountProfile
		WHERE AccountProfileId = @AccountProfileId

		UPDATE tAccountProfile
		SET
			AccountId=ISNULL(@AccountId, AccountId), ProfileId=ISNULL(@ProfileId, ProfileId), Value=@Value,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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
ALTER PROCEDURE pAccountProfileDelete
	@HistoryAccount INT,
	@AccountProfileId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AccountProfileId IS NULL OR NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AccountProfileId=%d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAccountProfile
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountProfileId
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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
-- IMAGE GALLERY tables
------------------------------------------------------------------------------------------------------------------------
-- Image gallery
CREATE TABLE [dbo].[tImageGallery](
	[ImageGalleryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Date] DATETIME NOT NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa blog bude zobrazovat*/
	[EnableComments] [bit] NULL,
	[EnableVotes] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dana galeria viditelna*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni galerie*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ImageGallery] PRIMARY KEY CLUSTERED ([ImageGalleryId] ASC)
)
GO
ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_RoleId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_UrlAliasId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_HistoryId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [CK_tImageGallery_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [CK_tImageGallery_HistoryType]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryTag
CREATE TABLE [dbo].[tImageGalleryTag](
	[ImageGalleryTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[ImageGalleryId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryTagId] PRIMARY KEY CLUSTERED ([ImageGalleryTagId] ASC)
)
GO

ALTER TABLE [tImageGalleryTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tImageGalleryTag] CHECK CONSTRAINT [FK_tImageGalleryTag_TagId]
GO

ALTER TABLE [tImageGalleryTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryTag_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryTag] CHECK CONSTRAINT [FK_tImageGalleryTag_ImageGalleryId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryComment
CREATE TABLE [dbo].[tImageGalleryComment](
	[ImageGalleryCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ImageGalleryId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryCommentId] PRIMARY KEY CLUSTERED ([ImageGalleryCommentId] ASC)
)
GO

ALTER TABLE [tImageGalleryComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tImageGalleryComment] CHECK CONSTRAINT [FK_tImageGalleryComment_CommentId]
GO

ALTER TABLE [tImageGalleryComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryComment_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryComment] CHECK CONSTRAINT [FK_tImageGalleryComment_ImageGalleryId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Image gallery item
CREATE TABLE [dbo].[tImageGalleryItem](
	[ImageGalleryItemId] [int] IDENTITY(1,1) NOT NULL,
	[ImageGalleryId] [int] NOT NULL,
	[VirtualPath] NVARCHAR(255) NOT NULL,
	[VirtualThumbnailPath] NVARCHAR(255) NOT NULL,
	[Position] INT NOT NULL,
	[Date] DATETIME NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	[ViewCount] [int] NULL, /*Pocet zobrazeni obrazku*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore obrazok obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore obrazok dostal pri hlasovani*/		
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ImageGalleryItem] PRIMARY KEY CLUSTERED ([ImageGalleryItemId] ASC)
)
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_ImageGalleryId]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tImageGalleryItem] ([ImageGalleryItemId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_HistoryId]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [CK_tImageGalleryItem_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [CK_tImageGalleryItem_HistoryType]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryItemComment
CREATE TABLE [dbo].[tImageGalleryItemComment](
	[ImageGalleryItemCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ImageGalleryItemId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryItemCommentId] PRIMARY KEY CLUSTERED ([ImageGalleryItemCommentId] ASC)
)
GO

ALTER TABLE [tImageGalleryItemComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItemComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tImageGalleryItemComment] CHECK CONSTRAINT [FK_tImageGalleryItemComment_CommentId]
GO

ALTER TABLE [tImageGalleryItemComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItemComment_ImageGalleryItemId] FOREIGN KEY([ImageGalleryItemId])
	REFERENCES [tImageGalleryItem] ([ImageGalleryItemId])
GO
ALTER TABLE [tImageGalleryItemComment] CHECK CONSTRAINT [FK_tImageGalleryItemComment_ImageGalleryItemId]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vImageGalleries AS SELECT A=1
GO
CREATE VIEW vImageGalleryTags AS SELECT TagId=1, ImageGalleryId=1
GO
CREATE VIEW vImageGalleryComments AS SELECT A=1
GO
CREATE VIEW vImageGalleryItems AS SELECT ImageGalleryId=1
GO
CREATE VIEW vImageGalleryItemComments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igc.ImageGalleryCommentId, igc.ImageGalleryId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryComment igc 
	INNER JOIN vComments c ON c.CommentId = igc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vImageGalleries
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryId, RoleId, g.[Name], g.[Date], g.EnableComments, g.EnableVotes, g.Visible,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryComments WHERE ImageGalleryId = g.ImageGalleryId  ),
	ItemsCount = ( SELECT Count(*) FROM vImageGalleryItems WHERE ImageGalleryId = g.ImageGalleryId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	g.UrlAliasId, a.Alias, a.Url
	
FROM tImageGallery g LEFT JOIN tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryTagId, g.ImageGalleryId, t.TagId, t.Tag
FROM
	tImageGalleryTag g 
	INNER JOIN vTags t ON t.TagId = g.TagId
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryItemComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igic.ImageGalleryItemCommentId, igic.ImageGalleryItemId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryItemComment igic 
	INNER JOIN vComments c ON c.CommentId = igic.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vImageGalleryItems
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ImageGalleryItemId, ImageGalleryId, [VirtualPath], [VirtualThumbnailPath], [Position], [Date], Description,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryItemComments WHERE ImageGalleryItemId = g.ImageGalleryItemId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	Votes = ISNULL(Votes, 0), 
	TotalRating = ISNULL(TotalRating, 0),
	RatingResult =  ISNULL(TotalRating*1.0/Votes*1.0, 0 )	
FROM tImageGalleryItem g
WHERE HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pImageGalleryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryTagCreate
	@HistoryAccount INT,
	@ImageGalleryId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ImageGalleryId FROM vImageGalleryTags WHERE TagId=@TagId AND ImageGalleryId=@ImageGalleryId) BEGIN
		INSERT INTO tImageGalleryTag ( TagId, ImageGalleryId ) VALUES ( @TagId, @ImageGalleryId )
	END

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryCommentCreate
	@HistoryAccount INT,
	@ImageGalleryId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryComment ( CommentId, ImageGalleryId ) VALUES ( @CommentId, @ImageGalleryId )

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryIncrementViewCount
	@ImageGalleryId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	UPDATE tImageGallery SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryId = @ImageGalleryId

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryCreate
	@HistoryAccount INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGallery ( [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Name, @RoleId, @Visible, @UrlAliasId, ISNULL(@Date,GETDATE()), @EnableComments, @EnableVotes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryModify
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGallery ( [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			[Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryId
		FROM tImageGallery
		WHERE ImageGalleryId = @ImageGalleryId

		UPDATE tImageGallery
		SET
			[Name]=@Name, [Date]=@Date, RoleId=@RoleId, Visible=@Visible, UrlAliasId=@UrlAliasId, EnableComments=@EnableComments, EnableVotes=@EnableVotes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryId = @ImageGalleryId

		SET @Result = @ImageGalleryId

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
ALTER PROCEDURE pImageGalleryDelete
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryId IS NULL OR NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryId=%d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGallery
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryId
		WHERE ImageGalleryId = @ImageGalleryId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tImageGallery SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ImageGalleryId

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
-- ImageGalleryItem
CREATE PROCEDURE pImageGalleryItemCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryItemIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemCreate
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGalleryItem ( ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ImageGalleryId, @VirtualPath, @VirtualThumbnailPath, @Position, GETDATE(), @Description,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryItemId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemModify
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGalleryItem ( ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryItemId
		FROM tImageGalleryItem
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		UPDATE tImageGalleryItem
		SET
			VirtualPath = ISNULL(@VirtualPath, VirtualPath), VirtualThumbnailPath = ISNULL(@VirtualThumbnailPath, VirtualThumbnailPath), Description = @Description, [Position] = @Position,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

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
ALTER PROCEDURE pImageGalleryItemDelete
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryItemId IS NULL OR NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryItemId=%d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGalleryItem
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryItemId
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

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
ALTER PROCEDURE pImageGalleryItemCommentCreate
	@HistoryAccount INT,
	@ImageGalleryItemId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryItemComment ( CommentId, ImageGalleryItemId ) VALUES ( @CommentId, @ImageGalleryItemId )

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemIncrementViewCount
	@ImageGalleryItemId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pImageGalleryItemIncrementVote
	@ImageGalleryItemId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO

------------------------------------------------------------------------------------------------------------------------
-- UPGARE tPage->tUrlAlias
------------------------------------------------------------------------------------------------------------------------
SET NOCOUNT ON;
DECLARE @PageId INT, @Name NVARCHAR(2000), @Url NVARCHAR(2000), @Locale CHAR(2)

DECLARE csxPages CURSOR 
FOR SELECT PageId, Name, Url, Locale  FROM tPage WHERE HistoryId IS NULL
	
OPEN csxPages

FETCH NEXT FROM csxPages INTO @PageId, @Name, @Url, @Locale

WHILE @@FETCH_STATUS = 0 BEGIN

	DECLARE @UrlAliasId INT, @Alias NVARCHAR(200)
	IF LEN(ISNULL(@Url, '')) = 0 
		SET @Url = '~/page.aspx?name=' + @Name
		
	SET @Alias = '~/' + @Name
	
	EXEC pUrlAliasCreate
		@Url = @Url,
		@Alias = @Alias,
		@Locale = @Locale,
		@Name = @Name,
		@Result = @UrlAliasId OUTPUT

	IF @UrlAliasId IS NOT NULL
		UPDATE tPage SET UrlAliasId = @UrlAliasId WHERE PageId = @PageId
	
	FETCH NEXT FROM csxPages INTO @PageId, @Name, @Url, @Locale
END

CLOSE csxPages
DEALLOCATE csxPages	
GO
------------------------------------------------------------------------------------------------------------------------
-- UPGARE tMenu->tUrlAlias
------------------------------------------------------------------------------------------------------------------------
SET NOCOUNT ON;
DECLARE @PageId INT, @MenuId INT, @UrlAliasId INT

DECLARE csxMenu CURSOR 
FOR SELECT m.PageId, m.MenuId, p.UrlAliasId FROM tMenu m
INNER JOIN tPage p ON p.PageId = m.PageId
INNER JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId 
	
OPEN csxMenu
FETCH NEXT FROM csxMenu INTO @PageId, @MenuId, @UrlAliasId
WHILE @@FETCH_STATUS = 0 BEGIN

	UPDATE tMenu SET UrlAliasId = @UrlAliasId WHERE MenuId = @MenuId
	FETCH NEXT FROM csxMenu INTO  @PageId, @MenuId, @UrlAliasId
END
CLOSE csxMenu
DEALLOCATE csxMenu	
GO
------------------------------------------------------------------------------------------------------------------------
-- UPGARE tNavigationMenu->tUrlAlias
------------------------------------------------------------------------------------------------------------------------
SET NOCOUNT ON;
DECLARE @PageId INT, @NavigationMenuId INT, @UrlAliasId INT

DECLARE csxNavigationMenu CURSOR 
FOR SELECT m.PageId, m.NavigationMenuId, p.UrlAliasId FROM tNavigationMenu m
INNER JOIN tPage p ON p.PageId = m.PageId
INNER JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId 
	
OPEN csxNavigationMenu
FETCH NEXT FROM csxNavigationMenu INTO @PageId, @NavigationMenuId, @UrlAliasId
WHILE @@FETCH_STATUS = 0 BEGIN

	UPDATE tNavigationMenu SET UrlAliasId = @UrlAliasId WHERE NavigationMenuId = @NavigationMenuId
	FETCH NEXT FROM csxNavigationMenu INTO  @PageId, @NavigationMenuId, @UrlAliasId
END
CLOSE csxNavigationMenu
DEALLOCATE csxNavigationMenu	
GO
------------------------------------------------------------------------------------------------------------------------
-- UPGARE tNavigationMenuItem->tUrlAlias
------------------------------------------------------------------------------------------------------------------------
SET NOCOUNT ON;
DECLARE @PageId INT, @NavigationMenuItemId INT, @UrlAliasId INT

DECLARE csxNavigationMenuItem CURSOR 
FOR SELECT m.PageId, m.NavigationMenuItemId, p.UrlAliasId FROM tNavigationMenuItem m
INNER JOIN tPage p ON p.PageId = m.PageId
INNER JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId 
	
OPEN csxNavigationMenuItem
FETCH NEXT FROM csxNavigationMenuItem INTO @PageId, @NavigationMenuItemId, @UrlAliasId
WHILE @@FETCH_STATUS = 0 BEGIN

	UPDATE tNavigationMenuItem SET UrlAliasId = @UrlAliasId WHERE NavigationMenuItemId = @NavigationMenuItemId
	FETCH NEXT FROM csxNavigationMenuItem INTO  @PageId, @NavigationMenuItemId, @UrlAliasId
END
CLOSE csxNavigationMenuItem
DEALLOCATE csxNavigationMenuItem	
GO
------------------------------------------------------------------------------------------------------------------------

ALTER TABLE tPage DROP COLUMN [Url]
GO

ALTER TABLE tMenu DROP CONSTRAINT FK_tMenu_PageId
GO
ALTER TABLE tMenu DROP COLUMN [PageId]
GO

ALTER TABLE tNavigationMenu DROP CONSTRAINT FK_tNavigationMenu_PageId
GO
ALTER TABLE tNavigationMenu DROP COLUMN [PageId]
GO

ALTER TABLE tNavigationMenuItem DROP CONSTRAINT FK_tNavigationMenuItem_PageId
GO
ALTER TABLE tNavigationMenuItem DROP COLUMN [PageId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Upravena Procedura, ktora neakceptuje duplicity
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasCreate
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist!' , 16, 1, @Url, @Locale);
		RETURN
	END	
	
	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias (Url, Locale, Alias, [Name] ) 
	VALUES ( @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
