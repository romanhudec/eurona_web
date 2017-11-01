------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.3 to 0.4
------------------------------------------------------------------------------------------------------------------------
-- UrlAliasPrefix
CREATE TABLE [cUrlAliasPrefix](
	[UrlAliasPrefixId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cUrlAliasPrefix_Locale]  DEFAULT ('sk'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cUrlAliasPrefix] PRIMARY KEY CLUSTERED ([UrlAliasPrefixId] ASC)
)
GO

ALTER TABLE [cUrlAliasPrefix]  WITH CHECK 
	ADD  CONSTRAINT [FK_cUrlAliasPrefix_cUrlAliasPrefix] FOREIGN KEY([HistoryId])
	REFERENCES [cUrlAliasPrefix] (UrlAliasPrefixId)
GO
ALTER TABLE [cUrlAliasPrefix] CHECK CONSTRAINT [FK_cUrlAliasPrefix_cUrlAliasPrefix]
GO

ALTER TABLE [cUrlAliasPrefix]  WITH CHECK 
	ADD  CONSTRAINT [CK_cUrlAliasPrefix_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cUrlAliasPrefix] CHECK CONSTRAINT [CK_cUrlAliasPrefix_HistoryType]
GO

------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vUrlAliasPrefixes AS SELECT A=1
GO


ALTER VIEW vUrlAliasPrefixes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	UrlAliasPrefixId, [Name], [Code], [Locale], [Notes]
FROM cUrlAliasPrefix
WHERE HistoryId IS NULL
GO

------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pUrlAliasPrefixModify AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pUrlAliasPrefixModify
	@HistoryAccount INT,
	@UrlAliasPrefixId INT,
	@Name NVARCHAR(100) = '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cUrlAliasPrefix WHERE UrlAliasPrefixId = @UrlAliasPrefixId AND HistoryId IS NULL) 
		RAISERROR('Invalid UrlAliasPrefixId %d', 16, 1, @UrlAliasPrefixId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cUrlAliasPrefix ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @UrlAliasPrefixId
		FROM cUrlAliasPrefix
		WHERE UrlAliasPrefixId = @UrlAliasPrefixId

		UPDATE cUrlAliasPrefix
		SET
			[Name] = @Name, [Notes] = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE UrlAliasPrefixId = @UrlAliasPrefixId

		SET @Result = @UrlAliasPrefixId

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
-- URL Alis prefix
SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( 1, 'articles', 'clanky', 'sk', 'alias prefix for articles aliases' )

INSERT INTO cUrlAliasPrefix ( UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( 2, 'blogs', 'blogy', 'sk', 'alias prefix for blogs aliases' )

INSERT INTO cUrlAliasPrefix ( UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( 3, 'image-galleries', 'galerie-obrazkov', 'sk', 'alias prefix for image galleries aliases' )

INSERT INTO cUrlAliasPrefix ( UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( 4, 'news', 'novinky', 'sk', 'alias prefix for news aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tMenu
ALTER COLUMN UrlAliasId INT NULL
GO

ALTER TABLE tNavigationMenu
ALTER COLUMN UrlAliasId INT NULL
GO

ALTER TABLE tNavigationMenuItem
ALTER COLUMN UrlAliasId INT NULL
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
			UPDATE tMenu SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
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
-- Upgrade
------------------------------------------------------------------------------------------------------------------------
