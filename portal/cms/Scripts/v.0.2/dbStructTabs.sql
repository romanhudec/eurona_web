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
-- Page

CREATE TABLE [dbo].[tPage](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[MasterPageId] [int] NOT NULL,
	[Locale] [char](2) NOT NULL CONSTRAINT [DF_tPage_Locale]  DEFAULT ('en'),
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](300) NOT NULL,
	[Url] [nvarchar](2000) NULL,
	[Content] [nvarchar](MAX) NULL,
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

ALTER TABLE [tPage]  WITH CHECK 
	ADD  CONSTRAINT [FK_tPage_PageId] FOREIGN KEY([PageId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tPage] CHECK CONSTRAINT [FK_tPage_PageId]
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
	[PageId] [int] NOT NULL,
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
	ADD  CONSTRAINT [FK_tMenu_PageId] FOREIGN KEY([PageId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tMenu] CHECK CONSTRAINT [FK_tMenu_PageId]
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
	[PageId] [int] NOT NULL,
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
	ADD  CONSTRAINT [FK_tNavigationMenu_PageId] FOREIGN KEY([PageId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tNavigationMenu] CHECK CONSTRAINT [FK_tNavigationMenu_PageId]
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
	[PageId] [int] NOT NULL,
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
	ADD  CONSTRAINT [FK_tNavigationMenuItem_PageId] FOREIGN KEY([PageId])
	REFERENCES [tPage] ([PageId])
GO
ALTER TABLE [tNavigationMenuItem] CHECK CONSTRAINT [FK_tNavigationMenuItem_PageId]
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
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------
