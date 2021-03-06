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
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- KARIERA s EURONOU
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Kariera s EURONOU'
SET @pageName = 'kariera-s-euronou'
SET @pageUrl = '~/eshop/kariera.aspx'
SET @pageAlias = '~/eshop/kariera-s-euronou'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name=@pageName, @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
		
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name=@pageName, @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT	

-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name=@pageName, @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
		
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name=@pageName, @Title=@pageTitle,
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT

---------------------------------------------------------------------------------------------------------
-- Anonymous BANNER content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1311, @MasterPageId, '', 'sk', 'anonymous-cart-banner-content', 'BANNER v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1312, @MasterPageId, '', 'cs', 'anonymous-cart-banner-content', 'BANNER v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1313, @MasterPageId, '', 'en', 'anonymous-cart-banner-content', 'BANNER v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1314, @MasterPageId, '', 'pl', 'anonymous-cart-banner-content', 'BANNER v košiku anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
---
---------------------------------------------------------------------------------------------------------
-- Anonymous BANNER2 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1321, @MasterPageId, '', 'sk', 'anonymous-cart-banner2-content', 'BANNER 2 v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1322, @MasterPageId, '', 'cs', 'anonymous-cart-banner2-content', 'BANNER 2 v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1323, @MasterPageId, '', 'en', 'anonymous-cart-banner2-content', 'BANNER 2 v košiku anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1324, @MasterPageId, '', 'pl', 'anonymous-cart-banner2-content', 'BANNER 2 v košiku anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

---------------------------------------------------------------------------------------------------------
-- Anonymous cart before BANNER1 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1331, @MasterPageId, '', 'sk', 'anonymous-cart-before-banner1-content', 'BANNER 1 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1332, @MasterPageId, '', 'cs', 'anonymous-cart-before-banner1-content', 'BANNER 1 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1333, @MasterPageId, '', 'en', 'anonymous-cart-before-banner1-content', 'BANNER 1 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1334, @MasterPageId, '', 'pl', 'anonymous-cart-before-banner1-content', 'BANNER 1 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous cart before BANNER2 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1341, @MasterPageId, '', 'sk', 'anonymous-cart-before-banner2-content', 'BANNER 2 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1342, @MasterPageId, '', 'cs', 'anonymous-cart-before-banner2-content', 'BANNER 2 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1343, @MasterPageId, '', 'en', 'anonymous-cart-before-banner2-content', 'BANNER 2 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1344, @MasterPageId, '', 'pl', 'anonymous-cart-before-banner2-content', 'BANNER 2 v košiku před přepočtem anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous cart after BANNER1 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1351, @MasterPageId, '', 'sk', 'anonymous-cart-after-banner1-content', 'BANNER 1 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1352, @MasterPageId, '', 'cs', 'anonymous-cart-after-banner1-content', 'BANNER 1 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1353, @MasterPageId, '', 'en', 'anonymous-cart-after-banner1-content', 'BANNER 1 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1354, @MasterPageId, '', 'pl', 'anonymous-cart-after-banner1-content', 'BANNER 1 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous cart after BANNER2 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1361, @MasterPageId, '', 'sk', 'anonymous-cart-after-banner2-content', 'BANNER 2 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1362, @MasterPageId, '', 'cs', 'anonymous-cart-after-banner2-content', 'BANNER 2 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1363, @MasterPageId, '', 'en', 'anonymous-cart-after-banner2-content', 'BANNER 2 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1364, @MasterPageId, '', 'pl', 'anonymous-cart-after-banner2-content', 'BANNER 2 v košiku po přepočtu anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous order finish BANNER1 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1371, @MasterPageId, '', 'sk', 'anonymous-order-finish-banner1-content', 'BANNER 1 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1372, @MasterPageId, '', 'cs', 'anonymous-order-finish-banner1-content', 'BANNER 1 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1373, @MasterPageId, '', 'en', 'anonymous-order-finish-banner1-content', 'BANNER 1 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1374, @MasterPageId, '', 'pl', 'anonymous-order-finish-banner1-content', 'BANNER 1 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous order finish BANNER2 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1381, @MasterPageId, '', 'sk', 'anonymous-order-finish-banner2-content', 'BANNER 2 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1382, @MasterPageId, '', 'cs', 'anonymous-order-finish-banner2-content', 'BANNER 2 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1383, @MasterPageId, '', 'en', 'anonymous-order-finish-banner2-content', 'BANNER 2 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1384, @MasterPageId, '', 'pl', 'anonymous-order-finish-banner2-content', 'BANNER 2 potvrzeni objednavky anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
-- Anonymous register BANNER1 content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1391, @MasterPageId, '', 'sk', 'anonymous-register-banner1-content', 'BANNER 1 registrace anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1392, @MasterPageId, '', 'cs', 'anonymous-register-banner1-content', 'BANNER 1 registrace anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1393, @MasterPageId, '', 'en', 'anonymous-register-banner1-content', 'BANNER 1 registrace anonymního poradce', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1394, @MasterPageId, '', 'pl', 'anonymous-register-banner1-content', 'BANNER 1 registrace anonymního poradce', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
---------------------------------------------------------------------------------------------------------
---
---------------------------------------------------------------------------------------------------------
-- Angel Team Profesional BANNER content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1411, @MasterPageId, '', 'sk', 'angel-team-profesional-banner-content', 'BANNER Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1412, @MasterPageId, '', 'cs', 'angel-team-profesional-banner-content', 'BANNER Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1413, @MasterPageId, '', 'en', 'angel-team-profesional-banner-content', 'BANNER Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1414, @MasterPageId, '', 'pl', 'angel-team-profesional-banner-content', 'BANNER Angel Team Profesional', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

