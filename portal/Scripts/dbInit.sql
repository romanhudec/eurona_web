--======================================================================================================================
-- Init
--======================================================================================================================
--======================================================================================================================
-- UPDATE LOCALE CONSTRAINT
ALTER TABLE [tAccount] DROP CONSTRAINT [CK_tAccount_Locale]
GO
ALTER TABLE [tFaq] DROP CONSTRAINT [CK_tFaq_Locale]
GO
ALTER TABLE [tUrlAlias] DROP CONSTRAINT [CK_tUrlAlias_Locale]
GO
ALTER TABLE [tPage] DROP CONSTRAINT [CK_tPage_Locale]
GO
ALTER TABLE [tMenu] DROP CONSTRAINT [CK_tMenu_Locale]
GO
ALTER TABLE [tNavigationMenu] DROP CONSTRAINT [CK_tNavigationMenu_Locale]
GO
ALTER TABLE [tNavigationMenuItem] DROP CONSTRAINT [CK_tNavigationMenuItem_Locale]
GO
ALTER TABLE [tNews] DROP CONSTRAINT [CK_tNews_Locale]
GO
ALTER TABLE [tPoll] DROP CONSTRAINT [CK_tPoll_Locale]
GO
ALTER TABLE [tNewsletter] DROP CONSTRAINT [CK_tNewsletter_Locale]
GO
ALTER TABLE [tVocabulary] DROP CONSTRAINT [CK_tVocabulary_Locale]
GO
ALTER TABLE [tArticle] DROP CONSTRAINT [CK_tArticle_Locale]
GO
ALTER TABLE [tBlog] DROP CONSTRAINT [CK_tBlog_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [tShpAttribute] DROP CONSTRAINT [CK_tShpAttribute_Locale]
GO

--======================================================================================================================
DECLARE @InstanceId INT
SET @InstanceId = 0 /*spolocne entity*/

--======================================================================================================================
-- CMS INIT
--======================================================================================================================
-- default setup locale for account to czech
ALTER TABLE dbo.tAccount DROP CONSTRAINT DF_tAccount_Locale
ALTER TABLE dbo.tAccount ADD CONSTRAINT DF_tAccount_Locale DEFAULT ('cs') FOR Locale

SET IDENTITY_INSERT tRole ON
-- role <= -100 su role, ktore nie je mozne prostrednictvom UI odoberat.
INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -100, 'RegisteredUser', 'Registered user')
INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -1, 'Administrator', 'System administrator')
INSERT INTO tRole(InstanceId, RoleId, Name, Notes) VALUES(@InstanceId, -2, 'Newsletter', 'Information bulletin')
SET IDENTITY_INSERT tRole OFF

EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=@InstanceId,
	@Login = 'system', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', -- @Password=0987oiuk
	@Roles = 'Administrator', @Verified = 1

------------------------------------------------------------------------------------------------------------------------
-- INTT [tIPNF]
-- SLOVAK sk location
-- Type - 2 - Mobile
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (905)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (906)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (907)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (908)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (915)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId, 2, 'sk', '+421 (916)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (917)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (918)', 'Orange' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (919)', 'Orange' )

INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (901)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (902)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (903)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (904)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (910)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (911)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (912)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (914)', 'T-mobile' )

INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (940)', 'Telefónica O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (944)', 'Telefónica O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (948)', 'Telefónica O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'sk', '+421 (949)', 'Telefónica O2' )

-- CZECH cs location
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (601)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (602)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (606)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (607)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (720)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (721)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (722)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (723)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (724)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (725)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (726)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (727)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (728)', 'O2' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (729)', 'O2' )

INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (603)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (604)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (605)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (730)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (731)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (732)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (733)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (734)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (735)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (736)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (737)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (738)', 'T-mobile' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (739)', 'T-mobile' )

INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (608)', 'vodafone' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (774)', 'vodafone' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (775)', 'vodafone' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (776)', 'vodafone' )
INSERT INTO tIPNF ( InstanceId, Type, Locale, IPF, Notes ) VALUES ( @InstanceId,2, 'cs', '+420 (777)', 'vodafone' )

------------------------------------------------------------------------------------------------------------------------
-- URL Alis prefix
SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1, 'articles', 'clanky', 'sk', 'alias prefix for articles aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 11, 'articles', 'clanky', 'cs', 'alias prefix for articles aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 111, 'articles', 'articled', 'en', 'alias prefix for articles aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1111, 'articles', 'clanky', 'pl', 'alias prefix for articles aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 2, 'blogs', 'blogy', 'sk', 'alias prefix for blogs aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 22, 'blogs', 'blogy', 'cs', 'alias prefix for blogs aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 222, 'blogs', 'blogy', 'en', 'alias prefix for blogs aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 2222, 'blogs', 'blogy', 'pl', 'alias prefix for blogs aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 3, 'image-galleries', 'galerie-obrazkov', 'sk', 'alias prefix for image galleries aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 33, 'image-galleries', 'obrazkove-galerie', 'cs', 'alias prefix for image galleries aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 333, 'image-galleries', 'obrazkove-galerie', 'en', 'alias prefix for image galleries aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 3333, 'image-galleries', 'obrazkove-galerie', 'pl', 'alias prefix for image galleries aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 4, 'news', 'novinky', 'sk', 'alias prefix for news aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 44, 'news', 'novinky', 'cs', 'alias prefix for news aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 444, 'news', 'novinky', 'en', 'alias prefix for news aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 4444, 'news', 'novinky', 'pl', 'alias prefix for news aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF

