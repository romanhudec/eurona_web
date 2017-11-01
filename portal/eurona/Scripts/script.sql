------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
-- AccountExt
CREATE TABLE [tAccountExt](
	[AccountId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[AdvisorId] [int] NULL
)
GO

ALTER TABLE [tAccountExt]  WITH CHECK 
	ADD CONSTRAINT [FK_tAccountExt_AdvisorId] FOREIGN KEY([AdvisorId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountExt] CHECK CONSTRAINT [FK_tAccountExt_AdvisorId]
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vAccountsExt AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Functions declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Functions declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Procedures declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Clasifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Standard procedures
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE pAccountExtCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountExtModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountExtDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Procedures declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Triggers declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Triggers declarations
------------------------------------------------------------------------------------------------------------------------


ALTER VIEW vAccountsExt
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.InstanceId, a.AdvisorId, AdvisorPersonId = p.PersonId
FROM
	tAccountExt a 
	LEFT JOIN tPerson p ON p.AccountId = a.AdvisorId
GO
ALTER PROCEDURE pAccountExtCreate
	@InstanceId INT,
	@AccountId INT,
	@AdvisorId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId = @InstanceId ) BEGIN
		RETURN
	END
	
	INSERT INTO tAccountExt ( InstanceId, AccountId, AdvisorId )
	VALUES (@InstanceId, @AccountId, @AdvisorId )
	
	SET @Result = @AccountId
	SELECT AccountId = @AccountId

END
GO

ALTER PROCEDURE pAccountExtDelete
	@InstanceId INT,
	@AccountId INT,
	@AdvisorId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId=@InstanceId) BEGIN
		RAISERROR( 'Invalid AccountId %d', 16, 1, @AccountId );
		RETURN
	END
	
	DELETE FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId=@InstanceId
	SET @Result = @AccountId

END
GO

ALTER PROCEDURE pAccountExtModify
	@InstanceId INT,
	@AccountId INT,
	@AdvisorId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountExt WHERE AccountId = @AccountId  AND InstanceId=@InstanceId) BEGIN
		RAISERROR( 'Invalid AccountId %d', 16, 1, @AccountId );
		RETURN
	END
	
	UPDATE tAccountExt SET AdvisorId=@AdvisorId
	WHERE AccountId = @AccountId AND InstanceId=@InstanceId

	SET @Result = @AccountId

END
GO
----======================================================================================================================
-- Init EURONA
----======================================================================================================================
DECLARE @InstanceId INT
SET @InstanceId = 1

SET IDENTITY_INSERT tRole ON
-- role <= -100 su role, ktore nie je mozne prostrednictvom UI odoberat.
INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -1001, 'Advisor', 'EURONA Advisor')
INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -1002, 'Operator', 'EURONA Operator')
--INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -1003, 'Host', 'EURONA Host')
SET IDENTITY_INSERT tRole OFF

--------------------------------------------------------------------------------------------------------------------------
-- Nastavenie podporovanych jazykov
SET IDENTITY_INSERT cSupportedLocale ON
--INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -1001, 'Slovenský jazyk', 'sk', 'SK', '~/userfiles/entityIcon/SupportedLocale/sk.png')
INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -1002, 'Český jazyk', 'cs', 'CZ', '~/userfiles/entityIcon/SupportedLocale/cs.png')
--INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -1003, 'Anglický jazyk', 'en', 'EN', '~/userfiles/entityIcon/SupportedLocale/en.png')
INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -1004, 'Polský jazyk', 'pl', 'PL', '~/userfiles/entityIcon/SupportedLocale/de.png')
SET IDENTITY_INSERT cSupportedLocale OFF
--------------------------------------------------------------------------------------------------------------------------

DECLARE @MasterPageId INT,  @MasterPage3ContentId INT,@ContactFormMasterPageId INT, @HostMasterPageId INT, @AdvisorMasterPageId INT, @ProductsMasterPageId INT
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [Default], [PageUrl]) VALUES(@InstanceId, 'Default', 'Default MasterPage', '~/page.master', 1, '~/page.aspx?name=')
SET @MasterPageId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Contents], [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 3, 'Default3Content', 'Default MasterPage with 3 contents', '~/page3content.master', '~/page3content.aspx?name=')
SET @MasterPage3ContentId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Contact form', 'Default MasterPage With Contact From', '~/pageWithContactForm.master', '~/page.aspx?name=')
SET @ContactFormMasterPageId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Host form', 'Default MasterPage for Host User Froms', '~/user/host/page.master', '~/page.aspx?name=')
SET @HostMasterPageId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Advisor form', 'Default MasterPage for Advisor User Froms', '~/user/advisor/page.master', '~/user/advisor/page.aspx?name=')
SET @AdvisorMasterPageId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Eshop products form', 'Default MasterPage for products Froms', '~/eshop/default.master', '~/eshop/page.aspx?name=')
SET @ProductsMasterPageId = SCOPE_IDENTITY()

DECLARE @UrlAliasId INT
DECLARE @PageId INT
-- predefined pages

