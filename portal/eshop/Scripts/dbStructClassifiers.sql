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
