
			USE webcms_HaBIS
			GO
		
------------------------------------------------------------------------------------------------------------------------
-- CMS version 0.0
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
-- EOF Classifiers
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Tabs
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
-- EOF Tabs
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- Views declarations
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- classifiers
CREATE VIEW vAddresses AS SELECT A=1
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
		INNER JOIN dbo.fStringToTable(@Pattern, ',') p ON dbo.fMakeAnsi(LTRIM(RTRIM(t.item))) = dbo.fMakeAnsi(LTRIM(RTRIM(p.item)))
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
	MenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tMenu
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vMenu
ALTER VIEW vNavigationMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tNavigationMenu
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenu
ALTER VIEW vNavigationMenuItem
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	NavigationMenuItemId, NavigationMenuId, Locale, [Order], [Name], Icon, RoleId, PageId
FROM
	tNavigationMenuItem
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vNavigationMenuItem
ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[NewsId], [Locale], [Date], [Icon], [Head], [Description], [Content]
FROM
	tNews
WHERE
	HistoryId IS NULL
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
	[PageId], [MasterPageId], [Locale], [Title], [Name], [Url], [Content], [RoleId]
FROM
	tPage
WHERE
	HistoryId IS NULL
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

ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@PageId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( Locale, [Order], [Name], Icon, PageId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @PageId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

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
	@PageId INT,
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

		INSERT INTO tMenu ( Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, PageId = @PageId, RoleId = @RoleId,
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
	@PageId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, PageId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @PageId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

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
	@PageId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, PageId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @NavigationMenuId, @Locale, @Order, @Name, @Icon, @PageId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

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
	@PageId INT,
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

		INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			NavigationMenuId, Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuItemId
		FROM tNavigationMenuItem
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		UPDATE tNavigationMenuItem
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, PageId = @PageId, RoleId = @RoleId,
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
	@PageId INT,
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

		INSERT INTO tNavigationMenu ( Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuId
		FROM tNavigationMenu
		WHERE NavigationMenuId = @NavigationMenuId

		UPDATE tNavigationMenu
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, PageId = @PageId, RoleId = @RoleId,
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

	INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Date, @Icon, @Head, @Description, @Content,
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

		INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Date], Icon, Head, Description, Content,
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Head = @Head, Description = @Description, Content = @Content,
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
	@Url NVARCHAR(2000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, Url, Content, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @Url, @Content, @RoleId,
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
	@Url NVARCHAR(2000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
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

		INSERT INTO tPage (MasterPageId, Locale, Title, [Name], Url, Content, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			MasterPageId, Locale, Title, [Name], Url, Content, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @PageId
		FROM tPage
		WHERE PageId = @PageId

		UPDATE tPage
		SET
			MasterPageId = @MasterPageId, RoleId = @RoleId,
			Locale = @Locale, [Name] = @Name, Title = @Title, Url = @Url, Content = @Content,
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

------------------------------------------------------------------------------------------------------------------------
-- Init
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
-- default account & credentials

SET IDENTITY_INSERT tRole ON
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-1, 'Administrator', 'System administrator')
INSERT INTO tRole(RoleId, Name, Notes) VALUES(-2, 'Newsletter', 'Information bulletin')
SET IDENTITY_INSERT tRole OFF

EXEC pAccountCreate @HistoryAccount = NULL,
	@Login = 'system', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68', -- @Password=0987oiuk
	@Roles = 'Administrator', @Verified = 1

GO

-- EOF Init
------------------------------------------------------------------------------------------------------------------------

