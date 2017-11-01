------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version
------------------------------------------------------------------------------------------------------------------------
-- SupportedLocale
CREATE TABLE [cSupportedLocale](
	[SupportedLocaleId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Icon] [nvarchar](255) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cSupportedLocale] PRIMARY KEY CLUSTERED ([SupportedLocaleId] ASC)
)
GO

ALTER TABLE [cSupportedLocale]  WITH CHECK 
	ADD  CONSTRAINT [FK_cSupportedLocale_cSupportedLocale] FOREIGN KEY([HistoryId])
	REFERENCES [cSupportedLocale] (SupportedLocaleId)
GO
ALTER TABLE [cSupportedLocale] CHECK CONSTRAINT [FK_cSupportedLocale_cSupportedLocale]
GO

ALTER TABLE [cSupportedLocale]  WITH CHECK 
	ADD  CONSTRAINT [CK_cSupportedLocale_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cSupportedLocale] CHECK CONSTRAINT [CK_cSupportedLocale_HistoryType]
GO
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vSupportedLocales AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Supported locale
CREATE PROCEDURE pSupportedLocaleCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSupportedLocaleCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pSupportedLocaleModify
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid SupportedLocaleId %d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @SupportedLocaleId
		FROM cSupportedLocale
		WHERE SupportedLocaleId = @SupportedLocaleId

		UPDATE cSupportedLocale
		SET
			[Name] = @Name, [Notes] = @Notes, Code = @Code, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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
ALTER PROCEDURE pSupportedLocaleDelete
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @SupportedLocaleId IS NULL OR NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @SupportedLocaleId=%d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cSupportedLocale
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @SupportedLocaleId
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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
ALTER VIEW vSupportedLocales
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	SupportedLocaleId, InstanceId, [Name], Notes, Code, Icon
FROM
	cSupportedLocale
WHERE
	HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------

ALTER TABLE tMasterPage ADD [Contents] [int] NULL CONSTRAINT [DF_tMasterPage_Contents]  DEFAULT (1)
GO
ALTER TABLE tMasterPage ADD [Default] [bit] NULL CONSTRAINT [DF_tMasterPage_Default]  DEFAULT (0)
GO
ALTER TABLE tMasterPage ADD [PageUrl] NVARCHAR(255) NULL
GO

UPDATE tMasterPage SET [Contents]=1
GO
UPDATE tMasterPage SET [Default]=0 WHERE MasterPageId!=1
GO
UPDATE tMasterPage SET [Default]=1 WHERE MasterPageId=1
GO
UPDATE tMasterPage SET [PageUrl]='~/page.aspx?name='
GO

ALTER TABLE  tPage ADD [ParentId] [int] NULL
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_ParentId]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [Default], [InstanceId], [Name], [Description], [Url], [Contents], [PageUrl]
FROM
	tMasterPage
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PageId], p.[ParentId], p.[InstanceId], p.[MasterPageId], p.[Locale], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
	a.Url, a.Alias
FROM
	tPage p LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@ParentId INT = NULL,
	@InstanceId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,
	-----------------------------------------
	-- Subpages settings
	@SubPageCreateContents BIT = 0,
	@SubPageMasterPageId INT = NULL,
	-----------------------------------------
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage ( InstanceId, ParentId, MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()


	DECLARE @MasterPageContents INT, @UrlAlias NVARCHAR(255)
	SELECT @MasterPageContents=Contents FROM tMasterPage WHERE MasterPageId=@MasterPageId
	SELECT @UrlAlias=Alias FROM tUrlAlias WHERE UrlAliasId=@UrlAliasId
	
	IF @SubPageCreateContents = 1 AND @MasterPageContents > 1
	BEGIN
		DECLARE @SubPageUrl NVARCHAR(255)

		IF @SubPageMasterPageId IS NULL
			SELECT @SubPageMasterPageId = MasterPageId FROM tMasterPage WHERE [Default]=1 AND InstanceId=@InstanceId

		SELECT @SubPageUrl = PageUrl FROM tMasterPage WHERE MasterPageId=@SubPageMasterPageId

		DECLARE @i INT
		SET @i=1
		WHILE (@i<=@MasterPageContents)
		BEGIN
			
			DECLARE @SubPageUrlAliasId INT,@SubPageName NVARCHAR(255), @SubPageAlias NVARCHAR(255), @Url NVARCHAR(255)
			SET @SubPageName = @Name+'_content'+ CONVERT(VARCHAR(3),@i)
			SET @Url = @SubPageUrl+@SubPageName
			SET @SubPageAlias = @UrlAlias + '/' + CONVERT(VARCHAR(3),@i)

			EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url=@Url, @Locale=@Locale, @Alias = @SubPageAlias, @Name=@Title,
			@Result = @SubPageUrlAliasId OUTPUT

			EXEC pPageCreate @HistoryAccount=@HistoryAccount, @ParentId=@Result, @InstanceId=@InstanceId, @Locale=@Locale, @Name=@SubPageName, @Title=@Title,
				@UrlAliasId = @SubPageUrlAliasId, @MasterPageId = @SubPageMasterPageId

			SET @i=@i+1
		END
	END


	SELECT PageId = @Result
END
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
--======================================================================================================================
-- SETUP CMS Version to 0.9
--======================================================================================================================
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 9, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------