---------------------------------------------------------------------------------------------------------
-- Angel Team Profesional Podminky content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1511, @MasterPageId, '', 'sk', 'angel-team-profesional-podminky-content', 'Podminky - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1512, @MasterPageId, '', 'cs', 'angel-team-profesional-podminky-content', 'Podminky - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1513, @MasterPageId, '', 'en', 'angel-team-profesional-podminky-content', 'Podminky - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1514, @MasterPageId, '', 'pl', 'angel-team-profesional-podminky-content', 'Podminky - Angel Team Profesional', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

---------------------------------------------------------------------------------------------------------
-- Angel Team Profesional Moje cest content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1611, @MasterPageId, 'Pro vstup a čerpání výhod V.I.P. Angel teamu je nutné splnit vstupní podmínky pro členství. Podrobné informace o podmínkách vstupu naleznete v <a href="/angel-team-professional/podminky">Podmínkách</a>.', 'sk', 'angel-team-profesional-mojecesta-content', 'Moje cesta - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1612, @MasterPageId, 'Pro vstup a čerpání výhod V.I.P. Angel teamu je nutné splnit vstupní podmínky pro členství. Podrobné informace o podmínkách vstupu naleznete v <a href="/angel-team-professional/podminky">Podmínkách</a>.', 'cs', 'angel-team-profesional-mojecesta-content', 'Moje cesta - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1613, @MasterPageId, 'Pro vstup a čerpání výhod V.I.P. Angel teamu je nutné splnit vstupní podmínky pro členství. Podrobné informace o podmínkách vstupu naleznete v <a href="/angel-team-professional/podminky">Podmínkách</a>.', 'en', 'angel-team-profesional-mojecesta-content', 'Moje cesta - Angel Team Profesional', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1614, @MasterPageId, 'Pro vstup a čerpání výhod V.I.P. Angel teamu je nutné splnit vstupní podmínky pro členství. Podrobné informace o podmínkách vstupu naleznete v <a href="/angel-team-professional/podminky">Podmínkách</a>.', 'pl', 'angel-team-profesional-mojecesta-content', 'Moje cesta - Angel Team Profesional', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF
------------------------------------------------------------------------------------------------------------------------
-- ALIASY - Zjednodusena registrace
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/default.aspx', @Locale='sk', @Alias = '~/eshop/prvni-objednavka-muj-kosik', @Name='prvni-objednavka-muj-kosik'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/default.aspx', @Locale='cs', @Alias = '~/eshop/prvni-objednavka-muj-kosik', @Name='prvni-objednavka-muj-kosik'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/default.aspx', @Locale='en', @Alias = '~/eshop/prvni-objednavka-muj-kosik', @Name='prvni-objednavka-muj-kosik'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/default.aspx', @Locale='pl', @Alias = '~/eshop/prvni-objednavka-muj-kosik', @Name='prvni-objednavka-muj-kosik'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/register.aspx', @Locale='sk', @Alias = '~/eshop/prvni-objednavka-vyplneni-udaju', @Name='prvni-objednavka-vyplneni-udaju'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/register.aspx', @Locale='cs', @Alias = '~/eshop/prvni-objednavka-vyplneni-udaju', @Name='prvni-objednavka-vyplneni-udaju'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/register.aspx', @Locale='en', @Alias = '~/eshop/prvni-objednavka-vyplneni-udaju', @Name='prvni-objednavka-vyplneni-udaju'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/register.aspx', @Locale='pl', @Alias = '~/eshop/prvni-objednavka-vyplneni-udaju', @Name='prvni-objednavka-vyplneni-udaju'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/cart.aspx', @Locale='sk', @Alias = '~/eshop/prvni-objednavka-pred-prepoctem', @Name='prvni-objednavka-pred-prepoctem'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/cart.aspx', @Locale='cs', @Alias = '~/eshop/prvni-objednavka-pred-prepoctem', @Name='prvni-objednavka-pred-prepoctem'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/cart.aspx', @Locale='en', @Alias = '~/eshop/prvni-objednavka-pred-prepoctem', @Name='prvni-objednavka-pred-prepoctem'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/cart.aspx', @Locale='pl', @Alias = '~/eshop/prvni-objednavka-pred-prepoctem', @Name='prvni-objednavka-pred-prepoctem'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/order.aspx', @Locale='sk', @Alias = '~/eshop/prvni-objednavka-po-prepoctu', @Name='prvni-objednavka-po-prepoctu'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/order.aspx', @Locale='cs', @Alias = '~/eshop/prvni-objednavka-po-prepoctu', @Name='prvni-objednavka-po-prepoctu'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/order.aspx', @Locale='en', @Alias = '~/eshop/prvni-objednavka-po-prepoctu', @Name='prvni-objednavka-po-prepoctu'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/order.aspx', @Locale='pl', @Alias = '~/eshop/prvni-objednavka-po-prepoctu', @Name='prvni-objednavka-po-prepoctu'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/finish.aspx', @Locale='sk', @Alias = '~/eshop/potvrdenie-objednavky', @Name='potvrzeni-objednavky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/finish.aspx', @Locale='cs', @Alias = '~/eshop/potvrzeni-objednavky', @Name='potvrzeni-objednavky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/finish.aspx', @Locale='en', @Alias = '~/eshop/potvrzeni-objednavky', @Name='potvrzeni-objednavky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/anonymous/finish.aspx', @Locale='pl', @Alias = '~/eshop/potvrzeni-objednavky', @Name='potvrzeni-objednavky'

