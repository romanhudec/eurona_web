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
	[Name] ASC,
	[InstanceId] ASC
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
-- FORUM tables TODO: 22.10.2010
------------------------------------------------------------------------------------------------------------------------
-- ForumThread
CREATE TABLE [dbo].[tForumThread](
	[ForumThreadId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[ObjectId] [int] NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(2000) NULL,
	[Icon] [nvarchar](255) NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tForumThread_Locale]  DEFAULT ('en'),
	[Locked] [bit] NULL, /*Priznak ci ma byt dane vlakno uzamknute*/
	[VisibleForRole] NVARCHAR(2000) NULL, /*Role pre ktore sa vlakno bude zobrazovat*/
	[EditableForRole] NVARCHAR(2000) NULL, /*Role pre ktore bude vlakno pristupne a vytvaranie prispevkov*/
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_ForumThread] PRIMARY KEY CLUSTERED ([ForumThreadId] ASC)
)
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_UrlAliasId]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForumThread] ([ForumThreadId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_HistoryId]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForumThread_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [CK_tForumThread_HistoryType]
GO

ALTER TABLE [tForumThread]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumThread_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumThread] CHECK CONSTRAINT [FK_tForumThread_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- Forum
CREATE TABLE [dbo].[tForum](
	[ForumId] [int] IDENTITY(1,1) NOT NULL,
	[ForumThreadId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[Icon] [nvarchar](255) NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(2000) NULL,
	[Pinned] [bit] NOT NULL,
	[Locked] [bit] NULL, /*Priznak ci ma byt dane vlakno uzamknute*/
	[ViewCount] [int] NULL,
	[UrlAliasId] [int] NULL,
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,	
 CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED ([ForumId] ASC)
)
GO
ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_ForumThreadId] FOREIGN KEY([ForumThreadId])
	REFERENCES [tForumThread] ([ForumThreadId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_ForumThreadId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_UrlAliasId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_HistoryId]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForum_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [CK_tForum_HistoryType]
GO

ALTER TABLE [tForum]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForum_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForum] CHECK CONSTRAINT [FK_tForum_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumPost
CREATE TABLE [dbo].[tForumPost](
	[ForumPostId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[InstanceId] [int] NULL,
	[ParentId] [int] NULL,
	[AccountId] [int] NOT NULL,
	[IPAddress] [nvarchar] (255) NULL,
	[Date] [datetime] NOT NULL,
	[Title] [nvarchar] (255) NULL,
	[Content] [nvarchar](MAX) NULL,
	[Votes] [int] NULL, /*Pocet hlasov, ktore post obdrzal*/
	[TotalRating] [int] NULL, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	[HistoryStamp] [datetime] NULL,
	[HistoryId] [int] NULL,
	[HistoryType] [char](1) NULL,
	[HistoryAccount] [int] NULL,
 CONSTRAINT [PK_ForumPostId] PRIMARY KEY CLUSTERED ([ForumPostId] ASC)
)
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_ParentId] FOREIGN KEY([ParentId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_ParentId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_ForumId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_AccountId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_HistoryId] FOREIGN KEY([HistoryId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_HistoryId]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [CK_tForumPost_HistoryType] CHECK  (([HistoryType]='D' OR [HistoryType]='M' OR [HistoryType]='C'))
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [CK_tForumPost_HistoryType]
GO

ALTER TABLE [tForumPost]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPost_HistoryAccount] FOREIGN KEY([HistoryAccount])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumPost] CHECK CONSTRAINT [FK_tForumPost_HistoryAccount]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumFavorites
CREATE TABLE [dbo].[tForumFavorites](
	[ForumFavoritesId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_ForumFavoritesId] PRIMARY KEY CLUSTERED ([ForumFavoritesId] ASC)
)
GO

ALTER TABLE [tForumFavorites]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumFavorites_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumFavorites] CHECK CONSTRAINT [FK_tForumFavorites_ForumId]
GO

ALTER TABLE [tForumFavorites]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumFavorites_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumFavorites] CHECK CONSTRAINT [FK_tForumFavorites_AccountId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumTracking
CREATE TABLE [dbo].[tForumTracking](
	[ForumTrackingId] [int] IDENTITY(1,1) NOT NULL,
	[ForumId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_ForumTrackingId] PRIMARY KEY CLUSTERED ([ForumTrackingId] ASC)
)
GO

ALTER TABLE [tForumTracking]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumTracking_ForumId] FOREIGN KEY([ForumId])
	REFERENCES [tForum] ([ForumId])
GO
ALTER TABLE [tForumTracking] CHECK CONSTRAINT [FK_tForumTracking_ForumId]
GO

ALTER TABLE [tForumTracking]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumTracking_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [tAccount] ([AccountId])
GO
ALTER TABLE [tForumTracking] CHECK CONSTRAINT [FK_tForumTracking_AccountId]
GO
------------------------------------------------------------------------------------------------------------------------
-- ForumPostAttachment
CREATE TABLE [dbo].[tForumPostAttachment](
	[ForumPostAttachmentId] [int] IDENTITY(1,1) NOT NULL,
	[ForumPostId] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](2000) NULL,
	[Type] [int] NULL,
	[Url] [nvarchar](255) NULL,
	[Size] [int] NULL,
	[Order] [int] NULL,
 CONSTRAINT [PK_ForumPostAttachmentId] PRIMARY KEY CLUSTERED ([ForumPostAttachmentId] ASC)
)
GO

ALTER TABLE [tForumPostAttachment]  WITH CHECK 
	ADD  CONSTRAINT [FK_tForumPostAttachment_ForumPostId] FOREIGN KEY([ForumPostId])
	REFERENCES [tForumPost] ([ForumPostId])
GO
ALTER TABLE [tForumPostAttachment] CHECK CONSTRAINT [FK_tForumPostAttachment_ForumPostId]
GO

------------------------------------------------------------------------------------------------------------------------
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------
