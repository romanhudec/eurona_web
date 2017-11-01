
			USE carinzert
			GO
		
------------------------------------------------------------------------------------------------------------------------
-- CMS version 0.5
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------
-- Address

CREATE TABLE [tAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[City] [nvarchar](100) NULL,
	[Street] [nvarchar](200) NULL,
	[Zip] [nvarchar](30) NULL,
	[District] [nvarchar](100) NULL,
	[Region] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_tAddress] PRIMARY KEY CLUSTERED ([AddressId] ASC)
)
GO

ALTER TABLE [tAddress]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAddress_tAddress] FOREIGN KEY([HistoryId])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tAddress] CHECK CONSTRAINT [FK_tAddress_tAddress]
GO

ALTER TABLE [tAddress]  WITH CHECK 
	ADD  CONSTRAINT [CK_tAddress_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAddress] CHECK CONSTRAINT [CK_tAddress_HistoryType]
GO
------------------------------------------------------------------------------------------------------------------------
-- PaidService

CREATE TABLE [cPaidService](
	[PaidServiceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CreditCost] [DECIMAL](19,2) NULL,
	[Notes] [nvarchar](2000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cPaidService] PRIMARY KEY CLUSTERED ([PaidServiceId] ASC)
)
GO

ALTER TABLE [cPaidService]  WITH CHECK 
	ADD  CONSTRAINT [FK_cPaidService_cPaidService] FOREIGN KEY([HistoryId])
	REFERENCES [cPaidService] ([PaidServiceId])
GO
ALTER TABLE [cPaidService] CHECK CONSTRAINT [FK_cPaidService_cPaidService]
GO

ALTER TABLE [cPaidService]  WITH CHECK 
	ADD  CONSTRAINT [CK_cPaidService_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cPaidService] CHECK CONSTRAINT [CK_cPaidService_HistoryType]
GO

------------------------------------------------------------------------------------------------------------------------
-- ArticleCategory
CREATE TABLE [cArticleCategory](
	[ArticleCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cArticleCategory_Locale]  DEFAULT ('sk'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cArticleCategory] PRIMARY KEY CLUSTERED ([ArticleCategoryId] ASC)
)
GO

ALTER TABLE [cArticleCategory]  WITH CHECK 
	ADD  CONSTRAINT [FK_cArticleCategory_cArticleCategory] FOREIGN KEY([HistoryId])
	REFERENCES [cArticleCategory] (ArticleCategoryId)
GO
ALTER TABLE [cArticleCategory] CHECK CONSTRAINT [FK_cArticleCategory_cArticleCategory]
GO

ALTER TABLE [cArticleCategory]  WITH CHECK 
	ADD  CONSTRAINT [CK_cArticleCategory_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cArticleCategory] CHECK CONSTRAINT [CK_cArticleCategory_HistoryType]
GO

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
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tSysUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tSysUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO
------------------------------------------------------------------------------------------------------------------------
-- Account
CREATE TABLE [tAccount](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](100) NULL,
	[Password] [nvarchar](1000) NULL CONSTRAINT [DF_tAccount_Password]  DEFAULT (N'D41D8CD98F00B204E9800998ECF8427E'),
	[Email] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_Account_Enabled]  DEFAULT ((1)),
	[Verified] [bit] NOT NULL CONSTRAINT [DF_Account_Verified]  DEFAULT ((0)),
	[VerifyCode] [nvarchar](1000) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tAccount_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([AccountId] ASC)
)
GO

ALTER TABLE [tAccount] WITH CHECK 
	ADD CONSTRAINT [CK_tAccount_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAccount] CHECK CONSTRAINT [CK_tAccount_HistoryType]
GO

ALTER TABLE [tAccount]  WITH CHECK 
	ADD CONSTRAINT [FK_tAccount_tAccount] FOREIGN KEY([HistoryId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccount] CHECK CONSTRAINT [FK_tAccount_tAccount]
GO

ALTER TABLE [tAccount]  WITH CHECK 
	ADD CONSTRAINT [CK_tAccount_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tAccount] CHECK CONSTRAINT [CK_tAccount_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- Person
CREATE TABLE [tPerson](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[Title] [nvarchar](20) NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Phone] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[AddressHomeId] [int] NULL,
	[AddressTempId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC)
)
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD CONSTRAINT [FK_Person_Account] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [FK_Person_Account]
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPerson_tPerson] FOREIGN KEY([HistoryId])
	REFERENCES [tPerson] ([PersonId])
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [FK_tPerson_tPerson]
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD  CONSTRAINT [CK_tPerson_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [CK_tPerson_HistoryType]
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPerson_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [FK_tPerson_HistoryAccount]
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD CONSTRAINT [FK_tPerson_tAddress_Home] FOREIGN KEY([AddressHomeId])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [FK_tPerson_tAddress_Home]
GO

ALTER TABLE [tPerson]  WITH CHECK 
	ADD CONSTRAINT [FK_tPerson_tAddress_Temp] FOREIGN KEY([AddressTempId])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tPerson] CHECK CONSTRAINT [FK_tPerson_tAddress_Temp]
GO

------------------------------------------------------------------------------------------------------------------------
-- tRole

CREATE TABLE [dbo].[tRole](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Notes] [nvarchar](2000) NULL,
 CONSTRAINT [PK_tRole] PRIMARY KEY CLUSTERED ([RoleId] ASC)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tRole] ON [tRole] 
(
	[Name] ASC
)
GO

------------------------------------------------------------------------------------------------------------------------
-- tAccountRole

CREATE TABLE [dbo].[tAccountRole](
	[AccountRoleId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_tAccountRole] PRIMARY KEY CLUSTERED ([AccountRoleId] ASC)
)
GO

ALTER TABLE [tAccountRole]  WITH CHECK 
	ADD CONSTRAINT [FK_tAccountRole_tAccount] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountRole] CHECK CONSTRAINT [FK_tAccountRole_tAccount]
GO

ALTER TABLE [tAccountRole]  WITH CHECK 
	ADD CONSTRAINT [FK_tAccountRole_tRole] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tAccountRole] CHECK CONSTRAINT [FK_tAccountRole_tRole]
GO

------------------------------------------------------------------------------------------------------------------------
-- Bank contact
CREATE TABLE [tBankContact](
	[BankContactId] [int] IDENTITY(1,1) NOT NULL,
	[BankName] [nvarchar](100) NULL,
	[BankCode] [nvarchar](100) NULL,
	[AccountNumber] [nvarchar](100) NULL,
	[IBAN] [nvarchar](100) NULL,
	[SWIFT] [nvarchar](100) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_tBankContact] PRIMARY KEY CLUSTERED ([BankContactId] ASC)
)
GO

ALTER TABLE [tBankContact]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBankContact_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tBankContact] ([BankContactId])
GO
ALTER TABLE [tBankContact] CHECK CONSTRAINT [FK_tBankContact_HistoryId]
GO

ALTER TABLE [tBankContact]  WITH CHECK 
	ADD  CONSTRAINT [CK_tBankContact_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tBankContact] CHECK CONSTRAINT [CK_tBankContact_HistoryType]
GO

ALTER TABLE [tBankContact]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBankContact_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tBankContact] CHECK CONSTRAINT [FK_tBankContact_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- Organization

CREATE TABLE [tOrganization](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[Id1] [nvarchar](100) NULL,
	[Id2] [nvarchar](100) NULL,
	[Id3] [nvarchar](100) NULL,	
	[Name] [nvarchar](100) NOT NULL,
	[Notes] [nvarchar](2000) NULL,
	[Web] [nvarchar](100) NULL,	
	[ContactEMail] [nvarchar](100) NULL,	
	[ContactPhone] [nvarchar](100) NULL,
	[ContactMobile] [nvarchar](100) NULL,
	[ContactPerson] [int] NULL,
	[RegisteredAddress] [int] NULL,
	[CorrespondenceAddress] [int] NULL,
	[InvoicingAddress] [int] NULL,
	[BankContact] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_tOrganization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC)
)
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD  CONSTRAINT [FK_tOrganization_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_AccountId]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD  CONSTRAINT [FK_tOrganization_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tOrganization] ([OrganizationId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_HistoryId]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD  CONSTRAINT [CK_tOrganization_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [CK_tOrganization_HistoryType]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD  CONSTRAINT [FK_tOrganization_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_HistoryAccount]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD CONSTRAINT [FK_tOrganization_ContactPerson] FOREIGN KEY([ContactPerson])
	REFERENCES [tPerson] ([PersonId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_ContactPerson]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD CONSTRAINT [FK_tOrganization_RegisteredAddress] FOREIGN KEY([RegisteredAddress])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_RegisteredAddress]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD CONSTRAINT [FK_tOrganization_CorrespondenceAddress] FOREIGN KEY([CorrespondenceAddress])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_CorrespondenceAddress]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD CONSTRAINT [FK_tOrganization_InvoicingAddress] FOREIGN KEY([InvoicingAddress])
	REFERENCES [tAddress] ([AddressId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_InvoicingAddress]
GO

ALTER TABLE [tOrganization]  WITH CHECK 
	ADD CONSTRAINT [FK_tOrganization_Bankcontact] FOREIGN KEY([BankContact])
	REFERENCES [tBankContact] ([BankContactId])
GO
ALTER TABLE [tOrganization] CHECK CONSTRAINT [FK_tOrganization_InvoicingAddress]
GO

------------------------------------------------------------------------------------------------------------------------
-- FAQ
CREATE TABLE [dbo].[tFaq](
	[FaqId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tFaq_Locale]  DEFAULT ('en'),
	[Order] [int] NULL,
	[Question] [nvarchar](4000) NOT NULL,
	[Answer] [nvarchar](4000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_FaqId] PRIMARY KEY CLUSTERED ([FaqId] ASC)
)
GO

ALTER TABLE [tFaq]  WITH CHECK 
	ADD  CONSTRAINT [FK_tFaq_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tFaq] ([FaqId])
GO
ALTER TABLE [tFaq] CHECK CONSTRAINT [FK_tFaq_HistoryId]
GO

ALTER TABLE [tFaq]  WITH CHECK 
	ADD  CONSTRAINT [CK_tFaq_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tFaq] CHECK CONSTRAINT [CK_tFaq_HistoryType]
GO

ALTER TABLE [tFaq]  WITH CHECK 
	ADD  CONSTRAINT [FK_tFaq_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tFaq] CHECK CONSTRAINT [FK_tFaq_HistoryAccount]
GO

ALTER TABLE [tFaq]  WITH CHECK 
	ADD CONSTRAINT [CK_tFaq_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tFaq] CHECK CONSTRAINT [CK_tFaq_Locale]
GO

------------------------------------------------------------------------------------------------------------------------
-- MasterPage

CREATE TABLE [dbo].[tMasterPage](
	[MasterPageId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](300) NOT NULL,
	[Url] [nvarchar](2000) NULL,
 CONSTRAINT [PK_MasterPageId] PRIMARY KEY CLUSTERED ([MasterPageId] ASC)
)
GO

------------------------------------------------------------------------------------------------------------------------
-- URL ALIAS
------------------------------------------------------------------------------------------------------------------------
-- UrlAlias
CREATE TABLE [dbo].[tUrlAlias](
	[UrlAliasId] [int] IDENTITY(1,1) NOT NULL,
	[Url] NVARCHAR(2000) NOT NULL,
	[Locale] [char](2) NOT NULL CONSTRAINT [DF_tUrlAlias_Locale]  DEFAULT ('en'),
	[Alias] NVARCHAR(2000) NOT NULL,
	[Name] NVARCHAR(500) NULL,
 CONSTRAINT [PK_UrlAlias] PRIMARY KEY CLUSTERED ([UrlAliasId] ASC)
)
GO
ALTER TABLE [tUrlAlias]  WITH CHECK 
	ADD CONSTRAINT [CK_tUrlAlias_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tUrlAlias] CHECK CONSTRAINT [CK_tUrlAlias_Locale]
GO

------------------------------------------------------------------------------------------------------------------------
-- Page
CREATE TABLE [dbo].[tPage](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[MasterPageId] [int] NOT NULL,
	[Locale] [char](2) NOT NULL CONSTRAINT [DF_tPage_Locale]  DEFAULT ('en'),
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
 CONSTRAINT [PK_PageId] PRIMARY KEY CLUSTERED ([PageId] ASC)
)
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_MasterPageId] FOREIGN KEY([MasterPageId])
	REFERENCES [tMasterPage] ([MasterPageId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_MasterPageId]
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_RoleId]
GO

GO
ALTER TABLE [tPage]  WITH CHECK 
	ADD CONSTRAINT [FK_tPage_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_tUrlAlias]
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_HistoryId]
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [CK_tPage_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [CK_tPage_HistoryType]
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_HistoryAccount]
GO

