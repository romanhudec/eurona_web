
			USE ledshop_cz
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
	tShpAddress  WITH (NOLOCK)
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
	tShpCart c  WITH (NOLOCK)
	LEFT JOIN cShpShipment s   WITH (NOLOCK) ON s.Code = c.ShipmentCode AND s.HistoryId IS NULL
	LEFT JOIN cShpPayment p  WITH (NOLOCK) ON p.Code = c.PaymentCode AND p.HistoryId IS NULL
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
	tShpOrder o WITH (NOLOCK)
	INNER JOIN vShpCarts c  WITH (NOLOCK) ON c.CartId = o.CartId
	INNER JOIN tAccount a  WITH (NOLOCK) ON a.AccountId = c.AccountId
	LEFT JOIN vShpShipments s  WITH (NOLOCK) ON s.Code = o.ShipmentCode AND s.Locale=a.Locale
	LEFT JOIN cShpPayment p  WITH (NOLOCK) ON p.Code = o.PaymentCode AND s.Locale=a.Locale AND p.HistoryId IS NULL
	LEFT JOIN cShpOrderStatus os  WITH (NOLOCK) ON os.Code = o.OrderStatusCode  AND os.HistoryId IS NULL AND os.Locale=a.Locale
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
	tShpProduct p   WITH (NOLOCK)
	LEFT JOIN cShpVAT v  WITH (NOLOCK) ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a  WITH (NOLOCK) ON a.UrlAliasId = p.UrlAliasId
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

	IF NOT EXISTS(SELECT * FROM tShpCart WITH (NOLOCK) WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCart WITH (ROWLOCK)
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

	SELECT @number = COUNT(*) + 1 FROM tShpOrder WITH (NOLOCK) WHERE HistoryId IS NULL AND InstanceId=@InstanceId AND YEAR(OrderDate) = YEAR(GETDATE()) AND MONTH(OrderDate) = MONTH(GETDATE())


	SET @number = replicate( 0, 4 - LEN(@number) ) + @number

	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
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

	IF NOT EXISTS(SELECT * FROM tShpOrder WITH (NOLOCK) WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder WITH (ROWLOCK)
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

	IF @OrderId IS NULL OR NOT EXISTS(SELECT * FROM tShpOrder WITH (NOLOCK) WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid @OrderId=%d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpOrder WITH (ROWLOCK)
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
-- Init
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

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
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -102, '-2', 'Zpracovává se', 'cs', 'Objednávka je právě spracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -103, '-3', 'Vybavená', 'cs', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -104, '-4', 'Storno', 'cs', 'Objednávka je stornovaná' )

-- en
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1001, '-1', 'Waiting for proccess', 'en', 'Objednávka čeká na spracování' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1002, '-2', 'In progress', 'en', 'Objednávka je právě spracovávána zodpovědným zaměstnancem' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1003, '-3', 'Success', 'en', 'Objednávka je vyřízena' )
INSERT INTO cShpOrderStatus ( InstanceId, OrderStatusId, Code, [Name], Locale, Notes ) VALUES ( @InstanceId, -1004, '-4', 'Storno', 'en', 'Objednávka je stornovaná' )

SET IDENTITY_INSERT cShpOrderStatus OFF
------------------------------------------------------------------------------------------------------------------------
-- URL Alis prefix
SET IDENTITY_INSERT cUrlAliasPrefix ON
INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1001, 'eshop', 'eshop', 'sk', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1002, 'eshop', 'eshop', 'cs', 'alias prefix for eshop aliases' )

INSERT INTO cUrlAliasPrefix ( InstanceId, UrlAliasPrefixId, Code, [Name], Locale, Notes ) 
VALUES ( @InstanceId, 1003, 'eshop', 'eshop', 'en', 'alias prefix for eshop aliases' )
SET IDENTITY_INSERT cUrlAliasPrefix OFF

/*
------------------------------------------------------------------------------------------------------------------------
-- Home content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
DECLARE @MasterPageId INT
SELECT TOP 1 @MasterPageId = MasterPageId FROM tMasterPage

SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId, PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -2001, @MasterPageId, '<h4>Elektornický obchod</h4>', 'sk', 'eshop-home-content', 'Úvodná stránka', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

------------------------------------------------------------------------------------------------------------------------
*/
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
-- EOF Init
------------------------------------------------------------------------------------------------------------------------
-- Upgrade db version
INSERT INTO tShpUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 2, GETDATE())
GO
