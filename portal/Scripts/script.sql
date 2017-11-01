
			USE eurona
			GO
		
------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------
-- Address

CREATE TABLE [tAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
-- SupportedLocale
CREATE TABLE [cSupportedLocale](
	[SupportedLocaleId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Icon] [nvarchar](255) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cSupportedLocale] PRIMARY KEY CLUSTERED ([SupportedLocaleId] ASC)
)
GO

ALTER TABLE [cSupportedLocale]  WITH CHECK 
	ADD  CONSTRAINT [FK_cSupportedLocale_cSupportedLocale] FOREIGN KEY([HistoryId])
	REFERENCES [cSupportedLocale] (SupportedLocaleId)
GO
ALTER TABLE [cSupportedLocale] CHECK CONSTRAINT [FK_cSupportedLocale_cSupportedLocale]
GO

ALTER TABLE [cSupportedLocale]  WITH CHECK 
	ADD  CONSTRAINT [CK_cSupportedLocale_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cSupportedLocale] CHECK CONSTRAINT [CK_cSupportedLocale_HistoryType]
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tSysUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tSysUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO
CREATE TABLE [tCMSUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tCMSUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO
------------------------------------------------------------------------------------------------------------------------
-- Account
CREATE TABLE [tAccount](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[Default] [bit] NULL CONSTRAINT [DF_tMasterPage_Default]  DEFAULT (0),
	[PageUrl] NVARCHAR(255) NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Contents] [int] NULL CONSTRAINT [DF_tMasterPage_Contents]  DEFAULT (1),
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
	[ParentId] [int] NULL,
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
	ADD  CONSTRAINT [FK_tPage_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_ParentId]
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
	[InstanceId] [int] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tMenu_Locale]  DEFAULT ('en'),
	[Name] [nvarchar](100) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[RoleId] [int] NULL,
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
	[InstanceId] [int] NULL,
	[MenuId] [int] NULL,
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
	ADD  CONSTRAINT [FK_tNavigationMenu_MenuId] FOREIGN KEY([MenuId])
	REFERENCES [tMenu] ([MenuId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_MenuId]
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tNews_Locale]  DEFAULT ('en'),
	[Date] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[ContentKeywords] [nvarchar](MAX) NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(2000) NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
	[InstanceId] [int] NULL,
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
CREATE VIEW vSupportedLocales AS SELECT A=1
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

CREATE FUNCTION fCompareStrings(@A NVARCHAR(1000), @B NVARCHAR(1000)) RETURNS INT AS BEGIN RETURN 0 END
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
-- Supported locale
CREATE PROCEDURE pSupportedLocaleCreate AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleModify AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pSupportedLocaleDelete AS BEGIN SET NOCOUNT ON; END
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
-- Comments
CREATE PROCEDURE pSearchArticleComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchBlogComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryComments AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pSearchImageGalleryItemComments AS BEGIN SET NOCOUNT ON; END
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

