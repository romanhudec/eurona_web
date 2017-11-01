------------------------------------------------------------------------------------------------------------------------
-- UPGRADE CMS version 0.0 to 0.1
------------------------------------------------------------------------------------------------------------------------
-- Upgrade

CREATE TABLE [tSysUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tSysUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO

------------------------------------------------------------------------------------------------------------------------
-- IPNF
CREATE TABLE [dbo].[tIPNF](
	[IPNFId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Locale] [char](2) NOT NULL,
	[IPF] [nvarchar](100) NOT NULL,
	[Notes] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_IPNFId] PRIMARY KEY CLUSTERED ([IPNFId] ASC)
)
GO

CREATE VIEW vIPNFs AS SELECT A=1
GO

ALTER VIEW vIPNFs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[IPNFId], [Type], [Locale], [IPF], [Notes]
FROM tIPNF
GO
------------------------------------------------------------------------------------------------------------------------
-- INTT [tIPNF]
-- SLOVAK sk location
-- Type - 2 - Mobile
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (905)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (906)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (907)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (908)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (915)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (916)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (917)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (918)', 'Orange' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (919)', 'Orange' )

INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (901)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (902)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (903)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (904)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (910)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (911)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (912)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (914)', 'T-mobile' )

INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (940)', 'Telefónica O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (944)', 'Telefónica O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (948)', 'Telefónica O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'sk', '+421 (949)', 'Telefónica O2' )

-- CZECH cs location
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (601)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (602)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (606)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (607)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (720)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (721)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (722)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (723)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (724)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (725)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (726)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (727)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (728)', 'O2' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (729)', 'O2' )

INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (603)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (604)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (605)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (730)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (731)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (732)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (733)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (734)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (735)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (736)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (737)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (738)', 'T-mobile' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (739)', 'T-mobile' )

INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (608)', 'vodafone' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (774)', 'vodafone' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (775)', 'vodafone' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (776)', 'vodafone' )
INSERT INTO tIPNF ( Type, Locale, IPF, Notes ) VALUES ( 2, 'cs', '+420 (777)', 'vodafone' )
GO
