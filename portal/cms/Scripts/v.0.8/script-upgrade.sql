------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version
------------------------------------------------------------------------------------------------------------------------
DROP TABLE tMenu
GO
-- Menu
CREATE TABLE [dbo].[tMenu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tMenu_Locale]  DEFAULT ('en'),
	[Name] [nvarchar](100) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[RoleId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_MenuId] PRIMARY KEY CLUSTERED ([MenuId] ASC)
)
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_RoleId]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tMenu] ([MenuId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_HistoryId]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [CK_tMenu_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [CK_tMenu_HistoryType]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_HistoryAccount]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD CONSTRAINT [CK_tMenu_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [CK_tMenu_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenu
ALTER TABLE tNavigationMenu ADD [MenuId] [int] NULL
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenu_MenuId] FOREIGN KEY([MenuId])
	REFERENCES [tMenu] ([MenuId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_MenuId]
GO

------------------------------------------------------------------------------------------------------------------------
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1001, 1, 'sk', 'Hlavné navigačné menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1002, 1, 'cs', 'Hlavní navigační menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1003, 1, 'en', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1004, 1, 'de', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF
GO

UPDATE tNavigationMenu SET MenuId=-1001 WHERE Locale='sk'
UPDATE tNavigationMenu SET MenuId=-1002 WHERE Locale='cs'
UPDATE tNavigationMenu SET MenuId=-1003 WHERE Locale='en'
UPDATE tNavigationMenu SET MenuId=-1004 WHERE Locale='de'
GO

------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.InstanceId, m.Locale, m.[Code], m.[Name], m.RoleId
FROM tMenu m
WHERE m.HistoryId IS NULL
GO
-- SELECT * FROM vMenu
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Code NVARCHAR(100),
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( InstanceId, Locale, Code, [Name], RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Code, @Name, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT MenuId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pMenuModify
	@HistoryAccount INT,
	@MenuId INT,
	@Locale [char](2) = 'en', 
	@Code NVARCHAR(100),
	@Name NVARCHAR(100),
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

		INSERT INTO tMenu ( Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, Code = @Code, [Name] = @Name, RoleId = @RoleId,
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
	m.NavigationMenuId, m.InstanceId, m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pNavigationMenuCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tNavigationMenu ( InstanceId, MenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @MenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NavigationMenuId = @Result

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
			UPDATE tNavigationMenu SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			UPDATE tNavigationMenuItem SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			
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
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
--======================================================================================================================
-- SETUP CMS Version to 0.8
--======================================================================================================================
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 8, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------