ALTER TABLE [tPage]  WITH CHECK 
	ADD CONSTRAINT [CK_tPage_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [CK_tPage_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- Menu
CREATE TABLE [dbo].[tMenu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tMenu_Locale]  DEFAULT ('en'),
	[Order] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Icon] [nvarchar](255) NULL,
	[RoleId] [int] NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_MenuId] PRIMARY KEY CLUSTERED ([MenuId] ASC)
)
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_RoleId]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_UrlAliasId]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tMenu] ([MenuId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_HistoryId]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [CK_tMenu_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [CK_tMenu_HistoryType]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMenu_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_HistoryAccount]
GO

ALTER TABLE [tMenu]  WITH CHECK 
	ADD CONSTRAINT [CK_tMenu_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [CK_tMenu_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenu
CREATE TABLE [dbo].[tNavigationMenu](
	[NavigationMenuId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tNavigationMenu_Locale]  DEFAULT ('en'),
	[Order] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Icon] [nvarchar](255) NULL,
	[RoleId] [int] NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_NavigationMenuId] PRIMARY KEY CLUSTERED ([NavigationMenuId] ASC)
)
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenu_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_RoleId]
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenu_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_UrlAliasId]
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenu_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tNavigationMenu] ([NavigationMenuId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_HistoryId]
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [CK_tNavigationMenu_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [CK_tNavigationMenu_HistoryType]
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenu_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_HistoryAccount]
GO

ALTER TABLE [tNavigationMenu]  WITH CHECK 
	ADD CONSTRAINT [CK_tNavigationMenu_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [CK_tNavigationMenu_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenuItem
CREATE TABLE [dbo].[tNavigationMenuItem](
	[NavigationMenuItemId] [int] IDENTITY(1,1) NOT NULL,
	[NavigationMenuId] [int] NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tNavigationMenuItem_Locale]  DEFAULT ('en'),
	[Order] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Icon] [nvarchar](255) NULL,
	[RoleId] [int] NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_NavigationMenuItemId] PRIMARY KEY CLUSTERED ([NavigationMenuItemId] ASC)
)
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenuItem_NavigationMenuId] FOREIGN KEY([NavigationMenuId])
	REFERENCES [tNavigationMenu] ([NavigationMenuId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_NavigationMenuId]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenuItem_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_RoleId]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenuItem_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_UrlAliasId]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenuItem_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tNavigationMenuItem] ([NavigationMenuItemId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_HistoryId]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [CK_tNavigationMenuItem_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [CK_tNavigationMenuItem_HistoryType]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNavigationMenuItem_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_HistoryAccount]
GO

ALTER TABLE [tNavigationMenuItem]  WITH CHECK 
	ADD CONSTRAINT [CK_tNavigationMenuItem_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [CK_tNavigationMenuItem_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- News
CREATE TABLE [dbo].[tNews](
	[NewsId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tNews_Locale]  DEFAULT ('en'),
	[Date] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[Head] [nvarchar](500) NULL,
	[Description] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_NewsId] PRIMARY KEY CLUSTERED ([NewsId] ASC)
)
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNews_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tNews] ([NewsId])
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [FK_tNews_HistoryId]
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD  CONSTRAINT [CK_tNews_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [CK_tNews_HistoryType]
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNews_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [FK_tNews_HistoryAccount]
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD CONSTRAINT [CK_tNews_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [CK_tNews_Locale]
GO

ALTER TABLE [tNews]  WITH CHECK 
	ADD  CONSTRAINT [FK_tNews_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tNews] CHECK CONSTRAINT [FK_tNews_UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Poll
CREATE TABLE [dbo].[tPoll](
	[PollId] [int] IDENTITY(1,1) NOT NULL,
	[Closed] [bit] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tPoll_Locale]  DEFAULT ('en'),
	[Question] [nvarchar](1000) NOT NULL,
	[DateFrom] [datetime] NOT NULL,
	[DateTo] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_PollId] PRIMARY KEY CLUSTERED ([PollId] ASC)
)
GO

ALTER TABLE [tPoll]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPoll_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tPoll] ([PollId])
GO
ALTER TABLE [tPoll] CHECK CONSTRAINT [FK_tPoll_HistoryId]
GO

ALTER TABLE [tPoll]  WITH CHECK 
	ADD  CONSTRAINT [CK_tPoll_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tPoll] CHECK CONSTRAINT [CK_tPoll_HistoryType]
GO

ALTER TABLE [tPoll]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPoll_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tPoll] CHECK CONSTRAINT [FK_tPoll_HistoryAccount]
GO

ALTER TABLE [tPoll]  WITH CHECK 
	ADD CONSTRAINT [CK_tPoll_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tPoll] CHECK CONSTRAINT [CK_tPoll_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- PollOption
CREATE TABLE [dbo].[tPollOption](
	[PollOptionId] [int] IDENTITY(1,1) NOT NULL,
	[PollId] [int] NOT NULL,
	[Order] [int] NULL,
	[Name] [nvarchar](1000) NULL,
 CONSTRAINT [PK_PollOptionId] PRIMARY KEY CLUSTERED ([PollOptionId] ASC)
)
GO

ALTER TABLE [tPollOption]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPollOption_PollId] FOREIGN KEY([PollId])
	REFERENCES [tPoll] ([PollId])
GO
ALTER TABLE [tPollOption] CHECK CONSTRAINT [FK_tPollOption_PollId]
GO
------------------------------------------------------------------------------------------------------------------------
-- PollAnswer
CREATE TABLE [dbo].[tPollAnswer](
	[PollAnswerId] [int] IDENTITY(1,1) NOT NULL,
	[PollOptionId] [int] NOT NULL,
	[IP] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PollAnswerId] PRIMARY KEY CLUSTERED ([PollAnswerId] ASC)
)
GO

ALTER TABLE [tPollAnswer]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPollAnswer_PollOptionId] FOREIGN KEY([PollOptionId])
	REFERENCES [tPollOption] ([PollOptionId])
GO
ALTER TABLE [tPollAnswer] CHECK CONSTRAINT [FK_tPollAnswer_PollOptionId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ProvidedService - Poskystnute sluzby
CREATE TABLE [dbo].[tProvidedService](
	[ProvidedServiceId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[PaidServiceId] [int] NOT NULL,
	[ObjectId] [int] NULL,
	[ServiceDate] [DATETIME] NOT NULL,
 CONSTRAINT [PK_ProvidedServiceId] PRIMARY KEY CLUSTERED ([ProvidedServiceId] ASC)
)
GO

ALTER TABLE [tProvidedService]  WITH CHECK 
	ADD  CONSTRAINT [FK_tProvidedService_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tProvidedService] CHECK CONSTRAINT [FK_tProvidedService_AccountId]
GO

ALTER TABLE [tProvidedService]  WITH CHECK 
	ADD  CONSTRAINT [FK_tProvidedService_PaidServiceId] FOREIGN KEY([PaidServiceId])
	REFERENCES [cPaidService] ([PaidServiceId])
GO
ALTER TABLE [tProvidedService] CHECK CONSTRAINT [FK_tProvidedService_PaidServiceId]
GO

------------------------------------------------------------------------------------------------------------------------
-- AccountCredit
CREATE TABLE [dbo].[tAccountCredit](
	[AccountCreditId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Credit] [DECIMAL](19,2) NOT NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_AccountCreditId] PRIMARY KEY CLUSTERED ([AccountCreditId] ASC)
)
GO

ALTER TABLE [tAccountCredit]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountCredit_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountCredit] CHECK CONSTRAINT [FK_tAccountCredit_AccountId]
GO

ALTER TABLE [tAccountCredit]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountCredit_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tAccountCredit] ([AccountCreditId])
GO
ALTER TABLE [tAccountCredit] CHECK CONSTRAINT [FK_tAccountCredit_HistoryId]
GO

ALTER TABLE [tAccountCredit]  WITH CHECK 
	ADD  CONSTRAINT [CK_tAccountCredit_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAccountCredit] CHECK CONSTRAINT [CK_tAccountCredit_HistoryType]
GO

ALTER TABLE [tAccountCredit]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountCredit_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountCredit] CHECK CONSTRAINT [FK_tAccountCredit_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- News
CREATE TABLE [dbo].[tNewsletter](
	[NewsletterId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tNewsletter_Locale]  DEFAULT ('en'),
	[Date] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[Subject] [nvarchar](500) NULL,
	[Content] [nvarchar](MAX) NULL,
	[Attachment] IMAGE NULL,
	[Roles] NVARCHAR(1000) NULL,
	[SendDate] [datetime] NULL,
 CONSTRAINT [PK_NewsletterId] PRIMARY KEY CLUSTERED ([NewsletterId] ASC)
)
GO

ALTER TABLE [tNewsletter]  WITH CHECK 
	ADD CONSTRAINT [CK_tNewsletter_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tNewsletter] CHECK CONSTRAINT [CK_tNewsletter_Locale]
GO
------------------------------------------------------------------------------------------------------------------------
-- IPNF
CREATE TABLE [dbo].[tIPNF](
	[IPNFId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Locale] [char](2) NOT NULL,
	[IPF] [nvarchar](100) NOT NULL,
	[Notes] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_IPNFId] PRIMARY KEY CLUSTERED ([IPNFId] ASC)
)
GO


------------------------------------------------------------------------------------------------------------------------
-- Vocabulary

CREATE TABLE [dbo].[tVocabulary](
	[VocabularyId] [int] IDENTITY(1,1) NOT NULL,
	[Locale] [char](2) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Notes] [nvarchar](200) NULL,
 CONSTRAINT [PK_VocabularyId] PRIMARY KEY CLUSTERED ([VocabularyId] ASC)
)
GO

ALTER TABLE [tVocabulary]  WITH CHECK 
	ADD CONSTRAINT [CK_tVocabulary_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tVocabulary] CHECK CONSTRAINT [CK_tVocabulary_Locale]
GO

------------------------------------------------------------------------------------------------------------------------
-- Translation

