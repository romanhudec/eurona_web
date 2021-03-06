------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tAccount ADD TVD_Id INT NULL, CanAccessIntensa BIT NULL, CanAccessEurona BIT NULL, Roles NVARCHAR(1000) NULL, MustChangeAccount BIT NOT NULL DEFAULT(0)
GO

ALTER TABLE tShpProduct ADD Body DECIMAL(19,2) NULL, Parfumacia INT NULL, VAT DECIMAL(19,2 ) NULL, 
[Novinka] BIT NULL, [Inovace] BIT NULL,[Doprodej] BIT NULL,[Vyprodano] BIT NULL,[ProdejUkoncen] BIT NULL, [Top] INT NULL,
[MaximalniPocetVBaleni] INT NULL, [Megasleva] BIT NULL, [Supercena] BIT NULL, [CLHit] BIT NULL, [Action] BIT NULL, [Vyprodej] BIT NULL
GO

ALTER TABLE tShpCartProduct ADD CurrencyId INT NULL
GO
ALTER TABLE tShpCartProduct ADD CerpatBK BIT NULL
GO
ALTER TABLE tShpCartProduct ADD POrder INT NOT NULL DEFAULT(0)
GO

ALTER TABLE tShpOrder ADD CurrencyId INT NULL, 
ParentId INT NULL/*Parent objednavka*/, 
AssociationAccountId INT NULL/*Pridruzienie tejto objednavky k objednavke pouzivatela*/, 
AssociationRequestStatus INT NULL, /*Status poziadavky na pridruzenie*/
CreatedByAccountId INT NULL, /*Pouzivatel, ktory objednavku vytvoril*/
ShipmentPrice DECIMAL(19,2) NULL,
ShipmentPriceWVAT DECIMAL(19,2) NULL,
ShipmentFrom DATETIME NULL,
ShipmentTo DATETIME NULL,
NoPostage BIT NULL/*Bez postovneho*/
GO

ALTER TABLE tShpProduct DROP CONSTRAINT [DF_tShpProduct_Locale]
GO
ALTER TABLE tShpProduct DROP CONSTRAINT [CK_tShpProduct_Locale]
GO
ALTER TABLE tShpProduct DROP COLUMN [Locale]
GO
ALTER TABLE tShpProduct DROP COLUMN [Name]
GO
ALTER TABLE tShpProduct DROP COLUMN [Description]
GO
ALTER TABLE tShpProduct DROP COLUMN [DescriptionLong]
GO
ALTER TABLE [tShpProduct] DROP CONSTRAINT [FK_tShpProduct_cShpVAT]
GO
ALTER TABLE tShpProduct DROP COLUMN [VATId]
GO
ALTER TABLE [tShpProduct] DROP CONSTRAINT [FK_tShpProduct_tUrlAlias]
GO
ALTER TABLE tShpProduct DROP COLUMN [UrlAliasId]
GO
ALTER TABLE tShpProduct DROP COLUMN Price
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpCart ADD Price [DECIMAL](19,2) NULL, PriceWVAT [DECIMAL](19,2) NULL, [Discount][DECIMAL](19,2) NULL, [Status] INT NULL
GO

ALTER TABLE tOrganization ADD ParentId INT NULL, Code NVARCHAR(100) NULL, VATPayment BIT NULL, TopManager INT NULL,
FAX NVARCHAR(100) NULL, Skype NVARCHAR(100) NULL, ICQ NVARCHAR(100) NULL, ContactBirthDay DATETIME NULL, ContactCardId NVARCHAR(100) NULL, ContactWorkPhone NVARCHAR(100) NULL, PF CHAR(1) NULL, 
RegionCode NVARCHAR(100) NULL, UserMargin DECIMAL(19,2) NULL, RestrictedAccess INT NULL, Statut NVARCHAR(10) NULL, SelectedCount INT NULL,
AnonymousRegistration BIT NULL DEFAULT(0), 
AnonymousAssignBy INT NULL,
AnonymousAssignAt DATETIME NULL
GO
------------------------------------------------------------------------------------------------------------------------
-- 
ALTER TABLE [tShpCategory] DROP COLUMN [Name]
GO
ALTER TABLE [tShpCategory] DROP CONSTRAINT [DF_tShpCategory_Locale]
GO
ALTER TABLE [tShpCategory] DROP CONSTRAINT [CK_tShpCategory_Locale]
GO
ALTER TABLE [tShpCategory] DROP COLUMN [Locale]
GO
ALTER TABLE [tShpCategory] DROP CONSTRAINT [FK_tShpCategory_tUrlAlias]
GO
ALTER TABLE [tShpCategory] DROP COLUMN [UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Vlastnosti
CREATE TABLE [tShpVlastnostiProduktu](
	[ProductId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpVlastnostiProduktu_Locale]  DEFAULT ('cs'),
	[Name] [nvarchar](2000) NULL,
	[ImageUrl] [nvarchar](255) NULL
)
GO

ALTER TABLE [tShpVlastnostiProduktu]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpVlastnostiProduktu_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpVlastnostiProduktu] CHECK CONSTRAINT [FK_tShpVlastnostiProduktu_ProductId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Piktogramy
CREATE TABLE [tShpPiktogramyProduktu](
	[ProductId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpPiktogramyProduktu_Locale]  DEFAULT ('cs'),
	[Name] [nvarchar](2000) NULL,
	[ImageUrl] [nvarchar](255) NULL
)
GO

ALTER TABLE [tShpPiktogramyProduktu]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpPiktogramyProduktu_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpPiktogramyProduktu] CHECK CONSTRAINT [FK_tShpPiktogramyProduktu_ProductId]
GO

