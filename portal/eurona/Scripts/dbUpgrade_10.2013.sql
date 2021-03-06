------------------------------------------------------------------------------------------------------------------------
-- UPGRADE Eurona
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT, @ProductsFBMasterPageId INT
SET @MasterPageId = 1

DECLARE @UrlAliasId INT
DECLARE @PageId INT
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)

------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
-- Home banner	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1901, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/images/banner_homepage.png" /></p>', 'sk', 'home-banner', 'Úvodná stránka - banner', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1902, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/images/banner_homepage.png" /></p>', 'cs', 'home-banner', 'Úvodní stránka - banner', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1903, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/images/banner_homepage.png" /></p>', 'en', 'home-banner', 'Homepage - banner', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-1904, @MasterPageId, '<p style="text-align: center;"><img alt="" src="/images/banner_homepage.png" /></p>', 'pl', 'home-banner', 'Homepage - banner', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

GO
------------------------------------------------------------------------------------------------------------------------
--INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Eshop products form with share on Facebook', 'Default MasterPage for products with Share on FB Froms', '~/eshop/default.master', '~/eshop/pageFB.aspx?name=')
--SET @ProductsFBMasterPageId = SCOPE_IDENTITY()
------------------------------------------------------------------------------------------------------------------------