--================================================================================================================================
-- MENU
--================================================================================================================================
-- MAIN NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1001, @InstanceId, 'sk', 'Hlavné navigačné menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1002, @InstanceId, 'cs', 'Hlavní navigační menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1003,@InstanceId, 'en', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1004, @InstanceId, 'pl', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- FOOTER NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1101, @InstanceId, 'sk', 'Navigačné menu pätičky', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1102, @InstanceId, 'cs', 'Navigační menu patičky', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1103,@InstanceId, 'en', 'Footer navigation menu', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1104, @InstanceId, 'pl', 'Footer navigation menu', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- PAGE NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1201, @InstanceId, 'sk', 'Navigačné menu stránky', 'page-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1202, @InstanceId, 'cs', 'Navigační menu stránky', 'page-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1203,@InstanceId, 'en', 'Page navigation menu', 'page-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1204, @InstanceId, 'pl', 'Page navigation menu', 'page-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- PRODUCTS NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1301, @InstanceId, 'sk', 'Navigačné menu vyrobkov', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1302, @InstanceId, 'cs', 'Navigační menu výrobků', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1303,@InstanceId, 'en', 'Products navigation menu', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1304, @InstanceId, 'pl', 'Products navigation menu', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- HOST USER NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1401, @InstanceId, 'sk', 'Navigačné menu hosťa', 'host-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1402, @InstanceId, 'cs', 'Navigační menu hosta', 'host-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1403,@InstanceId, 'en', 'Hosts navigation menu', 'host-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1404, @InstanceId, 'pl', 'Hosts navigation menu', 'host-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- ADVISOR USER NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1501, @InstanceId, 'sk', 'Navigačné menu poradcu', 'advisor-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1502, @InstanceId, 'cs', 'Navigační menu poradce', 'advisor-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1503,@InstanceId, 'en', 'Advisor navigation menu', 'advisor-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- DE
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-1504, @InstanceId, 'pl', 'Advisor navigation menu', 'advisor-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF
--================================================================================================================================
-- PAGES - LOGIN
--================================================================================================================================

DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)

-- SK
SET @pageTitle = 'Prihlásenie používateľa'
SET @pageName = 'prihlasenie-pouzivatela'
SET @pageUrl = '~/login.aspx'
SET @pageAlias = '~/prihlasit'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='user-login', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=1, @Name='Prihlásiť', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Přihlášení uživatele'
SET @pageName = 'prihlaseni-uzivatele'
SET @pageUrl = '~/login.aspx'
SET @pageAlias = '~/prihlasit'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='user-login', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=1, @Name='Přihlásit', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'User login'
SET @pageName = 'user-login'
SET @pageUrl = '~/login.aspx'
SET @pageAlias = '~/login'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='user-login', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003,@Locale='en', @Order=1, @Name='Login', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='user-login', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=1, @Name='Login', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	

--================================================================================================================================
-- PAGES - ABOUT US
--================================================================================================================================
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/page3content.aspx?name=about', @Locale='sk', @Alias = '~/o-spolocnosti', @Name='O Spoločnosti',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='about', @Title='O Spoločnosti',
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001, @Locale='sk', @Order=2, @Name='O Spoločnosti', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/page3content.aspx?name=about', @Locale='cs', @Alias = '~/o-spolecnosti', @Name='O Společnosti',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='about', @Title='O Společnosti',
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002, @Locale='cs', @Order=2, @Name='O Společnosti', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/page3content.aspx?name=about', @Locale='en', @Alias = '~/about', @Name='About Us',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='about', @Title='About Us',
	@Content = '', @UrlAliasId = @UrlAliasId, 
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003, @Locale='en', @Order=2, @Name='About Us', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
			
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/page3content.aspx?name=about', @Locale='pl', @Alias = '~/about', @Name='About Us',
	@Result = @UrlAliasId OUTPUT					
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='about', @Title='O Společnosti',
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004, @Locale='pl', @Order=2, @Name='About Us', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

--================================================================================================================================
-- PAGES - CUSTOMER SERVIS
--================================================================================================================================
SET @pageTitle = 'Zákaznický servis'
SET @pageName = 'zakaznicky-servis'
SET @pageUrl = '~/page.aspx?name=customer-service'
SET @pageAlias = '~/zakaznicky-servis'

-- SK
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='customer-service', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001, @Locale='sk', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CS
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='customer-service', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002, @Locale='cs', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
	
SET @pageTitle = 'Customer service'
SET @pageName = 'customer-service'
SET @pageUrl = '~/page.aspx?name=customer-service'
SET @pageAlias = '~/customer-service'			

-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='customer-service', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003, @Locale='en', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='customer-service', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004, @Locale='pl', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL

--================================================================================================================================
-- PAGES - HOST ACCESS
--================================================================================================================================
-- SK
SET @pageTitle = 'Vaša registrácia'
SET @pageName = 'vasa-registracia'
SET @pageUrl = '~/user/host/default.aspx'
SET @pageAlias = '~/vasa-registracia'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='host-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Vaše registrace'
SET @pageName = 'vase-registrace'
SET @pageUrl = '~/user/host/default.aspx'
SET @pageAlias = '~/vase-registrace'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='host-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Your registration'
SET @pageName = 'your-registration'
SET @pageUrl = '~/user/host/default.aspx'
SET @pageAlias = '~/your-registration'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='host-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003,@Locale='en', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
SET @pageTitle = 'Rejestracji'
SET @pageName = 'rejestracji'
SET @pageUrl = '~/user/host/default.aspx'
SET @pageAlias = '~/rejestracji'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='host-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
--================================================================================================================================
-- PAGES - ADVISOR ACCESS
--================================================================================================================================
-- SK
SET @pageTitle = 'Pre poradcu'
SET @pageName = 'pre-poradcu'
SET @pageUrl = '~/user/advisor/default.aspx'
SET @pageAlias = '~/pre-poradcu'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='advisor-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Pro poradce'
SET @pageName = 'pro-poradce'
SET @pageUrl = '~/user/advisor/default.aspx'
SET @pageAlias = '~/pro-poradce'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='advisor-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'For Advisor'
SET @pageName = 'for-advisor'
SET @pageUrl = '~/user/advisor/default.aspx'
SET @pageAlias = '~/for-advisor'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='advisor-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003,@Locale='en', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='advisor-access', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL

