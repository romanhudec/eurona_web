------------------------------------------------------------------------------------------------------------------------
-- Init
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

-- Classifiers
------------------------------------------------------------------------------------------------------------------------
-- Order Status
SET IDENTITY_INSERT cShpOrderStatus ON
-- default
--sk
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -1, -1, 'Čaká na spracovanie', 'sk', 'Objednávka čaká na spracovanie' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -2, -2, 'Spracováva sa', 'sk', 'Objednávka je práve spracovávaná zodpovedným zamestnancom' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -3, -3, 'Vybavená', 'sk', 'Objednávka je vybavená' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -4, -4, 'Storno', 'sk', 'Objednávka je stornovaná' )

-- cs
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -101, -1, 'Čeká na spracování', 'cs', 'Objednávka čeká na spracování' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -102, -2, 'Spracováva se', 'cs', 'Objednávka je právě spracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -103, -3, 'Vybavená', 'cs', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, CodeId, [Name], Locale, Notes ) VALUES ( @InstanceId, -104, -4, 'Storno', 'cs', 'Objednávka je stornovaná' )

SET IDENTITY_INSERT cShpOrderStatus OFF
------------------------------------------------------------------------------------------------------------------------
-- URL Alis prefix
SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1001, 'eshop', 'eshop', 'sk', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1002, 'eshop', 'eshop', 'cs', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1003, 'eshop', 'eshop', 'en', 'alias prefix for eshop aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF

/*
------------------------------------------------------------------------------------------------------------------------
-- Home content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
DECLARE @MasterPageId INT
SELECT TOP 1 @MasterPageId = MasterPageId FROM tMasterPage

SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId, PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -2001, @MasterPageId, '<h4>Elektornický obchod</h4>', 'sk', 'eshop-home-content', 'Úvodná stránka', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

------------------------------------------------------------------------------------------------------------------------
*/
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
-- EOF Init
------------------------------------------------------------------------------------------------------------------------
-- Upgrade db version
INSERT INTO tShpUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 2, GETDATE())
GO