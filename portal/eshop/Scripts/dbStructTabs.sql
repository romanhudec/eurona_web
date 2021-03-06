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
