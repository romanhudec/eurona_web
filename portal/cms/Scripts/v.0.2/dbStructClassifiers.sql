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