-- ALIASY - Angel Team Profesionals
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/default.aspx', @Locale='sk', @Alias = '~/angel-team-professional/podminky', @Name='angel-team-professional-podminky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/default.aspx', @Locale='cs', @Alias = '~/angel-team-professional/podminky', @Name='angel-team-professional-podminky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/default.aspx', @Locale='en', @Alias = '~/angel-team-professional/podminky', @Name='angel-team-professional-podminky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/default.aspx', @Locale='pl', @Alias = '~/angel-team-professional/podminky', @Name='angel-team-professional-podminky'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/vip.aspx', @Locale='sk', @Alias = '~/angel-team-professional/vip-zone', @Name='angel-team-professional-vip-zone'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/vip.aspx', @Locale='cs', @Alias = '~/angel-team-professional/vip-zone', @Name='angel-team-professional-vip-zone'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/vip.aspx', @Locale='en', @Alias = '~/angel-team-professional/vip-zone', @Name='angel-team-professional-vip-zone'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/vip.aspx', @Locale='pl', @Alias = '~/angel-team-professional/vip-zone', @Name='angel-team-professional-vip-zone'

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/mojecesta.aspx', @Locale='sk', @Alias = '~/angel-team-professional/moja-cesta', @Name='angel-team-professional-moje-cesta'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/mojecesta.aspx', @Locale='cs', @Alias = '~/angel-team-professional/moje-cesta', @Name='angel-team-professional-moje-cesta'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/mojecesta.aspx', @Locale='en', @Alias = '~/angel-team-professional/my-way', @Name='angel-team-professional-moje-cesta'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/advisor/angel-team/mojecesta.aspx', @Locale='pl', @Alias = '~/angel-team-professional/my-way', @Name='angel-team-professional-moje-cesta'
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pOrganizationDelete
	@HistoryAccount INT,
	@OrganizationId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tOrganization WHERE OrganizationId = @OrganizationId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrganizationId %d', 16, 1, @OrganizationId);

	BEGIN TRANSACTION;

	BEGIN TRY

		-- mark registered address as deleted
		UPDATE a 
		SET
			a.HistoryStamp = GETDATE(), a.HistoryType = 'D', a.HistoryAccount = @HistoryAccount, a.HistoryId = a.AddressId
		FROM tAddress a
		INNER JOIN tOrganization o (NOLOCK) ON o.RegisteredAddress = a.AddressId
		WHERE o.OrganizationId = @OrganizationId

		-- mark correspondence address as deleted
		UPDATE a 
		SET
			a.HistoryStamp = GETDATE(), a.HistoryType = 'D', a.HistoryAccount = @HistoryAccount, a.HistoryId = a.AddressId
		FROM tAddress a
		INNER JOIN tOrganization o (NOLOCK) ON o.CorrespondenceAddress = a.AddressId
		WHERE o.OrganizationId = @OrganizationId
		
		-- mark invoicing address as deleted
		UPDATE a 
		SET
			a.HistoryStamp = GETDATE(), a.HistoryType = 'D', a.HistoryAccount = @HistoryAccount, a.HistoryId = a.AddressId
		FROM tAddress a
		INNER JOIN tOrganization o (NOLOCK) ON o.InvoicingAddress = a.AddressId
		WHERE o.OrganizationId = @OrganizationId	
		
		-- mark bank contact as deleted
		UPDATE b 
		SET
			b.HistoryStamp = GETDATE(), b.HistoryType = 'D', b.HistoryAccount = @HistoryAccount, b.HistoryId = b.BankContactId
		FROM tBankContact b
		INNER JOIN tOrganization o (NOLOCK) ON o.BankContact = b.BankContactId
		WHERE o.OrganizationId = @OrganizationId			

		-- mark contact person as deleted
		UPDATE p
		SET
			p.HistoryStamp = GETDATE(), p.HistoryType = 'D', p.HistoryAccount = @HistoryAccount, p.HistoryId = p.PersonId
		FROM tPerson p
		INNER JOIN tOrganization o (NOLOCK) ON o.ContactPerson = p.PersonId
		WHERE o.OrganizationId = @OrganizationId
	
		-- mark organization as deleted
		UPDATE tOrganization 
		SET
			HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @OrganizationId
		WHERE OrganizationId = @OrganizationId

		SET @Result = @OrganizationId

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
ALTER TABLE tOrganization ADD  AnonymousRegistration BIT NULL DEFAULT(0),  AnonymousAssignBy INT NULL, AnonymousAssignAt DATETIME NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pOrganizationModify
	@HistoryAccount INT,
	@OrganizationId INT,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
	@ParentId INT = NULL,
	@Code NVARCHAR(100) = NULL,
	@VATPayment BIT = 0,
	@TopManager INT = 0,
	@FAX NVARCHAR(100) = NULL, 
	@Skype NVARCHAR(100) = NULL, 
	@ICQ NVARCHAR(100) = NULL, 
	@ContactBirthDay DATETIME = NULL, 
	@ContactCardId NVARCHAR(100) = NULL, 
	@ContactWorkPhone NVARCHAR(100) = NULL, 
	@PF CHAR(1) = NULL, 
	@RegionCode NVARCHAR(100) = NULL,
	@UserMargin DECIMAL(19,2) = NULL,
	@Statut NVARCHAR(10) = NULL,
	@SelectedCount INT = 0,
	@AnonymousRegistration BIT = 0,
	@AnonymousAssignBy INT = NULL,
	@AnonymousAssignAt DATETIME = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tOrganization WHERE OrganizationId = @OrganizationId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid OrganizationId %d', 16, 1, @OrganizationId)
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
	
		INSERT INTO tOrganization (
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,
			HistoryStamp, HistoryType, HistoryAccount, @OrganizationId
		FROM tOrganization
		WHERE OrganizationId = @OrganizationId

		UPDATE tOrganization 
		SET
			Id1 = @Id1, Id2 = @Id2, Id3 = @Id3, Name = @Name, Notes = @Notes, Web = @Web, 
			ContactEMail = @ContactEMail, ContactPhone = @ContactPhone, ContactMobile = @ContactMobile, 
			ParentId=@ParentId, Code=@Code, VATPayment=@VATPayment, TopManager=@TopManager,
			FAX=@FAX, Skype=@Skype, ICQ=@ICQ, ContactBirthDay=@ContactBirthDay, ContactCardId=@ContactCardId, ContactWorkPhone=@ContactWorkPhone, PF=@PF, RegionCode=@RegionCode, UserMargin=@UserMargin,Statut=@Statut, SelectedCount=@SelectedCount,
			AnonymousRegistration=@AnonymousRegistration, AnonymousAssignBy=@AnonymousAssignBy, AnonymousAssignAt=@AnonymousAssignAt,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrganizationId = @OrganizationId

		SET @Result = @OrganizationId

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
ALTER PROCEDURE pOrganizationCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT = NULL,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
	@ParentId INT = NULL,
	@Code NVARCHAR(100) = NULL,
	@VATPayment BIT = 0,
	@TopManager INT = 0,
	@FAX NVARCHAR(100) = NULL, 
	@Skype NVARCHAR(100) = NULL, 
	@ICQ NVARCHAR(100) = NULL, 
	@ContactBirthDay DATETIME = NULL, 
	@ContactCardId NVARCHAR(100) = NULL, 
	@ContactWorkPhone NVARCHAR(100) = NULL, 
	@PF CHAR(1) = NULL, 
	@RegionCode NVARCHAR(100) = NULL,
	@UserMargin DECIMAL(19,2) = NULL,
	@Statut NVARCHAR(10) = NULL,
	@SelectedCount INT = 0,
	@AnonymousRegistration BIT = 0,
	@AnonymousAssignBy INT = NULL,
	@AnonymousAssignAt DATETIME = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @RegisteredAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @RegisteredAddressId OUTPUT

		DECLARE @CorrespondenceAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @CorrespondenceAddressId OUTPUT
		
		DECLARE @InvoicingAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @InvoicingAddressId OUTPUT

		DECLARE @BankContactId INT
		EXEC pBankContactCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @BankContactId OUTPUT

		DECLARE @ContactPersonId INT
		EXEC pPersonCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @ContactPersonId OUTPUT

		INSERT INTO tOrganization (
			InstanceId, AccountId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			@ParentId, @Code, @VATPayment, @TopManager,
			@FAX, @Skype, @ICQ, @ContactBirthDay, @ContactCardId, @ContactWorkPhone, @PF, @RegionCode, @UserMargin, @Statut, @SelectedCount,
			@AnonymousRegistration, @AnonymousAssignBy, @AnonymousAssignAt,
			GETDATE(), 'C', @HistoryAccount)
			
		SET @Result = SCOPE_IDENTITY()

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

	SELECT OrganizationId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
	AccountId = o.AccountId, a.TVD_Id, a.Verified,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact,
	o.ParentId, o.Code, o.VATPayment, o.TopManager,
	o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin,  RestrictedAccess = ISNULL(o.RestrictedAccess, 0), o.Statut,
	Created = ( SELECT MIN(HistoryStamp) FROM tAccount WHERE ( AccountId=a.AccountId OR HistoryId=a.AccountId )  ),
	SelectedCount,
	AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

--SELECT * FROM vOrganizations

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pEuronaFindAdvisorForHost]
	@InstanceId INT = NULL,
	@Name NVARCHAR(100) = NULL,
	@City NVARCHAR(100) = NULL,
	@Region NVARCHAR(100) = NULL
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OrganizationId INT
	SELECT TOP 1 @OrganizationId=o.OrganizationId 
	FROM vOrganizations o
		LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
	WHERE 
		o.InstanceId=@InstanceId AND
		o.TopManager=1 AND
		( @Name IS NULL OR o.Name LIKE '%'+ @Name +'%') AND 
		( @City IS NULL OR ra.City LIKE @City +'%') AND 
		( @Region IS NULL OR o.RegionCode=@Region ) 
	ORDER BY o.SelectedCount ASC

	IF @OrganizationId IS NULL
	BEGIN
		SELECT TOP 1 @OrganizationId=op.OrganizationId 
		FROM vOrganizations o
			INNER JOIN vOrganizations op (NOLOCK) ON op.TVD_Id = o.ParentId
			LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
		WHERE 
			o.InstanceId=@InstanceId AND
			op.TopManager=1 AND
			( @Name IS NULL OR o.Name LIKE '%'+ @Name +'%') AND 
			( @City IS NULL OR ra.City LIKE @City +'%') AND 
			( @Region IS NULL OR o.RegionCode=@Region ) 
		ORDER BY op.SelectedCount ASC

	END

	-- RESULT
	SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
			o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
			o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
			RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
			CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
			InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
			BankContactId = o.BankContactId,
			ContactPersonId = o.ContactPersonId, 
			ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
			ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
			o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
			o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
			o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt
	FROM vOrganizations o
	LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
	LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
	LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
	LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
	WHERE OrganizationId=@OrganizationId AND @OrganizationId IS NOT NULL
	ORDER BY o.Name ASC