--================================================================================================================================
-- PAGES - KONTAKTY @ContactFormMasterPageId
--================================================================================================================================
-- SK
SET @pageTitle = 'Kontakty'
SET @pageName = 'kontakty'
SET @pageUrl = '~/page.aspx?name=contacts'
SET @pageAlias = '~/kontakty'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='contacts', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @ContactFormMasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Kontakty'
SET @pageName = 'kontakty'
SET @pageUrl = '~/page.aspx?name=contacts'
SET @pageAlias = '~/kontakty'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='contacts', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @ContactFormMasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Contacts'
SET @pageName = 'contacts'
SET @pageUrl = '~/page.aspx?name=contacts'
SET @pageAlias = '~/contacts'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='contacts', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @ContactFormMasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003,@Locale='en', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='contacts', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @ContactFormMasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
--================================================================================================================================
-- PAGES - HOME
--================================================================================================================================
-- SK
SET @pageTitle = 'Úvodná stránka'
SET @pageName = 'uvodna-stranka'
SET @pageUrl = '~/default.aspx'
SET @pageAlias = '~/home'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='home-page', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=6, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Úvodní stránka'
SET @pageName = 'uvodni-stranka'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='home-page', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=6, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Home page'
SET @pageName = 'home-page'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='home-page', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003,@Locale='en', @Order=6, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='home-page', @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=6, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

---------------------------------------------------------------------------------------------------------
-- Home content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1001, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/userfiles/home-text.png" /></p>', 'sk', 'home-content', 'Úvodná stránka', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1002, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/userfiles/home-text.png" /></p>', 'cs', 'home-content', 'Úvodní stránka', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1003, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/userfiles/home-text.png" /></p>', 'en', 'home-content', 'Homepage', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1004, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/userfiles/home-text.png" /></p>', 'pl', 'home-content', 'Homepage', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Advisor menu content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1101, @MasterPageId, '<a>Formuláře</a> <a href="../../userfiles/formulare/Inovace_2010_obchodni_podminky.pdf" class="advisor-pagesubmenu" target="_blank">Obchodní podmínky 2010</a> <a href="../../userfiles/formulare/registracni_formular_prihlaska.pdf" class="advisor-pagesubmenu" target="_blank">Registrační formulář</a> <a href="../../userfiles/formulare/formularz_rejestracja.pdf" class="advisor-pagesubmenu" target="_blank">Rejestracja formularz</a> <a href="../../userfiles/formulare/objednavkovy_formular.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (CZ,SK)</a><a href="../../userfiles/formulare/formularz_zamowienia.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (PL)</a><a href="../../userfiles/formulare/reklamace.pdf" class="advisor-pagesubmenu" target="_blank">Reklamační protokol</a> <a href="../../userfiles/formulare/2010%20cenik.pdf" class="advisor-pagesubmenu" target="_blank">Ceník 2010 CZ,SK</a> <a href="../../userfiles/formulare/cenik_pl_2009.pdf" class="advisor-pagesubmenu" target="_blank">Cennik 2009 PL</a> <a href="../../userfiles/formulare/parametry%20osobnich%20internetovych%20stranek.pdf" class="advisor-pagesubmenu" target="_blank">Parametry osobních internetových stránek</a>', 'sk', 'advisor-menu-content', 'Menu poradcu', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1102, @MasterPageId, '<a>Formuláře</a> <a href="../../userfiles/formulare/Inovace_2010_obchodni_podminky.pdf" class="advisor-pagesubmenu" target="_blank">Obchodní podmínky 2010</a> <a href="../../userfiles/formulare/registracni_formular_prihlaska.pdf" class="advisor-pagesubmenu" target="_blank">Registrační formulář</a> <a href="../../userfiles/formulare/formularz_rejestracja.pdf" class="advisor-pagesubmenu" target="_blank">Rejestracja formularz</a> <a href="../../userfiles/formulare/objednavkovy_formular.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (CZ,SK)</a><a href="../../userfiles/formulare/formularz_zamowienia.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (PL)</a><a href="../../userfiles/formulare/reklamace.pdf" class="advisor-pagesubmenu" target="_blank">Reklamační protokol</a> <a href="../../userfiles/formulare/2010%20cenik.pdf" class="advisor-pagesubmenu" target="_blank">Ceník 2010 CZ,SK</a> <a href="../../userfiles/formulare/cenik_pl_2009.pdf" class="advisor-pagesubmenu" target="_blank">Cennik 2009 PL</a> <a href="../../userfiles/formulare/parametry%20osobnich%20internetovych%20stranek.pdf" class="advisor-pagesubmenu" target="_blank">Parametry osobních internetových stránek</a>', 'cs', 'advisor-menu-content', 'Menu poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1103, @MasterPageId, '<a>Formuláře</a> <a href="../../userfiles/formulare/Inovace_2010_obchodni_podminky.pdf" class="advisor-pagesubmenu" target="_blank">Obchodní podmínky 2010</a> <a href="../../userfiles/formulare/registracni_formular_prihlaska.pdf" class="advisor-pagesubmenu" target="_blank">Registrační formulář</a> <a href="../../userfiles/formulare/formularz_rejestracja.pdf" class="advisor-pagesubmenu" target="_blank">Rejestracja formularz</a> <a href="../../userfiles/formulare/objednavkovy_formular.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (CZ,SK)</a><a href="../../userfiles/formulare/formularz_zamowienia.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (PL)</a><a href="../../userfiles/formulare/reklamace.pdf" class="advisor-pagesubmenu" target="_blank">Reklamační protokol</a> <a href="../../userfiles/formulare/2010%20cenik.pdf" class="advisor-pagesubmenu" target="_blank">Ceník 2010 CZ,SK</a> <a href="../../userfiles/formulare/cenik_pl_2009.pdf" class="advisor-pagesubmenu" target="_blank">Cennik 2009 PL</a> <a href="../../userfiles/formulare/parametry%20osobnich%20internetovych%20stranek.pdf" class="advisor-pagesubmenu" target="_blank">Parametry osobních internetových stránek</a>', 'en', 'advisor-menu-content', 'Advisor menu', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1104, @MasterPageId, '<a>Formuláře</a> <a href="../../userfiles/formulare/Inovace_2010_obchodni_podminky.pdf" class="advisor-pagesubmenu" target="_blank">Obchodní podmínky 2010</a> <a href="../../userfiles/formulare/registracni_formular_prihlaska.pdf" class="advisor-pagesubmenu" target="_blank">Registrační formulář</a> <a href="../../userfiles/formulare/formularz_rejestracja.pdf" class="advisor-pagesubmenu" target="_blank">Rejestracja formularz</a> <a href="../../userfiles/formulare/objednavkovy_formular.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (CZ,SK)</a><a href="../../userfiles/formulare/formularz_zamowienia.pdf" class="advisor-pagesubmenu" target="_blank">Objednávkový formulář (PL)</a><a href="../../userfiles/formulare/reklamace.pdf" class="advisor-pagesubmenu" target="_blank">Reklamační protokol</a> <a href="../../userfiles/formulare/2010%20cenik.pdf" class="advisor-pagesubmenu" target="_blank">Ceník 2010 CZ,SK</a> <a href="../../userfiles/formulare/cenik_pl_2009.pdf" class="advisor-pagesubmenu" target="_blank">Cennik 2009 PL</a> <a href="../../userfiles/formulare/parametry%20osobnich%20internetovych%20stranek.pdf" class="advisor-pagesubmenu" target="_blank">Parametry osobních internetových stránek</a>', 'pl', 'advisor-menu-content', 'Advisor menu', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