--======================================================================================================================
-- EOF CMS INIT
--======================================================================================================================

--======================================================================================================================
-- ESHOP INIT
--======================================================================================================================
/*
case 0: mena = "CZK"; CS
		break;
case 1: mena = "USD"; EN
		break;
case 2: mena = "EUR"; SK
		break;
case 3: mena = "EUR"; DE
		break;
case 7: mena = "PLN"; PL
		break;
default: mena = "CZK";
*/
SET IDENTITY_INSERT cShpCurrency ON
INSERT INTO cShpCurrency (CurrencyId, InstanceId, Name ,Code ,Rate ,Symbol ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (0, @InstanceId, 'Česká koruna' ,'CZK' ,1 ,'Kč' ,NULL ,NULL ,'cs' ,GETDATE() ,NULL ,'C' ,1)

INSERT INTO cShpCurrency (CurrencyId, InstanceId, Name ,Code ,Rate ,Symbol ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (3, @InstanceId, 'Euro' ,'EUR' ,1 ,'€' ,NULL ,NULL ,'sk' ,GETDATE() ,NULL ,'C' , 1)

INSERT INTO cShpCurrency (CurrencyId, InstanceId, Name ,Code ,Rate ,Symbol ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (2, @InstanceId, 'Euro' ,'EUR' ,1 ,'€' ,NULL ,NULL ,'de' ,GETDATE() ,NULL ,'C' , 1)

INSERT INTO cShpCurrency (CurrencyId, InstanceId, Name ,Code ,Rate ,Symbol ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (7, @InstanceId, 'Zloty' ,'ZL' ,1 ,'zł' ,NULL ,NULL ,'pl' ,GETDATE() ,NULL ,'C' , 1)

INSERT INTO cShpCurrency (CurrencyId, InstanceId, Name ,Code ,Rate ,Symbol ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (1, @InstanceId, 'USD' ,'USD' ,1 ,'$' ,NULL ,NULL ,'en' ,GETDATE() ,NULL ,'C' , 1)
SET IDENTITY_INSERT cShpCurrency OFF
-- Classifiers
------------------------------------------------------------------------------------------------------------------------
-- Order Status
SET IDENTITY_INSERT cShpOrderStatus ON
-- default
--sk
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1, '-1', 'Čaká na spracovanie', 'sk', 'Objednávka čaká na spracovanie' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -2, '-2', 'Spracováva sa', 'sk', 'Objednávka je práve spracovávaná zodpovedným zamestnancom' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -3, '-3', 'Vybavená', 'sk', 'Objednávka je vybavená' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -4, '-4', 'Storno', 'sk', 'Objednávka je stornovaná' )

-- cs
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -101, '-1', 'Čeká na zpracování', 'cs', 'Objednávka čeká na zpracování' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -102, '-2', 'Zpracováva se', 'cs', 'Objednávka je právě zpracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -103, '-3', 'Vybavená', 'cs', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -104, '-4', 'Storno', 'cs', 'Objednávka je stornovaná' )

-- en
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1001, '-1', 'Waiting for proccess', 'en', 'Objednávka čeká na spracování' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1002, '-2', 'In progress', 'en', 'Objednávka je právě spracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1003, '-3', 'Success', 'en', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1004, '-4', 'Storno', 'en', 'Objednávka je stornovaná' )

-- pl
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -10001, '-1', 'Waiting for proccess', 'pl', 'Objednávka čeká na spracování' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -10002, '-2', 'In progress', 'pl', 'Objednávka je právě spracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -10003, '-3', 'Success', 'pl', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -10004, '-4', 'Storno', 'pl', 'Objednávka je stornovaná' )
SET IDENTITY_INSERT cShpOrderStatus OFF
------------------------------------------------------------------------------------------------------------------------
-- URL Alis prefix
SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1101, 'eshop', 'eshop', 'sk', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1102, 'eshop', 'eshop', 'cs', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1103, 'eshop', 'eshop', 'en', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1104, 'eshop', 'eshop', 'pl', 'alias prefix for eshop aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF

--======================================================================================================================
-- EOF ESHOP INIT
--======================================================================================================================

--======================================================================================================================
-- EOF Init
--======================================================================================================================
-- Upgrade db version
INSERT INTO tSysUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 1, GETDATE())
GO