END
GO
------------------------------------------------------------------------------------------------------------------------
-- tAnonymniRegistrace
CREATE TABLE [tAnonymniRegistrace](
	[AnonymniRegistraceId] [int] NOT NULL,
	[ZobrazitVSeznamuNeomezene] [bit] NULL CONSTRAINT [DF_tAnonymniRegistrace_ZobrazitVSeznamuNeomezene]  DEFAULT (0),
	[ZobrazitVSeznamuDni] [int] NULL,
	[ZobrazitVSeznamuHodin] [int] NULL,
	[ZobrazitVSeznamuMinut] [int] NULL
)
GO
------------------------------------------------------------------------------------------------------------------------
INSERT INTO tAnonymniRegistrace (AnonymniRegistraceId) VALUES (-1001)
INSERT INTO tAnonymniRegistrace (AnonymniRegistraceId) VALUES (-3001)
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tOrganization ADD  Angel_team_clen BIT NOT NULL DEFAULT(0),  Angel_team_manager BIT NOT NULL DEFAULT(0)
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
	AccountId = o.AccountId, a.TVD_Id, a.Verified,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact,
	o.ParentId, o.Code, o.VATPayment, o.TopManager,
	o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin,  RestrictedAccess = ISNULL(o.RestrictedAccess, 0), o.Statut,
	Created = ( SELECT MIN(HistoryStamp) FROM tAccount WHERE ( AccountId=a.AccountId OR HistoryId=a.AccountId )  ),
	SelectedCount,
	AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,
	Angel_team_clen, Angel_team_manager
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

------------------------------------------------------------------------------------------------------------------------
-- tAnonymniRegistrace
CREATE TABLE [tAngelTeam](
	[AngelTeamId] [int] NOT NULL,
	[PocetEuronaStarProVstup] [int] NOT NULL DEFAULT(0), 
	[PocetEuronaStarProUdrzeni] [int] NOT NULL DEFAULT(0)
)
GO
------------------------------------------------------------------------------------------------------------------------
INSERT INTO [tAngelTeam] (AngelTeamId) VALUES (-1001)
INSERT INTO [tAngelTeam] (AngelTeamId) VALUES (-3001)
GO

UPDATE cShpShipment SET Name='Osobní odběr v sídle společnosti' where InstanceId=1 and ShipmentId=6
UPDATE cShpShipment SET Name='Osobný odber v sídle spoločnosti' where InstanceId=1 and ShipmentId=5
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
--INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Eshop products form with share on Facebook', 'Default MasterPage for products with Share on FB Froms', '~/eshop/default.master', '~/eshop/pageFB.aspx?name=')
--SET @ProductsFBMasterPageId = SCOPE_IDENTITY()
------------------------------------------------------------------------------------------------------------------------