---------------------------------------------------------------------------------------------------------
-- Advisor BANNER content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1201, @MasterPageId, '', 'sk', 'advisor-banner-content', 'BANNER poradcu', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1202, @MasterPageId, '', 'cs', 'advisor-banner-content', 'BANNER poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1203, @MasterPageId, '', 'en', 'advisor-banner-content', 'Advisor BANNER', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1204, @MasterPageId, '', 'pl', 'advisor-banner-content', 'Doradca BANNER', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

---------------------------------------------------------------------------------------------------------
-- Advisor menu Podpora prodeje - Uspesny start
SET IDENTITY_INSERT tPage ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-uspesny-start', @Locale='sk', @Alias = '~/user/advisor/uspesny-start', @Name='podpora-prodeje-uspesny-start',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId, -1301, @MasterPageId, '', 'sk', 'advisor-menu-podpora-prodeje-uspesny-start', 'Podpora prodeje - úspešný štart', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-uspesny-start', @Locale='cs', @Alias = '~/user/advisor/uspesny-start', @Name='podpora-prodeje-uspesny-start',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1302, @MasterPageId, '', 'cs', 'advisor-menu-podpora-prodeje-uspesny-start', 'Podpora prodeje - úspěšný start', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-uspesny-start', @Locale='en', @Alias = '~/user/advisor/successful-start', @Name='podpora-prodeje-uspesny-start',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1303, @MasterPageId, '', 'en', 'advisor-menu-podpora-prodeje-uspesny-start', 'Sales Support - successful start', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-uspesny-start', @Locale='pl', @Alias = '~/user/advisor/wsparcie-sprzedazy', @Name='podpora-prodeje-uspesny-start',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1304, @MasterPageId, '', 'pl', 'advisor-menu-podpora-prodeje-uspesny-start', 'Wsparcie sprzedaży - pomyślna', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

-- Advisor menu Podpora prodeje - Akcni cennik
SET IDENTITY_INSERT tPage ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-akcni-cennik', @Locale='sk', @Alias = '~/user/advisor/akcny-cennik', @Name='podpora-prodeje-akcni-cennik',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId, -1401, @MasterPageId, '', 'sk', 'advisor-menu-podpora-prodeje-akcni-cennik', 'Podpora prodeje - akčný cenník', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-akcni-cennik', @Locale='cs', @Alias = '~/user/advisor/akcni-cennik', @Name='podpora-prodeje-akcni-cennik',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1402, @MasterPageId, '', 'cs', 'advisor-menu-podpora-prodeje-akcni-cennik', 'Podpora prodeje - akční cenník', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-akcni-cennik', @Locale='en', @Alias = '~/user/advisor/action-pricelist', @Name='podpora-prodeje-akcni-cennik',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1403, @MasterPageId, '', 'en', 'advisor-menu-podpora-prodeje-akcni-cennik', 'Sales Support - Action Pricelist', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-akcni-cennik', @Locale='pl', @Alias = '~/user/advisor/cennik-akcji', @Name='podpora-prodeje-akcni-cennik',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1404, @MasterPageId, '', 'pl', 'advisor-menu-podpora-prodeje-akcni-cennik', 'Wsparcie sprzedaży - cennik akcji', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

-- Advisor menu Podpora prodeje - EURONA News
SET IDENTITY_INSERT tPage ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-eurona-news', @Locale='sk', @Alias = '~/user/advisor/eurona-news', @Name='podpora-prodeje-eurona-news',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId, -1501, @MasterPageId, '', 'sk', 'advisor-menu-podpora-prodeje-eurona-news', 'Podpora prodeje - EURONA News', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-eurona-news', @Locale='cs', @Alias = '~/user/advisor/eurona-news', @Name='podpora-prodeje-eurona-news',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1502, @MasterPageId, '', 'cs', 'advisor-menu-podpora-prodeje-eurona-news', 'Podpora prodeje - EURONA News', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-eurona-news', @Locale='en', @Alias = '~/user/advisor/eurona-news', @Name='podpora-prodeje-eurona-news',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1503, @MasterPageId, '', 'en', 'advisor-menu-podpora-prodeje-eurona-news', 'Sales Support - EURONA News', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-eurona-news', @Locale='pl', @Alias = '~/user/advisor/eurona-news', @Name='podpora-prodeje-eurona-news',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1504, @MasterPageId, '', 'pl', 'advisor-menu-podpora-prodeje-eurona-news', 'Wsparcie sprzedaży - EURONA News', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

