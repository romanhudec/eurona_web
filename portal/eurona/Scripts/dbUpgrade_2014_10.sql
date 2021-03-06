UPDATE tUrlAlias SET Alias='~/eshop/novinky!', Name='Novinky'  WHERE Alias='~/eshop/novinky2014'
GO
UPDATE tPage SET Name='Novinky!' WHERE Name = 'Novinky 2014!'
GO
UPDATE tNavigationMenu SET Name='Novinky!' WHERE Name = 'Novinky 2014!'
GO

UPDATE tUrlAlias SET Alias='~/eshop/novinky!', Name='Novinky'  WHERE Alias='~/eshop/novinky2013'
GO
UPDATE tPage SET Name='Novinky!' WHERE Name = 'Novinky 2013!'
GO
UPDATE tNavigationMenu SET Name='Novinky!' WHERE Name = 'Novinky 2013!'
GO

INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (1, 'ESHOP_VYSYPANIVSECHKOSIKU', 'ESHOP', 'Vysypání všech nákupních košíků : ', '')
GO
------------------------------------------------------------------------------------------------------------------------
-- AdvisorPage
CREATE TABLE [dbo].[tAdvisorPage](
	[AdvisorPageId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ParentId] [int] NULL,
	[MasterPageId] [int] NOT NULL,
	[Locale] [char](2) NOT NULL CONSTRAINT [DF_tAdvisorPage_Locale]  DEFAULT ('en'),
	[AdvisorAccountId] [int] NOT NULL,
	[Blocked] [bit] NULL DEFAULT(0),
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](300) NOT NULL,
	[UrlAliasId] [int] NULL,
	[Content] [nvarchar](MAX) NULL,
	[ContentKeywords] [nvarchar](MAX) NULL,
	[RoleId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_AdvisorPageId] PRIMARY KEY CLUSTERED ([AdvisorPageId] ASC)
)
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tAdvisorPage] ([AdvisorPageId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_ParentId]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_MasterPageId] FOREIGN KEY([MasterPageId])
	REFERENCES [tMasterPage] ([MasterPageId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_MasterPageId]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_AdvisorAccountId] FOREIGN KEY([AdvisorAccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_AdvisorAccountId]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_RoleId]
GO