CREATE TABLE [dbo].[tTranslation](
	[TranslationId] [int] IDENTITY(1,1) NOT NULL,
	[VocabularyId] [int] NOT NULL,
	[Term] [nvarchar](500) NOT NULL,
	[Translation] [nvarchar](4000) NOT NULL,
	[Notes] [nvarchar](4000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_TranslationId] PRIMARY KEY CLUSTERED ([TranslationId] ASC)
)
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_Vocabulary] FOREIGN KEY([VocabularyId])
	REFERENCES [tVocabulary] ([VocabularyId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_Vocabulary]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tTranslation] ([TranslationId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_HistoryId]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [CK_tTranslation_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [CK_tTranslation_HistoryType]
GO

ALTER TABLE [tTranslation]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTranslation_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tTranslation] CHECK CONSTRAINT [FK_tTranslation_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- Vote
CREATE TABLE [dbo].[tAccountVote](
	[AccountVoteId] [int] IDENTITY(1,1) NOT NULL,
	[ObjectType] [int] NOT NULL,
	[ObjectId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Date] [DATETIME] NOT NULL,
 CONSTRAINT [PK_AccountVoteId] PRIMARY KEY CLUSTERED ([AccountVoteId] ASC)
)
GO

ALTER TABLE [tAccountVote]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountVote_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountVote] CHECK CONSTRAINT [FK_tAccountVote_AccountId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Tag
CREATE TABLE [dbo].[tTag](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](255) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_TagId] PRIMARY KEY CLUSTERED ([TagId] ASC)
)
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTag_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [FK_tTag_HistoryId]
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [CK_tTag_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [CK_tTag_HistoryType]
GO

ALTER TABLE [tTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tTag_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tTag] CHECK CONSTRAINT [FK_tTag_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- Comment
CREATE TABLE [dbo].[tComment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[AccountId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Title] [nvarchar] (255) NULL,
	[Content] [nvarchar](1000) NULL,
	[Votes] [int] NULL, /*Pocet hlasov, ktore clanok obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/	
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_CommentId] PRIMARY KEY CLUSTERED ([CommentId] ASC)
)
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_ParentId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_AccountId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_HistoryId]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [CK_tComment_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [CK_tComment_HistoryType]
GO

ALTER TABLE [tComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tComment_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tComment] CHECK CONSTRAINT [FK_tComment_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ARTICLE tables
------------------------------------------------------------------------------------------------------------------------
-- Articles
CREATE TABLE [dbo].[tArticle](
	[ArticleId] [int] IDENTITY(1,1) NOT NULL,
	[ArticleCategoryId] INT NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tArticle_Locale]  DEFAULT ('en'),
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[ContentKeywords] [nvarchar](MAX) NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	[Country] [nvarchar](255 ) NULL, /*Stat, ktoreho sa clanok tyka*/
	[City] [nvarchar](255 ) NULL /*Mesto, ktoreho sa clanok tyka*/,
	[Approved] [bit] NULL, /*Indikuje, ci je clanok schvaleny redaktorom*/
	[ReleaseDate] [datetime] NOT NULL, /*Datum a cas zverejnenia clanku*/
	[ExpiredDate] [datetime] NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	[EnableComments] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dany clanok viditelny*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni clanku*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore clanok obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_ArticleId] PRIMARY KEY CLUSTERED ([ArticleId] ASC)
)
GO
ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [FK_tArticle_cArticleCategory] FOREIGN KEY([ArticleCategoryId])
	REFERENCES [cArticleCategory] ([ArticleCategoryId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_cArticleCategory]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_HistoryId]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [CK_tArticle_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [CK_tArticle_HistoryType]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_HistoryAccount]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [CK_tArticle_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [CK_tArticle_Locale]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD CONSTRAINT [FK_tArticle_tRole] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_tRole]
GO

ALTER TABLE [tArticle]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticle_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tArticle] CHECK CONSTRAINT [FK_tArticle_UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ArticleTag
CREATE TABLE [dbo].[tArticleTag](
	[ArticleTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[ArticleId] INT NOT NULL,
 CONSTRAINT [PK_ArticleTagId] PRIMARY KEY CLUSTERED ([ArticleTagId] ASC)
)
GO

ALTER TABLE [tArticleTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tArticleTag] CHECK CONSTRAINT [FK_tArticleTag_TagId]
GO

ALTER TABLE [tArticleTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleTag_ArticleId] FOREIGN KEY([ArticleId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticleTag] CHECK CONSTRAINT [FK_tArticleTag_ArticleId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ArticleComment
CREATE TABLE [dbo].[tArticleComment](
	[ArticleCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ArticleId] INT NOT NULL,
 CONSTRAINT [PK_ArticleCommentId] PRIMARY KEY CLUSTERED ([ArticleCommentId] ASC)
)
GO

ALTER TABLE [tArticleComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tArticleComment] CHECK CONSTRAINT [FK_tArticleComment_CommentId]
GO

ALTER TABLE [tArticleComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tArticleComment_ArticleId] FOREIGN KEY([ArticleId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tArticleComment] CHECK CONSTRAINT [FK_tArticleComment_ArticleId]
GO

------------------------------------------------------------------------------------------------------------------------
-- BLOG tables
------------------------------------------------------------------------------------------------------------------------
-- Blogs
CREATE TABLE [dbo].[tBlog](
	[BlogId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tBlog_Locale]  DEFAULT ('en'),
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[ContentKeywords] [nvarchar](MAX) NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa blog bude zobrazovat*/
	[Country] [nvarchar](255 ) NULL, /*Stat, ktoreho sa blog tyka*/
	[City] [nvarchar](255 ) NULL /*Mesto, ktoreho sa blog tyka*/,
	[Approved] [bit] NULL, /*Indikuje, ci je blog schvaleny redaktorom*/
	[ReleaseDate] [datetime] NOT NULL, /*Datum a cas zverejnenia blogu*/
	[ExpiredDate] [datetime] NULL, /*Datum a cas stiahnutia blogu (uz nebude verejne dostupny)*/
	[EnableComments] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dany blog viditelny*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni blogu*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore blog obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore blog dostal pri hlasovani*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_BlogId] PRIMARY KEY CLUSTERED ([BlogId] ASC)
)
GO
ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [FK_tBlog_tAccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_tAccountId]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_HistoryId]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [CK_tBlog_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [CK_tBlog_HistoryType]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_HistoryAccount]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [CK_tBlog_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [CK_tBlog_Locale]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD CONSTRAINT [FK_tBlog_tRole] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_tRole]
GO

ALTER TABLE [tBlog]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlog_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tBlog] CHECK CONSTRAINT [FK_tBlog_UrlAliasId]
GO
------------------------------------------------------------------------------------------------------------------------
-- BlogTag
CREATE TABLE [dbo].[tBlogTag](
	[BlogTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[BlogId] INT NOT NULL,
 CONSTRAINT [PK_BlogTagId] PRIMARY KEY CLUSTERED ([BlogTagId] ASC)
)
GO

ALTER TABLE [tBlogTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tBlogTag] CHECK CONSTRAINT [FK_tBlogTag_TagId]
GO

ALTER TABLE [tBlogTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogTag_BlogId] FOREIGN KEY([BlogId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlogTag] CHECK CONSTRAINT [FK_tBlogTag_BlogId]
GO
------------------------------------------------------------------------------------------------------------------------
-- BlogComment
CREATE TABLE [dbo].[tBlogComment](
	[BlogCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[BlogId] INT NOT NULL,
 CONSTRAINT [PK_BlogCommentId] PRIMARY KEY CLUSTERED ([BlogCommentId] ASC)
)
GO

ALTER TABLE [tBlogComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tBlogComment] CHECK CONSTRAINT [FK_tBlogComment_CommentId]
GO

ALTER TABLE [tBlogComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tBlogComment_BlogId] FOREIGN KEY([BlogId])
	REFERENCES [tBlog] ([BlogId])
GO
ALTER TABLE [tBlogComment] CHECK CONSTRAINT [FK_tBlogComment_BlogId]
GO
------------------------------------------------------------------------------------------------------------------------
-- PROFILE tables
------------------------------------------------------------------------------------------------------------------------
-- Profile
CREATE TABLE [dbo].[tProfile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NULL,
	[Type] INT NULL,
	[Description] NVARCHAR(1000) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([ProfileId] ASC)
)
GO

------------------------------------------------------------------------------------------------------------------------
-- Account Profile
CREATE TABLE [dbo].[tAccountProfile](
	[AccountProfileId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] INT NOT NULL,
	[ProfileId] INT NOT NULL,
	[Value] NVARCHAR(MAX) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_AccountProfile] PRIMARY KEY CLUSTERED ([AccountProfileId] ASC)
)
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_AccountId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_ProfileId] FOREIGN KEY([ProfileId])
	REFERENCES [tProfile] ([ProfileId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_ProfileId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tAccountProfile] ([AccountProfileId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_HistoryId]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [CK_tAccountProfile_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [CK_tAccountProfile_HistoryType]
GO

ALTER TABLE [tAccountProfile]  WITH CHECK 
	ADD  CONSTRAINT [FK_tAccountProfile_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tAccountProfile] CHECK CONSTRAINT [FK_tAccountProfile_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- IMAGE GALLERY tables
------------------------------------------------------------------------------------------------------------------------
-- Image gallery
CREATE TABLE [dbo].[tImageGallery](
	[ImageGalleryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Date] DATETIME NOT NULL,
	[RoleId] [int] NULL, /*Role pre ktore sa blog bude zobrazovat*/
	[EnableComments] [bit] NULL,
	[EnableVotes] [bit] NULL,
	[Visible] [bit] NULL, /*Priznak ci ma byt dana galeria viditelna*/
	[ViewCount] [int] NULL, /*Pocet zobrazeni galerie*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ImageGallery] PRIMARY KEY CLUSTERED ([ImageGalleryId] ASC)
)
GO
ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_RoleId] FOREIGN KEY([RoleId])
	REFERENCES [tRole] ([RoleId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_RoleId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_UrlAliasId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_HistoryId]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [CK_tImageGallery_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [CK_tImageGallery_HistoryType]
GO

ALTER TABLE [tImageGallery]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGallery_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tImageGallery] CHECK CONSTRAINT [FK_tImageGallery_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryTag
CREATE TABLE [dbo].[tImageGalleryTag](
	[ImageGalleryTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] INT NOT NULL,
	[ImageGalleryId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryTagId] PRIMARY KEY CLUSTERED ([ImageGalleryTagId] ASC)
)
GO

ALTER TABLE [tImageGalleryTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryTag_TagId] FOREIGN KEY([TagId])
	REFERENCES [tTag] ([TagId])
GO
ALTER TABLE [tImageGalleryTag] CHECK CONSTRAINT [FK_tImageGalleryTag_TagId]
GO

ALTER TABLE [tImageGalleryTag]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryTag_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryTag] CHECK CONSTRAINT [FK_tImageGalleryTag_ImageGalleryId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryComment
CREATE TABLE [dbo].[tImageGalleryComment](
	[ImageGalleryCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ImageGalleryId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryCommentId] PRIMARY KEY CLUSTERED ([ImageGalleryCommentId] ASC)
)
GO

ALTER TABLE [tImageGalleryComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tImageGalleryComment] CHECK CONSTRAINT [FK_tImageGalleryComment_CommentId]
GO

ALTER TABLE [tImageGalleryComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryComment_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryComment] CHECK CONSTRAINT [FK_tImageGalleryComment_ImageGalleryId]
GO
------------------------------------------------------------------------------------------------------------------------
-- Image gallery item
CREATE TABLE [dbo].[tImageGalleryItem](
	[ImageGalleryItemId] [int] IDENTITY(1,1) NOT NULL,
	[ImageGalleryId] [int] NOT NULL,
	[VirtualPath] NVARCHAR(255) NOT NULL,
	[VirtualThumbnailPath] NVARCHAR(255) NOT NULL,
	[Position] INT NOT NULL,
	[Date] DATETIME NOT NULL,
	[Description] NVARCHAR(1000) NULL,
	[ViewCount] [int] NULL, /*Pocet zobrazeni obrazku*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore obrazok obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore obrazok dostal pri hlasovani*/		
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ImageGalleryItem] PRIMARY KEY CLUSTERED ([ImageGalleryItemId] ASC)
)
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_ImageGalleryId] FOREIGN KEY([ImageGalleryId])
	REFERENCES [tImageGallery] ([ImageGalleryId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_ImageGalleryId]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tImageGalleryItem] ([ImageGalleryItemId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_HistoryId]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [CK_tImageGalleryItem_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [CK_tImageGalleryItem_HistoryType]
GO

ALTER TABLE [tImageGalleryItem]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItem_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tImageGalleryItem] CHECK CONSTRAINT [FK_tImageGalleryItem_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryItemComment
CREATE TABLE [dbo].[tImageGalleryItemComment](
	[ImageGalleryItemCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] INT NOT NULL,
	[ImageGalleryItemId] INT NOT NULL,
 CONSTRAINT [PK_ImageGalleryItemCommentId] PRIMARY KEY CLUSTERED ([ImageGalleryItemCommentId] ASC)
)
GO

ALTER TABLE [tImageGalleryItemComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItemComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tImageGalleryItemComment] CHECK CONSTRAINT [FK_tImageGalleryItemComment_CommentId]
GO

ALTER TABLE [tImageGalleryItemComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tImageGalleryItemComment_ImageGalleryItemId] FOREIGN KEY([ImageGalleryItemId])
	REFERENCES [tImageGalleryItem] ([ImageGalleryItemId])
GO
ALTER TABLE [tImageGalleryItemComment] CHECK CONSTRAINT [FK_tImageGalleryItemComment_ImageGalleryItemId]
GO

------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- classifiers
CREATE VIEW vAddresses AS SELECT A=1
GO
CREATE VIEW vUrlAliasPrefixes AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- permissions

CREATE VIEW vRoles AS SELECT A=1
GO

CREATE VIEW vAccountRoles AS SELECT AccountId=-1, RoleName=''
GO

------------------------------------------------------------------------------------------------------------------------
-- OS&Persons
CREATE VIEW vAccounts AS SELECT AccountId=1, Email=''
GO

CREATE VIEW vPersons AS SELECT A=1
GO

CREATE VIEW vBankContacts AS SELECT BankContactId=1
GO

CREATE VIEW vOrganizations AS SELECT OrganizationId=1, [Name]=''
GO

------------------------------------------------------------------------------------------------------------------------
-- 

CREATE VIEW vMasterPages AS SELECT A=1
GO

CREATE VIEW vUrlAliases AS SELECT A=1
GO

CREATE VIEW vPages AS SELECT A=1
GO

CREATE VIEW vMenu AS SELECT A=1
GO

CREATE VIEW vNavigationMenu AS SELECT A=1
GO

CREATE VIEW vNavigationMenuItem AS SELECT A=1
GO

CREATE VIEW vFaqs AS SELECT A=1
GO

CREATE VIEW vNews AS SELECT A=1
GO

CREATE VIEW vNewsletter AS SELECT A=1
GO

CREATE VIEW vPolls AS SELECT A=1
GO

CREATE VIEW vPollOptions AS SELECT A=1
GO

CREATE VIEW vPollAnswers AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- CREDIT MANAGEMENT
CREATE VIEW vPaidServices AS SELECT A=1
GO
CREATE VIEW vAccountsCredit AS SELECT AccountId=1, Credit=0
GO
CREATE VIEW vProvidedServices AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- IPNF
CREATE VIEW vIPNFs AS SELECT A=1
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & translation

CREATE VIEW vVocabularies AS SELECT A=1
GO

CREATE VIEW vTranslations AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- AccountVotes
CREATE VIEW vAccountVotes AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Tags
CREATE VIEW vTags AS SELECT TagId=1, Tag=1
GO
CREATE VIEW vArticleTags AS SELECT TagId=1, ArticleId=1
GO
CREATE VIEW vBlogTags AS SELECT TagId=1, BlogId=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Comment
CREATE VIEW vComments AS SELECT CommentId=1, ParentId=1, AccountId=1, Date=1, Title=1, Content=1, Votes=1, TotalRating=1
GO
CREATE VIEW vArticleComments AS SELECT A=1
GO
CREATE VIEW vBlogComments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Articles
CREATE VIEW vArticleCategories AS SELECT A=1
GO
CREATE VIEW vArticles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Blogs
CREATE VIEW vBlogs AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- Profile
CREATE VIEW vProfiles AS SELECT A=1
GO
CREATE VIEW vAccountProfiles AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGallery
CREATE VIEW vImageGalleries AS SELECT A=1
GO
CREATE VIEW vImageGalleryTags AS SELECT TagId=1, ImageGalleryId=1
GO
CREATE VIEW vImageGalleryComments AS SELECT A=1
GO
CREATE VIEW vImageGalleryItems AS SELECT ImageGalleryId=1
GO
CREATE VIEW vImageGalleryItemComments AS SELECT A=1
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Functions declarations
------------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION fFormatAddress(@Street NVARCHAR(100), @Zip NVARCHAR(100), @City NVARCHAR(100)) RETURNS NVARCHAR(100) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fFormatPerson(@FirstName NVARCHAR(100), @LastName NVARCHAR(100), @EMail NVARCHAR(100)) RETURNS NVARCHAR(100) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fAccountRoles(@AccountId INT) RETURNS NVARCHAR(4000) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fStringToTable(@InputString NVARCHAR(4000), @separator NVARCHAR(10))
	RETURNS @table TABLE ([index] INT, item NVARCHAR(200))
AS 
BEGIN
	RETURN
END
GO

CREATE FUNCTION fMakeAnsi(@Text NVARCHAR(4000)) RETURNS NVARCHAR(4000) AS BEGIN RETURN 1 END
GO

CREATE FUNCTION fCompareKeywords(@Pattern NVARCHAR(4000), @Test NVARCHAR(4000)) RETURNS INT AS BEGIN RETURN '' END
GO

CREATE FUNCTION fCompareKeywordsEx(@Pattern NVARCHAR(4000), @Test NVARCHAR(4000), @Spliter CHAR) RETURNS INT AS BEGIN RETURN '' END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Functions declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Procedures declarations
------------------------------------------------------------------------------------------------------------------------
-- address

CREATE PROCEDURE pAddressCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAddressModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAddressDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- url alias prefix
CREATE PROCEDURE pUrlAliasPrefixModify AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- roles
CREATE PROCEDURE pRoleCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pRoleModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pRoleDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- person and accounts
CREATE PROCEDURE pAccountCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountVerify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPersonDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Bank contact

CREATE PROCEDURE pBankContactCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBankContactModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBankContactDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Organization

CREATE PROCEDURE pOrganizationCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pOrganizationModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pOrganizationDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- UrlAlias
CREATE PROCEDURE pUrlAliasCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pUrlAliasDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Page
CREATE PROCEDURE pPageCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pPageModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pPageDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Menu
CREATE PROCEDURE pMenuCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pMenuModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pMenuDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenu
CREATE PROCEDURE pNavigationMenuCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- NavigationMenuItem
CREATE PROCEDURE pNavigationMenuItemCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuItemModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNavigationMenuItemDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Faq
CREATE PROCEDURE pFaqCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pFaqModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pFaqDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- News
CREATE PROCEDURE pNewsCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Newsletter
CREATE PROCEDURE pNewsletterCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsletterModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pNewsletterDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Poll
CREATE PROCEDURE pPollCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollDelete AS BEGIN SET NOCOUNT ON; END
GO

-- PollOption
CREATE PROCEDURE pPollOptionCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollOptionModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pPollOptionDelete AS BEGIN SET NOCOUNT ON; END
GO

-- PollAnswer
CREATE PROCEDURE pPollAnswerCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- CREDIT management
CREATE PROCEDURE pPaidServiceModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pAccountCreditCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountCreditModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountCreditDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pProvidedServiceCreate AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Vocabulary & Translation

-- iba tato procka je urcena pre UI. User si moze len upravit text
CREATE PROCEDURE pTranslationModify AS BEGIN SET NOCOUNT ON; END
GO

-- developerska procka pre zalozenie noveho textu v scripte
CREATE PROCEDURE pTranslationCreateEx AS BEGIN SET NOCOUNT ON; END
GO

-- TODO: zamysliet sa nad pTransaltionDeleteEx... ak sa zrusi niekde text, mat ho moznost vymazat... developer si musi byt isty ze text je mozne vymazat

------------------------------------------------------------------------------------------------------------------------
-- AccountVotes
CREATE PROCEDURE pAccountVoteCreate AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Tags
CREATE PROCEDURE pTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pTagDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Comments
CREATE PROCEDURE pCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pCommentDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pCommentIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ArticleCategory
CREATE PROCEDURE pArticleCategoryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCategoryDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Article
CREATE PROCEDURE pArticleCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pArticleIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pArticleTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pArticleCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Blog
CREATE PROCEDURE pBlogCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogIncrementVote AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pBlogTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pBlogCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- AccountProfile
CREATE PROCEDURE pAccountProfileCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pAccountProfileDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGallery
CREATE PROCEDURE pImageGalleryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryTagCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ImageGalleryItem
CREATE PROCEDURE pImageGalleryItemCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemDelete AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pImageGalleryItemIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pImageGalleryItemCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Search Engine procedures
CREATE PROCEDURE pSearchPages AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchArticles AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogs AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchNews AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleries AS BEGIN SET NOCOUNT ON; END
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

ALTER FUNCTION fAccountRoles(@AccountId INT) RETURNS NVARCHAR(4000)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Roles NVARCHAR(200)
	SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=@AccountId
	RETURN @Roles
END
GO

/*
SELECT dbo.fAccountRoles(1)
*/

ALTER FUNCTION fCompareKeywords
(	@Pattern NVARCHAR(4000), 
	@Test NVARCHAR(400)
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result INT
	SET @Result = 0
	
	IF EXISTS(
		SELECT * FROM dbo.fStringToTable(@Test, ',') t
		INNER JOIN dbo.fStringToTable(@Pattern, ',') p ON dbo.fMakeAnsi(LTRIM(RTRIM(lower(t.item)))) = dbo.fMakeAnsi(LTRIM(RTRIM(lower(p.item))))
	)
		SET @Result = 1
	
	RETURN @Result
END
GO

/*
SELECT dbo.fCompareKeywords('software, development, application', 'software')
SELECT dbo.fCompareKeywords('software, development, application', 'softwares')
SELECT dbo.fCompareKeywords('software, development, application', 'development')
SELECT dbo.fCompareKeywords('software, development, application', 'application')
SELECT dbo.fCompareKeywords('software, development, application', 'software, application')
SELECT dbo.fCompareKeywords('software, development, application', 'software, applycation')
SELECT dbo.fCompareKeywords('software, development, application', 'zoftware, applycation')
SELECT dbo.fCompareKeywords('software, development, application', 'zoftware, application')
SELECT dbo.fCompareKeywords('software, development, application', 'software application')
SELECT dbo.fCompareKeywords('software, development, application', 'hardware')
SELECT dbo.fCompareKeywords('software, development, application', '')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hračky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hraČky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky, čačky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky, cacky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracka, cicka')
*/

ALTER FUNCTION fCompareKeywordsEx
(	@Pattern NVARCHAR(4000), 
	@Test NVARCHAR(400),
	@Spliter CHAR = NULL
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result INT
	SET @Result = 0
	
	IF EXISTS(
		SELECT * FROM dbo.fStringToTable(@Test, ISNULL(@Spliter,',')) t
		INNER JOIN dbo.fStringToTable(@Pattern, ISNULL(@Spliter, ',')) p ON dbo.fMakeAnsi(LTRIM(RTRIM(lower(t.item)))) = dbo.fMakeAnsi(LTRIM(RTRIM(lower(p.item))))
	)
		SET @Result = 1
	
	RETURN @Result
END
GO

/*
SELECT dbo.fCompareKeywordsEx('software, development, application', 'software')
SELECT dbo.fCompareKeywordsEx('Nákladní / Užitkové', 'Nákladní', '/' )
*/

ALTER FUNCTION fFormatAddress
(	@Street NVARCHAR(100), 
	@Zip NVARCHAR(100), 
	@City NVARCHAR(100)
)
RETURNS NVARCHAR(100)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result NVARCHAR(100)
	SET @Result = ISNULL(@Street, '') + ', ' + ISNULL(@Zip, '') + ' ' + ISNULL(@City, '')
	SET @Result = RTRIM(LTRIM(@Result))
	IF LEN(@Result) < 2 SET @Result = ''
	RETURN @Result
END
GO

/*
SELECT dbo.fFormatAddress('Sásovská cesta 16/A', '97411', 'Banská Bystrica')
SELECT dbo.fFormatAddress('', '', '')
SELECT dbo.fFormatAddress(NULL, NULL, NULL)
*/

ALTER FUNCTION fFormatPerson
(	@FirstName NVARCHAR(100), 
	@LastName NVARCHAR(100), 
	@Email NVARCHAR(100)
)
RETURNS NVARCHAR(100)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result NVARCHAR(100)
	SET @Result = ISNULL(@FirstName, '') + ' ' + ISNULL(@LastName, '')
	IF @Email IS NOT NULL AND LEN(@Email) > 0 SET @Result = @Result + ' (' + @Email + ')'
	SET @Result = RTRIM(LTRIM(@Result))
	IF LEN(@Result) < 2 SET @Result = ''
	RETURN @Result
END
GO

/*
SELECT dbo.fFormatPerson('Jozef', 'Prídavok', 'jozef.pridavok@mothiva.com')
SELECT dbo.fFormatPerson('', 'Prídavok', '')
SELECT dbo.fFormatPerson(NULL, NULL, NULL)
*/

ALTER FUNCTION fMakeAnsi
(
	@Text NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET @Text = REPLACE(@Text, 'á', 'a')
	SET @Text = REPLACE(@Text, 'Á', 'A')
	SET @Text = REPLACE(@Text, 'ä', 'a')
	SET @Text = REPLACE(@Text, 'í', 'i')
	SET @Text = REPLACE(@Text, 'Í', 'i')
	SET @Text = REPLACE(@Text, 'ó', 'o')
	SET @Text = REPLACE(@Text, 'Ó', 'O')
	SET @Text = REPLACE(@Text, 'ô', 'o')
	SET @Text = REPLACE(@Text, 'é', 'e')
	SET @Text = REPLACE(@Text, 'ě', 'e')
	SET @Text = REPLACE(@Text, 'É', 'E')
	SET @Text = REPLACE(@Text, 'ú', 'u')
	SET @Text = REPLACE(@Text, 'Ú', 'U')
	SET @Text = REPLACE(@Text, 'ů', 'u')
	SET @Text = REPLACE(@Text, 'Ů', 'U')
	SET @Text = REPLACE(@Text, 'ľ', 'l')
	SET @Text = REPLACE(@Text, 'Ľ', 'L')
	SET @Text = REPLACE(@Text, 'ĺ', 'l')
	SET @Text = REPLACE(@Text, 'Ĺ', 'L')
	SET @Text = REPLACE(@Text, 'š', 's')
	SET @Text = REPLACE(@Text, 'Š', 's')
	SET @Text = REPLACE(@Text, 'č', 'c')
	SET @Text = REPLACE(@Text, 'Č', 'C')
	SET @Text = REPLACE(@Text, 'ť', 't')
	SET @Text = REPLACE(@Text, 'Ť', 'T')
	SET @Text = REPLACE(@Text, 'ž', 'z')
	SET @Text = REPLACE(@Text, 'Ž', 'Z')
	SET @Text = REPLACE(@Text, 'ř', 'r')
	SET @Text = REPLACE(@Text, 'Ř', 'R')
	SET @Text = REPLACE(@Text, 'ý', 'y')
	SET @Text = REPLACE(@Text, 'Ý', 'Y')
	SET @Text = REPLACE(@Text, 'ň', 'n')
	SET @Text = REPLACE(@Text, 'Ň', 'N')
	SET @Text = REPLACE(@Text, 'ď', 'd')
	SET @Text = REPLACE(@Text, 'ö', 'o')
	RETURN @Text
END
GO

--SELECT dbo.fCorMakeAnsi('Jozef Prídavok, ľščťžýáíé úôř')

ALTER FUNCTION fStringToTable(@InputString NVARCHAR(4000), @separator NVARCHAR(10))
	RETURNS @table
		TABLE ([index] INT, item NVARCHAR(200))
WITH ENCRYPTION
AS 
BEGIN
	DECLARE @index INT
	SET @index = 0
	DECLARE @item nvarchar(200)
	DECLARE @str nvarchar(4000)
	SET @str = @InputString + @separator

	DECLARE @position int
	SET @position = CHARINDEX(@separator,@str,0)
	WHILE (@position > 0)
	BEGIN
		SET @item = NULL

		DECLARE @PartialStr varchar(8000)
		SET @PartialStr = LEFT(@str,@position - 1)
		SET @item = @PartialStr
		SET @str = RIGHT(@str,LEN(@str) - @position)
		SET @position = CHARINDEX(@separator,@str,0)

		INSERT @table([index], item) VALUES (@index, @item)
		SET @index = @index + 1
	END

	RETURN
END
GO

/*

SELECT * FROM fStringToTable('a;b;c;d', ';')

*/


ALTER VIEW vAccountProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ap.AccountProfileId, ap.AccountId, ap.ProfileId, ap.[Value], ProfileType = p.Type, ProfileName = p.Name
FROM tAccountProfile ap 
INNER JOIN tProfile p ON p.ProfileId = ap.ProfileId
WHERE ap.HistoryId IS NULL
GO

ALTER VIEW vAccountRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, ar.AccountRoleId, r.[RoleId], RoleName = r.[Name]
FROM tRole r
INNER JOIN tAccountRole ar (NOLOCK) ON ar.RoleId = r.RoleId
INNER JOIN tAccount a (NOLOCK) ON ar.AccountId = a.AccountId
GO

-- SELECT * FROM vAccountRoles


ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	Roles = dbo.fAccountRoles(a.AccountId)
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	HistoryId IS NULL
ORDER BY [Login]
GO

/*
SELECT * FROM vAccounts
SELECT * FROM vAccountRoles

DECLARE @Roles NVARCHAR(200)
SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=0
SELECT @Roles
*/

ALTER VIEW vAccountsCredit
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	[AccountCreditId], [AccountId], [Credit]
FROM
	tAccountCredit
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vAccountVotes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	AccountVoteId, ObjectType, ObjectId, AccountId, Rating, [Date]
FROM tAccountVote
GO

ALTER VIEW vAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [City], [Street], [Zip], [Notes], [District], [Region], [Country], [State]
FROM
	tAddress
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vAddresses


ALTER VIEW vArticleCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	a.ArticleCategoryId, a.[Name], a.[Code], a.[Locale], a.[Notes], 
	ArticlesInCategory = (SELECT Count(*) FROM tArticle 
		WHERE HistoryId IS NULL AND
			  Visible=1 AND 
			  ReleaseDate<=GETDATE() AND 
			  ArticleCategoryId = a.ArticleCategoryId )
FROM
	cArticleCategory a
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vArticleComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.ArticleCommentId, ac.ArticleId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tArticleComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO


ALTER VIEW vArticles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleId, a.Locale, a.Icon, a.Title, a.Teaser, a.Content, a.RoleId, a.Country,
	a.ArticleCategoryId, ArticleCategoryName = c.Name,
	a.City, a.Approved, a.ReleaseDate, a.ExpiredDate, 
	a.EnableComments, a.Visible, 
	CommentsCount = ( SELECT Count(*) FROM vArticleComments WHERE ArticleId = a.ArticleId ),
	ViewCount = ISNULL(a.ViewCount, 0 ), 
	Votes = ISNULL(a.Votes, 0), 
	TotalRating = ISNULL(a.TotalRating, 0),
	RatingResult =  ISNULL(a.TotalRating*1.0/a.Votes*1.0, 0 ),
	a.UrlAliasId, alias.Alias, alias.Url

FROM
	tArticle a INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = a.UrlAliasId

WHERE
	HistoryId IS NULL
GO


ALTER VIEW vArticleTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.ArticleTagId, a.ArticleId, t.TagId, t.Tag
FROM
	tArticleTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO

ALTER VIEW vBankContacts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[BankContactId], [BankName], [BankCode], [AccountNumber], [IBAN], [SWIFT]
FROM
	tBankContact b
WHERE
	b.HistoryId IS NULL
GO

/*
SELECT * FROM vBankContacts
*/


ALTER VIEW vBlogComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.BlogCommentId, ac.BlogId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tBlogComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO


ALTER VIEW vBlogs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	b.BlogId, b.Locale, b.Icon, b.Title, b.Teaser, b.Content, b.RoleId, b.Country,
	b.AccountId, Login = a.Login,
	b.City, b.Approved, b.ReleaseDate, b.ExpiredDate, 
	b.EnableComments, b.Visible, 
	b.UrlAliasId, alias.Alias, alias.Url,
	CommentsCount = ( SELECT Count(*) FROM vBlogComments WHERE BlogId = b.BlogId ),
	ViewCount = ISNULL(b.ViewCount, 0 ), 
	Votes = ISNULL(b.Votes, 0), 
	TotalRating = ISNULL(b.TotalRating, 0),
	RatingResult =  ISNULL(b.TotalRating*1.0/b.Votes*1.0, 0 )
FROM
	tBlog b INNER JOIN vAccounts a ON a.AccountId = b.AccountId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = b.UrlAliasId
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vBlogTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.BlogTagId, a.BlogId, t.TagId, t.Tag
FROM
	tBlogTag a 
	INNER JOIN vTags t ON t.TagId = a.TagId
GO


ALTER VIEW vComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[CommentId], [ParentId], [AccountId], [Date], [Title], [Content], [Votes], [TotalRating]
FROM
	tComment
WHERE
	HistoryId IS NULL
GO

ALTER VIEW vFaqs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[FaqId], [Locale], [Order], [Question], [Answer]
FROM
	tFaq
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vFaqs


ALTER VIEW vImageGalleries
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryId, RoleId, g.[Name], g.[Date], g.EnableComments, g.EnableVotes, g.Visible,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryComments WHERE ImageGalleryId = g.ImageGalleryId  ),
	ItemsCount = ( SELECT Count(*) FROM vImageGalleryItems WHERE ImageGalleryId = g.ImageGalleryId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	g.UrlAliasId, a.Alias, a.Url
	
FROM tImageGallery g LEFT JOIN tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
WHERE HistoryId IS NULL
GO

ALTER VIEW vImageGalleryComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igc.ImageGalleryCommentId, igc.ImageGalleryId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryComment igc 
	INNER JOIN vComments c ON c.CommentId = igc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO


ALTER VIEW vImageGalleryItemComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	igic.ImageGalleryItemCommentId, igic.ImageGalleryItemId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tImageGalleryItemComment igic 
	INNER JOIN vComments c ON c.CommentId = igic.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO


ALTER VIEW vImageGalleryItems
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ImageGalleryItemId, ImageGalleryId, [VirtualPath], [VirtualThumbnailPath], [Position], [Date], Description,
	CommentsCount = ( SELECT Count(*) FROM vImageGalleryItemComments WHERE ImageGalleryItemId = g.ImageGalleryItemId  ),
	ViewCount = ISNULL(ViewCount, 0 ),
	Votes = ISNULL(Votes, 0), 
	TotalRating = ISNULL(TotalRating, 0),
	RatingResult =  ISNULL(TotalRating*1.0/Votes*1.0, 0 )	
FROM tImageGalleryItem g
WHERE HistoryId IS NULL
GO


ALTER VIEW vImageGalleryTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	g.ImageGalleryTagId, g.ImageGalleryId, t.TagId, t.Tag
FROM
	tImageGalleryTag g 
	INNER JOIN vTags t ON t.TagId = g.TagId
GO


ALTER VIEW vIPNFs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[IPNFId], [Type], [Locale], [IPF], [Notes]
FROM tIPNF
GO

ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [Name], [Description], [Url]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages

ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO
-- SELECT * FROM vMenu
ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenu m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu
ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuItemId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
FROM
	tNavigationMenuItem m LEFT JOIN tUrlAlias a ON a.UrlAliasId = m.UrlAliasId
WHERE
	m.HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenuItem
ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	n.[NewsId], n.[Locale], n.[Date], n.[Icon], n.[Head], n.[Description], n.[Content],
	n.UrlAliasId, alias.Alias, alias.Url
FROM
	tNews n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId

WHERE
	n.HistoryId IS NULL 
GO

-- SELECT * FROM vNews

ALTER VIEW vNewsletter
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[NewsletterId], [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles], [SendDate]
FROM
	tNewsletter
GO

-- SELECT * FROM vNewsletter

ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, 
	AccountId = o.AccountId,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

/*
SELECT * FROM vOrganizations
*/

ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PageId], p.[MasterPageId], p.[Locale], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
	a.Url, a.Alias
FROM
	tPage p LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO

-- SELECT * FROM vPages


ALTER VIEW vPaidServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	PaidServiceId, [Name], [Notes], [CreditCost]
FROM
	cPaidService
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vPersons
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.PersonId, p.AccountId, p.Title, p.LastName, p.FirstName, ISNULL(p.Email, a.EMail) as Email, p.Notes,
	p.Phone, p.Mobile, p.AddressHomeId, p.AddressTempId
FROM
	tPerson p LEFT JOIN
	tAccount a ON a.AccountId = p.AccountId	
WHERE
	p.HistoryId IS NULL
ORDER BY p.LastName, p.FirstName
GO

-- SELECT * FROM vPersons

ALTER VIEW vPollAnswers
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[PollAnswerId], [PollOptionId], [IP]
FROM
	tPollAnswer
GO

-- SELECT * FROM vPollAnswers

ALTER VIEW vPollOptions
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	o.[PollOptionId], o.[PollId], o.[Order], o.[Name], 
	Votes = (SELECT COUNT(*) FROM tPollAnswer WHERE PollOptionId = o.[PollOptionId] )
FROM
	tPollOption o 
GO

-- SELECT * FROM vPollOptions

ALTER VIEW vPolls
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PollId], p.[Closed], p.[Locale], p.[Question], p.[DateFrom], p.[DateTo], p.[Icon],
	VotesTotal = ( SELECT SUM(Votes) FROM vPollOptions WHERE PollId = p.PollId )
FROM
	tPoll p
WHERE
	p.HistoryId IS NULL
GO

-- SELECT * FROM vPools


ALTER VIEW vProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ProfileId, [Name], [Type], [Description]
FROM tProfile
GO


ALTER VIEW vProvidedServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ps.[ProvidedServiceId], ps.[AccountId], ps.[PaidServiceId], ps.ObjectId, ps.[ServiceDate], p.CreditCost, p.[Name], p.[Notes]
FROM
	tProvidedService ps INNER JOIN
	vPaidServices p ON p.PaidServiceId = ps.PaidServiceId
GO

ALTER VIEW vRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[RoleId], [Name], [Notes]
FROM tRole
GO

-- SELECT * FROM vRoles


ALTER VIEW vTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	TagId, Tag
FROM tTag WHERE HistoryId IS NULL
GO

ALTER VIEW vTranslations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.VocabularyId, VocabularyName = v.Name, v.Locale, t.TranslationId, t.Term, t.Translation, t.Notes
FROM tTranslation t (NOLOCK)
INNER JOIN tVocabulary v (NOLOCK) ON t.VocabularyId = v.VocabularyId
WHERE t.HistoryId IS NULL
GO

-- SELECT * FROM vTranslations

ALTER VIEW vUrlAliases
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[UrlAliasId], [Url], [Locale], [Alias], [Name]
FROM tUrlAlias
GO

-- SELECT * FROM vUrlAliases

ALTER VIEW vUrlAliasPrefixes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	UrlAliasPrefixId, [Name], [Code], [Locale], [Notes]
FROM cUrlAliasPrefix
WHERE HistoryId IS NULL
GO

ALTER VIEW vVocabularies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[VocabularyId], [Locale], [Name], [Notes]
FROM tVocabulary
GO

-- SELECT * FROM vVocabularies

ALTER PROCEDURE pAccountCreate
	@HistoryAccount INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM tAccount WHERE [Login] = @Login AND HistoryId IS NULL) BEGIN
		RETURN
	END
	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ([Login], [Password], [Email], [Enabled], [VerifyCode], [Verified],
		HistoryStamp, HistoryType, HistoryAccount)
	VALUES (@Login, @Password, @Email, @Enabled, @VerifyCode, @Verified,
		GETDATE(), 'C', @HistoryAccount)
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole (AccountId, RoleId)
		SELECT @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END
GO

-- EXEC pAccountCreate @HistoryAccount = NULL, @Login = 'aaa', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68' -- @Password=0987oiuk

ALTER PROCEDURE pAccountCreditCreate
	@HistoryAccount INT,
	@AccountId INT,
	@Credit DECIMAL(19,2),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountCredit ( AccountId, Credit, HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @Credit, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountCreditId = @Result

END
GO

ALTER PROCEDURE pAccountCreditDelete
	@HistoryAccount INT,
	@AccountCreditId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AccountCreditId IS NULL OR NOT EXISTS(SELECT * FROM tAccountCredit WHERE AccountCreditId = @AccountCreditId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AccountCreditId=%d', 16, 1, @AccountCreditId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAccountCredit
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountCreditId
		WHERE AccountCreditId = @AccountCreditId

		SET @Result = @AccountCreditId

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

ALTER PROCEDURE pAccountCreditModify
	@HistoryAccount INT,
	@AccountCreditId INT,
	@Credit DECIMAL(19,2), 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountCredit WHERE AccountCreditId = @AccountCreditId AND HistoryId IS NULL) 
		RAISERROR('Invalid AccountCreditId %d', 16, 1, @AccountCreditId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccountCredit ( AccountId, Credit,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, Credit,
			HistoryStamp, HistoryType, HistoryAccount, @AccountCreditId
		FROM tAccountCredit
		WHERE AccountCreditId = @AccountCreditId

		UPDATE tAccountCredit
		SET
			Credit = @Credit,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountCreditId = @AccountCreditId

		SET @Result = @AccountCreditId

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

ALTER PROCEDURE pAccountDelete
	@HistoryAccount INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	UPDATE tAccount
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountId
	WHERE AccountId = @AccountId

	SET @Result = @AccountId

END
GO

ALTER PROCEDURE pAccountModify
	@HistoryAccount INT,
	@AccountId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000),
	@Email NVARCHAR(100) = NULL,
	@Roles NVARCHAR(4000) = NULL,
	@Enabled BIT,
	@Locale CHAR(2),
	@Verified BIT = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccount ([Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			[Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], 
			HistoryStamp, HistoryType, HistoryAccount, @AccountId
		FROM tAccount
		WHERE AccountId = @AccountId

		UPDATE tAccount 
		SET
			[Login] = @Login, [Password] = @Password, Email = @Email, [Enabled] = @Enabled, [Locale] = @Locale,
			Verified = ISNULL(@Verified, Verified), VerifyCode = ISNULL(@VerifyCode, VerifyCode),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountId = @AccountId
		
		IF @Roles IS NOT NULL BEGIN
			DELETE FROM tAccountRole WHERE AccountId = @AccountId
			INSERT INTO tAccountRole (AccountId, RoleId)
			SELECT @AccountId, r.RoleId
				FROM dbo.fStringToTable(@Roles, ';') x
				INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
		END

		SET @Result = @AccountId

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

-- EXEC pAccountModify @HistoryAccount = 1, @Login='system', @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Enabled = 1, @Locale = 'en', @AccountId = 1, @Email = 'roman.hude@admin.en'

ALTER PROCEDURE pAccountProfileCreate
	@HistoryAccount INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @ProfileId, @Value, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountProfileId = @Result

END
GO

ALTER PROCEDURE pAccountProfileDelete
	@HistoryAccount INT,
	@AccountProfileId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AccountProfileId IS NULL OR NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AccountProfileId=%d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAccountProfile
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountProfileId
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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

ALTER PROCEDURE pAccountProfileModify
	@HistoryAccount INT,
	@AccountProfileId INT,
	@AccountId INT = NULL,
	@ProfileId INT = NULL,
	@Value NVARCHAR(MAX),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountProfile WHERE AccountProfileId = @AccountProfileId AND HistoryId IS NULL) 
		RAISERROR('Invalid AccountProfileId %d', 16, 1, @AccountProfileId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccountProfile ( AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, @AccountProfileId
		FROM tAccountProfile
		WHERE AccountProfileId = @AccountProfileId

		UPDATE tAccountProfile
		SET
			AccountId=ISNULL(@AccountId, AccountId), ProfileId=ISNULL(@ProfileId, ProfileId), Value=@Value,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountProfileId = @AccountProfileId

		SET @Result = @AccountProfileId

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

ALTER PROCEDURE pAccountVerify
	@HistoryAccount INT,
	@AccountId INT,
	@VerifyCode NVARCHAR(1000),
	@Result BIT = 0 OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @ExpectedCode NVARCHAR(1000)
		SELECT @ExpectedCode = VerifyCode FROM tAccount WHERE AccountId = @AccountId
		
		IF ISNULL(@ExpectedCode, '') = ISNULL(@VerifyCode, '') BEGIN
		
			INSERT INTO tAccount ([Login], [Password], [Email], [Enabled], Verified, VerifyCode, [Locale], 
				HistoryStamp, HistoryType, HistoryAccount, HistoryId)
			SELECT
				[Login], [Password], [Email], [Enabled], Verified, VerifyCode, [Locale],
				HistoryStamp, HistoryType, HistoryAccount, @AccountId
			FROM tAccount
			WHERE AccountId = @AccountId

			UPDATE tAccount 
			SET
				Verified = 1,
				HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
			WHERE AccountId = @AccountId
			
			SET @Result = 1
		
		END

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

-- EXEC pAccountModify @HistoryAccount = 1, @Login='system', @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Enabled = 1, @Locale = 'en', @AccountId = 1, @Email = 'roman.hude@admin.en'

ALTER PROCEDURE pAccountVoteCreate
	@AccountId INT,
	@ObjectType INT,
	@ObjectId INT,
	@Rating INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountVote ( AccountId, ObjectType, ObjectId, Rating, [Date]) 
	VALUES ( @AccountId, @ObjectType, @ObjectId, @Rating, GETDATE())

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountVoteId = @Result

END
GO

ALTER PROCEDURE pAddressCreate
	@HistoryAccount INT,
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@City NVARCHAR(100) = '',
	@District NVARCHAR(100) = '',
	@Region NVARCHAR(100) = '',
	@Country NVARCHAR(100) = '',
	@State NVARCHAR(100)= '',	
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAddress ( City, Street, Zip, District, Region, Country, State, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @City, @Street, @Zip, @District, @Region, @Country, @State, @Notes,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AddressId = @Result

END
GO

ALTER PROCEDURE pAddressDelete
	@HistoryAccount INT,
	@AddressId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AddressId IS NULL OR NOT EXISTS(SELECT * FROM tAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AddressId=%d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAddress
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AddressId
		WHERE AddressId = @AddressId

		SET @Result = @AddressId

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

ALTER PROCEDURE pAddressModify
	@HistoryAccount INT,
	@AddressId INT,
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@City NVARCHAR(100) = '',
	@District NVARCHAR(100) = '',
	@Region NVARCHAR(100) = '',
	@Country NVARCHAR(100) = '',
	@State NVARCHAR(100)= '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid AddressId %d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAddress ( City, Street, Zip, District, Region, Country, State, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			City, Street, Zip, District, Region, Country, State, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @AddressId
		FROM tAddress
		WHERE AddressId = @AddressId

		UPDATE tAddress
		SET
			Street = @Street, Zip = @Zip, District = @District, Region = @Region, Country = @Country, State = @State, 
			City = @City,
			Notes = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AddressId = @AddressId

		SET @Result = @AddressId

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

ALTER PROCEDURE pArticleCategoryCreate
	@HistoryAccount INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Locale [char](2) = 'en', 
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cArticleCategory ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Name, @Code, @Notes, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleCategoryId = @Result

END
GO

ALTER PROCEDURE pArticleCategoryDelete
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleCategoryId IS NULL OR NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleCategoryId=%d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cArticleCategory
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleCategoryId
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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

ALTER PROCEDURE pArticleCategoryModify
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleCategoryId %d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cArticleCategory ( Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @ArticleCategoryId
		FROM cArticleCategory
		WHERE ArticleCategoryId = @ArticleCategoryId

		UPDATE cArticleCategory
		SET
			[Name] = @Name, [Code] = @Code, [Notes] = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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

ALTER PROCEDURE pArticleCommentCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tArticleComment ( CommentId, ArticleId ) VALUES ( @CommentId, @ArticleId )

END
GO
ALTER PROCEDURE pArticleCreate
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ArticleCategoryId, @Locale, @Icon, @Title, @Teaser, @ContentKeywords, @Content, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleId = @Result

END
GO

ALTER PROCEDURE pArticleDelete
	@HistoryAccount INT,
	@ArticleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ArticleId IS NULL OR NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ArticleId=%d', 16, 1, @ArticleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tArticle
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ArticleId
		WHERE ArticleId = @ArticleId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tArticle WHERE ArticleId = @ArticleId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tArticle SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ArticleId

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

ALTER PROCEDURE pArticleIncrementViewCount
	@ArticleId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ArticleId = @ArticleId

END
GO

ALTER PROCEDURE pArticleIncrementVote
	@ArticleId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	UPDATE tArticle 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ArticleId = @ArticleId

END
GO

ALTER PROCEDURE pArticleModify
	@HistoryAccount INT,
	@ArticleId INT,
	@ArticleCategoryId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tArticle WHERE ArticleId = @ArticleId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleId %d', 16, 1, @ArticleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tArticle ( ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId,
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @ArticleId
		FROM tArticle
		WHERE ArticleId = @ArticleId

		UPDATE tArticle
		SET
			ArticleCategoryId=ISNULL(@ArticleCategoryId, ArticleCategoryId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, ContentKeywords=@ContentKeywords,
			RoleId=@RoleId, UrlAliasId=@UrlAliasId, Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), 
			ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ArticleId = @ArticleId

		SET @Result = @ArticleId

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

ALTER PROCEDURE pArticleTagCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ArticleId FROM vArticleTags WHERE TagId=@TagId AND ArticleId=@ArticleId) BEGIN
		INSERT INTO tArticleTag ( TagId, ArticleId ) VALUES ( @TagId, @ArticleId )
	END

END
GO

ALTER PROCEDURE pBankContactCreate
	@HistoryAccount INT,
	@BankName NVARCHAR(100) = '',
	@BankCode NVARCHAR(100) = '',
	@AccountNumber NVARCHAR(100) = '',
	@IBAN NVARCHAR(100) = '',
	@SWIFT NVARCHAR(100) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBankContact ( AccountNumber, BankName, BankCode, SWIFT, IBAN,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountNumber, @BankName, @BankCode, @SWIFT, @IBAN,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BankContactId = @Result
END
GO

ALTER PROCEDURE pBankContactDelete
	@HistoryAccount INT,
	@BankContactId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @BankContactId IS NULL OR NOT EXISTS(SELECT * FROM tBankContact WHERE BankContactId = @BankContactId AND HistoryId IS NULL) 
		RAISERROR('Invalid @BankContactId=%d', 16, 1, @BankContactId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tBankContact
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @BankContactId
		WHERE BankContactId = @BankContactId

		SET @Result = @BankContactId

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

ALTER PROCEDURE pBankContactModify
	@HistoryAccount INT,
	@BankContactId INT,
	@BankName NVARCHAR(100) = '',
	@BankCode NVARCHAR(100) = '',
	@AccountNumber NVARCHAR(100) = '',
	@IBAN NVARCHAR(100) = '',
	@SWIFT NVARCHAR(100) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
	
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBankContact WHERE BankContactId = @BankContactId AND HistoryId IS NULL) 
		RAISERROR('Invalid BankContactId %d', 16, 1, @BankContactId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tBankContact ( AccountNumber, BankName, BankCode, IBAN, SWIFT,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountNumber, BankName, BankCode, IBAN, SWIFT,
			HistoryStamp, HistoryType, HistoryAccount, @BankContactId
		FROM tBankContact
		WHERE BankContactId = @BankContactId

		UPDATE tBankContact
		SET
			BankName = @BankName, BankCode = @BankCode, AccountNumber = @AccountNumber, IBAN = @IBAN, SWIFT = @SWIFT,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE BankContactId = @BankContactId

		SET @Result = @BankContactId

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

ALTER PROCEDURE pBlogCommentCreate
	@HistoryAccount INT,
	@BlogId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tBlogComment ( CommentId, BlogId ) VALUES ( @CommentId, @BlogId )

END
GO
ALTER PROCEDURE pBlogCreate
	@HistoryAccount INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @Locale, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @RoleId, @UrlAliasId, 
		@Country, @City, @Approved, @ReleaseDate, @ExpiredDate, @EnableComments, @Visible,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BlogId = @Result

END
GO

ALTER PROCEDURE pBlogDelete
	@HistoryAccount INT,
	@BlogId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @BlogId IS NULL OR NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid @BlogId=%d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tBlog
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @BlogId
		WHERE BlogId = @BlogId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tBlog WHERE BlogId = @BlogId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tBlog SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @BlogId

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

ALTER PROCEDURE pBlogIncrementViewCount
	@BlogId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE BlogId = @BlogId

END
GO

ALTER PROCEDURE pBlogIncrementVote
	@BlogId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	UPDATE tBlog 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE BlogId = @BlogId

END
GO

ALTER PROCEDURE pBlogModify
	@HistoryAccount INT,
	@BlogId INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @BlogId
		FROM tBlog
		WHERE BlogId = @BlogId

		UPDATE tBlog
		SET
			AccountId=ISNULL(@AccountId, AccountId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, ContentKeywords=@ContentKeywords, RoleId=@RoleId, UrlAliasId=@UrlAliasId,
			Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE BlogId = @BlogId

		SET @Result = @BlogId

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

ALTER PROCEDURE pBlogTagCreate
	@HistoryAccount INT,
	@BlogId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, BlogId FROM vBlogTags WHERE TagId=@TagId AND BlogId=@BlogId) BEGIN
		INSERT INTO tBlogTag ( TagId, BlogId ) VALUES ( @TagId, @BlogId )
	END

END
GO

ALTER PROCEDURE pCommentCreate
	@HistoryAccount INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@Date DATETIME,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tComment ( ParentId, AccountId, [Date], Title, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ParentId, @AccountId, @Date, @Title, @Content, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CommentId = @Result

END
GO

ALTER PROCEDURE pCommentDelete
	@HistoryAccount INT,
	@CommentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CommentId IS NULL OR NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CommentId=%d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tComment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CommentId
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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

ALTER PROCEDURE pCommentIncrementVote
	@CommentId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	UPDATE tComment 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE CommentId = @CommentId

END
GO

ALTER PROCEDURE pCommentModify
	@HistoryAccount INT,
	@CommentId INT,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tComment WHERE CommentId = @CommentId AND HistoryId IS NULL) 
		RAISERROR('Invalid CommentId %d', 16, 1, @CommentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tComment ( Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, @CommentId
		FROM tComment
		WHERE CommentId = @CommentId

		UPDATE tComment
		SET
			Title = @Title, Content = @Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CommentId = @CommentId

		SET @Result = @CommentId

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

ALTER PROCEDURE pFaqCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tFaq ( Locale, [Order], Question, Answer,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Question, @Answer,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT FaqId = @Result

END
GO

ALTER PROCEDURE pFaqDelete
	@HistoryAccount INT,
	@FaqId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @FaqId IS NULL OR NOT EXISTS(SELECT * FROM tFaq WHERE FaqId = @FaqId AND HistoryId IS NULL) 
		RAISERROR('Invalid @FaqId=%d', 16, 1, @FaqId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tFaq
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @FaqId
		WHERE FaqId = @FaqId

		SET @Result = @FaqId

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

ALTER PROCEDURE pFaqModify
	@HistoryAccount INT,
	@FaqId INT,
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tFaq WHERE FaqId = @FaqId AND HistoryId IS NULL) 
		RAISERROR('Invalid FaqId %d', 16, 1, @FaqId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tFaq ( Locale, [Order], Question, Answer,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], Question, Answer,
			HistoryStamp, HistoryType, HistoryAccount, @FaqId
		FROM tFaq
		WHERE FaqId = @FaqId

		UPDATE tFaq
		SET
			[Order] = @Order, Question = @Question, Answer = @Answer,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE FaqId = @FaqId

		SET @Result = @FaqId

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

ALTER PROCEDURE pImageGalleryCommentCreate
	@HistoryAccount INT,
	@ImageGalleryId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryComment ( CommentId, ImageGalleryId ) VALUES ( @CommentId, @ImageGalleryId )

END
GO
ALTER PROCEDURE pImageGalleryCreate
	@HistoryAccount INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGallery ( [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Name, @RoleId, @Visible, @UrlAliasId, ISNULL(@Date,GETDATE()), @EnableComments, @EnableVotes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryId = @Result

END
GO

ALTER PROCEDURE pImageGalleryDelete
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryId IS NULL OR NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryId=%d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGallery
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryId
		WHERE ImageGalleryId = @ImageGalleryId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tImageGallery SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END			

		SET @Result = @ImageGalleryId

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

ALTER PROCEDURE pImageGalleryIncrementViewCount
	@ImageGalleryId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	UPDATE tImageGallery SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryId = @ImageGalleryId

END
GO

ALTER PROCEDURE pImageGalleryItemCommentCreate
	@HistoryAccount INT,
	@ImageGalleryItemId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryItemComment ( CommentId, ImageGalleryItemId ) VALUES ( @CommentId, @ImageGalleryItemId )

END
GO
ALTER PROCEDURE pImageGalleryItemCreate
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGalleryItem ( ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ImageGalleryId, @VirtualPath, @VirtualThumbnailPath, @Position, GETDATE(), @Description,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryItemId = @Result

END
GO

ALTER PROCEDURE pImageGalleryItemDelete
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ImageGalleryItemId IS NULL OR NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ImageGalleryItemId=%d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tImageGalleryItem
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ImageGalleryItemId
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

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

ALTER PROCEDURE pImageGalleryItemIncrementViewCount
	@ImageGalleryItemId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO

ALTER PROCEDURE pImageGalleryItemIncrementVote
	@ImageGalleryItemId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	UPDATE tImageGalleryItem 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ImageGalleryItemId = @ImageGalleryItemId

END
GO

ALTER PROCEDURE pImageGalleryItemModify
	@HistoryAccount INT,
	@ImageGalleryItemId INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGalleryItem WHERE ImageGalleryItemId = @ImageGalleryItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryItemId %d', 16, 1, @ImageGalleryItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGalleryItem ( ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryItemId
		FROM tImageGalleryItem
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		UPDATE tImageGalleryItem
		SET
			VirtualPath = ISNULL(@VirtualPath, VirtualPath), VirtualThumbnailPath = ISNULL(@VirtualThumbnailPath, VirtualThumbnailPath), Description = @Description, [Position] = @Position,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryItemId = @ImageGalleryItemId

		SET @Result = @ImageGalleryItemId

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

ALTER PROCEDURE pImageGalleryModify
	@HistoryAccount INT,
	@ImageGalleryId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tImageGallery WHERE ImageGalleryId = @ImageGalleryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ImageGalleryId %d', 16, 1, @ImageGalleryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tImageGallery ( [Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			[Name], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryId
		FROM tImageGallery
		WHERE ImageGalleryId = @ImageGalleryId

		UPDATE tImageGallery
		SET
			[Name]=@Name, [Date]=@Date, RoleId=@RoleId, Visible=@Visible, UrlAliasId=@UrlAliasId, EnableComments=@EnableComments, EnableVotes=@EnableVotes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ImageGalleryId = @ImageGalleryId

		SET @Result = @ImageGalleryId

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

ALTER PROCEDURE pImageGalleryTagCreate
	@HistoryAccount INT,
	@ImageGalleryId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ImageGalleryId FROM vImageGalleryTags WHERE TagId=@TagId AND ImageGalleryId=@ImageGalleryId) BEGIN
		INSERT INTO tImageGalleryTag ( TagId, ImageGalleryId ) VALUES ( @TagId, @ImageGalleryId )
	END

END
GO

ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO

ALTER PROCEDURE pMenuDelete
	@HistoryAccount INT,
	@MenuId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @MenuId IS NULL OR NOT EXISTS(SELECT * FROM tMenu WHERE MenuId = @MenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid @MenuId=%d', 16, 1, @MenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tMenu
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @MenuId
		WHERE MenuId = @MenuId

		SET @Result = @MenuId

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

ALTER PROCEDURE pMenuModify
	@HistoryAccount INT,
	@MenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tMenu WHERE MenuId = @MenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid MenuId %d', 16, 1, @MenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE MenuId = @MenuId

		SET @Result = @MenuId

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

ALTER PROCEDURE pNavigationMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO

ALTER PROCEDURE pNavigationMenuDelete
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NavigationMenuId IS NULL OR NOT EXISTS(SELECT * FROM tNavigationMenu WHERE NavigationMenuId = @NavigationMenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NavigationMenuId=%d', 16, 1, @NavigationMenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tNavigationMenu
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NavigationMenuId
		WHERE NavigationMenuId = @NavigationMenuId

		SET @Result = @NavigationMenuId

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

ALTER PROCEDURE pNavigationMenuItemCreate
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @NavigationMenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO

ALTER PROCEDURE pNavigationMenuItemDelete
	@HistoryAccount INT,
	@NavigationMenuItemId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NavigationMenuItemId IS NULL OR NOT EXISTS(SELECT * FROM tNavigationMenuItem WHERE NavigationMenuItemId = @NavigationMenuItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NavigationMenuItemId=%d', 16, 1, @NavigationMenuItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tNavigationMenuItem
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NavigationMenuItemId
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		SET @Result = @NavigationMenuItemId

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

ALTER PROCEDURE pNavigationMenuItemModify
	@HistoryAccount INT,
	@NavigationMenuItemId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNavigationMenuItem WHERE NavigationMenuItemId = @NavigationMenuItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid NavigationMenuItemId %d', 16, 1, @NavigationMenuItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuItemId
		FROM tNavigationMenuItem
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		UPDATE tNavigationMenuItem
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		SET @Result = @NavigationMenuItemId

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

ALTER PROCEDURE pNavigationMenuModify
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNavigationMenu WHERE NavigationMenuId = @NavigationMenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid NavigationMenuId %d', 16, 1, @NavigationMenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuId
		FROM tNavigationMenu
		WHERE NavigationMenuId = @NavigationMenuId

		UPDATE tNavigationMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, UrlAliasId = @UrlAliasId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NavigationMenuId = @NavigationMenuId

		SET @Result = @NavigationMenuId

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

ALTER PROCEDURE pNewsCreate
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Head NVARCHAR(255) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Date, @Icon, @Head, @Description, @Content, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsId = @Result

END
GO

ALTER PROCEDURE pNewsDelete
	@HistoryAccount INT,
	@NewsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NewsId IS NULL OR NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NewsId=%d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY
	
		UPDATE tNews
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NewsId
		WHERE NewsId = @NewsId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tNews WHERE NewsId = @NewsId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tNews SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END
		
		SET @Result = @NewsId

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

ALTER PROCEDURE pNewsletterCreate
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Subject NVARCHAR(255) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Attachment IMAGE = NULL,
	@SendDate DATETIME = NULL,
	@Roles NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNewsletter ( Locale, [Date], Icon, Subject, Attachment, Content, Roles, SendDate )
	VALUES ( @Locale, @Date, @Icon, @Subject, @Attachment, @Content, @Roles, @SendDate )

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsletterId = @Result

END
GO

ALTER PROCEDURE pNewsletterDelete
	@NewsletterId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NewsletterId IS NULL OR NOT EXISTS(SELECT * FROM tNewsletter WHERE NewsletterId = @NewsletterId) 
		RAISERROR('Invalid @NewsletterId=%d', 16, 1, @NewsletterId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tNewsletter WHERE NewsletterId = @NewsletterId
		SET @Result = @NewsletterId

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

ALTER PROCEDURE pNewsletterModify
	@NewsletterId INT,
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Subject NVARCHAR(255) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Attachment IMAGE = NULL,
	@SendDate DATETIME = NULL,
	@Roles NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNewsletter WHERE NewsletterId = @NewsletterId ) 
		RAISERROR('Invalid NewsletterId %d', 16, 1, @NewsletterId);

	BEGIN TRANSACTION;

	BEGIN TRY
		UPDATE tNewsletter
		SET
			[Date] = @Date, Icon = @Icon, Subject = @Subject, Content = @Content, Attachment = @Attachment, Roles = @Roles, SendDate = @SendDate
		WHERE NewsletterId = @NewsletterId

		SET @Result = @NewsletterId

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

ALTER PROCEDURE pNewsModify
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@NewsId INT,
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Head NVARCHAR(255) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid NewsId %d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Date], Icon, Head, Description, Content, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Head = @Head, Description = @Description, Content = @Content, UrlAliasId=@UrlAliasId, 
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NewsId = @NewsId

		SET @Result = @NewsId

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

ALTER PROCEDURE pOrganizationCreate
	@HistoryAccount INT,
	@AccountId INT = NULL,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @RegisteredAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @Result = @RegisteredAddressId OUTPUT

		DECLARE @CorrespondenceAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @Result = @CorrespondenceAddressId OUTPUT
		
		DECLARE @InvoicingAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @Result = @InvoicingAddressId OUTPUT

		DECLARE @BankContactId INT
		EXEC pBankContactCreate @HistoryAccount = @HistoryAccount, @Result = @BankContactId OUTPUT

		DECLARE @ContactPersonId INT
		EXEC pPersonCreate @HistoryAccount = @HistoryAccount, @Result = @ContactPersonId OUTPUT

		INSERT INTO tOrganization (
			AccountId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
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

/*
DECLARE @Result INT
EXEC pAccountCreate @HistoryAccount = NULL, @Login = 'mothiva', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pOrganizationCreate @HistoryAccount=1, @AccountId=@Result, @Name='Mothiva, s.r.o.'

SELECT * FROM tAccount
SELECT * from vOrganizations
*/

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
		FROM tCorPerson p
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

ALTER PROCEDURE pOrganizationModify
	@HistoryAccount INT,
	@OrganizationId INT,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
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
			Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			HistoryStamp, HistoryType, HistoryAccount, @OrganizationId
		FROM tOrganization
		WHERE OrganizationId = @OrganizationId

		UPDATE tOrganization 
		SET
			Id1 = @Id1, Id2 = @Id2, Id3 = @Id3, Name = @Name, Notes = @Notes, Web = @Web, 
			ContactEMail = @ContactEMail, ContactPhone = @ContactPhone, ContactMobile = @ContactMobile, 
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

/*
exec pOrganizationModify 1, 1, -2, '1', '2', '3', 'xx', @Web='http://mothiva.com'
*/
ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
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

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO

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
ALTER PROCEDURE pPageModify
	@HistoryAccount INT,
	@PageId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
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

	IF NOT EXISTS(SELECT * FROM tPage WHERE PageId = @PageId AND HistoryId IS NULL)  BEGIN
		RAISERROR('Invalid PageId %d', 16, 1, @PageId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
	
		-- Normalizacia nazvu
		SET @Name = dbo.fMakeAnsi(@Name)

		INSERT INTO tPage (MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @PageId
		FROM tPage
		WHERE PageId = @PageId

		UPDATE tPage
		SET
			MasterPageId = @MasterPageId, RoleId = @RoleId,
			Locale = @Locale, [Name] = @Name, Title = @Title, UrlAliasId = @UrlAliasId, Content = @Content, ContentKeywords = @ContentKeywords,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PageId = @PageId

		SET @Result = @PageId

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

ALTER PROCEDURE pPaidServiceModify
	@HistoryAccount INT,
	@PaidServiceId INT,
	@CreditCost DECIMAL(19,2) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cPaidService WHERE PaidServiceId = @PaidServiceId AND HistoryId IS NULL) 
		RAISERROR('Invalid PaidServiceId %d', 16, 1, @PaidServiceId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cPaidService ( [Name], [Notes], CreditCost,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			[Name], [Notes], CreditCost,
			HistoryStamp, HistoryType, HistoryAccount, @PaidServiceId
		FROM cPaidService
		WHERE PaidServiceId = @PaidServiceId

		UPDATE cPaidService
		SET
			CreditCost = @CreditCost,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PaidServiceId = @PaidServiceId

		SET @Result = @PaidServiceId

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

ALTER PROCEDURE pPersonCreate
	@HistoryAccount INT,
	@AccountId INT = NULL,
	@Title NVARCHAR(20) = '',
	@FirstName NVARCHAR(100) = '',
	@LastName NVARCHAR(100) = '',
	@Email NVARCHAR(100) = '',
	@Phone NVARCHAR(100) = NULL, @Mobile NVARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @AddressHomeId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @Result = @AddressHomeId OUTPUT

		DECLARE @AddressTempId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @Result = @AddressTempId OUTPUT
		

		INSERT INTO tPerson ( AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount)
		VALUES ( @AccountId, @Title, @FirstName, @LastName, @Email,
			@Phone, @Mobile, @AddressHomeId, @AddressTempId,
			GETDATE(), 'C', @HistoryAccount)
			
		SET @Result = SCOPE_IDENTITY()
	
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

		SET @Result = NULL

	END CATCH	
	
	SELECT PersonId = @Result

END
GO

/*
DECLARE @Result INT
EXEC pAccountCreate @HistoryAccount = NULL, @Login = 'hudy', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pPersonCreate @HistoryAccount = 1, @AccountId = @Result, @FirstName='Roman', @LastName='Hudec', @Result = @Result OUTPUT
SELECT * FROM tPerson
SELECT * FROM tAccount
*/
ALTER PROCEDURE pPersonDelete
	@HistoryAccount INT,
	@PersonId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPerson WHERE PersonId = @PersonId AND HistoryId IS NULL) 
		RAISERROR('Invalid PersonId %d', 16, 1, @PersonId);

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @AddressHomeId INT, @AddressTempId INT
		SELECT @AddressHomeId = AddressHomeId, @AddressTempId = AddressTempId 
		FROM tPerson WHERE PersonId = @PersonId

		-- mark home address as deleted
		IF @AddressHomeId IS NOT NULL
			EXEC pAddressDelete @HistoryAccount = @HistoryAccount, @AddressId = @AddressHomeId
		

		-- mark temp address as deleted
		IF @AddressTempId IS NOT NULL
			EXEC pAddressDelete @HistoryAccount = @HistoryAccount, @AddressId = @AddressTempId

	
		-- mark person as deleted
		UPDATE tPerson 
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PersonId
		WHERE PersonId = @PersonId

		SET @Result = @PersonId

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

ALTER PROCEDURE pPersonModify
	@HistoryAccount INT,
	@PersonId INT,
	@Title NVARCHAR(20),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@Email NVARCHAR(100),
	@Phone NVARCHAR(100), @Mobile NVARCHAR(100),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPerson WHERE PersonId = @PersonId AND HistoryId IS NULL) 
		RAISERROR('Invalid PersonId %d', 16, 1, @PersonId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tPerson ( AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			AccountId, Title, FirstName, LastName, Email, 
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount, @PersonId
		FROM tPerson
		WHERE PersonId = @PersonId

		UPDATE tPerson 
		SET
			Title = @Title, FirstName = @FirstName, LastName = @LastName, Email = @Email,
			Phone = @Phone, Mobile = @Mobile,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PersonId = @PersonId

		SET @Result = @PersonId

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

/*
DECLARE @Result INT
--EXEC pPersonModify @HistoryAccount = 7, @PersonId = 19, @FirstName='Jozef', @LastName='Prídavok'
EXEC pPersonModify @HistoryAccount = 7, @PersonId = 24, @FirstName='Roman', @LastName='Hudec'
SELECT @Result
SELECT * FROM tPerson

select * from tPerson where historyid=19 order by historystamp
*/
ALTER PROCEDURE pPollAnswerCreate
	@PollOptionId INT,
	@IP NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollAnswer ( PollOptionId, IP ) 
	VALUES ( @PollOptionId, @IP )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollAnswerId = @Result

END
GO

ALTER PROCEDURE pPollCreate
	@HistoryAccount INT,
	@Closed BIT = 0,
	@Locale [char](2) = 'en', 
	@Question NVARCHAR(1000),
	@DateFrom DATETIME,
	@DateTo DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPoll ( Closed, Locale, Question, DateFrom, DateTo, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Closed, @Locale, @Question, @DateFrom, @DateTo, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PollId = @Result

END
GO

ALTER PROCEDURE pPollDelete
	@HistoryAccount INT,
	@PollId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PollId IS NULL OR NOT EXISTS(SELECT * FROM tPoll WHERE PollId = @PollId AND HistoryId IS NULL) 
		RAISERROR('Invalid @PollId=%d', 16, 1, @PollId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tPoll
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PollId
		WHERE PollId = @PollId

		SET @Result = @PollId

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

ALTER PROCEDURE pPollModify
	@HistoryAccount INT,
	@PollId INT,
	@Closed BIT = 0,
	@Question NVARCHAR(1000),
	@DateFrom DATETIME,
	@DateTo DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPoll WHERE PollId = @PollId AND HistoryId IS NULL) 
		RAISERROR('Invalid PollId %d', 16, 1, @PollId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tPoll ( Closed, Locale, Question, DateFrom, DateTo, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Closed, Locale, Question, DateFrom, DateTo, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @PollId
		FROM tPoll
		WHERE PollId = @PollId

		UPDATE tPoll
		SET
			Closed = @Closed, Question = @Question, DateFrom = @DateFrom, DateTo = @DateTo, Icon = @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PollId = @PollId

		SET @Result = @PollId

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

ALTER PROCEDURE pPollOptionCreate
	@PollId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollOption ( PollId, [Order], [Name] )
	VALUES ( @PollId, @Order, @Name )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollOptionId = @Result

END
GO

ALTER PROCEDURE pPollOptionDelete
	@PollOptionId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PollOptionId IS NULL OR NOT EXISTS(SELECT * FROM tPollOption WHERE PollOptionId = @PollOptionId ) 
		RAISERROR('Invalid @PollOptionId=%d', 16, 1, @PollOptionId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tPollAnswer WHERE PollOptionId = @PollOptionId
		DELETE FROM tPollOption WHERE PollOptionId = @PollOptionId

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

ALTER PROCEDURE pPollOptionModify
	@PollOptionId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPollOption WHERE PollOptionId = @PollOptionId ) 
		RAISERROR('Invalid PollOptionId %d', 16, 1, @PollOptionId);

	BEGIN TRANSACTION;

	BEGIN TRY


		UPDATE tPollOption
		SET [Order] = @Order, [Name] = @Name
		WHERE PollOptionId = @PollOptionId

		SET @Result = @PollOptionId

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

ALTER PROCEDURE pProvidedServiceCreate
	@HistoryAccount INT,
	@AccountId INT,
	@PaidServiceId INT,
	@ObjectId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
	INSERT INTO tProvidedService ( AccountId, PaidServiceId, ObjectId, ServiceDate ) 
	VALUES ( @AccountId, @PaidServiceId, @ObjectId, GETDATE() )
	SET @Result = SCOPE_IDENTITY()
	
	DECLARE @CreditCost DECIMAL(19,2)
	SELECT @CreditCost = CreditCost FROM vPaidServices WHERE PaidServiceId = @PaidServiceId
	
	--Update aktualny kredit pouzivatela
	IF @CreditCost IS NOT NULL
	BEGIN
		DECLARE @AccountCreditId INT, @CurrentCredit DECIMAL(19,2), @NewCredit DECIMAL(19,2)
		SET @NewCredit = @CreditCost*(-1)
		-- A neexistuje zaznam o kredite pouzivatela, vytvorim ho a odpocitam jeho credit od aktualneho kreditu
		SELECT @AccountCreditId=AccountCreditId, @CurrentCredit=Credit FROM vAccountsCredit WHERE AccountId=@AccountId 	
		IF @AccountCreditId IS NULL
		BEGIN
			EXEC pAccountCreditCreate @HistoryAccount = @HistoryAccount, @AccountId = @AccountId, @Credit=@NewCredit
		END	
		ELSE
		BEGIN
			SET @NewCredit = ( @CurrentCredit - @CreditCost)
			EXEC pAccountCreditModify @HistoryAccount = @HistoryAccount, @AccountCreditId = @AccountCreditId, @Credit=@NewCredit
		END

	END	
	
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

		SET @Result = NULL

	END CATCH	

	SELECT ProvidedServiceId = @Result

END
GO

ALTER PROCEDURE pRoleCreate
	@Name NVARCHAR(200),
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tRole ( [Name], [Notes] ) VALUES ( @Name, @Notes )
	SET @Result = SCOPE_IDENTITY()

	SELECT RoleId = @Result

END
GO

ALTER PROCEDURE pRoleDelete
	@RoleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @RoleId < 0  
		RAISERROR ('Can not delete system role!', 16, 1);
		
	IF @RoleId IS NULL OR NOT EXISTS(SELECT * FROM tRole WHERE RoleId = @RoleId ) 
		RAISERROR('Invalid @RoleId=%d', 16, 1, @RoleId);
		
	DELETE FROM tRole WHERE RoleId = @RoleId

END	

GO

ALTER PROCEDURE pSearchArticles
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = art.ArticleId, art.Title, 
		Content = art.Teaser + art.ContentKeywords, 
		UrlAlias = a.Alias 
	FROM tArticle art INNER JOIN
	tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
	WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND
	(
		art.Title LIKE '%'+@Keywords+'%' OR 
		art.Teaser LIKE '%'+@Keywords+'%' OR 
		art.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSearchBlogs
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = b.BlogId, b.Title, 
		Content = b.Teaser + b.ContentKeywords, 
		UrlAlias = a.Alias 
	FROM tBlog b INNER JOIN
	tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
	WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND
	(
		b.Title LIKE '%'+@Keywords+'%' OR 
		b.Teaser LIKE '%'+@Keywords+'%' OR 
		b.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSearchImageGalleries
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = i.ImageGalleryId, Title = i.Name, 
		Content = NULL, 
		UrlAlias = a.Alias 
	FROM tImageGallery i INNER JOIN
	tUrlAlias a ON a.UrlAliasId = i.UrlAliasId
	WHERE i.HistoryId IS NULL AND
	(
		i.Name LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Head, 
		Content = n.Description + ' ' + n.Content , UrlAlias = a.Alias 
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND
	(
		n.Head LIKE '%'+@Keywords+'%' OR 
		n.Description LIKE '%'+@Keywords+'%' OR 
		n.Content LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSearchPages
	@Keywords NVARCHAR(255),
	@Locale CHAR(2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.PageId, p.Title, Content = p.ContentKeywords, UrlAlias = a.Alias 
	FROM tPage p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pTagCreate
	@HistoryAccount INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tTag ( Tag, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Tag, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT TagId = @Result

END
GO

ALTER PROCEDURE pTagDelete
	@HistoryAccount INT,
	@TagId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @TagId IS NULL OR NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid @TagId=%d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tTag
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @TagId
		WHERE TagId = @TagId

		SET @Result = @TagId

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

ALTER PROCEDURE pTagModify
	@HistoryAccount INT,
	@TagId INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid TagId %d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTag ( Tag,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Tag,
			HistoryStamp, HistoryType, HistoryAccount, @TagId
		FROM tTag
		WHERE TagId = @TagId

		UPDATE tTag
		SET
			Tag = ISNULL(@Tag, Tag ),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TagId = @TagId

		SET @Result = @TagId

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

ALTER PROCEDURE pTranslationCreateEx
	@HistoryAccount INT,
	@Vocabulary NVARCHAR(100),
	@Locale CHAR(2),
	@Term NVARCHAR(500), 
	@Translation NVARCHAR(4000), 
	@Notes NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @VocabularyId INT
		SELECT @VocabularyId = VocabularyId FROM vVocabularies WHERE Name = @Vocabulary AND Locale = @Locale		
		IF @VocabularyId IS NULL BEGIN
			INSERT INTO tVocabulary(Locale, Name, Notes) VALUES (@Locale, @Vocabulary, '')
			SET @VocabularyId = SCOPE_IDENTITY()
		END

		DECLARE @TranslationId INT
		SELECT @TranslationId = TranslationId FROM vTranslations WHERE VocabularyId = @VocabularyId AND Term = @Term		
		IF @TranslationId IS NULL BEGIN
			INSERT INTO tTranslation(VocabularyId, Term, Translation, Notes,
				HistoryStamp, HistoryType, HistoryAccount, HistoryId) 
			VALUES (@VocabularyId, @Term, @Translation, @Notes,
				GETDATE(), 'C', @HistoryAccount, NULL)
			SET @TranslationId = SCOPE_IDENTITY()
		END

		SET @Result = @TranslationId

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

ALTER PROCEDURE pTranslationModify
	@HistoryAccount INT,
	@TranslationId INT,
	@Translation NVARCHAR(4000) = NULL, 
	@Notes NVARCHAR(4000), 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTranslation WHERE TranslationId = @TranslationId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @Translation %d', 16, 1, @Translation);
		RETURN;
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTranslation (VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @TranslationId
		FROM tTranslation
		WHERE Translation = @Translation

		UPDATE tTranslation
		SET
			Translation = @Translation, Notes = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TranslationId = @TranslationId

		SET @Result = @TranslationId

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

ALTER PROCEDURE pUrlAliasCreate
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist!' , 16, 1, @Url, @Locale);
		RETURN
	END	

	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias (Url, Locale, Alias, [Name] ) 
	VALUES ( @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO

ALTER PROCEDURE pUrlAliasDelete
	@UrlAliasId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @UrlAliasId IS NULL OR NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId) BEGIN
		RAISERROR('Invalid @UrlAliasId=%d', 16, 1, @UrlAliasId);
		RETURN
	END

	UPDATE tPage SET UrlAliasId = NULL WHERE UrlAliasId = @UrlAliasId
	DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId

	SET @Result = @UrlAliasId

END	

GO

ALTER PROCEDURE pUrlAliasModify
	@UrlAliasId INT,
	@Url NVARCHAR(2000),
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId)  BEGIN
		RAISERROR('Invalid UrlAliasId %d', 16, 1, @UrlAliasId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
		SET @Alias = REPLACE( @Alias, '.', '-')
		SET @Alias = REPLACE( @Alias, '_', '-')

		UPDATE tUrlAlias
		SET Url = ISNULL(@Url, Url ), Alias = ISNULL(dbo.fMakeAnsi(@Alias), Alias), [Name] = ISNULL(@Name, [Name] )
		WHERE UrlAliasId = @UrlAliasId

		SET @Result = @UrlAliasId

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
-- Init
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- default account & credentials

SET IDENTITY_INSERT tRole ON
-- role <= -100 su role, ktore nie je mozne prostrednictvom UI odoberat.
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-100, 'RegisteredUser', 'Registered user')
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-1, 'Administrator', 'System administrator')
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-2, 'Newsletter', 'Information bulletin')
SET IDENTITY_INSERT tRole OFF

EXEC pAccountCreate @HistoryAccount = NULL,
	@Login = 'system', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', -- @Password=0987oiuk
	@Roles = 'Administrator', @Verified = 1

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

-- EOF Init
------------------------------------------------------------------------------------------------------------------------