-- Advisor menu Podpora prodeje - Prezentacni Letaky
SET IDENTITY_INSERT tPage ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-prezentacni-letaky', @Locale='sk', @Alias = '~/user/advisor/prezentacne-letaky', @Name='podpora-prodeje-prezentacni-letaky',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId, -1601, @MasterPageId, '', 'sk', 'advisor-menu-podpora-prodeje-prezentacni-letaky', 'Podpora prodeje - prezentačné letáky', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-prezentacni-letaky', @Locale='cs', @Alias = '~/user/advisor/prezentacni-letaky', @Name='podpora-prodeje-prezentacni-letaky',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1602, @MasterPageId, '', 'cs', 'advisor-menu-podpora-prodeje-prezentacni-letaky', 'Podpora prodeje - prezentační letáky', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-prezentacni-letaky', @Locale='en', @Alias = '~/user/advisor/presentation-flyers', @Name='podpora-prodeje-prezentacni-letaky',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1603, @MasterPageId, '', 'en', 'advisor-menu-podpora-prodeje-prezentacni-letaky', 'Sales Support - presentation leaflets', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-prezentacni-letaky', @Locale='pl', @Alias = '~/user/advisor/prezentacja-ulotki', @Name='podpora-prodeje-prezentacni-letaky',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1604, @MasterPageId, '', 'pl', 'advisor-menu-podpora-prodeje-prezentacni-letaky', 'Wsparcie sprzedaży - ulotki prezentacji', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

-- Advisor menu Podpora prodeje - Vzdelavani
SET IDENTITY_INSERT tPage ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-vzdelavani', @Locale='sk', @Alias = '~/user/advisor/vzdelavani', @Name='podpora-prodeje-vzdelavani',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId, -1701, @MasterPageId, '', 'sk', 'advisor-menu-podpora-prodeje-vzdelavani', 'Podpora prodeje - vzdelávanie', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-vzdelavani', @Locale='cs', @Alias = '~/user/advisor/vzdelavani', @Name='podpora-prodeje-vzdelavani',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1702, @MasterPageId, '', 'cs', 'advisor-menu-podpora-prodeje-vzdelavani', 'Podpora prodeje - vzdelávání', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-vzdelavani', @Locale='en', @Alias = '~/user/advisor/education', @Name='podpora-prodeje-vzdelavani',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1703, @MasterPageId, '', 'en', 'advisor-menu-podpora-prodeje-vzdelavani', 'Podpora prodeje - education', GETDATE(), 'C', 1)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/page.aspx?name=advisor-menu-podpora-prodeje-vzdelavani', @Locale='pl', @Alias = '~/user/advisor/edukacji', @Name='podpora-prodeje-vzdelavani',
	@Result = @UrlAliasId OUTPUT
