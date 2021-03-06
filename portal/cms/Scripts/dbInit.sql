------------------------------------------------------------------------------------------------------------------------
-- Init
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1
------------------------------------------------------------------------------------------------------------------------
-- default account & credentials

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
VALUES ( @InstanceId, 1001, 'articles', 'articles', 'en', 'alias prefix for articles aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 2, 'blogs', 'blogy', 'sk', 'alias prefix for blogs aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1002, 'blogs', 'blogs', 'en', 'alias prefix for blogs aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 3, 'image-galleries', 'galerie-obrazkov', 'sk', 'alias prefix for image galleries aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1003, 'image-galleries', 'image-galleries', 'en', 'alias prefix for image galleries aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 4, 'news', 'novinky', 'sk', 'alias prefix for news aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1004, 'news', 'news', 'en', 'alias prefix for news aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 5, 'forum', 'forum', 'sk', 'alias prefix for forum aliases' )
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1005, 'forum', 'forum', 'en', 'alias prefix for forum aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF
GO

-- EOF Init
------------------------------------------------------------------------------------------------------------------------