GO
ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD CONSTRAINT [FK_tAdvisorPage_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_tUrlAlias]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tAdvisorPage] ([AdvisorPageId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_HistoryId]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [CK_tAdvisorPage_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [CK_tAdvisorPage_HistoryType]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAdvisorPage_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [FK_tAdvisorPage_HistoryAccount]
GO

ALTER TABLE [tAdvisorPage]  WITH CHECK 
	ADD CONSTRAINT [CK_tAdvisorPage_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tAdvisorPage] CHECK CONSTRAINT [CK_tAdvisorPage_Locale]
GO

------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vAdvisorPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[AdvisorPageId], p.[ParentId], p.[InstanceId], p.[MasterPageId], p.[Locale], p.[AdvisorAccountId], p.[Blocked], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
	a.Url, a.Alias,
	ac.Email, o.OrganizationId, OrganizationName = o.Name, OrganizationCode = o.Code
FROM
	tAdvisorPage p 
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	INNER JOIN tAccount ac ON ac.AccountId = p.AdvisorAccountId
	INNER JOIN tOrganization o ON o.AccountId = p.AdvisorAccountId
WHERE
	p.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pAdvisorPageCreate
	@HistoryAccount INT,
	@ParentId INT = NULL,
	@InstanceId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@AdvisorAccountId INT,
	@Blocked BIT = 0,
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
		
	INSERT INTO tAdvisorPage ( InstanceId, ParentId, MasterPageId, Locale, AdvisorAccountId, Blocked, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @MasterPageId, @Locale, @AdvisorAccountId, @Blocked, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
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

			EXEC pAdvisorPageCreate @HistoryAccount=@HistoryAccount, @ParentId=@Result, @InstanceId=@InstanceId, @Locale=@Locale, @Name=@SubPageName, 
				@AdvisorAccountId=@AdvisorAccountId, @Blocked=@Blocked, @Title=@Title,
				@UrlAliasId = @SubPageUrlAliasId, @MasterPageId = @SubPageMasterPageId

			SET @i=@i+1
		END
	END


	SELECT PageId = @Result
END
GO

------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pAdvisorPageModify
	@HistoryAccount INT,
	@AdvisorPageId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Blocked BIT = 0,
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

	IF NOT EXISTS(SELECT * FROM tAdvisorPage WHERE AdvisorPageId = @AdvisorPageId AND HistoryId IS NULL)  BEGIN
		RAISERROR('Invalid AdvisorPageId %d', 16, 1, @AdvisorPageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
	
		-- Normalizacia nazvu
		SET @Name = dbo.fMakeAnsi(@Name)

		INSERT INTO tAdvisorPage (InstanceId, MasterPageId, Locale, Title, [Name], AdvisorAccountId, Blocked, UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, MasterPageId, Locale, Title, [Name], AdvisorAccountId, Blocked, UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @AdvisorPageId
		FROM tAdvisorPage
		WHERE AdvisorPageId = @AdvisorPageId

		UPDATE tAdvisorPage
		SET
			MasterPageId = @MasterPageId, RoleId = @RoleId,
			Locale = @Locale, Blocked=@Blocked, [Name] = @Name, Title = @Title, UrlAliasId = @UrlAliasId, Content = @Content, ContentKeywords = @ContentKeywords,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AdvisorPageId = @AdvisorPageId

		SET @Result = @AdvisorPageId

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
CREATE PROCEDURE pAdvisorPageDelete
	@HistoryAccount INT,
	@AdvisorPageId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AdvisorPageId IS NULL OR NOT EXISTS(SELECT * FROM tAdvisorPage WHERE AdvisorPageId = @AdvisorPageId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @AdvisorPageId=%d', 16, 1, @AdvisorPageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT, @AdvisorAccountId INT
		SELECT @AdvisorAccountId = AdvisorAccountId FROM tAdvisorPage WHERE AdvisorPageId = @AdvisorPageId AND HistoryId IS NULL
		SELECT @UrlAliasId = UrlAliasId FROM tAdvisorPage WHERE AdvisorPageId = @AdvisorPageId
		
		DELETE FROM tAdvisorPage WHERE AdvisorAccountId = @AdvisorAccountId
		IF @UrlAliasId IS NOT NULL
		BEGIN				
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId			
		END		

		SET @Result = @AdvisorPageId

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
CREATE TABLE [dbo].[tReklamniZasilky](
	[Id_zasilky] [smallint] NOT NULL,
	[Popis] [nvarchar](100) NOT NULL,
	[Default_souhlas] [bit] NOT NULL,
 CONSTRAINT [PK_reklamni_zasilky] PRIMARY KEY CLUSTERED ([Id_zasilky] ASC) ON [PRIMARY]
)
GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tReklamniZasilkySouhlas](
	[Id_zasilky] [smallint] NOT NULL,
	[Id_odberatele] [int] NOT NULL,
	[Souhlas] [bit] NOT NULL,
	[Datum_zmeny] [datetime] NOT NULL,
 CONSTRAINT [PK_reklamni_zasilky_souhlas] PRIMARY KEY CLUSTERED ([Id_zasilky] ASC,[Id_odberatele] ASC) ON [PRIMARY]
)
GO
------------------------------------------------------------------------------------------------------------------------

UPDATE tUrlAlias SET Alias='~/eshop/vanoce2014!', Name='Vanoce 2014'  WHERE Alias='~/eshop/novinky'
GO
UPDATE tPage SET Name='Vanoce 2014!' WHERE Name = 'Novinky!'
GO
UPDATE tNavigationMenu SET Name='Vanoce 2014!' WHERE Name = 'Novinky!'
GO
UPDATE tUrlAlias SET Url='~/eshop/catalog_vanoce.aspx' WHERE Alias='~/eshop/vanoce2014!'
GO
------------------------------------------------------------------------------------------------------------------------