INSERT INTO tPage (InstanceId, UrlAliasId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, @UrlAliasId,-1704, @MasterPageId, '', 'pl', 'advisor-menu-podpora-prodeje-vzdelavani', 'Podpora prodeje - edukacja', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
--===========================================================================================================================================================
-- REGISTER USER
--===========================================================================================================================================================	
-- Register	Host
DECLARE @NavigationMenuId INT
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx', @Locale='sk', @Alias = '~/registracia-host', @Name='Registrácia host',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia-host', @Title='Registrácia hosťa',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=5, @Name='registrácia-host', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL, @Result = @NavigationMenuId OUTPUT
-- Organization	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=1', @Locale='sk', @Alias = '~/registracia-host-organizacia', @Name='Registrácia host organizácia',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia-host-organizacia', @Title='Registrácia hosťa organizácie',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='sk', @Order=5, @Name='registrácia-host-organizacia', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
-- Person
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=2', @Locale='sk', @Alias = '~/registracia-host-osoba', @Name='Registrácia host osoba',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia-host-osoba', @Title='Registrácia hosťa osoby',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='sk', @Order=5, @Name='registrácia-host-organizacia', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL

-- cs	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx', @Locale='cs', @Alias = '~/registrace-host', @Name='Registrace hosta',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace-host', @Title='Registrace hosta',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=5, @Name='registrace-host', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL, @Result = @NavigationMenuId OUTPUT			
-- Organization	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=1', @Locale='cs', @Alias = '~/registrace-host-organizace', @Name='Registrace host organizace',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace-host-organizace', @Title='Registrace hosta organizace',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='cs', @Order=5, @Name='registrace-host-organizace', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
-- Person
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=2', @Locale='cs', @Alias = '~/registrace-host-osoba', @Name='Registrace host osoba',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace-host-osoba', @Title='Registrace hosta osoby',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='cs', @Order=5, @Name='registrace-host-osoby', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
	
-- en		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx', @Locale='en', @Alias = '~/registration-host', @Name='Registration host',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration-host', @Title='Registration host',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003, @Locale='en', @Order=5, @Name='registration-host', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL, @Result = @NavigationMenuId OUTPUT		
-- Organization	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=1', @Locale='en', @Alias = '~/registration-host-organization', @Name='Registration host organization',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration-host-organizace', @Title='Registration host organization',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='en', @Order=5, @Name='registration-host-organization', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
-- Person
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=2', @Locale='en', @Alias = '~/registration-host-person', @Name='Registration host person',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration-host-person', @Title='Registration host person',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='en', @Order=5, @Name='registration-host-person', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
		
-- PL	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx', @Locale='pl', @Alias = '~/registration-host', @Name='Registration host',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration-host', @Title='Rejestracja gosta',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=5, @Name='registration-host', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL, @Result = @NavigationMenuId OUTPUT
-- Organization	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=1', @Locale='pl', @Alias = '~/registration-host-organization', @Name='Registration host organization',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration-host-organizace', @Title='Registration host organization',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='pl', @Order=5, @Name='registration-host-organization', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
-- Person
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/host/register.aspx?type=2', @Locale='pl', @Alias = '~/registration-host-person', @Name='Registration host person',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration-host-person', @Title='Registration host person',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuItemCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @NavigationMenuId=@NavigationMenuId, @Locale='pl', @Order=5, @Name='registration-host-person', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	
---------------------------------------------------------------------------------------------------------		
-- Register	Advisor
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/register.aspx', @Locale='sk', @Alias = '~/registracia-poradca', @Name='Registrácia poradcu',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia-poradca', @Title='Registrácia poradcu',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=98, @Name='Registrácia poradcu', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1401,@Locale='sk', @Order=98, @Name='Registrácia poradcu', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/register.aspx', @Locale='cs', @Alias = '~/registrace-poradce', @Name='Registrace poradce',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace-poradce', @Title='Registrace poradce',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=98, @Name='Registrace poradce', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL			
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1402,@Locale='cs', @Order=98, @Name='Registrace poradce', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
			
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/register.aspx', @Locale='en', @Alias = '~/registration-advisor', @Name='Registration advisor',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration-advisor', @Title='Registration advisor',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=98, @Name='Registration advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1403,@Locale='en', @Order=98, @Name='Registration advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/register.aspx', @Locale='pl', @Alias = '~/registration-advisor', @Name='Registration advisor',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration-advisor', @Title='Rejestracja poradca',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=98, @Name='Registration advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1404,@Locale='pl', @Order=98, @Name='Registration advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
---------------------------------------------------------------------------------------------------------		
-- Register	Operator
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/operator/register.aspx', @Locale='sk', @Alias = '~/registracia-operator', @Name='Registrácia operatora',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia-operator', @Title='Registrácia operatora',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1001,@Locale='sk', @Order=5, @Name='registrácia-operator', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/operator/register.aspx', @Locale='cs', @Alias = '~/registrace-operator', @Name='Registrace operatora',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace-operator', @Title='Registrace operatora',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1002,@Locale='cs', @Order=5, @Name='registrace-operator', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL			
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/operator/register.aspx', @Locale='en', @Alias = '~/registration-operator', @Name='Registration operator',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration-operator', @Title='Registration operator',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1003, @Locale='en', @Order=5, @Name='registration-operator', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/operator/register.aspx', @Locale='pl', @Alias = '~/registration-operator', @Name='Registration operator',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration-operator', @Title='Rejestracja operatora',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1004,@Locale='pl', @Order=5, @Name='registration-operator', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	

---------------------------------------------------------------------------------------------------------		
-- Find	Advisor
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/findAdvisor.aspx', @Locale='sk', @Alias = '~/vyhladavanie-poradcu', @Name='Vyhladavanie poradcu',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='vyhladavanie-poradcu', @Title='Vyhladavanie poradcu',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=99, @Name='Vyhľadávanie poradcu', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1401,@Locale='sk', @Order=99, @Name='Vyhľadávanie poradcu', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/findAdvisor.aspx', @Locale='cs', @Alias = '~/vyhledavani-poradce', @Name='Vyhledavani poradce',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='vyhledavani-poradce', @Title='Vyhledavani poradce',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=99, @Name='Vyhledáváni poradce', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL			
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1402,@Locale='cs', @Order=99, @Name='Vyhledáváni poradce', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
			
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/findAdvisor.aspx', @Locale='en', @Alias = '~/find-advisor', @Name='Find advisor',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='find-advisor', @Title='Find advisor',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=99, @Name='Find advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1403,@Locale='en', @Order=99, @Name='Find advisor', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/findAdvisor.aspx', @Locale='pl', @Alias = '~/doradca-wyszukiwania', @Name='Doradca wyszukiwania',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='doradca-wyszukiwania', @Title='Doradca wyszukiwania',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=99, @Name='Doradca wyszukiwania', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1404,@Locale='pl', @Order=99, @Name='Doradca wyszukiwania', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL			
---------------------------------------------------------------------------------------------------------
-- Articles		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/articleArchiv.aspx', @Locale='sk', @Alias = '~/clanky', @Name='Články',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='clanky', @Title='Články',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/articleArchiv.aspx', @Locale='cs', @Alias = '~/clanky', @Name='Články',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='clanky', @Title='Články',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/articleArchiv.aspx', @Locale='en', @Alias = '~/articles', @Name='Articles',
	@Result = @UrlAliasId OUTPUT				
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='articles', @Title='Articles',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/articleArchiv.aspx', @Locale='pl', @Alias = '~/articles', @Name='Articles',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='articles', @Title='Artykuły',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT

-- ADVISOR Articles		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/articleArchiv.aspx', @Locale='sk', @Alias = '~/clanky-poradcu', @Name='Poradca - Články',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='clanky-poradcu', @Title='Články',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/articleArchiv.aspx', @Locale='cs', @Alias = '~/clanky-poradce', @Name='Poradce - Články',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='clanky-poradce', @Title='Články',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/articleArchiv.aspx', @Locale='en', @Alias = '~/advisor-articles', @Name='Advisor - Articles',
	@Result = @UrlAliasId OUTPUT				
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='advisor-articles', @Title='Articles',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/articleArchiv.aspx', @Locale='pl', @Alias = '~/advisor-articles', @Name='Advisor - Articles',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='advisor-articles', @Title='Artykuły',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
---------------------------------------------------------------------------------------------------------
-- ImageGalleries		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/imageGalleries.aspx', @Locale='sk', @Alias = '~/galerie-obrazkov', @Name='Galérie obrázkov',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='galeria-obrazkov', @Title='Galérie obrázkov',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/imageGalleries.aspx', @Locale='cs', @Alias = '~/galerie-obrazku', @Name='Galerie obrázků',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='galerie-obrazku', @Title='Galerie obrázků',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
				
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/imageGalleries.aspx', @Locale='en', @Alias = '~/image-galleries', @Name='Image galeries',
	@Result = @UrlAliasId OUTPUT					
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='image-galleries', @Title='Image galeries',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/imageGalleries.aspx', @Locale='pl', @Alias = '~/image-galleries', @Name='Image galeries',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='image-galleries', @Title='Galerie zdjęć',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT

---------------------------------------------------------------------------------------------------------
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/polls.aspx', @Locale='sk', @Alias = '~/ankety', @Name='Ankety',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='ankety', @Title='Ankety',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/polls.aspx', @Locale='cs', @Alias = '~/ankety', @Name='Ankety',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='ankety', @Title='Ankety',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/polls.aspx', @Locale='en', @Alias = '~/polls', @Name='Polls',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='polls', @Title='Polls',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/polls.aspx', @Locale='pl', @Alias = '~/polls', @Name='Polls',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='polls', @Title='Sondy',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId

-- ADVISOR POOLS
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/polls.aspx', @Locale='sk', @Alias = '~/ankety-poradcu', @Name='Poradca - Ankety',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='ankety-poradcu', @Title='Ankety',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/polls.aspx', @Locale='cs', @Alias = '~/ankety-poradce', @Name='Poradce - Ankety',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='ankety-poradce', @Title='Ankety',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/polls.aspx', @Locale='en', @Alias = '~/advisor-polls', @Name='Advisor - Polls',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='advisor-polls', @Title='Polls',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/polls.aspx', @Locale='pl', @Alias = '~/advisor-polls', @Name='Advisor - Polls',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='advisor-polls', @Title='Sondy',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
---------------------------------------------------------------------------------------------------------
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/newsArchiv.aspx', @Locale='sk', @Alias = '~/novinky', @Name='Novinky',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='novinky', @Title='Novinky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/newsArchiv.aspx', @Locale='cs', @Alias = '~/novinky', @Name='Novinky',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='novinky', @Title='Novinky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/newsArchiv.aspx', @Locale='en', @Alias = '~/news', @Name='News',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='news', @Title='News',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/newsArchiv.aspx', @Locale='pl', @Alias = '~/news', @Name='News',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='news', @Title='Aktualności',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId

-- ADVISOR News
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/newsArchiv.aspx', @Locale='sk', @Alias = '~/novinky-poradcu', @Name='Poradca - Novinky',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='novinky-poradcu', @Title='Novinky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/newsArchiv.aspx', @Locale='cs', @Alias = '~/novinky-poradce', @Name='Poradce - Novinky',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='novinky-poradce', @Title='Novinky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/newsArchiv.aspx', @Locale='en', @Alias = '~/advisor-news', @Name='Advisor - News',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='advisor-news', @Title='News',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/newsArchiv.aspx', @Locale='pl', @Alias = '~/advisor-news', @Name='Advisor - News',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='advisor-news', @Title='Aktualności',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId
	
---------------------------------------------------------------------------------------------------------
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/faqs.aspx', @Locale='sk', @Alias = '~/casto-kladene-otazky', @Name='Často kladené otázky',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='casto-kladene-otazky', @Title='Často kladené otázky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/faqs.aspx', @Locale='cs', @Alias = '~/casto-kladene-otazky', @Name='Často kladené otázky',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='casto-kladene-otazky', @Title='Často kladené otázky',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/faqs.aspx', @Locale='en', @Alias = '~/faqs', @Name='Frequently Asked Question',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='faqs', @Title='Frequently Asked Question',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/faqs.aspx', @Locale='pl', @Alias = '~/faqs', @Name='Frequently Asked Question',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='faqs', @Title='Najczęściej zadawane pytania',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
			

----------------------------------------------------------------------------------------------------------------------------------------------------------
-- NAVIGACNE MENU VYROBKOV
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- TOP VYROBKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'TOP Výrobky'
SET @pageUrl = '~/eshop/products.aspx?id=top'
SET @pageAlias = '~/eshop/top-vyrobky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'TOP Products'
SET @pageUrl = '~/eshop/products.aspx?id=top'
SET @pageAlias = '~/eshop/top-products'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		

-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- AKCNE PONUKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Akčné ponuky'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-action-products'
SET @pageAlias = '~/eshop/akcne-ponuky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='eshop-action-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Akční nabídky'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-action-products'
SET @pageAlias = '~/eshop/akcni-nabidky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='eshop-action-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Action Products'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-action-products'
SET @pageAlias = '~/eshop/action-products'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='eshop-action-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='eshop-action-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SPECIALNE PONUKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Špeciálne ponuky'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-special-products'
SET @pageAlias = '~/eshop/specialne-ponuky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='eshop-special-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Speciální nabídky'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-special-products'
SET @pageAlias = '~/eshop/specialni-nabidky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='eshop-special-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Special Products'
SET @pageUrl = '~/eshop/page.aspx?name=eshop-special-products'
SET @pageAlias = '~/eshop/special-products'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='eshop-special-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @AdvisorMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='eshop-special-products', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @ProductsMasterPageId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- NOVINKY PONUKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Novinky'
SET @pageUrl = '~/eshop/products.aspx?id=news'
SET @pageAlias = '~/eshop/novinky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'New Products'
SET @pageUrl = '~/eshop/products.aspx?id=news'
SET @pageAlias = '~/eshop/news'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL

--================================================================================================================================
-- PAGES - KATALOG VYROBKOV
--================================================================================================================================
--================================================================================================================================
-- PAGES - VYROBKY
--================================================================================================================================
-- SK
SET @pageTitle = 'Výrobky'
SET @pageName = 'vyrobky'
SET @pageUrl = '~/eshop/default.aspx'
SET @pageAlias = '~/eshop/vyrobky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='eshop-products-master', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1201,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='eshop-products-master', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1202,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Products'
SET @pageName = 'products'
SET @pageUrl = '~/eshop/default.aspx'
SET @pageAlias = '~/eshop/products'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='eshop-products-master', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1203,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		

-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='eshop-products-master', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1204,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- VIRTUALNY KATALOG
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Virtuálny katalóg'
SET @pageName = 'virtualny-katalog'
SET @pageUrl = '~/eshop/catalog.aspx'
SET @pageAlias = '~/eshop/virtualny-katalog'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1201,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Virtuální katalog'
SET @pageName = 'virtualni-katalog'
SET @pageUrl = '~/eshop/catalog.aspx'
SET @pageAlias = '~/eshop/virtualni-katalog'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1202,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Products catalog'
SET @pageName = 'products-catalog'
SET @pageUrl = '~/eshop/catalog.aspx'
SET @pageAlias = '~/eshop/products-catalog'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1203,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1204,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- VASE PRILEZITOSTI
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Vaše príležitosti'
SET @pageName = 'vase-prilezitosti'
SET @pageUrl = '~/page3content.aspx?name=vase-prilezitosti'
SET @pageAlias = '~/vase-prilezitosti'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name=@pageName, @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1201,@Locale='sk', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Vaše příležitosti'
SET @pageName = 'vase-prilezitosti'
SET @pageUrl = '~/page3content.aspx?name=vase-prilezitosti'
SET @pageAlias = '~/vase-prilezitosti'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name=@pageName, @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1202,@Locale='cs', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Your Opportunities'
SET @pageName = 'vase-prilezitosti'
SET @pageUrl = '~/page3content.aspx?name=vase-prilezitosti'
SET @pageAlias = '~/your-opportunities'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name=@pageName, @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1203,@Locale='en', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
-- PL
SET @pageTitle = 'Swoje możliwości'
SET @pageName = 'vase-prilezitosti'
SET @pageUrl = '~/page3content.aspx?name=vase-prilezitosti'
SET @pageAlias = '~/swoje-mozliwosci'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name=@pageName, @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@MasterPageId = @MasterPage3ContentId, @SubPageCreateContents=1, @SubPageMasterPageId=@MasterPageId, @Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-1204,@Locale='pl', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
---------------------------------------------------------------------------------------------------------
-- SETUP ACCOUNT LOCALE
UPDATE tAccount SET Locale='cs', [Enabled]=1 WHERE AccountId = 1
---------------------------------------------------------------------------------------------------------
-- VOCABULARY
-- search
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='sk', @Translation='Hľadaj'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='cs', @Translation='Hledej'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='en', @Translation='Find'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='pl', @Translation='Find'

-- news
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='News', @Locale='sk', @Translation='Exkluzívne predstavujeme naše výrobky 2010'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='News', @Locale='cs', @Translation='Exkluzivně představujeme naše výrobky 2010'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='News', @Locale='en', @Translation='Exkluzivně představujeme naše výrobky 2010'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='News', @Locale='pl', @Translation='Exkluzivně představujeme naše výrobky 2010'

-- Banner infomartion
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='Banner', @Locale='sk', @Translation='Banner informace ...'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='Banner', @Locale='cs', @Translation='Banner informace ...'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='Banner', @Locale='en', @Translation='Banner informace ...'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='Banner', @Locale='pl', @Translation='Banner informace ...'

-- EShop-CurrencyChoice
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='sk', @Translation='Vyberťe menu'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='cs', @Translation='Vyberte měnu'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='en', @Translation='Choice currency'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='pl', @Translation='Choice currency'

-- Host Accesss
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='HostAccessPage', @Term='DefaultQuestion', @Locale='sk', @Translation='Predal Vám poradca heslo pre hosťa?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='HostAccessPage', @Term='DefaultQuestion', @Locale='cs', @Translation='Předal Vám poradce heslo pro hosta?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='HostAccessPage', @Term='DefaultQuestion', @Locale='en', @Translation='Předal Vám poradce heslo pro hosta?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='HostAccessPage', @Term='DefaultQuestion', @Locale='pl', @Translation='Předal Vám poradce heslo pro hosta?'

------------------------------------------------------------------------------------------------------------------------
-- sk
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/user/orders.aspx', @Locale='sk', @Alias = '~/eshop/moje-objednavky', @Name='Moje objednávky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx', @Locale='sk', @Alias = '~/eshop/nakupny-kosik', @Name='Nákupný košík'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx?step=2', @Locale='sk', @Alias = '~/eshop/nakupny-kosik-preprava', @Name='Nákupný košík - preprava'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx?step=3', @Locale='sk', @Alias = '~/eshop/nakupny-kosik-zakaznik', @Name='Nákupný košík - zákazník'

-- cs
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/user/orders.aspx', @Locale='cs', @Alias = '~/eshop/moje-objednavky', @Name='Moje objednávky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx', @Locale='cs', @Alias = '~/eshop/nakupni-kosik', @Name='Nákupní košík'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx?step=2', @Locale='cs', @Alias = '~/eshop/nakupni-kosik-preprava', @Name='Nákupní košík - přeprava'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/cart.aspx?step=3', @Locale='cs', @Alias = '~/eshop/nakupni-kosik-zakaznik', @Name='Nákupní košík - zákazník'
-- en
------------------------------------------------------------------------------------------------------------------------
-- SHP CATEGORY
/* -- Kategorie sa importuju
SET IDENTITY_INSERT tShpCategory ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1001', @Locale='cs', @Alias = '~/eshop/eurona', @Name='eurona', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (1001, @InstanceId, @UrlAliasId, 1, 'eurona', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1002', @Locale='cs', @Alias = '~/eshop/cerny-cosmetix-professional', @Name='cerny cosmetix professional', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (1002, @InstanceId, @UrlAliasId, 2, 'cerny cosmetix professional', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1003', @Locale='cs', @Alias = '~/eshop/cerny-cosmetix', @Name='cerny cosmetix professional', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (1003, @InstanceId, @UrlAliasId, 3, 'cerny cosmetix', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1004', @Locale='cs', @Alias = '~/eshop/intesa-for-live', @Name='intesa for live', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (1004, @InstanceId, @UrlAliasId, 4, 'intesa for live', 'C', 'cs');
SET IDENTITY_INSERT tShpCategory OFF
*/
------------------------------------------------------------------------------------------------------------------------
-- Shipment
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'DPD', '2', 'sk', NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'DPD', '2', 'cs' , NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'DPD', '2', 'en' , NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'DPD', '2', 'pl' , NULL, NULL )

INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'Osobný odber', '1', 'sk', NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'Osobní odběr', '1', 'cs' , NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'Personal collection', '1', 'en' , NULL, NULL )
INSERT INTO cShpShipment(InstanceId, Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, 'Odbiór osobisty', '1', 'pl' , NULL, NULL )

-- EURONA ADMINISTRATOR
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=@InstanceId,
	@Login = 'eurona', @Enabled = 1, @Password= '75CA60089F2B1E37C2CF13C360979576', -- @Password=0987oiuk
	@Roles = 'Administrator', @Verified = 1
	
UPDATE tAccount SET Locale='cs', [Enabled]=1 WHERE AccountId = 2	
GO

----======================================================================================================================
-- EOF Init
----======================================================================================================================
