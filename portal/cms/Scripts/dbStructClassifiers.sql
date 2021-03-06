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