ALTER FUNCTION fCompareStrings
(
	@A NVARCHAR(1000), 
	@B NVARCHAR(1000)
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	IF @A IS NULL AND @B IS NULL RETURN 1
	IF LTRIM(RTRIM(LOWER(dbo.fMakeAnsi(@A)))) = LTRIM(RTRIM(LOWER(dbo.fMakeAnsi(@B)))) RETURN 1
	RETURN 0
END
GO


/*
SELECT dbo.fCompareStrings(NULL, NULL)
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Pridavok')
SELECT dbo.fCompareStrings('Jozef Prídavok', '')
SELECT dbo.fCompareStrings('Jozef Prídavok', NULL)
SELECT dbo.fCompareStrings('Jozef Prídavok', 'jozef pridavok')
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Prídavok ')
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Prydavok ')
SELECT dbo.fCompareStrings('BanskÁ BystricA', ' banska bystrica')
SELECT dbo.fCompareStrings('Praha', ' praha ')
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
	ap.AccountProfileId, ap.InstanceId, ap.AccountId, ap.ProfileId, ap.[Value], ProfileType = p.Type, ProfileName = p.Name
FROM tAccountProfile ap 
INNER JOIN tProfile p ON p.ProfileId = ap.ProfileId
WHERE ap.HistoryId IS NULL
GO

ALTER VIEW vAccountRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.InstanceId, ar.AccountRoleId, r.[RoleId], RoleName = r.[Name]
FROM tRole r
INNER JOIN tAccountRole ar (NOLOCK) ON ar.RoleId = r.RoleId
INNER JOIN tAccount a (NOLOCK) ON ar.AccountId = a.AccountId
GO

-- SELECT * FROM vAccountRoles


ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
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
	[AccountCreditId], [InstanceId], [AccountId], [Credit]
FROM
	tAccountCredit
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vAccountVotes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	AccountVoteId, InstanceId, ObjectType, ObjectId, AccountId, Rating, [Date]
FROM tAccountVote
GO

ALTER VIEW vAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [InstanceId], [City], [Street], [Zip], [Notes], [District], [Region], [Country], [State]
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
	a.ArticleCategoryId, a.[InstanceId], a.[Name], a.[Code], a.[Locale], a.[Notes], 
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
	ac.ArticleCommentId, ac.InstanceId, ac.ArticleId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
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
	a.ArticleId, a.InstanceId, a.Locale, a.Icon, a.Title, a.Teaser, a.Content, a.RoleId, a.Country,
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
	tArticle a 
	INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
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
	[BankContactId], [InstanceId], [BankName], [BankCode], [AccountNumber], [IBAN], [SWIFT]
FROM
	tBankContact b
WHERE
	b.HistoryId IS NULL
GO

--SELECT * FROM vBankContacts


ALTER VIEW vBlogComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.BlogCommentId, ac.InstanceId, ac.BlogId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
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
	b.BlogId, b.InstanceId, b.Locale, b.Icon, b.Title, b.Teaser, b.Content, b.RoleId, b.Country,
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
	[CommentId], [InstanceId], [ParentId], [AccountId], [Date], [Title], [Content], [Votes], [TotalRating]
FROM
	tComment
WHERE
	HistoryId IS NULL
GO

ALTER VIEW vFaqs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[FaqId], [InstanceId], [Locale], [Order], [Question], [Answer]
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
	g.ImageGalleryId, g.InstanceId, g.RoleId, g.[Name], g.[Description], g.[Date], g.EnableComments, g.EnableVotes, g.Visible,
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
	igc.ImageGalleryCommentId, igc.InstanceId, igc.ImageGalleryId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
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
	igic.ImageGalleryItemCommentId, igic.InstanceId, igic.ImageGalleryItemId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
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
	ImageGalleryItemId, InstanceId, ImageGalleryId, [VirtualPath], [VirtualThumbnailPath], [Position], [Date], Description,
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
	[IPNFId], [InstanceId], [Type], [Locale], [IPF], [Notes]
FROM tIPNF
GO

ALTER VIEW vMasterPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[MasterPageId], [Default], [InstanceId], [Name], [Description], [Url], [Contents], [PageUrl]
FROM
	tMasterPage
GO

-- SELECT * FROM vMasterPages

ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.InstanceId, m.Locale, m.[Code], m.[Name], m.RoleId
FROM tMenu m
WHERE m.HistoryId IS NULL
GO
-- SELECT * FROM vMenu
ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.NavigationMenuId, m.InstanceId, m.MenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
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
	m.NavigationMenuItemId, m.InstanceId, m.NavigationMenuId, m.Locale, m.[Order], m.[Name], m.Icon, m.RoleId, m.UrlAliasId, a.Alias, a.Url
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
	n.[NewsId], n.InstanceId, n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
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
	[NewsletterId], [InstanceId], [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles], [SendDate]
FROM
	tNewsletter
GO

-- SELECT * FROM vNewsletter

ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
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

--SELECT * FROM vOrganizations

ALTER VIEW vPages
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PageId], p.[ParentId], p.[InstanceId], p.[MasterPageId], p.[Locale], p.[Title], p.[Name], p.[UrlAliasId], p.[Content], p.[RoleId],
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
	PaidServiceId, [InstanceId], [Name], [Notes], [CreditCost]
FROM
	cPaidService
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vPersons
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.PersonId, p.InstanceId, p.AccountId, p.Title, p.LastName, p.FirstName, ISNULL(p.Email, a.EMail) as Email, p.Notes,
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
	[PollAnswerId], [InstanceId], [PollOptionId], [IP]
FROM
	tPollAnswer
GO

-- SELECT * FROM vPollAnswers

ALTER VIEW vPollOptions
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	o.[PollOptionId], o.InstanceId, o.[PollId], o.[Order], o.[Name], 
	Votes = (SELECT COUNT(*) FROM tPollAnswer WHERE PollOptionId = o.[PollOptionId] )
FROM
	tPollOption o 
GO

-- SELECT * FROM vPollOptions

ALTER VIEW vPolls
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.[PollId], p.InstanceId, p.[Closed], p.[Locale], p.[Question], p.[DateFrom], p.[DateTo], p.[Icon],
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
	ProfileId, InstanceId, [Name], [Type], [Description]
FROM tProfile
GO


ALTER VIEW vProvidedServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ps.[ProvidedServiceId], ps.InstanceId, ps.[AccountId], ps.[PaidServiceId], ps.ObjectId, ps.[ServiceDate], p.CreditCost, p.[Name], p.[Notes]
FROM
	tProvidedService ps INNER JOIN
	vPaidServices p ON p.PaidServiceId = ps.PaidServiceId
GO

ALTER VIEW vRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[RoleId], [InstanceId], [Name], [Notes]
FROM tRole
GO

-- SELECT * FROM vRoles


ALTER VIEW vSupportedLocales
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	SupportedLocaleId, InstanceId, [Name], Notes, Code, Icon
FROM
	cSupportedLocale
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vTags
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	TagId, InstanceId, Tag
FROM tTag WHERE HistoryId IS NULL
GO

ALTER VIEW vTranslations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.VocabularyId, v.InstanceId, VocabularyName = v.Name, v.Locale, t.TranslationId, t.Term, t.Translation, t.Notes
FROM tTranslation t (NOLOCK)
INNER JOIN tVocabulary v (NOLOCK) ON t.VocabularyId = v.VocabularyId
WHERE t.HistoryId IS NULL
GO

-- SELECT * FROM vTranslations

ALTER VIEW vUrlAliases
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[UrlAliasId], [InstanceId], [Url], [Locale], [Alias], [Name]
FROM tUrlAlias
GO

-- SELECT * FROM vUrlAliases

ALTER VIEW vUrlAliasPrefixes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	UrlAliasPrefixId, [InstanceId], [Name], [Code], [Locale], [Notes]
FROM cUrlAliasPrefix
WHERE HistoryId IS NULL
GO

ALTER VIEW vVocabularies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[VocabularyId], [InstanceId], [Locale], [Name], [Notes]
FROM tVocabulary
GO

-- SELECT * FROM vVocabularies

ALTER PROCEDURE pAccountCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	IF EXISTS(SELECT * FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		RETURN
	END
	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified],
		HistoryStamp, HistoryType, HistoryAccount)
	VALUES (@InstanceId, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified,
		GETDATE(), 'C', @HistoryAccount)
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole ( InstanceId, AccountId, RoleId)
		SELECT @InstanceId, @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END
GO

-- EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'aaa', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68' -- @Password=0987oiuk

ALTER PROCEDURE pAccountCreditCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@Credit DECIMAL(19,2),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountCredit ( InstanceId, AccountId, Credit, HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @Credit, GETDATE(), 'C', @HistoryAccount)

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

		INSERT INTO tAccountCredit ( InstanceId, AccountId, Credit,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountId, Credit,
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

		INSERT INTO tAccount ( InstanceId, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], 
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
	@InstanceId INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( InstanceId, AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @ProfileId, @Value, 
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

		INSERT INTO tAccountProfile ( InstanceId, AccountId, ProfileId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountId, ProfileId, Value,
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
	@InstanceId INT,
	@AccountId INT,
	@ObjectType INT,
	@ObjectId INT,
	@Rating INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountVote ( InstanceId, AccountId, ObjectType, ObjectId, Rating, [Date]) 
	VALUES ( @InstanceId, @AccountId, @ObjectType, @ObjectId, @Rating, GETDATE())

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountVoteId = @Result

END
GO

ALTER PROCEDURE pAddressCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tAddress ( InstanceId, City, Street, Zip, District, Region, Country, State, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @City, @Street, @Zip, @District, @Region, @Country, @State, @Notes,
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

		INSERT INTO tAddress ( InstanceId, City, Street, Zip, District, Region, Country, State, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, City, Street, Zip, District, Region, Country, State, Notes,
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
	@InstanceId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Locale [char](2) = 'en', 
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cArticleCategory ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Code, @Notes, GETDATE(), 'C', @HistoryAccount)

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

		INSERT INTO cArticleCategory ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @ArticleCategoryId
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
	@InstanceId INT,
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
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tArticleComment ( InstanceId, CommentId, ArticleId ) VALUES ( @InstanceId, @CommentId, @ArticleId )

END
GO
ALTER PROCEDURE pArticleCreate
	@HistoryAccount INT,
	@InstanceId INT,
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
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tArticle ( InstanceId, ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ArticleCategoryId, @Locale, @Icon, @Title, @Teaser, @ContentKeywords, @Content, @RoleId, @UrlAliasId, 
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

		INSERT INTO tArticle ( InstanceId, ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, ArticleCategoryId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId,
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
	@InstanceId INT, 
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
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ArticleId FROM vArticleTags WHERE TagId=@TagId AND ArticleId=@ArticleId) BEGIN
		INSERT INTO tArticleTag ( TagId, ArticleId ) VALUES ( @TagId, @ArticleId )
	END

END
GO

ALTER PROCEDURE pBankContactCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tBankContact ( InstanceId, AccountNumber, BankName, BankCode, SWIFT, IBAN,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountNumber, @BankName, @BankCode, @SWIFT, @IBAN,
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

		INSERT INTO tBankContact ( InstanceId, AccountNumber, BankName, BankCode, IBAN, SWIFT,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountNumber, BankName, BankCode, IBAN, SWIFT,
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
	@InstanceId INT,
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
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tBlogComment ( InstanceId, CommentId, BlogId ) VALUES ( @InstanceId, @CommentId, @BlogId )

END
GO
ALTER PROCEDURE pBlogCreate
	@HistoryAccount INT,
	@InstanceId INT,
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
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tBlog ( InstanceId, AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
		Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @Locale, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @RoleId, @UrlAliasId, 
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

		INSERT INTO tBlog ( InstanceId, AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
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
	@InstanceId INT,
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

	INSERT INTO tComment ( InstanceId, ParentId, AccountId, [Date], Title, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @AccountId, @Date, @Title, @Content, 
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

		INSERT INTO tComment ( InstanceId, Title, Content,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Title, Content,
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
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tFaq ( InstanceId, Locale, [Order], Question, Answer,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Order, @Question, @Answer,
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

		INSERT INTO tFaq ( InstanceId, Locale, [Order], Question, Answer,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Order], Question, Answer,
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
	@InstanceId INT,
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
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryComment ( InstanceId, CommentId, ImageGalleryId ) VALUES ( @InstanceId, @CommentId, @ImageGalleryId )

END
GO
ALTER PROCEDURE pImageGalleryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Description NVARCHAR(255) = NULL,
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGallery ( InstanceId, [Name], [Description], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Description, @RoleId, @Visible, @UrlAliasId, ISNULL(@Date,GETDATE()), @EnableComments, @EnableVotes, 
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
	@InstanceId INT,
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
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tImageGalleryItemComment ( InstanceId, CommentId, ImageGalleryItemId ) VALUES ( @InstanceId, @CommentId, @ImageGalleryItemId )

END
GO
ALTER PROCEDURE pImageGalleryItemCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tImageGalleryItem ( InstanceId, ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ImageGalleryId, @VirtualPath, @VirtualThumbnailPath, @Position, GETDATE(), @Description,
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

		INSERT INTO tImageGalleryItem ( InstanceId, ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description,
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
	@Description NVARCHAR(255) = NULL,
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

		INSERT INTO tImageGallery ( InstanceId, [Name], [Description], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], [Description], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes,
			HistoryStamp, HistoryType, HistoryAccount, @ImageGalleryId
		FROM tImageGallery
		WHERE ImageGalleryId = @ImageGalleryId

		UPDATE tImageGallery
		SET
			[Name]=@Name, [Description]=@Description, [Date]=@Date, RoleId=@RoleId, Visible=@Visible, UrlAliasId=@UrlAliasId, EnableComments=@EnableComments, EnableVotes=@EnableVotes,
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
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Code NVARCHAR(100),
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( InstanceId, Locale, Code, [Name], RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Code, @Name, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT MenuId = @Result

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
	@Code NVARCHAR(100),
	@Name NVARCHAR(100),
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

		INSERT INTO tMenu ( InstanceId, Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, Code = @Code, [Name] = @Name, RoleId = @RoleId,
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
	@InstanceId INT,
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

	INSERT INTO tNavigationMenu ( InstanceId, MenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @MenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NavigationMenuId = @Result

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
	@InstanceId INT,
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

	INSERT INTO tNavigationMenuItem ( InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @NavigationMenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NavigationMenuItemId = @Result

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

		INSERT INTO tNavigationMenuItem ( InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
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

		INSERT INTO tNavigationMenu ( InstanceId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
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
	@InstanceId INT,
	@UrlAliasId INT = NULL,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( InstanceId, Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Date, @Icon, @Title, @Teaser, @Content, @ContentKeywords, @UrlAliasId, 
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
	@InstanceId INT,
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

	INSERT INTO tNewsletter ( InstanceId, Locale, [Date], Icon, Subject, Attachment, Content, Roles, SendDate )
	VALUES ( @InstanceId, @Locale, @Date, @Icon, @Subject, @Attachment, @Content, @Roles, @SendDate )

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
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid NewsId %d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNews ( InstanceId, Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Title = @Title, Teaser = @Teaser, Content = @Content, ContentKeywords = @ContentKeywords, UrlAliasId=@UrlAliasId, 
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
	@InstanceId INT,
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
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
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
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'mothiva', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pOrganizationCreate @HistoryAccount=1, @InstanceId=1, @AccountId=@Result, @Name='Mothiva, s.r.o.'

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
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
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
	@ParentId INT = NULL,
	@InstanceId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,
	-----------------------------------------
	-- Subpages settings
	@SubPageCreateContents BIT = 0,
	@SubPageMasterPageId INT = NULL,
	-----------------------------------------
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage ( InstanceId, ParentId, MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()


	DECLARE @MasterPageContents INT, @UrlAlias NVARCHAR(255)
	SELECT @MasterPageContents=Contents FROM tMasterPage WHERE MasterPageId=@MasterPageId
	SELECT @UrlAlias=Alias FROM tUrlAlias WHERE UrlAliasId=@UrlAliasId
	
	IF @SubPageCreateContents = 1 AND @MasterPageContents > 1
	BEGIN
		DECLARE @SubPageUrl NVARCHAR(255)

		IF @SubPageMasterPageId IS NULL
			SELECT @SubPageMasterPageId = MasterPageId FROM tMasterPage WHERE [Default]=1 AND InstanceId=@InstanceId

		SELECT @SubPageUrl = PageUrl FROM tMasterPage WHERE MasterPageId=@SubPageMasterPageId

		DECLARE @i INT
		SET @i=1
		WHILE (@i<=@MasterPageContents)
		BEGIN
			
			DECLARE @SubPageUrlAliasId INT,@SubPageName NVARCHAR(255), @SubPageAlias NVARCHAR(255), @Url NVARCHAR(255)
			SET @SubPageName = @Name+'_content'+ CONVERT(VARCHAR(3),@i)
			SET @Url = @SubPageUrl+@SubPageName
			SET @SubPageAlias = @UrlAlias + '/' + CONVERT(VARCHAR(3),@i)

			EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url=@Url, @Locale=@Locale, @Alias = @SubPageAlias, @Name=@Title,
			@Result = @SubPageUrlAliasId OUTPUT

			EXEC pPageCreate @HistoryAccount=@HistoryAccount, @ParentId=@Result, @InstanceId=@InstanceId, @Locale=@Locale, @Name=@SubPageName, @Title=@Title,
				@UrlAliasId = @SubPageUrlAliasId, @MasterPageId = @SubPageMasterPageId

			SET @i=@i+1
		END
	END


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

		INSERT INTO tPage (InstanceId, MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, MasterPageId, Locale, Title, [Name], UrlAliasId, Content, ContentKeywords, RoleId,
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
	@InstanceId INT,
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
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @Result = @AddressHomeId OUTPUT

		DECLARE @AddressTempId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @Result = @AddressTempId OUTPUT
		

		INSERT INTO tPerson ( InstanceId, AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount)
		VALUES ( @InstanceId, @AccountId, @Title, @FirstName, @LastName, @Email,
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
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'hudy', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pPersonCreate @HistoryAccount = 1, @InstanceId=1, @AccountId = @Result, @FirstName='Roman', @LastName='Hudec', @Result = @Result OUTPUT
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

		INSERT INTO tPerson ( InstanceId, AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, AccountId, Title, FirstName, LastName, Email, 
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
	@InstanceId INT,
	@IP NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollAnswer ( InstanceId, PollOptionId, IP ) 
	VALUES ( @InstanceId, @PollOptionId, @IP )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollAnswerId = @Result

END
GO

ALTER PROCEDURE pPollCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tPoll ( InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Closed, @Locale, @Question, @DateFrom, @DateTo, @Icon,
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

		INSERT INTO tPoll ( InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Closed, Locale, Question, DateFrom, DateTo, Icon,
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
	@InstanceId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollOption ( InstanceId, PollId, [Order], [Name] )
	VALUES ( @InstanceId, @PollId, @Order, @Name )

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
	@InstanceId INT,
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
	
	INSERT INTO tProvidedService ( InstanceId, AccountId, PaidServiceId, ObjectId, ServiceDate ) 
	VALUES ( @InstanceId, @AccountId, @PaidServiceId, @ObjectId, GETDATE() )
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
			EXEC pAccountCreditCreate @HistoryAccount = @HistoryAccount, @InstanceId = @InstanceId, @AccountId = @AccountId, @Credit=@NewCredit
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
	@InstanceId INT,
	@Name NVARCHAR(200),
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tRole ( InstanceId, [Name], [Notes] ) VALUES ( @InstanceId, @Name, @Notes )
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

ALTER PROCEDURE pSearchArticleComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vArticleComments gc INNER JOIN
		tArticle art ON art.ArticleId = gc.ArticleId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
		WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
ALTER PROCEDURE pSearchArticles
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = art.ArticleId, art.Title, 
		Content = art.Teaser + art.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tArticle art INNER JOIN
	tUrlAlias a ON a.UrlAliasId = art.UrlAliasId
	WHERE art.HistoryId IS NULL AND art.Locale = @Locale AND art.InstanceId = @InstanceId AND
	(
		art.Title LIKE '%'+@Keywords+'%' OR 
		art.Teaser LIKE '%'+@Keywords+'%' OR 
		art.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
ALTER PROCEDURE pSearchBlogComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias 
		FROM vBlogComments gc INNER JOIN
		tBlog b ON b.BlogId = gc.BlogId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
		WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl = NULL
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
ALTER PROCEDURE pSearchBlogs
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = b.BlogId, b.Title, 
		Content = b.Teaser + b.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tBlog b INNER JOIN
	tUrlAlias a ON a.UrlAliasId = b.UrlAliasId
	WHERE b.HistoryId IS NULL AND b.Locale = @Locale AND b.InstanceId = @InstanceId AND
	(
		b.Title LIKE '%'+@Keywords+'%' OR 
		b.Teaser LIKE '%'+@Keywords+'%' OR 
		b.ContentKeywords LIKE '%'+@Keywords+'%'
	)
END
GO
ALTER PROCEDURE pSearchImageGalleries
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = i.ImageGalleryId, Title = i.Name, Content = NULL, UrlAlias = a.Alias,
	ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = i.ImageGalleryId ORDER BY gi.Position ASC) 
	FROM tImageGallery i INNER JOIN
	tUrlAlias a ON a.UrlAliasId = i.UrlAliasId
	WHERE i.HistoryId IS NULL AND i.InstanceId = @InstanceId AND
	(
		i.Name LIKE '%'+@Keywords+'%'
	)
END
GO
ALTER PROCEDURE pSearchImageGalleryComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS,
	ImageUrl NVARCHAR(500)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias,
		ImageUrl = (SELECT TOP 1 gi.VirtualThumbnailPath FROM vImageGalleryItems gi WHERE gi.ImageGalleryId = g.ImageGalleryId ORDER BY gi.Position ASC) 
		FROM vImageGalleryComments gc INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gc.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
ALTER PROCEDURE pSearchImageGalleryItemComments
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@CommentAliasPostFix NVARCHAR(255),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #result (Id INT NOT NULL, 
	Title NVARCHAR(255) COLLATE Slovak_CI_AS, 
	Content NVARCHAR(MAX) COLLATE Slovak_CI_AS, 
	UrlAlias NVARCHAR(2000) COLLATE Slovak_CI_AS,
	ImageUrl NVARCHAR(500)  )
	
	INSERT INTO #result
		SELECT Id = gc.CommentId, gc.Title, Content = gc.Content, UrlAlias = a.Alias + '/' +  CAST(gc.ImageGalleryItemId AS NVARCHAR),
		ImageUrl = gi.VirtualThumbnailPath
		FROM vImageGalleryItemComments gc INNER JOIN
		tImageGalleryItem gi ON gi.ImageGalleryItemId = gc.ImageGalleryItemId INNER JOIN
		tImageGallery g ON g.ImageGalleryId = gi.ImageGalleryId INNER JOIN
		tUrlAlias a ON a.UrlAliasId = g.UrlAliasId
		WHERE g.HistoryId IS NULL AND g.InstanceId = @InstanceId AND
		(
			gc.Title LIKE '%'+@Keywords+'%' OR 
			gc.Content LIKE '%'+@Keywords+'%'
		)
		
	SELECT Id, Title, Content, UrlAlias = a.Alias + '#' + CAST(Id as NVARCHAR), ImageUrl
	FROM #result r INNER JOIN 
	tUrlAlias a ON a.Alias = UrlAlias + '/' + @CommentAliasPostFix
	
END
GO
ALTER PROCEDURE pSearchNews
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = n.NewsId, Title = n.Title, 
		Content = n.Teaser + n.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tNews n INNER JOIN
	tUrlAlias a ON a.UrlAliasId = n.UrlAliasId
	WHERE n.HistoryId IS NULL AND n.Locale = @Locale AND n.InstanceId = @InstanceId AND
	(
		n.Title LIKE '%'+@Keywords+'%' OR 
		n.Teaser LIKE '%'+@Keywords+'%' OR 
		n.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSearchPages
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.PageId, p.Title, Content = p.ContentKeywords, UrlAlias = a.Alias, ImageUrl = NULL
	FROM tPage p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND
	(
		p.Title LIKE '%'+@Keywords+'%' OR 
		p.ContentKeywords LIKE '%'+@Keywords+'%'
	)
	
END
GO
ALTER PROCEDURE pSupportedLocaleCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO

ALTER PROCEDURE pSupportedLocaleDelete
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @SupportedLocaleId IS NULL OR NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid @SupportedLocaleId=%d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cSupportedLocale
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @SupportedLocaleId
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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

ALTER PROCEDURE pSupportedLocaleModify
	@HistoryAccount INT,
	@SupportedLocaleId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cSupportedLocale WHERE SupportedLocaleId = @SupportedLocaleId AND HistoryId IS NULL) 
		RAISERROR('Invalid SupportedLocaleId %d', 16, 1, @SupportedLocaleId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @SupportedLocaleId
		FROM cSupportedLocale
		WHERE SupportedLocaleId = @SupportedLocaleId

		UPDATE cSupportedLocale
		SET
			[Name] = @Name, [Notes] = @Notes, Code = @Code, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE SupportedLocaleId = @SupportedLocaleId

		SET @Result = @SupportedLocaleId

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

ALTER PROCEDURE pTagCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tTag ( InstanceId, Tag, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Tag, 
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

		INSERT INTO tTag ( InstanceId, Tag,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Tag,
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
	@InstanceId INT,
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
		SELECT @VocabularyId = VocabularyId FROM vVocabularies WHERE Name = @Vocabulary AND Locale = @Locale AND InstanceId = @InstanceId		
		IF @VocabularyId IS NULL BEGIN
			INSERT INTO tVocabulary( InstanceId, Locale, Name, Notes) VALUES ( @InstanceId, @Locale, @Vocabulary, '')
			SET @VocabularyId = SCOPE_IDENTITY()
		END

		DECLARE @TranslationId INT
		SELECT @TranslationId = TranslationId FROM vTranslations WHERE VocabularyId = @VocabularyId AND Term = @Term AND InstanceId = @InstanceId
		IF @TranslationId IS NULL BEGIN
			INSERT INTO tTranslation( InstanceId, VocabularyId, Term, Translation, Notes,
				HistoryStamp, HistoryType, HistoryAccount, HistoryId) 
			VALUES ( @InstanceId, @VocabularyId, @Term, @Translation, @Notes,
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

		INSERT INTO tTranslation (InstanceId, VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, VocabularyId, Term, Translation, Notes,
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
	@InstanceId INT,
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Url, @Locale, @InstanceId);
		RETURN
	END	

	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias ( InstanceId, Url, Locale, Alias, [Name] ) 
	VALUES ( @InstanceId, @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

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

		INSERT INTO cUrlAliasPrefix ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @UrlAliasPrefixId
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

--======================================================================================================================
-- SETUP CMS Version to 0.10
--======================================================================================================================
INSERT INTO tCMSUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 10, GETDATE())
GO
------------------------------------------------------------------------------------------------------------------------



			USE eurona
			GO
		
------------------------------------------------------------------------------------------------------------------------
-- E-Shop version 0.2
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------
--cShpVAT
CREATE TABLE [cShpVAT](
	[VATId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Percent] [decimal](19,2) NULL,
	[Code] [varchar](100) NULL,
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpVAT_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpVAT] PRIMARY KEY CLUSTERED ([VATId] ASC)
)
GO

ALTER TABLE [cShpVAT]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpVAT_cShpVAT] FOREIGN KEY([HistoryId])
	REFERENCES [cShpVAT] (VATId)
GO
ALTER TABLE [cShpVAT] CHECK CONSTRAINT [FK_cShpVAT_cShpVAT]
GO

ALTER TABLE [cShpVAT]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpVAT_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpVAT] CHECK CONSTRAINT [CK_cShpVAT_HistoryType]
GO

ALTER TABLE [cShpVAT]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpVAT_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpVAT] CHECK CONSTRAINT [FK_cShpVAT_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
--cShpHighlight
CREATE TABLE [cShpHighlight](
	[HighlightId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL,
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpHighlight_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpHighlight] PRIMARY KEY CLUSTERED ([HighlightId] ASC)
)
GO

ALTER TABLE [cShpHighlight]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpHighlight_cShpHighlight] FOREIGN KEY([HistoryId])
	REFERENCES [cShpHighlight] (HighlightId)
GO
ALTER TABLE [cShpHighlight] CHECK CONSTRAINT [FK_cShpHighlight_cShpHighlight]
GO

ALTER TABLE [cShpHighlight]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpHighlight_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpHighlight] CHECK CONSTRAINT [CK_cShpHighlight_HistoryType]
GO

ALTER TABLE [cShpHighlight]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpHighlight_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpHighlight] CHECK CONSTRAINT [FK_cShpHighlight_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
--cShpOrderStatus
CREATE TABLE [cShpOrderStatus](
	[OrderStatusId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Code] [varchar](100) NULL, /*Unikatne ID pre vsetky locale -1, -2, pre riadenie procesu */
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpOrderStatus_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpOrderStatus] PRIMARY KEY CLUSTERED ([OrderStatusId] ASC )
)
GO

--ALTER TABLE [cShpOrderStatus]
--ADD CONSTRAINT [UQ_cShpOrderStatus_Code_Locale] UNIQUE ([Code], [Locale])
--GO