------------------------------------------------------------------------------------------------------------------------
-- Specialne ucinky
CREATE TABLE [tShpUcinkyProduktu](
	[ProductId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpUcinkyProduktu_Locale]  DEFAULT ('cs'),
	[Name] [nvarchar](2000) NULL,
	[ImageUrl] [nvarchar](255) NULL
)
GO

ALTER TABLE [tShpUcinkyProduktu]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpUcinkyProduktu_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpUcinkyProduktu] CHECK CONSTRAINT [FK_tShpUcinkyProduktu_ProductId]
GO

------------------------------------------------------------------------------------------------------------------------
-- Ceny
CREATE TABLE [tShpCenyProduktu](
	[ProductId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpCenyProduktu_Locale]  DEFAULT ('cs'),
	[CurrencyId] INT NULL,
	[Body] INT NULL,
	[Cena] [decimal](19,2) NULL,
	[BeznaCena] [decimal](19,2) NULL,
	[MarzePovolena] [bit] NULL, -- zda je povoleno odečítání marže z ceny produktu
	[MarzePovolenaMinimalni] [bit] NULL, -- v případě povoleného odečítání marže a nároku na marži 25% nebo 30% se uplatní jen 20% (momentálně Eurona nemá u žádného produktu nastaveno)
	[CenaBK] [decimal](19,2) NULL
)
GO

ALTER TABLE [tShpCenyProduktu]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCenyProduktu_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpCenyProduktu] CHECK CONSTRAINT [FK_tShpCenyProduktu_ProductId]
GO

------------------------------------------------------------------------------------------------------------------------
-- Lokalizacia produktu
CREATE TABLE [tShpProductLocalization](
	[ProductId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpProductLocalization_Locale]  DEFAULT ('cs'),
	[Name] [nvarchar](1000) NULL,
	[Description] [nvarchar](1000) NULL,
	[DescriptionLong] [nvarchar](2000) NULL,
	[AdditionalInformation] [nvarchar](2000) NULL,
	[InstructionsForUse] [nvarchar](2000) NULL,
	[UrlAliasId] INT NULL
)
GO

ALTER TABLE tShpProductLocalization  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProductLocalization_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE tShpProductLocalization CHECK CONSTRAINT [FK_tShpProductLocalization_ProductId]
GO

ALTER TABLE tShpProductLocalization  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProductLocalization_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE tShpProductLocalization CHECK CONSTRAINT [FK_tShpProductLocalization_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
-- Lokalizacia kategorie
CREATE TABLE [tShpCategoryLocalization](
	[CategoryId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpCategoryLocalization_Locale]  DEFAULT ('cs'),
	[Name] [nvarchar](1000) NULL,
	[UrlAliasId] INT NULL
)
GO

ALTER TABLE tShpCategoryLocalization  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCategoryLocalization_CategoryId] FOREIGN KEY([CategoryId])
	REFERENCES [tShpCategory] ([CategoryId])
GO
ALTER TABLE tShpCategoryLocalization CHECK CONSTRAINT [FK_tShpCategoryLocalization_CategoryId]
GO

ALTER TABLE tShpCategoryLocalization  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCategoryLocalization_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE tShpCategoryLocalization CHECK CONSTRAINT [FK_tShpCategoryLocalization_tUrlAlias]
GO
------------------------------------------------------------------------------------------------------------------------
-- Uzavierka
CREATE TABLE [tShpUzavierka](
	[UzavierkaId] [int] NOT NULL,
	[Povolena] [bit] NULL CONSTRAINT [DF_tShpUzavierka_Povolena]  DEFAULT (0),
	[UzavierkaOd] [datetime] NULL,
	[UzavierkaDo] [datetime] NULL,
	[OperatorOrderOd] [datetime] NULL,
	[OperatorOrderDo] [datetime] NULL,
	[OperatorOrderDate] [datetime] NULL
)
GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tShpOrderCounter](
	[CounterId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] INT NOT NULL,
	[Year] INT NOT NULL,
	[Month] INT NOT NULL,
	[Counter] INT NOT NULL,
CONSTRAINT [PK_tShpOrderCounter] PRIMARY KEY CLUSTERED ([CounterId] ASC)
)
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
-- tAngelTeam
CREATE TABLE [tAngelTeam](
	[AngelTeamId] [int] NOT NULL,
	[PocetEuronaStarProVstup] [int] NOT NULL DEFAULT(0), 
	[PocetEuronaStarProUdrzeni] [int] NOT NULL DEFAULT(0)
)
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------