ALTER TABLE [cShpOrderStatus]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpOrderStatus_cShpOrderStatus] FOREIGN KEY([HistoryId])
	REFERENCES [cShpOrderStatus] (OrderStatusId)
GO
ALTER TABLE [cShpOrderStatus] CHECK CONSTRAINT [FK_cShpOrderStatus_cShpOrderStatus]
GO

ALTER TABLE [cShpOrderStatus]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpOrderStatus_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpOrderStatus] CHECK CONSTRAINT [CK_cShpOrderStatus_HistoryType]
GO

ALTER TABLE [cShpOrderStatus]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpOrderStatus_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpOrderStatus] CHECK CONSTRAINT [FK_cShpOrderStatus_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
--cShpShipment
CREATE TABLE [cShpShipment](
	[ShipmentId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Price] [decimal](19,2) NULL,
	[VATId] [INT] NULL, /*DPH*/
	[Code] [varchar](100) NULL, /*Unikatne ID pre vsetky locale */
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpShipment_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpShipment] PRIMARY KEY CLUSTERED ([ShipmentId] ASC)
)
GO

--ALTER TABLE [cShpShipment]
--ADD CONSTRAINT [UQ_cShpShipment_Code_Locale] UNIQUE ([Code], [Locale])
--GO

ALTER TABLE [cShpShipment]  WITH CHECK 
	ADD CONSTRAINT [FK_cShpShipment_cShpVAT] FOREIGN KEY ([VATId] )
	REFERENCES [cShpVAT] ([VATId])
GO
ALTER TABLE [cShpShipment] CHECK CONSTRAINT [FK_cShpShipment_cShpVAT]
GO
ALTER TABLE [cShpShipment]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpShipment_cShpShipment] FOREIGN KEY([HistoryId])
	REFERENCES [cShpShipment] (ShipmentId)
GO
ALTER TABLE [cShpShipment] CHECK CONSTRAINT [FK_cShpShipment_cShpShipment]
GO

ALTER TABLE [cShpShipment]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpShipment_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpShipment] CHECK CONSTRAINT [CK_cShpShipment_HistoryType]
GO

ALTER TABLE [cShpShipment]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpShipment_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpShipment] CHECK CONSTRAINT [FK_cShpShipment_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
--cShpPayment
CREATE TABLE [cShpPayment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Code] [varchar](100) NULL, /*Unikatne ID pre vsetky locale */
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpPayment_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpPayment] PRIMARY KEY CLUSTERED ([PaymentId] ASC)
)
GO

--ALTER TABLE [cShpPayment]
--ADD CONSTRAINT [UQ_cShpPayment_Code_Locle] UNIQUE ([Code], [Locale])
--GO

ALTER TABLE [cShpPayment]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpPayment_cShpPayment] FOREIGN KEY([HistoryId])
	REFERENCES [cShpPayment] (PaymentId)
GO
ALTER TABLE [cShpPayment] CHECK CONSTRAINT [FK_cShpPayment_cShpPayment]
GO

ALTER TABLE [cShpPayment]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpPayment_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpPayment] CHECK CONSTRAINT [CK_cShpPayment_HistoryType]
GO

ALTER TABLE [cShpPayment]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpPayment_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpPayment] CHECK CONSTRAINT [FK_cShpPayment_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
--cShpCurrency
CREATE TABLE [cShpCurrency](
	[CurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [varchar](100) NULL,
	[Rate] [decimal](19,2) NULL,
	[Symbol] [varchar](100) NULL,
	[Icon] [nvarchar](255) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_cShpCurrency_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
	CONSTRAINT [PK_cShpCurrency] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
)
GO

ALTER TABLE [cShpCurrency]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpCurrency_cShpCurrency] FOREIGN KEY([HistoryId])
	REFERENCES [cShpCurrency] (CurrencyId)
GO
ALTER TABLE [cShpCurrency] CHECK CONSTRAINT [FK_cShpCurrency_cShpCurrency]
GO

ALTER TABLE [cShpCurrency]  WITH CHECK 
	ADD  CONSTRAINT [CK_cShpCurrency_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [cShpCurrency] CHECK CONSTRAINT [CK_cShpCurrency_HistoryType]
GO

ALTER TABLE [cShpCurrency]  WITH CHECK 
	ADD  CONSTRAINT [FK_cShpCurrency_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [cShpCurrency] CHECK CONSTRAINT [FK_cShpCurrency_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tShpUpgrade](
	[UpgradeId] [int] IDENTITY(1,1) NOT NULL,
	[VersionMinor] [int] NOT NULL,
	[VersionMajor] [int] NOT NULL,
	[UpgradeDate] [datetime] NULL,
	CONSTRAINT [PK_tShpUpgrade] PRIMARY KEY CLUSTERED ([UpgradeId] ASC)
)
GO

-- tShpCategory
CREATE TABLE [tShpCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Order] [int] NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpCategory_Locale]  DEFAULT ('en'),
	[Icon] [nvarchar](255) NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
	CONSTRAINT [PK_tShpCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [tShpCategory]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCategory_Parent] FOREIGN KEY([ParentId])
	REFERENCES [tShpCategory] ([CategoryId])
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [FK_tShpCategory_Parent]
GO

ALTER TABLE [tShpCategory]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCategory_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [FK_tShpCategory_tUrlAlias]
GO

--locale
ALTER TABLE [tShpCategory]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCategory_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [CK_tShpCategory_Locale]
GO

-- history
ALTER TABLE [tShpCategory] WITH CHECK 
	ADD CONSTRAINT [CK_tShpCategory_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [CK_tShpCategory_HistoryType]
GO

ALTER TABLE [tShpCategory]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpCategory_tShpCategory] FOREIGN KEY([HistoryId])
	REFERENCES [tShpCategory] ([CategoryId])
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [FK_tShpCategory_tShpCategory]
GO

ALTER TABLE [tShpCategory]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpCategory_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpCategory] CHECK CONSTRAINT [FK_tShpCategory_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpProduct
CREATE TABLE [tShpProduct](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Code] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Manufacturer] [nvarchar](500) NULL,
	[Description] [nvarchar](1000) NULL,
	[DescriptionLong] [nvarchar](MAX) NULL,
	[Availability] [nvarchar](500) NULL, /*dostupnost ('na objednanie', '24Ks', ...)*/
	[StorageCount] INT NULL, /*Pocet KS na sklade*/
	[Price] [DECIMAL](19,2) NOT NULL, /*Cena BEZ DPH*/
	[VATId] [INT] NULL, /*DPH*/
	[Discount][DECIMAL](19,2) NULL, /*Zlava*/
	[DiscountTypeId] [INT] NULL CONSTRAINT [DF_tShpProduct_DiscountTypeId]  DEFAULT (0),
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpProduct_Locale]  DEFAULT ('en'),
	[ViewCount] [int] NULL, /*Pocet zobrazeni produktu*/
	[Votes] [int] NULL, /*Pocet hlasov, ktore produkt obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore produkt dostal pri hlasovani*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
	CONSTRAINT [PK_tShpProduct] PRIMARY KEY CLUSTERED ([ProductId] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [tShpProduct]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProduct_cShpVAT] FOREIGN KEY ([VATId] )
	REFERENCES [cShpVAT] ([VATId])
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [FK_tShpProduct_cShpVAT]
GO

--locale
ALTER TABLE [tShpProduct]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProduct_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [CK_tShpProduct_Locale]
GO

ALTER TABLE [tShpProduct]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProduct_tUrlAlias] FOREIGN KEY ([UrlAliasId] )
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [FK_tShpProduct_tUrlAlias]
GO

-- history
ALTER TABLE [tShpProduct] WITH CHECK 
	ADD CONSTRAINT [CK_tShpProduct_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [CK_tShpProduct_HistoryType]
GO

ALTER TABLE [tShpProduct]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProduct_tShpProduct] FOREIGN KEY([HistoryId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [FK_tShpProduct_tShpProduct]
GO

ALTER TABLE [tShpProduct]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpProduct_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpProduct] CHECK CONSTRAINT [FK_tShpProduct_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ProductComment
CREATE TABLE [dbo].[tShpProductComment](
	[ProductCommentId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[CommentId] INT NOT NULL,
	[ProductId] INT NOT NULL,
 CONSTRAINT [PK_ProductCommentId] PRIMARY KEY CLUSTERED ([ProductCommentId] ASC)
)
GO

ALTER TABLE [tShpProductComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpProductComment_CommentId] FOREIGN KEY([CommentId])
	REFERENCES [tComment] ([CommentId])
GO
ALTER TABLE [tShpProductComment] CHECK CONSTRAINT [FK_tShpProductComment_CommentId]
GO

ALTER TABLE [tShpProductComment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpProductComment_ProductId] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductComment] CHECK CONSTRAINT [FK_tShpProductComment_ProductId]
GO
------------------------------------------------------------------------------------------------------------------------
-- tShpProductCategories
CREATE TABLE [tShpProductCategories](
	[InstanceId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	CONSTRAINT [PK_tShpProductCategories] PRIMARY KEY CLUSTERED( [InstanceId] ASC, [ProductId] ASC, [CategoryId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpProductCategories]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductCategories_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductCategories] CHECK CONSTRAINT [CK_tShpProductCategories_Product]
GO

ALTER TABLE [tShpProductCategories]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductCategories_Category] FOREIGN KEY([CategoryId])
	REFERENCES [tShpCategory] ([CategoryId])
GO
ALTER TABLE [tShpProductCategories] CHECK CONSTRAINT [CK_tShpProductCategories_Category]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpAttribute
CREATE TABLE [tShpAttribute](
	[AttributeId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[DefaultValue] [nvarchar](1000) NULL,
	[Type] [int]NOT NULL,
	[TypeLimit] [nvarchar](MAX) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tShpAttribute_Locale]  DEFAULT ('en'),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,		
	CONSTRAINT [PK_tShpAttribute] PRIMARY KEY CLUSTERED( [AttributeId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpAttribute]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpAttribute_Category] FOREIGN KEY([CategoryId])
	REFERENCES [tShpCategory] ([CategoryId])
GO
ALTER TABLE [tShpAttribute] CHECK CONSTRAINT [CK_tShpAttribute_Category]
GO

--locale
ALTER TABLE [tShpAttribute]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpAttribute_Locale] CHECK  (([Locale]='en' OR [Locale]='cs' OR [Locale]='sk' OR [Locale]='de'))
GO
ALTER TABLE [tShpAttribute] CHECK CONSTRAINT [CK_tShpAttribute_Locale]
GO

-- history
ALTER TABLE [tShpAttribute] WITH CHECK 
	ADD CONSTRAINT [CK_tShpAttribute_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpAttribute] CHECK CONSTRAINT [CK_tShpAttribute_HistoryType]
GO

ALTER TABLE [tShpAttribute]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpAttribute_tShpAttribute] FOREIGN KEY([HistoryId])
	REFERENCES [tShpAttribute] ([AttributeId])
GO
ALTER TABLE [tShpAttribute] CHECK CONSTRAINT [FK_tShpAttribute_tShpAttribute]
GO

ALTER TABLE [tShpAttribute]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpAttribute_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpAttribute] CHECK CONSTRAINT [FK_tShpAttribute_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpProductValue
CREATE TABLE [tShpProductValue](
	[ProductValueId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[AttributeId] [int] NOT NULL,
	[Value] [nvarchar](1000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,		
	CONSTRAINT [PK_tShpProductValue] PRIMARY KEY CLUSTERED( [ProductValueId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpProductValue]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductValue_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductValue] CHECK CONSTRAINT [CK_tShpProductValue_Product]
GO

ALTER TABLE [tShpProductValue]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductValue_Attribute] FOREIGN KEY([AttributeId])
	REFERENCES [tShpAttribute] ([AttributeId])
GO
ALTER TABLE [tShpProductValue] CHECK CONSTRAINT [CK_tShpProductValue_Attribute]
GO

-- history
ALTER TABLE [tShpProductValue] WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductValue_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpProductValue] CHECK CONSTRAINT [CK_tShpProductValue_HistoryType]
GO

ALTER TABLE [tShpProductValue]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpProductValue_tShpProductValue] FOREIGN KEY([HistoryId])
	REFERENCES [tShpProductValue] ([ProductValueId])
GO
ALTER TABLE [tShpProductValue] CHECK CONSTRAINT [FK_tShpProductValue_tShpProductValue]
GO

ALTER TABLE [tShpProductValue]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpProductValue_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpProductValue] CHECK CONSTRAINT [FK_tShpProductValue_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpAddress
CREATE TABLE [tShpAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[FirstName] [NVARCHAR](200) NULL,
	[LastName] [NVARCHAR](200) NULL,
	[Organization] [NVARCHAR](200) NULL,
	[Id1] [nvarchar](100) NULL,
	[Id2] [nvarchar](100) NULL,
	[Id3] [nvarchar](100) NULL,	
	[City] [nvarchar](100) NULL,
	[Street] [nvarchar](200) NULL,
	[Zip] [nvarchar](30) NULL,
	[State] [nvarchar](100) NULL,
	[Phone] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Notes] [nvarchar](2000) NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
	CONSTRAINT [PK_tShpAddress] PRIMARY KEY CLUSTERED( [AddressId] ASC )	
) ON [PRIMARY]
GO
-- history
ALTER TABLE [tShpAddress] WITH CHECK 
	ADD CONSTRAINT [CK_tShpAddress_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpAddress] CHECK CONSTRAINT [CK_tShpAddress_HistoryType]
GO

ALTER TABLE [tShpAddress]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpAddress_tShpAddress] FOREIGN KEY([HistoryId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpAddress] CHECK CONSTRAINT [FK_tShpAddress_tShpAddress]
GO

ALTER TABLE [tShpAddress]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpAddress_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpAddress] CHECK CONSTRAINT [FK_tShpAddress_HistoryAccount]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpCart
CREATE TABLE [tShpCart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[SessionId] [int] NULL,
	[AccountId] [int] NULL,
	[Created] [DateTime] NOT NULL,
	[ShipmentCode] [varchar](100) NULL,
	[PaymentCode] [varchar](100) NULL,
	[DeliveryAddressId] [int] NULL,
	[InvoiceAddressId] [int] NULL,	
	[Notes] [nvarchar](2000) NULL,
	[Closed] [DateTime] NULL,
	CONSTRAINT [PK_tShpCart] PRIMARY KEY CLUSTERED( [CartId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_Account] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_Account]
GO

-- address
ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_DeliveryAddress]
GO
ALTER TABLE [tShpCart]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCart_InvoiceAddress] FOREIGN KEY([InvoiceAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpCart] CHECK CONSTRAINT [CK_tShpCart_InvoiceAddress]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpCartProduct
CREATE TABLE [tShpCartProduct](
	[CartProductId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[PriceWVAT] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[VAT] [DECIMAL](19,2) NOT NULL, /*DPH*/
	[Discount][DECIMAL](19,2) NULL, /*Zlava*/
	[PriceTotal] [DECIMAL](19,2) NULL, /*Cena spolu BEZ DPH*/
	[PriceTotalWVAT] [DECIMAL](19,2) NULL, /*Cena spolu BEZ DPH*/

	CONSTRAINT [PK_tShpCartProduct] PRIMARY KEY CLUSTERED( [CartProductId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpCartProduct]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCartProduct_Cart] FOREIGN KEY([CartId])
	REFERENCES [tShpCart] ([CartId])
GO
ALTER TABLE [tShpCartProduct] CHECK CONSTRAINT [CK_tShpCartProduct_Cart]
GO

ALTER TABLE [tShpCartProduct]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpCartProduct_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpCartProduct] CHECK CONSTRAINT [CK_tShpCartProduct_Product]
GO

------------------------------------------------------------------------------------------------------------------------
-- tShpOrder
CREATE TABLE [tShpOrder](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[OrderNumber] [nvarchar](100) NOT NULL,
	[OrderDate][datetime] NOT NULL,
	[CartId] [int] NOT NULL,
	[OrderStatusCode] [varchar](100) NULL,
	[PaydDate] [datetime] NULL,
	[ShipmentCode] [varchar](100) NULL,
	[PaymentCode] [varchar](100) NULL,
	[DeliveryAddressId] [int] NULL,
	[InvoiceAddressId] [int] NULL,
	[InvoiceUrl] [nvarchar](500) NULL,
	[Notes] [nvarchar](2000) NULL,
	[Price] [DECIMAL](19,2) NULL, /*Cena BEZ DPH*/
	[PriceWVAT] [DECIMAL](19,2) NULL, /*Cena S DPH*/
	[Notified] [bit] NULL CONSTRAINT [DF_tShpOrder_Notified]  DEFAULT (0),
	[Exported] [bit] NULL CONSTRAINT [DF_tShpOrder_Exported]  DEFAULT (0),
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,		
	CONSTRAINT [PK_tShpOrder] PRIMARY KEY CLUSTERED( [OrderId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_Cart] FOREIGN KEY([CartId])
	REFERENCES [tShpCart] ([CartId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_Cart]
GO

-- address
ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_DeliveryAddress]
GO
ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_InvoiceAddress] FOREIGN KEY([InvoiceAddressId])
	REFERENCES [tShpAddress] ([AddressId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_InvoiceAddress]
GO

-- history
ALTER TABLE [tShpOrder] WITH CHECK 
	ADD CONSTRAINT [CK_tShpOrder_HistoryType] 
	CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [CK_tShpOrder_HistoryType]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD CONSTRAINT [FK_tShpOrder_tShpOrder] FOREIGN KEY([HistoryId])
	REFERENCES [tShpOrder] ([OrderId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [FK_tShpOrder_tShpOrder]
GO

ALTER TABLE [tShpOrder]  WITH CHECK 
	ADD  CONSTRAINT [FK_tShpOrder_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tShpOrder] CHECK CONSTRAINT [FK_tShpOrder_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- tShpProductRelation
CREATE TABLE [tShpProductRelation](
	[ProductRelationId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ParentProductId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[RelationType] [int] NOT NULL,
	CONSTRAINT [PK_tShpProductRelation] PRIMARY KEY CLUSTERED( [ProductRelationId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpProductRelation]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductRelation_ParentProductId] FOREIGN KEY([ParentProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductRelation] CHECK CONSTRAINT [CK_tShpProductRelation_ParentProductId]
GO

ALTER TABLE [tShpProductRelation]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductRelation_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductRelation] CHECK CONSTRAINT [CK_tShpProductRelation_Product]
GO
------------------------------------------------------------------------------------------------------------------------
-- tShpProductReviews
CREATE TABLE [tShpProductReviews](
	[ProductReviewsId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[ArticleId] [int] NOT NULL,
	CONSTRAINT [PK_tShpProductReviews] PRIMARY KEY CLUSTERED( [ProductReviewsId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpProductReviews]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductReviews_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductReviews] CHECK CONSTRAINT [CK_tShpProductReviews_Product]
GO

ALTER TABLE [tShpProductReviews]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductReviews_Article] FOREIGN KEY([ArticleId])
	REFERENCES [tArticle] ([ArticleId])
GO
ALTER TABLE [tShpProductReviews] CHECK CONSTRAINT [CK_tShpProductReviews_Article]
GO
------------------------------------------------------------------------------------------------------------------------
-- tShpProductHighlights
CREATE TABLE [tShpProductHighlights](
	[ProductHighlightsId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[HighlightId] [int] NOT NULL,
	CONSTRAINT [PK_tShpProductHighlights] PRIMARY KEY CLUSTERED( [ProductHighlightsId] ASC )	
) ON [PRIMARY]
GO

ALTER TABLE [tShpProductHighlights]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductHighlights_Product] FOREIGN KEY([ProductId])
	REFERENCES [tShpProduct] ([ProductId])
GO
ALTER TABLE [tShpProductHighlights] CHECK CONSTRAINT [CK_tShpProductHighlights_Product]
GO

ALTER TABLE [tShpProductHighlights]  WITH CHECK 
	ADD CONSTRAINT [CK_tShpProductHighlights_Highlight] FOREIGN KEY([HighlightId])
	REFERENCES [cShpHighlight] ([HighlightId])
GO
ALTER TABLE [tShpProductHighlights] CHECK CONSTRAINT [CK_tShpProductHighlights_Highlight]
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------
-- classifiers
CREATE VIEW vShpVATs AS SELECT A=1
GO
CREATE VIEW vShpHighlights AS SELECT A=1
GO
CREATE VIEW vShpOrderStatuses AS SELECT A=1
GO
CREATE VIEW vShpShipments AS SELECT [ShipmentId]=1, [Name]=1, [Code]=1, [Icon]=1, [Price]=1, [PriceWVAT]=1, [Locale]='en'
GO
CREATE VIEW vShpPayments AS SELECT A=1
GO
CREATE VIEW vShpCurrencies AS SELECT A=1
GO

-- tables
CREATE VIEW vShpAddresses AS SELECT A=1
GO
CREATE VIEW vShpCategories AS SELECT ParentId=1, CategoryId=1
GO
CREATE VIEW vShpProducts AS SELECT ProductId=1
GO
CREATE VIEW vShpProductComments AS SELECT ProductId=1
GO
CREATE VIEW vShpAttributes AS SELECT CategoryId=1, AttributeId=1
GO
CREATE VIEW vShpProductValues AS SELECT A=1
GO
CREATE VIEW vShpCarts AS SELECT A=1
GO
CREATE VIEW vShpCartProducts AS SELECT A=1
GO
CREATE VIEW vShpOrders AS SELECT A=1
GO
CREATE VIEW vShpProductRelations AS SELECT ParentProductId=1, ProductId=1
GO
CREATE VIEW vShpProductReviews AS SELECT ProductId=1, ArticleId=1
GO
CREATE VIEW vShpProductHighlights AS SELECT ProductId=1, HighlightId=1
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Functions declarations
------------------------------------------------------------------------------------------------------------------------
-- Vráti všetky aj zdedené atribúty pre danú kategóriu
CREATE FUNCTION fAllInheritCategoryAttributes(@CategoryId INT)
	RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId INT NOT NULL,
		ParentId INT NULL,
		AttributeId INT NULL
)
AS 
BEGIN
	RETURN
END
GO

-- Vráti kategórie a podkategórie pre danú kategoriu
CREATE FUNCTION fAllChildCategories(@CategoryId INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS 
BEGIN
	RETURN
END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Functions declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Procedures declarations
------------------------------------------------------------------------------------------------------------------------

-- ShpHighlight
CREATE PROCEDURE pShpHighlightCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpHighlightModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpHighlightDelete AS BEGIN SET NOCOUNT ON; END
GO

-- ShpOrderStatus
CREATE PROCEDURE pShpOrderStatusCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpOrderStatusModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpOrderStatusDelete AS BEGIN SET NOCOUNT ON; END
GO

-- ShpShipment
CREATE PROCEDURE pShpShipmentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpShipmentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpShipmentDelete AS BEGIN SET NOCOUNT ON; END
GO

-- ShpPayment
CREATE PROCEDURE pShpPaymentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpPaymentModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpPaymentDelete AS BEGIN SET NOCOUNT ON; END
GO

-- ShpVAT
CREATE PROCEDURE pShpVATCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpVATModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpVATDelete AS BEGIN SET NOCOUNT ON; END
GO

-- ShpCurrency
CREATE PROCEDURE pShpCurrencyCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCurrencyModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCurrencyDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- ShpAddress
CREATE PROCEDURE pShpAddressCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAddressModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAddressDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpCategory
CREATE PROCEDURE pShpCategoryCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCategoryModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCategoryDelete AS BEGIN SET NOCOUNT ON; END
GO
-- ShpProductCategories
CREATE PROCEDURE pShpProductCategoriesCreate AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpProduct
CREATE PROCEDURE pShpProductCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductDelete AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductCommentCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductIncrementVote AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductIncrementViewCount AS BEGIN SET NOCOUNT ON; END
GO
-- ShpProductRelation
CREATE PROCEDURE pShpProductRelationCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductRelationDelete AS BEGIN SET NOCOUNT ON; END
GO
-- ShpProductReviews
CREATE PROCEDURE pShpProductReviewsCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductReviewsDelete AS BEGIN SET NOCOUNT ON; END
GO
-- ShpProductHighlights
CREATE PROCEDURE pShpProductHighlightsCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductHighlightsDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpAttribute
CREATE PROCEDURE pShpAttributeCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAttributeModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAttributeDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpProductValue
CREATE PROCEDURE pShpProductValueCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductValueModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductValueDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpCart
CREATE PROCEDURE pShpCartCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCartModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCartDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpCartProduct
CREATE PROCEDURE pShpCartProductCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCartProductModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpCartProductDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpOrder
CREATE PROCEDURE pShpOrderCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpOrderModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpOrderDelete AS BEGIN SET NOCOUNT ON; END
GO

------------------------------------------------------------------------------------------------------------------------
-- Search engine
CREATE PROCEDURE pShpSearchProducts AS BEGIN SET NOCOUNT ON; END
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

ALTER FUNCTION fAllChildCategories(@CategoryId as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS
BEGIN

	Declare @Tier as int
	SET @Tier = 2

	INSERT INTO @table (CategoryId,Level,LineageId) 
	VALUES(@CategoryId, 1, '(1)')

	INSERT INTO @table
	Select CategoryId, 2, '(1)' from vShpCategories where ParentId = @CategoryId

	UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'

	WHILE @@rowcount > 0 BEGIN
		SET @Tier = @Tier + 1
		/*Go get children nodes for the next tier that are not already accounted for */

		INSERT INTO @table (CategoryId,Level,LineageId)
		SELECT CategoryId, @Tier, (select LineageId from @table where CategoryId = ParentId) 
		FROM vShpCategories 
		WHERE ParentId IN (select CategoryId from @table) 
		AND CategoryId NOT in (select CategoryId from @table)

		UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'
	END
	
	RETURN;
END
GO

--SELECT * FROM fAllChildCategories(1)
ALTER FUNCTION fAllInheritCategoryAttributes(@CategoryId as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId INT NOT NULL,
		ParentId INT NULL,
		AttributeId INT NULL
)
AS
BEGIN

	-- Ziskam prveho parent danej kategorie
	DECLARE @ParentId INT
	SELECT @ParentId = ParentId FROM vShpCategories WHERE CategoryId=@CategoryId	

	-- Vlozim prvy zaznam z informaciami o prvej kategorii (@CategoryId)
	INSERT INTO @table (CategoryId, ParentId, AttributeId ) 
		SELECT @CategoryId, @ParentId, AttributeId FROM vShpAttributes
		WHERE CategoryId=@CategoryId
	
	-- Dokila ma kategoria parent, plni sa tabulka.
	WHILE @ParentId IS NOT NULL BEGIN
		SELECT @ParentId = ParentId FROM vShpCategories WHERE CategoryId=@CategoryId	
				
		INSERT INTO @table (CategoryId, ParentId, AttributeId ) 
			SELECT c.CategoryId, c.ParentId, a.AttributeId FROM vShpAttributes a 
				INNER JOIN vShpCategories c ON c.CategoryId=a.CategoryId
			WHERE a.CategoryId=@ParentId		
			
		SET @CategoryId = @ParentId
	END

	RETURN;
END
GO

--SELECT * FROM dbo.fAllInheritCategoryAttributes(1)
ALTER VIEW vShpAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [InstanceId], [FirstName], [LastName], [Organization], [Id1], [Id2], [Id3],
	[City], [Street], [Zip], [State],
	[Phone], [Email], [Notes]
FROM
	tShpAddress
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vShpAddresses


ALTER VIEW vShpAttributes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AttributeId, a.InstanceId, a.CategoryId, a.[Name], a.Description, a.DefaultValue, a.Type, a.TypeLimit, a.Locale
FROM
	tShpAttribute a
WHERE
	a.HistoryId IS NULL
GO


ALTER VIEW vShpCartProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, cp.InstanceId, cp.CartId, c.AccountId, cp.ProductId, ProductCode = p.Code,ProductName = p.Name, 
	cp.Quantity,
	cp.Price,/*Cena BEZ DPH*/
	cp.PriceWVAT,  /*Cena S DPH*/
	cp.VAT,/*DPH*/
	cp.Discount, /*Zlava*/
	cp.PriceTotal, /*Cena spolu BEZ DPH*/
	PriceTotalWVAT,/*Cena spolu S DPH*/
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p ON p.ProductId = cp.ProductId
	INNER JOIN tShpCart c ON c.CartId = cp.CartId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO


ALTER VIEW vShpCarts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed,
	c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
	c.PaymentCode, PaymentName = p.Name,
	c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
	PriceTotal = (SELECT SUM(PriceTotal) FROM vShpCartProducts WHERE CartId=c.CartId),
	PriceTotalWVAT = (SELECT SUM(PriceTotalWVAT) FROM vShpCartProducts WHERE CartId=c.CartId)
FROM
	tShpCart c
	LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode AND s.HistoryId IS NULL
	LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode AND p.HistoryId IS NULL
GO


ALTER VIEW vShpCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CategoryId, c.[Order], c.InstanceId, c.ParentId, c.[Name], c.Locale,
	c.Icon, a.UrlAliasId, a.Url, a.Alias
FROM
	tShpCategory c LEFT JOIN tUrlAlias a ON a.UrlAliasId = c.UrlAliasId
WHERE
	c.HistoryId IS NULL
GO


ALTER VIEW vShpCurrencies
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	CurrencyId, InstanceId, [Name], Notes, Code, Icon, Locale, Rate, Symbol
FROM
	cShpCurrency
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vShpHighlights
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	HighlightId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpHighlight
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vShpOrders
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.[Login], o.PaydDate,
	o.OrderStatusCode, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentCode, ShipmentName = s.Name, ShipmentIcon = s.Icon, ShipmentPrice = s.Price, ShipmentPriceWVAT = s.PriceWVAT,
	o.PaymentCode, PaymentName = p.Name, PaymentIcon = p.Icon,
	o.Price, o.PriceWVAT,
	o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.[Notes],
	o.Notified, o.Exported
FROM
	tShpOrder o
	INNER JOIN vShpCarts c ON c.CartId = o.CartId
	INNER JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s ON s.Code = o.ShipmentCode AND s.Locale=a.Locale
	LEFT JOIN cShpPayment p ON p.Code = o.PaymentCode AND p.HistoryId IS NULL
	LEFT JOIN cShpOrderStatus os ON os.Code = o.OrderStatusCode  AND os.HistoryId IS NULL AND os.Locale=a.Locale
WHERE o.HistoryId IS NULL
GO


ALTER VIEW vShpOrderStatuses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrderStatusId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpOrderStatus
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vShpPayments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	PaymentId, InstanceId, [Name], Notes, Code, Icon, Locale
FROM
	cShpPayment
WHERE
	HistoryId IS NULL
GO


ALTER VIEW vShpProductComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pc.ProductCommentId, pc.InstanceId, pc.ProductId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tShpProductComment pc 
	INNER JOIN vComments c ON c.CommentId = pc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO

ALTER VIEW vShpProductHighlights
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ph.ProductHighlightsId, ph.InstanceId, ph.ProductId, ph.HighlightId,
	h.Name, h.Code, h.Icon, h.Notes
FROM
	tShpProductHighlights ph
	INNER JOIN vShpHighlights h ON h.HighlightId = ph.HighlightId
GO

ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (p.Price - ( p.Price * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (p.Price - p.Discount )/*Zlava Suma*/
		ELSE p.Price
		END, 

	PriceTotal = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (p.Price - ( p.Price * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (p.Price - p.Discount )/*Zlava Suma*/
		ELSE p.Price
		END, 

	PriceTotalWVAT = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN ROUND((p.Price - ( p.Price * ( p.Discount / 100 ) )) * (1 + ISNULL(v.[Percent], 0)/100), 2 )/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN ROUND((p.Price - p.Discount ) * (1 + ISNULL(v.[Percent], 0)/100), 2 )/*Zlava Suma*/
		ELSE p.Price
		END, 

	ProductAvailability = p.Availability, a.Alias
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO

ALTER VIEW vShpProductReviews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductReviewsId, pr.InstanceId, pr.ProductId, pr.ArticleId,
	a.Icon, a.Title, a.Teaser, a.RoleId, a.Country, a.City, a.ReleaseDate, a.Visible, 
	alias.Alias
FROM
	tShpProductReviews pr
	INNER JOIN vArticles a ON a.ArticleId = pr.ArticleId
	INNER JOIN vArticleCategories c ON a.ArticleCategoryId = c.ArticleCategoryId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = a.UrlAliasId
GO


ALTER VIEW vShpProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, p.[Name], p.[Manufacturer], p.[Description], p.[DescriptionLong], p.Availability, 
	p.StorageCount, p.Price, p.Discount, DiscountTypeId = ISNULL( p.DiscountTypeId, 0 ), 
	p.VATId, VAT = ISNULL(v.[Percent], 0),
	p.Locale, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 )
FROM
	tShpProduct p 
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
WHERE
	p.HistoryId IS NULL
GO


ALTER VIEW vShpProductValues
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	v.ProductValueId, v.InstanceId, v.ProductId, v.AttributeId, v.Value
FROM
	tShpProductValue v
WHERE
	v.HistoryId IS NULL
GO


ALTER VIEW vShpShipments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	s.ShipmentId, s.InstanceId, s.[Name], s.Notes, s.Code, s.Icon, s.Locale, s.Price, s.VATId, VAT = v.[Percent],
	PriceWVAT = ROUND(s.Price * (1 + v.[Percent]/100 ), 2)
FROM
	cShpShipment s LEFT JOIN
	cShpVAT v ON v.VATId = s.VATId
WHERE
	s.HistoryId IS NULL
GO


ALTER VIEW vShpVATs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	VATId, InstanceId, [Name], Notes, Code, Icon, Locale, [Percent]
FROM
	cShpVAT
WHERE
	HistoryId IS NULL
GO

ALTER PROCEDURE pShpAddressCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@FirstName NVARCHAR(200) = NULL,
	@LastName NVARCHAR(200) = NULL,
	@Organization NVARCHAR(200) = NULL,
	@Id1 NVARCHAR(100) = NULL,
	@Id2 NVARCHAR(100) = NULL,
	@Id3 NVARCHAR(100) = NULL,	
	@City NVARCHAR(100) = '',
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@State NVARCHAR(100)= '',
	@Phone NVARCHAR(100) = NULL,
	@Email NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpAddress ( InstanceId, FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @FirstName, @LastName, @Organization, @Id1, @Id2, @Id3, @City, @Street, @Zip, @State, @Phone, @Email, @Notes,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AddressId = @Result

END
GO

ALTER PROCEDURE pShpAddressDelete
	@HistoryAccount INT,
	@AddressId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AddressId IS NULL OR NOT EXISTS(SELECT * FROM tShpAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AddressId=%d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpAddress
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

ALTER PROCEDURE pShpAddressModify
	@HistoryAccount INT,
	@AddressId INT,
	@FirstName NVARCHAR(200) = NULL,
	@LastName NVARCHAR(200) = NULL,
	@Organization NVARCHAR(200) = NULL,
	@Id1 NVARCHAR(100) = NULL,
	@Id2 NVARCHAR(100) = NULL,
	@Id3 NVARCHAR(100) = NULL,	
	@City NVARCHAR(100) = '',
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@State NVARCHAR(100)= '',
	@Phone NVARCHAR(100) = NULL,
	@Email NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid AddressId %d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpAddress ( InstanceId, FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @AddressId
		FROM tShpAddress
		WHERE AddressId = @AddressId

		UPDATE tShpAddress
		SET
			FirstName=@FirstName, LastName=@LastName, Organization=@Organization, Id1=@Id1, Id2=@Id2, Id3=@Id3, 
			City=@City, Street=@Street, Zip=@Zip, State=@State, Phone=@Phone, Email=@Email, Notes=@Notes,
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

ALTER PROCEDURE pShpAttributeCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CategoryId INT,
	@Name NVARCHAR(500) = NULL,
	@Description NVARCHAR(1000)  = NULL,
	@DefaultValue NVARCHAR(1000)  = NULL,
	@Type INT,
	@TypeLimit NVARCHAR(MAX) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpAttribute ( InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @CategoryId, @Name, @Description, @DefaultValue, @Type, @TypeLimit, @Locale,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AttributeId = @Result

END
GO

ALTER PROCEDURE pShpAttributeDelete
	@HistoryAccount INT,
	@AttributeId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AttributeId IS NULL OR NOT EXISTS(SELECT * FROM tShpAttribute WHERE AttributeId = @AttributeId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AttributeId=%d', 16, 1, @AttributeId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpAttribute
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AttributeId
		WHERE AttributeId = @AttributeId

		SET @Result = @AttributeId

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

ALTER PROCEDURE pShpAttributeModify
	@HistoryAccount INT,
	@AttributeId INT,
	@CategoryId INT,
	@Name NVARCHAR(500) = NULL,
	@Description NVARCHAR(1000)  = NULL,
	@DefaultValue NVARCHAR(1000)  = NULL,
	@Type INT,
	@TypeLimit NVARCHAR(MAX) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpAttribute WHERE AttributeId = @AttributeId AND HistoryId IS NULL) 
		RAISERROR('Invalid AttributeId %d', 16, 1, @AttributeId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpAttribute ( InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
			HistoryStamp, HistoryType, HistoryAccount, @AttributeId
		FROM tShpAttribute
		WHERE AttributeId = @AttributeId

		UPDATE tShpAttribute
		SET
			CategoryId = @CategoryId, [Name] = @Name, Description = @Description, DefaultValue = @DefaultValue, Type = @Type, TypeLimit = @TypeLimit, Locale = @Locale,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AttributeId = @AttributeId

		SET @Result = @AttributeId

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

ALTER PROCEDURE pShpCartCreate
	@InstanceId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentCode, @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END
GO

ALTER PROCEDURE pShpCartDelete
	@CartId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CartId IS NULL OR NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId ) 
		RAISERROR('Invalid @CartId=%d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpCart WHERE CartId = @CartId

		SET @Result = @CartId

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

ALTER PROCEDURE pShpCartModify
	@CartId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCart
		SET AccountId = @AccountId, SessionId = @SessionId, ShipmentCode = @ShipmentCode, PaymentCode = @PaymentCode,
			Closed = @Closed, Notes=@Notes
		WHERE CartId = @CartId

		SET @Result = @CartId

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

ALTER PROCEDURE pShpCartProductCreate
	@InstanceId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity, Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity, @Price, @PriceWVAT, @VAT, @Discount, @PriceTotal, @PriceTotalWVAT )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO

ALTER PROCEDURE pShpCartProductDelete
	@CartProductId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CartProductId IS NULL OR NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId ) 
		RAISERROR('Invalid @CartProductId=%d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpCartProduct WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

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

ALTER PROCEDURE pShpCartProductModify
	@CartProductId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId) 
		RAISERROR('Invalid CartProductId %d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCartProduct
		SET CartId = @CartId, ProductId = @ProductId, Quantity = @Quantity,
		Price=@Price, PriceWVAT=@PriceWVAT, VAT=@VAT, Discount=@Discount, PriceTotal=@PriceTotal, PriceTotalWVAT=@PriceTotalWVAT
		WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

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

ALTER PROCEDURE pShpCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Order INT = NULL,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCategory ( InstanceId, [Order], ParentId, [Name], Locale, Icon, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Order, @ParentId, @Name, @Locale, @Icon, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CategoryId = @Result

END
GO

ALTER PROCEDURE pShpCategoryDelete
	@HistoryAccount INT,
	@CategoryId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CategoryId IS NULL OR NOT EXISTS(SELECT * FROM tShpCategory WHERE CategoryId = @CategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CategoryId=%d', 16, 1, @CategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCategory
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CategoryId
		WHERE CategoryId = @CategoryId
		
		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tShpCategory WHERE CategoryId = @CategoryId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tShpCategory SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END		

		SET @Result = @CategoryId

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

ALTER PROCEDURE pShpCategoryModify
	@HistoryAccount INT,
	@CategoryId INT,
	@Order INT = NULL,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCategory WHERE CategoryId = @CategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid CategoryId %d', 16, 1, @CategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpCategory ( InstanceId, ParentId, [Order], [Name], Icon, Locale, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, ParentId, [Order], [Name], Icon, Locale, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @CategoryId
		FROM tShpCategory
		WHERE CategoryId = @CategoryId

		UPDATE tShpCategory
		SET
			ParentId = @ParentId, [Order] = @Order, Locale = @Locale, [Name] = @Name, Icon=@Icon, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CategoryId = @CategoryId

		SET @Result = @CategoryId

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

ALTER PROCEDURE pShpCurrencyCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Rate DECIMAL(19,2) = 0,
	@Symbol VARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpCurrency ( InstanceId, Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Rate, @Symbol, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CurrencyId = @Result

END
GO

ALTER PROCEDURE pShpCurrencyDelete
	@HistoryAccount INT,
	@CurrencyId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CurrencyId IS NULL OR NOT EXISTS(SELECT * FROM cShpCurrency WHERE CurrencyId = @CurrencyId AND HistoryId IS NULL) 
		RAISERROR('Invalid @CurrencyId=%d', 16, 1, @CurrencyId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpCurrency
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @CurrencyId
		WHERE CurrencyId = @CurrencyId

		SET @Result = @CurrencyId

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

ALTER PROCEDURE pShpCurrencyModify
	@HistoryAccount INT,
	@CurrencyId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Rate DECIMAL(19,2) = 0,
	@Symbol VARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpCurrency WHERE CurrencyId = @CurrencyId AND HistoryId IS NULL) 
		RAISERROR('Invalid CurrencyId %d', 16, 1, @CurrencyId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpCurrency ( InstanceId, Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @CurrencyId
		FROM cShpCurrency
		WHERE CurrencyId = @CurrencyId

		UPDATE cShpCurrency
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Rate = @Rate, Symbol = @Symbol, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CurrencyId = @CurrencyId

		SET @Result = @CurrencyId

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

ALTER PROCEDURE pShpHighlightCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpHighlight ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT HighlightId = @Result

END
GO

ALTER PROCEDURE pShpHighlightDelete
	@HistoryAccount INT,
	@HighlightId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @HighlightId IS NULL OR NOT EXISTS(SELECT * FROM cShpHighlight WHERE HighlightId = @HighlightId AND HistoryId IS NULL) 
		RAISERROR('Invalid @HighlightId=%d', 16, 1, @HighlightId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpHighlight
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @HighlightId
		WHERE HighlightId = @HighlightId

		SET @Result = @HighlightId

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

ALTER PROCEDURE pShpHighlightModify
	@HistoryAccount INT,
	@HighlightId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpHighlight WHERE HighlightId = @HighlightId AND HistoryId IS NULL) 
		RAISERROR('Invalid HighlightId %d', 16, 1, @HighlightId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpHighlight ( InstanceId, Locale, [Name], [Notes], Code, Icon, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @HighlightId
		FROM cShpHighlight
		WHERE HighlightId = @HighlightId

		UPDATE cShpHighlight
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Icon= @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE HighlightId = @HighlightId

		SET @Result = @HighlightId

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

ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(5)

	SET @year =  CAST( YEAR(GETDATE()) as nvarchar(4) )
	SET @month = CAST( MONTH(GETDATE()) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month

	SELECT @number = COUNT(*) + 1 from tShpOrder 
	SET @number = replicate( 0, 4- LEN(@number) ) + @number

	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, GETDATE(), @OrderStatusCode, @ShipmentCode,  @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, @InvoiceUrl, @PaydDate, @Notes, @Price, @PriceWVAT, @Notified, @Exported, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderId = @Result

END
GO

ALTER PROCEDURE pShpOrderModify
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100),	
	@PaymentCode VARCHAR(100),		
	@PaydDate DATETIME = NULL,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder
		SET
			CartId=@CartId, OrderStatusCode=@OrderStatusCode, ShipmentCode=@ShipmentCode, PaymentCode=@PaymentCode, PaydDate=@PaydDate, InvoiceUrl=@InvoiceUrl, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Notified=@Notified, Exported=@Exported,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderId = @OrderId

		SET @Result = @OrderId

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

ALTER PROCEDURE pShpOrderStatusCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM cShpOrderStatus WHERE Code = @Code AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('Code with @Code=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Code, @Locale, @InstanceId);
		RETURN
	END	

	INSERT INTO cShpOrderStatus ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderStatusId = @Result

END
GO

ALTER PROCEDURE pShpOrderStatusDelete
	@HistoryAccount INT,
	@OrderStatusId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @OrderStatusId IS NULL OR NOT EXISTS(SELECT * FROM cShpOrderStatus WHERE OrderStatusId = @OrderStatusId AND HistoryId IS NULL) 
		RAISERROR('Invalid @OrderStatusId=%d', 16, 1, @OrderStatusId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpOrderStatus
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @OrderStatusId
		WHERE OrderStatusId = @OrderStatusId

		SET @Result = @OrderStatusId

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

ALTER PROCEDURE pShpOrderStatusModify
	@HistoryAccount INT,
	@OrderStatusId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpOrderStatus WHERE OrderStatusId = @OrderStatusId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderStatusId %d', 16, 1, @OrderStatusId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpOrderStatus ( InstanceId, Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @OrderStatusId
		FROM cShpOrderStatus
		WHERE OrderStatusId = @OrderStatusId

		UPDATE cShpOrderStatus
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Icon = @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderStatusId = @OrderStatusId

		SET @Result = @OrderStatusId

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

ALTER PROCEDURE pShpOrderDelete
	@HistoryAccount INT,
	@OrderId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @OrderId IS NULL OR NOT EXISTS(SELECT * FROM tShpOrder WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid @OrderId=%d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpOrder
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @OrderId
		WHERE OrderId = @OrderId
		
		SET @Result = @OrderId

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

ALTER PROCEDURE pShpPaymentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM cShpPayment WHERE Code = @Code AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('Code with @Code=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Code, @Locale, @InstanceId);
		RETURN
	END	

	INSERT INTO cShpPayment ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PaymentId = @Result

END
GO

ALTER PROCEDURE pShpPaymentDelete
	@HistoryAccount INT,
	@PaymentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PaymentId IS NULL OR NOT EXISTS(SELECT * FROM cShpPayment WHERE PaymentId = @PaymentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @PaymentId=%d', 16, 1, @PaymentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpPayment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PaymentId
		WHERE PaymentId = @PaymentId

		SET @Result = @PaymentId

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

ALTER PROCEDURE pShpPaymentModify
	@HistoryAccount INT,
	@PaymentId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpPayment WHERE PaymentId = @PaymentId AND HistoryId IS NULL) 
		RAISERROR('Invalid PaymentId %d', 16, 1, @PaymentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpPayment ( InstanceId, Locale, [Name], [Notes], Code, Icon, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Icon, 
			HistoryStamp, HistoryType, HistoryAccount, @PaymentId
		FROM cShpPayment
		WHERE PaymentId = @PaymentId

		UPDATE cShpPayment
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Icon = @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PaymentId = @PaymentId

		SET @Result = @PaymentId

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

ALTER PROCEDURE pShpProductCategoriesCreate
	@InstanceId INT,
	@ProductId INT = NULL,
	@CategoryId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductCategories ( InstanceId, ProductId, CategoryId ) 
	VALUES ( @InstanceId, @ProductId, @CategoryId)

	SET @Result = @ProductId

	SELECT ProductId = @Result

END
GO

ALTER PROCEDURE pShpProductCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ProductId INT, 
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
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tShpProductComment ( InstanceId, CommentId, ProductId ) VALUES ( @InstanceId, @CommentId, @ProductId )

END
GO
ALTER PROCEDURE pShpProductCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Manufacturer NVARCHAR(500),
	@Code NVARCHAR(500) = NULL,
	@Name NVARCHAR(500),
	@Description NVARCHAR(1000) = NULL,
	@DescriptionLong NVARCHAR(MAX) = NULL,
	@Availability NVARCHAR(500)  = NULL, /*dostupnost ('na objednanie', '24Ks', ...)*/
	@StorageCount INT = NULL, /*Pocet KS na sklade*/
	@Price DECIMAL(19,2), /*Cena BEZ DPH*/	
	@VATId INT = NULL, /*DPH%*/	
	@Discount DECIMAL(19,2) = 0, /*Zlava %*/	
	@DiscountTypeId INT = 0, /*Typ Zlavy 0=%, 1=Price*/	
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProduct ( InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, DiscountTypeId, Locale, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Code, @Name, @Manufacturer, @Description, @DescriptionLong, @Availability, @StorageCount, @Price, @VATId, @Discount, @DiscountTypeId, @Locale, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductId = @Result

END
GO

ALTER PROCEDURE pShpProductDelete
	@HistoryAccount INT,
	@ProductId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductId IS NULL OR NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ProductId=%d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProduct
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ProductId
		WHERE ProductId = @ProductId

		-- Unbind and Delete UrlAlias
		DECLARE @UrlAliasId INT
		SELECT @UrlAliasId = UrlAliasId FROM tShpProduct WHERE ProductId = @ProductId
		
		IF @UrlAliasId IS NOT NULL
		BEGIN
			UPDATE tShpProduct SET UrlAliasId=NULL WHERE UrlAliasId=@UrlAliasId
			DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
		END	
		
		SET @Result = @ProductId

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

ALTER PROCEDURE pShpProductHighlightsCreate
	@InstanceId INT,
	@ProductId INT,
	@HighlightId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductHighlights ( InstanceId, ProductId, HighlightId ) 
	VALUES ( @InstanceId, @ProductId, @HighlightId )

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductHighlightsId = @Result

END
GO

ALTER PROCEDURE pShpProductHighlightsDelete
	@ProductHighlightsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductHighlightsId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductHighlights WHERE ProductHighlightsId = @ProductHighlightsId ) 
		RAISERROR('Invalid @ProductHighlightsId=%d', 16, 1, @ProductHighlightsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpProductHighlights WHERE ProductHighlightsId = @ProductHighlightsId
		SET @Result = @ProductHighlightsId

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

ALTER PROCEDURE pShpProductIncrementViewCount
	@ProductId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	UPDATE tShpProduct SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ProductId = @ProductId

END
GO

ALTER PROCEDURE pShpProductIncrementVote
	@ProductId INT,
	@Rating INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	UPDATE tShpProduct 
		SET Votes = ISNULL(Votes, 0) + 1,
		TotalRating = ISNULL(TotalRating, 0) + @Rating
	WHERE ProductId = @ProductId

END
GO

ALTER PROCEDURE pShpProductModify
	@HistoryAccount INT,
	@ProductId INT,
	@Manufacturer NVARCHAR(500),
	@Code NVARCHAR(500) = NULL,
	@Name NVARCHAR(500),
	@Description NVARCHAR(1000) = NULL,
	@DescriptionLong NVARCHAR(MAX) = NULL,
	@Availability NVARCHAR(500)  = NULL, /*dostupnost ('na objednanie', '24Ks', ...)*/
	@StorageCount INT = NULL, /*Pocet KS na sklade*/
	@Price DECIMAL(19,2), /*Cena BEZ DPH*/	
	@VATId INT = NULL, /*DPH%*/	
	@Discount DECIMAL(19,2) = 0, /*Zlava %*/
	@DiscountTypeId INT = 0, /*Typ Zlavy 0=%, 1=Price*/			
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpProduct ( InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, DiscountTypeId, Locale, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Code, [Name], [Manufacturer], [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, DiscountTypeId, Locale, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, @ProductId
		FROM tShpProduct
		WHERE ProductId = @ProductId

		UPDATE tShpProduct
		SET
			Code = @Code, [Name] = @Name, Manufacturer = @Manufacturer, [Description]=@Description, DescriptionLong=@DescriptionLong, Availability = @Availability, StorageCount=@StorageCount, Price = @Price, VATId = @VATId, Discount = @Discount, DiscountTypeId=@DiscountTypeId, Locale = @Locale, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductId = @ProductId

		SET @Result = @ProductId

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

ALTER PROCEDURE pShpProductRelationCreate
	@InstanceId INT,
	@ParentProductId INT,
	@ProductId INT,
	@RelationType INT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductRelation ( InstanceId, ParentProductId, ProductId, RelationType ) 
	VALUES ( @InstanceId, @ParentProductId, @ProductId, @RelationType )

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductRelationId = @Result

END
GO

ALTER PROCEDURE pShpProductRelationDelete
	@ProductRelationId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductRelationId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductRelation WHERE ProductRelationId = @ProductRelationId ) 
		RAISERROR('Invalid @ProductRelationId=%d', 16, 1, @ProductRelationId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpProductRelation WHERE ProductRelationId = @ProductRelationId
		SET @Result = @ProductRelationId

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

ALTER PROCEDURE pShpProductReviewsCreate
	@InstanceId INT,
	@ProductId INT,
	@ArticleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductReviews ( InstanceId, ProductId, ArticleId ) 
	VALUES ( @InstanceId, @ProductId, @ArticleId )

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductReviewsId = @Result

END
GO

ALTER PROCEDURE pShpProductReviewsDelete
	@ProductReviewsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductReviewsId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductReviews WHERE ProductReviewsId = @ProductReviewsId ) 
		RAISERROR('Invalid @ProductReviewsId=%d', 16, 1, @ProductReviewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpProductReviews WHERE ProductReviewsId = @ProductReviewsId
		SET @Result = @ProductReviewsId

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

ALTER PROCEDURE pShpProductValueCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ProductId INT,
	@AttributeId INT,
	@Value NVARCHAR(1000)  = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductValue ( InstanceId, ProductId, AttributeId, Value,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ProductId, @AttributeId, @Value,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductValueId = @Result

END
GO

ALTER PROCEDURE pShpProductValueDelete
	@HistoryAccount INT,
	@ProductValueId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductValueId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductValue WHERE ProductValueId = @ProductValueId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ProductValueId=%d', 16, 1, @ProductValueId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProductValue
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ProductValueId
		WHERE ProductValueId = @ProductValueId

		SET @Result = @ProductValueId

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

ALTER PROCEDURE pShpProductValueModify
	@HistoryAccount INT,
	@ProductValueId INT,
	@ProductId INT,
	@AttributeId INT,
	@Value NVARCHAR(1000)  = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProductValue WHERE ProductValueId = @ProductValueId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductValueId %d', 16, 1, @ProductValueId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpProductValue ( InstanceId, ProductId, AttributeId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, ProductId, AttributeId, Value,
			HistoryStamp, HistoryType, HistoryAccount, @ProductValueId
		FROM tShpProductValue
		WHERE ProductValueId = @ProductValueId

		UPDATE tShpProductValue
		SET
			ProductId = @ProductId, AttributeId = @AttributeId, Value = @Value,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductValueId = @ProductValueId

		SET @Result = @ProductValueId

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

ALTER PROCEDURE pShpSearchProducts
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.ProductId, Title = p.Name, 
		Content = ISNULL(p.Manufacturer,'') + ISNULL( p.Description, '')/* + ISNULL(p.DescriptionLong, '')*/ , 
		UrlAlias = a.Alias, ImageUrl = NULL
	FROM tShpProduct p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND 
	(
		p.Name LIKE '%'+@Keywords+'%' OR 
		p.Description LIKE '%'+@Keywords+'%' OR
		--p.DescriptionLong LIKE '%'+@Keywords+'%' OR
		p.Manufacturer LIKE '%'+@Keywords+'%' 
	)
	
END
GO
ALTER PROCEDURE pShpShipmentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Price DECIMAL(19,2) = 0,
	@VATId INT = NULL, /*DPH%*/	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM cShpShipment WHERE Code = @Code AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('Code with @Code=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Code, @Locale, @InstanceId);
		RETURN
	END	

	INSERT INTO cShpShipment ( InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Price, @VATId, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ShipmentId = @Result

END
GO

ALTER PROCEDURE pShpShipmentDelete
	@HistoryAccount INT,
	@ShipmentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ShipmentId IS NULL OR NOT EXISTS(SELECT * FROM cShpShipment WHERE ShipmentId = @ShipmentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ShipmentId=%d', 16, 1, @ShipmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpShipment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ShipmentId
		WHERE ShipmentId = @ShipmentId

		SET @Result = @ShipmentId

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

ALTER PROCEDURE pShpShipmentModify
	@HistoryAccount INT,
	@ShipmentId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Price DECIMAL(19,2) = 0,
	@VATId INT = NULL, /*DPH%*/	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpShipment WHERE ShipmentId = @ShipmentId AND HistoryId IS NULL) 
		RAISERROR('Invalid ShipmentId %d', 16, 1, @ShipmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpShipment ( InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @ShipmentId
		FROM cShpShipment
		WHERE ShipmentId = @ShipmentId

		UPDATE cShpShipment
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Price = @Price, VATId=@VATId, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ShipmentId = @ShipmentId

		SET @Result = @ShipmentId

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

ALTER PROCEDURE pShpVATCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Percent DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpVAT ( InstanceId, Locale, [Name], [Notes], Code, [Percent], Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Percent, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO

ALTER PROCEDURE pShpVATDelete
	@HistoryAccount INT,
	@VATId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @VATId IS NULL OR NOT EXISTS(SELECT * FROM cShpVAT WHERE VATId = @VATId AND HistoryId IS NULL) 
		RAISERROR('Invalid @VATId=%d', 16, 1, @VATId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpVAT
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @VATId
		WHERE VATId = @VATId

		SET @Result = @VATId

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

ALTER PROCEDURE pShpVATModify
	@HistoryAccount INT,
	@VATId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Percent DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpVAT WHERE VATId = @VATId AND HistoryId IS NULL) 
		RAISERROR('Invalid VATId %d', 16, 1, @VATId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpVAT ( InstanceId, Locale, [Name], [Notes], Code, [Percent], Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, [Percent], Icon,
			HistoryStamp, HistoryType, HistoryAccount, @VATId
		FROM cShpVAT
		WHERE VATId = @VATId

		UPDATE cShpVAT
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, [Percent] = @Percent, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE VATId = @VATId

		SET @Result = @VATId

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
-- Eurona Portal version 0.1
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tAccount ADD TVD_Id INT NULL, CanAccessIntensa BIT NULL, Roles NVARCHAR(1000) NULL
GO

ALTER TABLE tShpProduct ADD Body DECIMAL(19,2) NULL, Parfumacia INT NULL, VAT DECIMAL(19,2 ) NULL, 
[Novinka] BIT NULL, [Inovace] BIT NULL,[Doprodej] BIT NULL,[Vyprodano] BIT NULL,[ProdejUkoncen] BIT NULL, [Top] INT NULL
GO

ALTER TABLE tShpCartProduct ADD CurrencyId INT NULL
GO
ALTER TABLE tShpOrder ADD CurrencyId INT NULL, 
ParentId INT NULL/*Parent objednavka*/, 
AssociationAccountId INT NULL/*Pridruzienie tejto objednavky k objednavke pouzivatela*/, 
AssociationRequestStatus INT NULL, /*Status poziadavky na pridruzenie*/
CreatedByAccountId INT NULL, /*Pouzivatel, ktory objednavku vytvoril*/
ShipmentFrom DATETIME NULL,
ShipmentTo DATETIME NULL
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
RegionCode NVARCHAR(100) NULL, UserMargin DECIMAL(19,2) NULL, RestrictedAccess INT NULL, Statut NVARCHAR(10) NULL
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
	[Cena] [decimal](19,2) NULL,
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
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------
CREATE VIEW vShpVlastnostiProduktu AS SELECT A=1
GO
CREATE VIEW vShpPiktogramyProduktu AS SELECT A=1
GO
CREATE VIEW vShpParfumacieProduktu AS SELECT A=1
GO
CREATE VIEW vShpUcinkyProduktu AS SELECT A=1
GO
CREATE VIEW vShpCenyProduktu AS SELECT A=1
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
CREATE PROCEDURE pShpCarts AS BEGIN SET NOCOUNT ON; END
GO

CREATE PROCEDURE pShpAssociatedOrderCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAssociatedOrderModify AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpAssociatedOrderDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
-- Clasifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Standard procedures
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Procedures declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Triggers declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Triggers declarations
------------------------------------------------------------------------------------------------------------------------


ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0), a.Roles,
	Created = ah.HistoryStamp
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
	INNER JOIN tAccount ah ON a.AccountId=ISNULL(ah.HistoryId, ah.AccountId) AND ah.HistoryType='C'
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]
GO

/*
SELECT * FROM vAccounts
SELECT * FROM vAccountRoles

DECLARE @Roles NVARCHAR(200)
SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=0
SELECT @Roles
*/
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
	Created = ( SELECT MIN(HistoryStamp) FROM tAccount WHERE ( AccountId=a.AccountId OR HistoryId=a.AccountId )  )
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

--SELECT * FROM vOrganizations


ALTER VIEW vShpCartProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	cp.CartProductId, cp.InstanceId, cp.CartId, c.AccountId, cp.ProductId, ProductCode = p.Code,ProductName = pl.Name, 
	cp.Quantity,
	cp.Price,/*Cena BEZ DPH*/
	cp.PriceWVAT,  /*Cena S DPH*/
	cp.VAT,/*DPH*/
	cp.Discount, /*Zlava*/
	cp.PriceTotal, /*Cena spolu BEZ DPH*/
	PriceTotalWVAT,/*Cena spolu S DPH*/
	ProductAvailability = p.Availability, a.Alias, pl.Locale,
	cp.CurrencyId, CurrencySymbol = cur.Symbol, CurrencyCode = cur.Code,
	p.Body, BodyCelkem = (p.Body * cp.Quantity)
FROM
	tShpCartProduct cp
	INNER JOIN tShpProduct p ON p.ProductId = cp.ProductId
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	INNER JOIN tShpCart c ON c.CartId = cp.CartId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId = cp.CurrencyId
GO


ALTER VIEW vShpCarts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed, a.Locale,
	c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
	c.PaymentCode, PaymentName = p.Name,
	c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
	PriceTotal = c.Price, PriceTotalWVAT = c.PriceWVAT, c.Discount, c.[Status]
FROM
	tShpCart c
	LEFT JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode
	LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode
GO


ALTER VIEW vShpCategories
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	c.CategoryId, c.[Order], c.InstanceId, c.ParentId, cl.[Name], cl.Locale,
	c.Icon, a.UrlAliasId, a.Url, a.Alias
FROM
	tShpCategory c 
	LEFT JOIN tShpCategoryLocalization cl ON cl.CategoryId = c.CategoryId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = cl.UrlAliasId AND a.Locale = cl.Locale 
WHERE
	c.HistoryId IS NULL
GO


ALTER VIEW vShpCenyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	c.ProductId, c.Locale, c.CurrencyId, c.Cena, SurrencySymbol = cur.Symbol, SurrencyCode = cur.Code  
FROM tShpCenyProduktu c INNER JOIN
cShpCurrency cur ON cur.CurrencyId=c.CurrencyId
GO


ALTER VIEW vShpOrders
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	o.OrderId, o.InstanceId, o.OrderNumber, o.OrderDate, o.CartId, c.AccountId, AccountName = a.[Login], o.PaydDate,
	o.OrderStatusCode, OrderStatusName = os.Name, OrderStatusIcon = os.Icon,
	o.ShipmentCode, ShipmentName = s.Name, ShipmentIcon = s.Icon, ShipmentPrice = s.Price, ShipmentPriceWVAT = s.PriceWVAT,
	o.PaymentCode, PaymentName = p.Name, PaymentIcon = p.Icon,
	o.Price, o.PriceWVAT,
	o.DeliveryAddressId, o.InvoiceAddressId, o.InvoiceUrl, o.[Notes],
	o.Notified, o.Exported,
	o.CurrencyId, CurrencySymbol = cur.Symbol, CurrencyCode = cur.Code,
	o.ParentId/*Parent objednavka*/, 
	o.AssociationAccountId/*Pridruzienie tejto objednavky k objednavke pouzivatela*/, 
	o.AssociationRequestStatus, /*Status poziadavky na pridruzenie*/
	o.CreatedByAccountId/*Pouzivatel, ktory objednavku vytvoril*/,
	o.ShipmentFrom,o.ShipmentTo,
	OwnerName = org1.Name,
	CreatedByName = org2.Name,
	a.TVD_Id
FROM
	tShpOrder o
	INNER JOIN vShpCarts c ON c.CartId = o.CartId
	INNER JOIN tAccount a ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s ON s.Code = o.ShipmentCode AND s.Locale=a.Locale AND ( s.InstanceId = 0 OR s.InstanceId = o.InstanceId )
	LEFT JOIN cShpPayment p ON p.Code = o.PaymentCode AND p.HistoryId IS NULL AND ( p.InstanceId = 0 OR p.InstanceId = o.InstanceId )
	LEFT JOIN cShpOrderStatus os ON os.Code = o.OrderStatusCode AND os.Locale=a.Locale AND ( os.InstanceId = 0 OR os.InstanceId = o.InstanceId )
	LEFT JOIN cShpCurrency cur ON o.CurrencyId = cur.CurrencyId AND ( cur.InstanceId = 0 OR cur.InstanceId = o.InstanceId )
	LEFT JOIN vOrganizations org1 ON org1.AccountId = a.AccountId
	LEFT JOIN vOrganizations org2 ON org2.AccountId = o.CreatedByAccountId
WHERE o.HistoryId IS NULL
GO


ALTER VIEW vShpPiktogramyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpPiktogramyProduktu 
GO

ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = pl.Name, ProductPrice= cp.Cena, cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code, ProductDiscount= p.Discount,
	ProductPriceWDiscount = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (cp.Cena - ( cp.Cena * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (cp.Cena - p.Discount )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	PriceTotal = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN (cp.Cena - ( cp.Cena * ( p.Discount / 100 ) ))/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN (cp.Cena - p.Discount )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	PriceTotalWVAT = CASE 
		WHEN p.DiscountTypeId=0 OR p.DiscountTypeId IS NULL THEN ROUND((cp.Cena - ( cp.Cena * ( p.Discount / 100 ) )) * (1 + ISNULL(p.VAT, 0)/100), 2 )/*Zlava %*/
		WHEN p.DiscountTypeId=1 THEN ROUND((cp.Cena - p.Discount ) * (1 + ISNULL(p.VAT, 0)/100), 2 )/*Zlava Suma*/
		ELSE cp.Cena
		END, 

	ProductAvailability = p.Availability, a.Alias,
	pl.Locale
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
GO


ALTER VIEW vShpProducts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , p.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], p.Parfumacia, p.Discount, 
	p.VAT, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL
GO


ALTER VIEW vShpUcinkyProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpUcinkyProduktu 
GO


ALTER VIEW vShpVlastnostiProduktu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ProductId, Locale, [Name], ImageUrl
FROM tShpVlastnostiProduktu 
GO

ALTER PROCEDURE pAccountCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		-- ak account existuje vrati existujuce ID
		SELECT @Result=AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL
		SELECT AccountId = @Result
		RETURN
	END

	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified], CanAccessIntensa, Roles,
		HistoryStamp, HistoryType, HistoryAccount)
	VALUES (@InstanceId, @TVD_Id, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified, @CanAccessIntensa, @Roles,
		GETDATE(), 'C', @HistoryAccount)
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole ( InstanceId, AccountId, RoleId)
		SELECT @InstanceId, @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END
GO

-- EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'aaa', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68' -- @Password=0987oiuk

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
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
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

		INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, Roles, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, Roles, 
			HistoryStamp, HistoryType, HistoryAccount, @AccountId
		FROM tAccount
		WHERE AccountId = @AccountId

		UPDATE tAccount 
		SET
			TVD_Id=ISNULL(@TVD_Id,TVD_Id), Roles=ISNULL(@Roles, Roles), [Login] = @Login, [Password] = @Password, Email = @Email, [Enabled] = @Enabled, [Locale] = @Locale, CanAccessIntensa=@CanAccessIntensa,
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
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			@ParentId, @Code, @VATPayment, @TopManager,
			@FAX, @Skype, @ICQ, @ContactBirthDay, @ContactCardId, @ContactWorkPhone, @PF, @RegionCode, @UserMargin, @Statut,
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
EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'mothiva', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Result = @Result OUTPUT
EXEC pOrganizationCreate @HistoryAccount=1, @InstanceId=1, @AccountId=@Result, @Name='Mothiva, s.r.o.'

SELECT * FROM tAccount
SELECT * from vOrganizations
*/

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
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut,
			HistoryStamp, HistoryType, HistoryAccount, @OrganizationId
		FROM tOrganization
		WHERE OrganizationId = @OrganizationId

		UPDATE tOrganization 
		SET
			Id1 = @Id1, Id2 = @Id2, Id3 = @Id3, Name = @Name, Notes = @Notes, Web = @Web, 
			ContactEMail = @ContactEMail, ContactPhone = @ContactPhone, ContactMobile = @ContactMobile, 
			ParentId=@ParentId, Code=@Code, VATPayment=@VATPayment, TopManager=@TopManager,
			FAX=@FAX, Skype=@Skype, ICQ=@ICQ, ContactBirthDay=@ContactBirthDay, ContactCardId=@ContactCardId, ContactWorkPhone=@ContactWorkPhone, PF=@PF, RegionCode=@RegionCode, UserMargin=@UserMargin,Statut=@Statut,
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
ALTER PROCEDURE pShpCartCreate
	@InstanceId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = NULL,
	@PriceWVAT DECIMAL(19,2) = NULL,
	@Discount DECIMAL(19,2) = NULL,
	@Status INT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes, Price, PriceWVAT, Discount, [Status] ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentCode, @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes, @Price, @PriceWVAT, @Discount, @Status )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END
GO

ALTER PROCEDURE pShpCartModify
	@CartId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = NULL,
	@PriceWVAT DECIMAL(19,2) = NULL,
	@Discount DECIMAL(19,2) = NULL,
	@Status INT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCart
		SET AccountId = @AccountId, SessionId = @SessionId, ShipmentCode = @ShipmentCode, PaymentCode = @PaymentCode,
			Closed = @Closed, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Discount=@Discount, [Status]=@Status
		WHERE CartId = @CartId

		SET @Result = @CartId

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

ALTER PROCEDURE pShpCartProductCreate
	@InstanceId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@CurrencyId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity, Price, PriceWVAT, VAT, Discount, PriceTotal, PriceTotalWVAT, CurrencyId ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity, @Price, @PriceWVAT, @VAT, @Discount, @PriceTotal, @PriceTotalWVAT, @CurrencyId )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO

ALTER PROCEDURE pShpCartProductModify
	@CartProductId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@VAT DECIMAL(19,2) = 0,
	@Discount DECIMAL(19,2) = 0,
	@PriceTotal DECIMAL(19,2) = 0,
	@PriceTotalWVAT DECIMAL(19,2) = 0,
	@CurrencyId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId) 
		RAISERROR('Invalid CartProductId %d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCartProduct
		SET CartId = @CartId, ProductId = @ProductId, Quantity = @Quantity,
		Price=@Price, PriceWVAT=@PriceWVAT, VAT=@VAT, Discount=@Discount, PriceTotal=@PriceTotal, PriceTotalWVAT=@PriceTotalWVAT, CurrencyId=@CurrencyId
		WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

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

ALTER PROCEDURE pShpCarts
	@Locale CHAR(2),
	@InstanceId INT,
	@CartId INT = NULL,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@Closed BIT = NULL
--%%WITH ENCRYPTION%%
AS
BEGIN
	SELECT
		c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed, Locale = @Locale,
		c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
		c.PaymentCode, PaymentName = p.Name,
		c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
		PriceTotal = c.Price, PriceTotalWVAT = c.PriceWVAT, c.Discount, c.[Status]
	FROM
		tShpCart c
		LEFT JOIN tAccount a ON a.AccountId = c.AccountId
		LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode
		LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode
	WHERE 
		c.InstanceId = @InstanceId AND 
		c.CartId = ISNULL( @CartId, c.CartId ) AND
		( @AccountId IS NULL OR c.AccountId = @AccountId ) AND
		( @SessionId IS NULL OR c.SessionId = @SessionId  ) AND
		( @Closed IS NULL OR ( @Closed =1 AND c.Closed IS NULL ) )
END
GO

ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@CurrencyId INT = NULL,
	@ParentId INT = NULL,/*Parent objednavka*/
	@AssociationAccountId INT = NULL,/*Pridruzienie tejto objednavky k objednavke pouzivatela*/
	@AssociationRequestStatus INT = 0,/*Status poziadavky na pridruzenie*/
	@CreatedByAccountId INT = NULL,/*Pouzivatel, ktory objednavku vytvoril*/
	@ShipmentFrom DATETIME = NULL,
	@ShipmentTo DATETIME = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(5)

	SET @year =  CAST( YEAR(GETDATE()) as nvarchar(4) )
	SET @month = CAST( MONTH(GETDATE()) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month

	SELECT @number = COUNT(*) + 1 from tShpOrder 
	SET @number = replicate( 0, 4- LEN(@number) ) + @number

	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, GETDATE(), @OrderStatusCode, @ShipmentCode,  @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, @InvoiceUrl, @PaydDate, @Notes, @Price, @PriceWVAT, @Notified, @Exported, @CurrencyId, @ParentId, @AssociationAccountId,@AssociationRequestStatus, @CreatedByAccountId, @ShipmentFrom,  @ShipmentTo,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderId = @Result

END
GO

ALTER PROCEDURE pShpOrderModify
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100),	
	@PaymentCode VARCHAR(100),		
	@PaydDate DATETIME = NULL,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@CurrencyId INT = NULL,
	@ParentId INT = NULL,/*Parent objednavka*/
	@AssociationAccountId INT = NULL,/*Pridruzienie tejto objednavky k objednavke pouzivatela*/
	@AssociationRequestStatus INT = 0,/*Status poziadavky na pridruzenie*/
	@CreatedByAccountId INT = NULL,/*Pouzivatel, ktory objednavku vytvoril*/
	@ShipmentFrom DATETIME = NULL,
	@ShipmentTo DATETIME = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder
		SET
			CartId=@CartId, OrderStatusCode=@OrderStatusCode, ShipmentCode=@ShipmentCode, PaymentCode=@PaymentCode, PaydDate=@PaydDate, InvoiceUrl=@InvoiceUrl, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Notified=@Notified, Exported=@Exported, CurrencyId=@CurrencyId,
			ParentId=@ParentId, AssociationAccountId=@AssociationAccountId, AssociationRequestStatus=@AssociationRequestStatus, CreatedByAccountId=@CreatedByAccountId, ShipmentFrom=@ShipmentFrom, ShipmentTo=@ShipmentTo,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderId = @OrderId

		SET @Result = @OrderId

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

ALTER PROCEDURE pShpSearchProducts
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.ProductId, Title = p.Name, 
		Content = ISNULL(p.Manufacturer,'') + ISNULL( p.Description, '')/* + ISNULL(p.DescriptionLong, '')*/ , 
		UrlAlias = a.Alias, ImageUrl = NULL
	FROM vShpProducts p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.Locale = @Locale AND p.InstanceId = @InstanceId AND 
	(
		p.Name LIKE '%'+@Keywords+'%' OR 
		p.Description LIKE '%'+@Keywords+'%' OR
		--p.DescriptionLong LIKE '%'+@Keywords+'%' OR
		p.Manufacturer LIKE '%'+@Keywords+'%' 
	)
	
END
GO
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

------------------------------------------------------------------------------------------------------------------------
-- Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------

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

------------------------------------------------------------------------------------------------------------------------
-- EOF Procedures declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Triggers declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- EOF Triggers declarations
------------------------------------------------------------------------------------------------------------------------

--======================================================================================================================
-- Init INTENZA
--======================================================================================================================
DECLARE @InstanceId INT
SET @InstanceId = 2

DECLARE @MasterPageId INT, @ContactFormMasterPageId INT
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [Default], [PageUrl]) VALUES(@InstanceId, 'Default', 'Default MasterPage', '~/page.master', 1, '~/page.aspx?name=')
SET @MasterPageId = SCOPE_IDENTITY()
INSERT INTO tMasterPage(InstanceId, [Name], [Description], [Url], [PageUrl]) VALUES(@InstanceId, 'Contact form', 'Default MasterPage With Contact From', '~/pageWithContactForm.master', '~/page.aspx?name=')
SET @ContactFormMasterPageId = SCOPE_IDENTITY()

DECLARE @UrlAliasId INT
DECLARE @PageId INT
-- predefined pages

--------------------------------------------------------------------------------------------------------------------------
-- Nastavenie DPH
SET IDENTITY_INSERT cShpVAT ON
INSERT INTO cShpVAT(InstanceId, VATId, [Percent], Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, -1001, 19.0, 'DHP', '', 'sk', NULL, NULL )
INSERT INTO cShpVAT(InstanceId, VATId, [Percent], Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, -1002, 20.0, 'DHP', '', 'cs', NULL, NULL )
INSERT INTO cShpVAT(InstanceId, VATId, [Percent], Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, -1003, 20.0, 'DHP', '', 'pl', NULL, NULL )
INSERT INTO cShpVAT(InstanceId, VATId, [Percent], Name, Code, Locale, Notes, Icon) VALUES(@InstanceId, -1004, 19.0, 'DHP', '', 'en' , NULL, NULL )
SET IDENTITY_INSERT cShpVAT OFF
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
-- Nastavenie podporovanych jazykov
SET IDENTITY_INSERT cSupportedLocale ON
--INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -2001, 'Slovenský jazyk', 'sk', 'SK', '~/userfiles/entityIcon/SupportedLocale/sk.png')
INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -2002, 'Český jazyk', 'cs', 'CZ', '~/userfiles/entityIcon/SupportedLocale/cs.png')
--INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -2003, 'Anglický jazyk', 'en', 'EN', '~/userfiles/entityIcon/SupportedLocale/en.png')
INSERT INTO cSupportedLocale(InstanceId, SupportedLocaleId, Name, Code, Notes, Icon) VALUES(@InstanceId, -2004, 'Polský jazyk', 'pl', 'PL', '~/userfiles/entityIcon/SupportedLocale/de.png')
SET IDENTITY_INSERT cSupportedLocale OFF
--------------------------------------------------------------------------------------------------------------------------
-- NAVIGATION MENU
--------------------------------------------------------------------------------------------------------------------------
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2001, @InstanceId, 'sk', 'Hlavné navigačné menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2002, @InstanceId, 'cs', 'Hlavní navigační menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2003, @InstanceId, 'en', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- PL
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2004, @InstanceId, 'pl', 'Main navigation menu', 'main-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- FOOTER NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2101, @InstanceId, 'sk', 'Navigačné menu pätičky', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2102, @InstanceId, 'cs', 'Navigační menu patičky', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2103,@InstanceId, 'en', 'Footer navigation menu', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- PL
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2104, @InstanceId, 'pl', 'Footer navigation menu', 'footer-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF

-- PRODUCTS NAVIGATION MENU
-- SK
SET IDENTITY_INSERT tMenu ON
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2301, @InstanceId, 'sk', 'Navigačné menu vyrobkov', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- CS
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2302, @InstanceId, 'cs', 'Navigační menu výrobků', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- EN
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2303,@InstanceId, 'en', 'Products navigation menu', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
-- PL
INSERT INTO tMenu 
([MenuId],  [InstanceId], [Locale], [Name], [Code], [RoleId], [HistoryStamp], [HistoryId], [HistoryType], [HistoryAccount])
VALUES
(-2304, @InstanceId, 'pl', 'Products navigation menu', 'products-menu', NULL, GETDATE(), NULL, 'C', 1 )
SET IDENTITY_INSERT tMenu OFF
--================================================================================================================================
-- PAGES - HOME
--================================================================================================================================
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/default.aspx', @Locale='sk', @Alias = '~/home', @Name='Úvodná stránka',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='Home', @Title='Úvodná stránka',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2001, @Locale='sk', @Order=1, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/default.aspx', @Locale='cs', @Alias = '~/home', @Name='Úvítací stránka',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='Home', @Title='Úvítací stránka',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2002, @Locale='cs', @Order=1, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/default.aspx', @Locale='en', @Alias = '~/home', @Name='Homepage',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='Home', @Title='Homepage',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2003, @Locale='en', @Order=1, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/default.aspx', @Locale='pl', @Alias = '~/home', @Name='Homepage',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='Home', @Title='Homepage',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2004, @Locale='pl', @Order=1, @Name='Home', @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

---------------------------------------------------------------------------------------------------------
-- Home content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -2001, @MasterPageId, '<h4>Domovská stránka</h4>', 'sk', 'home-content', 'Úvodná stránka', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-2002, @MasterPageId, '<h4>Úvítací stránka</h4>', 'cs', 'home-content', 'Úvítací stránka', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-2003, @MasterPageId, '<h4>Home page</h4>', 'en', 'home-content', 'Homepage', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId,-2004, @MasterPageId, '<h4>Home page</h4>', 'pl', 'home-content', 'Homepage', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

--================================================================================================================================
-- PAGES - Terms and Conditions Obchodne podmienky
--================================================================================================================================
SET @pageTitle = 'Obchodné podmienky'
SET @pageName = 'obchodne-podmienky'
SET @pageUrl = '~/page.aspx?name=terms-and-conditions'
SET @pageAlias = '~/obchodne-podmienky'

-- SK
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='terms-and-conditions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2001, @Locale='sk', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2101, @Locale='sk', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

SET @pageTitle = 'Obchodní podmínky'
SET @pageName = 'obchodni-podminky'
SET @pageUrl = '~/page.aspx?name=terms-and-conditions'
SET @pageAlias = '~/obchodni-podminky'	
-- CS
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='terms-and-conditions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2002, @Locale='cs', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2102, @Locale='cs', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
SET @pageTitle = 'Terms and Conditions'
SET @pageName = 'terms-and-conditions'
SET @pageUrl = '~/page.aspx?name=terms-and-conditions'
SET @pageAlias = '~/terms-and-conditions'			
-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='terms-and-conditions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2003, @Locale='en', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2103, @Locale='en', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
				
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='terms-and-conditions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2004, @Locale='pl', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2104, @Locale='pl', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
--================================================================================================================================
-- PAGES - JAK NAKUPOVAT
--================================================================================================================================
SET @pageTitle = 'Ako nakupovať'
SET @pageName = 'ako-nakupovat'
SET @pageUrl = '~/page.aspx?name=how-to-buy'
SET @pageAlias = '~/ako-nakupovat'

-- SK
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='how-to-buy', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2001, @Locale='sk', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2101, @Locale='sk', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

SET @pageTitle = 'Jak nakupovat'
SET @pageName = 'jak-nakupovat'
SET @pageUrl = '~/page.aspx?name=how-to-buy'
SET @pageAlias = '~/jak-nakupovat'	
-- CS
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='how-to-buy', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2002, @Locale='cs', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2102, @Locale='cs', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
SET @pageTitle = 'How to buy'
SET @pageName = 'how-to-buy'
SET @pageUrl = '~/page.aspx?name=how-to-buy'
SET @pageAlias = '~/how-to-buy'			
-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='how-to-buy', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2003, @Locale='en', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2103, @Locale='en', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
			
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='how-to-buy', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2004, @Locale='pl', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2104, @Locale='pl', @Order=3, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
--================================================================================================================================
-- PAGES - DOTAZY
--================================================================================================================================
SET @pageTitle = 'Otázky'
SET @pageName = 'otazky'
SET @pageUrl = '~/page.aspx?name=questions'
SET @pageAlias = '~/otazky'

-- SK
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='questions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2001, @Locale='sk', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2101, @Locale='sk', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

SET @pageTitle = 'Dotazy'
SET @pageName = 'dotazy'
SET @pageUrl = '~/page.aspx?name=questions'
SET @pageAlias = '~/dotazy'	
-- CS
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='questions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2002, @Locale='cs', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2102, @Locale='cs', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
SET @pageTitle = 'Questions'
SET @pageName = 'questions'
SET @pageUrl = '~/page.aspx?name=questions'
SET @pageAlias = '~/questions'			
-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='questions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2003, @Locale='en', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2103, @Locale='en', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
			
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='questions', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @MasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2004, @Locale='pl', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2104, @Locale='pl', @Order=4, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
--================================================================================================================================
-- PAGES - KONTAKTY
--================================================================================================================================
SET @pageTitle = 'Kontakty'
SET @pageName = 'kontakty'
SET @pageUrl = '~/page.aspx?name=contacts'
SET @pageAlias = '~/kontakty'

-- SK
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='contacts', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @ContactFormMasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2001, @Locale='sk', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2101, @Locale='sk', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	

-- CS
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='contacts', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @ContactFormMasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2002, @Locale='cs', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2102, @Locale='cs', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
SET @pageTitle = 'Contacts'
SET @pageName = 'contacts'
SET @pageUrl = '~/page.aspx?name=contacts'
SET @pageAlias = '~/contacts'			
-- EN
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='contacts', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @ContactFormMasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2003, @Locale='en', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2103, @Locale='en', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
		
-- PL
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='contacts', @Title=@pageTitle,
	@Content = '', @UrlAliasId = @UrlAliasId,
	@Result = @PageId OUTPUT, @MasterPageId = @ContactFormMasterPageId
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2004, @Locale='pl', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
EXEC pNavigationMenuCreate @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2104, @Locale='pl', @Order=5, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
---------------------------------------------------------------------------------------------------------		
-- Register	

EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/register.aspx', @Locale='sk', @Alias = '~/registracia', @Name='Registrácia',
	@Result = @UrlAliasId OUTPUT
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='sk', @Name='registracia', @Title='Registrácia',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2001, @Locale='sk', @Order=5, @Name='registrácia', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/register.aspx', @Locale='cs', @Alias = '~/registrace', @Name='Registrace',
	@Result = @UrlAliasId OUTPUT	
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='cs', @Name='registrace', @Title='Registrace',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2002, @Locale='cs', @Order=5, @Name='registerace', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL			
		
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/register.aspx', @Locale='en', @Alias = '~/registration', @Name='Registration',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='en', @Name='registration', @Title='Registration',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2003, @Locale='en', @Order=5, @Name='registration', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL	
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = '~/user/register.aspx', @Locale='pl', @Alias = '~/registration', @Name='Registration',
	@Result = @UrlAliasId OUTPUT		
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='registration', @Title='Registration',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
--EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId,  @MenuId=-2004, @Locale='pl', @Order=5, @Name='registration', @UrlAliasId = @UrlAliasId,
--	@RoleId = NULL
		
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
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='articles', @Title='Articles',
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
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='image-galleries', @Title='Image galeries',
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
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='polls', @Title='Polls',
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
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='news', @Title='News',
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
EXEC pPageCreate @HistoryAccount=1, @InstanceId=@InstanceId, @Locale='pl', @Name='faqs', @Title='Frequently Asked Question',
	@UrlAliasId = @UrlAliasId, @MasterPageId = @MasterPageId,
	@Result = @PageId OUTPUT
			
	
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- NAVIGACNE MENU PRODUKTY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- NOVINKY PRODUKTY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Novinky'
SET @pageUrl = '~/eshop/products.aspx?id=news'
SET @pageAlias = '~/eshop/novinky'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'New Products'
SET @pageUrl = '~/eshop/products.aspx?id=news'
SET @pageAlias = '~/eshop/news'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- TOP VYROBKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Najpredávanejšie'
SET @pageUrl = '~/eshop/products.aspx?id=top'
SET @pageAlias = '~/eshop/najpredavanejsie'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2301,@Locale='sk', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Nejprodávanejší'
SET @pageUrl = '~/eshop/products.aspx?id=top'
SET @pageAlias = '~/eshop/najpredavanejsie'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2302,@Locale='cs', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Best selling'
SET @pageUrl = '~/eshop/products.aspx?id=top'
SET @pageAlias = '~/eshop/best-selling'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2303,@Locale='en', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2304,@Locale='pl', @Order=2, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- AKCNE PONUKY
----------------------------------------------------------------------------------------------------------------------------------------------------------
-- SK
SET @pageTitle = 'Akcie'
SET @pageUrl = '~/eshop/products.aspx?id=action'
SET @pageAlias = '~/eshop/akcie'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='sk', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2301,@Locale='sk', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- CZ
SET @pageTitle = 'Akce'
SET @pageUrl = '~/eshop/products.aspx?id=action'
SET @pageAlias = '~/eshop/akce'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='cs', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2302,@Locale='cs', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL	
	
-- EN
SET @pageTitle = 'Action'
SET @pageUrl = '~/eshop/products.aspx?id=action'
SET @pageAlias = '~/eshop/action'
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='en', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2303,@Locale='en', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL		
	
EXEC pUrlAliasCreate @InstanceId=@InstanceId,  @Url = @pageUrl, @Locale='pl', @Alias = @pageAlias, @Name=@pageTitle,
	@Result = @UrlAliasId OUTPUT
EXEC pNavigationMenuCreate  @HistoryAccount=1, @InstanceId=@InstanceId, @MenuId=-2304,@Locale='pl', @Order=1, @Name=@pageTitle, @UrlAliasId = @UrlAliasId,
	@RoleId = NULL

---------------------------------------------------------------------------------------------------------
UPDATE tAccount SET Locale='cs', [Enabled]=1 WHERE AccountId = 1

---------------------------------------------------------------------------------------------------------
-- VOCABULARY
-- search
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='sk', @Translation='Hľadaj'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='cs', @Translation='Hledej'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='SearchLabel', @Locale='en', @Translation='Find'
-- EShop-CurrencyChoice
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='sk', @Translation='Vyberťe menu'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='cs', @Translation='Vyberte měnu'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='EShop', @Term='CurrencyChoice', @Locale='en', @Translation='Choice currency'

-- INFO CONTROL
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel1', @Locale='sk', @Translation='Potrebujete pomoc?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel1', @Locale='cs', @Translation='Potřebujete pomoc?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel1', @Locale='en', @Translation='You need help?'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel1', @Locale='pl', @Translation='You need help?'

EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel2', @Locale='sk', @Translation='777 123 456 | info@intenza.sk'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel2', @Locale='cs', @Translation='777 123 456 | info@intenza.sk'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel2', @Locale='en', @Translation='777 123 456 | info@intenza.sk'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage', @Term='InfoLabel2', @Locale='pl', @Translation='777 123 456 | info@intenza.sk'

-- FOOTER CONTROL
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='NewslatterText', @Locale='sk', @Translation='Přihlašte se k odběru novinek e-mailem'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='NewslatterText', @Locale='cs', @Translation='Přihlašte se k odběru novinek e-mailem'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='NewslatterText', @Locale='en', @Translation='Přihlašte se k odběru novinek e-mailem'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='NewslatterText', @Locale='pl', @Translation='Přihlašte se k odběru novinek e-mailem'

EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='Recommendations', @Locale='sk', @Translation='Doporučte Nás příbuzným'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='Recommendations', @Locale='cs', @Translation='Doporučte Nás příbuzným'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='Recommendations', @Locale='en', @Translation='Doporučte Nás příbuzným'
EXEC pTranslationCreateEx @HistoryAccount=1, @InstanceId=@InstanceId, @Vocabulary='MasterPage-Footer', @Term='Recommendations', @Locale='pl', @Translation='Doporučte Nás příbuzným'

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
/*
SET IDENTITY_INSERT tShpCategory ON
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1001', @Locale='cs', @Alias = '~/eshop/eurona', @Name='eurona', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (2001, @InstanceId, @UrlAliasId, 1, 'eurona', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1002', @Locale='cs', @Alias = '~/eshop/cerny-cosmetix-professional', @Name='cerny cosmetix professional', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (2002, @InstanceId, @UrlAliasId, 2, 'cerny cosmetix professional', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1003', @Locale='cs', @Alias = '~/eshop/cerny-cosmetix', @Name='cerny cosmetix professional', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (2003, @InstanceId, @UrlAliasId, 3, 'cerny cosmetix', 'C', 'cs');

EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/eshop/category.aspx?id=1004', @Locale='cs', @Alias = '~/eshop/intesa-for-live', @Name='intesa for live', @Result = @UrlAliasId OUTPUT
INSERT INTO tShpCategory ( CategoryId, InstanceId, UrlAliasId, [Order], Name, HistoryType, Locale ) VALUES (2004, @InstanceId, @UrlAliasId, 4, 'intesa for live', 'C', 'cs');
SET IDENTITY_INSERT tShpCategory OFF
*/
------------------------------------------------------------------------------------------------------------------------
SET IDENTITY_INSERT cShpHighlight ON
INSERT INTO cShpHighlight (HighlightId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2002, @InstanceId, 'Akcia' ,'action', NULL ,NULL ,'sk' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpHighlight (HighlightId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2102, @InstanceId, 'Akce' ,'action', NULL ,NULL ,'cs' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpHighlight (HighlightId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2202, @InstanceId, 'Action' ,'action', NULL ,NULL ,'en' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpHighlight (HighlightId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2302, @InstanceId, 'Action' ,'action', NULL ,NULL ,'pl' ,GETDATE() ,NULL ,'C' , 1)
SET IDENTITY_INSERT cShpHighlight OFF

------------------------------------------------------------------------------------------------------------------------
-- ShpPayment
SET IDENTITY_INSERT cShpPayment ON
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2001, @InstanceId, 'VISA' ,'VISA', '~/userfiles/entityIcon/ShpPayment/visa.png' ,NULL ,'sk' ,GETDATE() ,NULL ,'C' ,1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2101, @InstanceId, 'VISA' ,'VISA', '~/userfiles/entityIcon/ShpPayment/visa.png' ,NULL ,'cs' ,GETDATE() ,NULL ,'C' ,1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2201, @InstanceId, 'VISA' ,'VISA', '~/userfiles/entityIcon/ShpPayment/visa.png' ,NULL ,'en' ,GETDATE() ,NULL ,'C' ,1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2301, @InstanceId, 'VISA' ,'VISA', '~/userfiles/entityIcon/ShpPayment/visa.png' ,NULL ,'pl' ,GETDATE() ,NULL ,'C' ,1)

INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2002, @InstanceId, 'VisaElectron' ,'VisaElectron', '~/userfiles/entityIcon/ShpPayment/visa-elektron.png' ,NULL ,'sk' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2102, @InstanceId, 'VisaElectron' ,'VisaElectron', '~/userfiles/entityIcon/ShpPayment/visa-elektron.png' ,NULL ,'cs' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2202, @InstanceId, 'VisaElectron' ,'VisaElectron', '~/userfiles/entityIcon/ShpPayment/visa-elektron.png' ,NULL ,'en' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2302, @InstanceId, 'VisaElectron' ,'VisaElectron', '~/userfiles/entityIcon/ShpPayment/visa-elektron.png' ,NULL ,'pl' ,GETDATE() ,NULL ,'C' , 1)

INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2003, @InstanceId, 'MasterCard' ,'MasterCard', '~/userfiles/entityIcon/ShpPayment/master-card.png' ,NULL ,'sk' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2103, @InstanceId, 'MasterCard' ,'MasterCard', '~/userfiles/entityIcon/ShpPayment/master-card.png' ,NULL ,'cs' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2203, @InstanceId, 'MasterCard' ,'MasterCard', '~/userfiles/entityIcon/ShpPayment/master-card.png' ,NULL ,'en' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2303, @InstanceId, 'MasterCard' ,'MasterCard', '~/userfiles/entityIcon/ShpPayment/master-card.png' ,NULL ,'pl' ,GETDATE() ,NULL ,'C' , 1)

INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2004, @InstanceId, 'Maestro' ,'Maestro', '~/userfiles/entityIcon/ShpPayment/maestro.png' ,NULL ,'sk' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2104, @InstanceId, 'Maestro' ,'Maestro', '~/userfiles/entityIcon/ShpPayment/maestro.png' ,NULL ,'cs' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2204, @InstanceId, 'Maestro' ,'Maestro', '~/userfiles/entityIcon/ShpPayment/maestro.png' ,NULL ,'en' ,GETDATE() ,NULL ,'C' , 1)
INSERT INTO cShpPayment (PaymentId, InstanceId, Name ,Code ,Icon ,Notes ,Locale ,HistoryStamp ,HistoryId ,HistoryType ,HistoryAccount)
VALUES (-2304, @InstanceId, 'Maestro' ,'Maestro', '~/userfiles/entityIcon/ShpPayment/maestro.png' ,NULL ,'pl' ,GETDATE() ,NULL ,'C' , 1)
SET IDENTITY_INSERT cShpPayment OFF

GO
--======================================================================================================================
-- EOF Init
--======================================================================================================================

