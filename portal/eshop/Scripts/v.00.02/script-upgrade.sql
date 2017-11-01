
--======================================================================================================================
-- UPGRADE ESHOP version 0.1 to 0.2
--======================================================================================================================

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
CREATE VIEW vShpProductRelations AS SELECT ParentProductId=1, ProductId=1
GO
------------------------------------------------------------------------------------------------------------------------
-- ShpProductRelation
CREATE PROCEDURE pShpProductRelationCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductRelationDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vShpProductRelations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pr.ProductRelationId, pr.InstanceId, pr.ParentProductId, pr.ProductId, pr.RelationType,
	ProductName = p.Name, ProductPrice= p.Price, ProductDiscount= p.Discount, 
	ProductPriceWDiscount = (p.Price - ( p.Price * ( p.Discount / 100 ) )), 
	PriceTotal = (p.Price - ( p.Price * ( p.Discount / 100 ))),
	PriceTotalWVAT = ROUND((p.Price - ( p.Price * ( p.Discount / 100 ) )) * (1 + ISNULL(v.[Percent], 0)/100), 2),
	ProductAvailability = p.Availability, a.Alias
FROM
	tShpProductRelation pr
	INNER JOIN tShpProduct p ON p.ProductId = pr.ProductId
	LEFT JOIN cShpVAT v ON v.VATId = p.VATId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
CREATE VIEW vShpProductReviews AS SELECT ProductId=1, ArticleId=1
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
-- ShpProductReviews
CREATE PROCEDURE pShpProductReviewsCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductReviewsDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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

		INSERT INTO cShpHighlight ( Locale, [Name], [Notes], Code, Icon, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Name], [Notes], Code, Icon,
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
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------
DROP TABLE [tShpProductHighlights]
GO
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
CREATE VIEW vShpProductHighlights AS SELECT ProductId=1, HighlightId=1
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
-- ShpProductHighlights
CREATE PROCEDURE pShpProductHighlightsCreate AS BEGIN SET NOCOUNT ON; END
GO
CREATE PROCEDURE pShpProductHighlightsDelete AS BEGIN SET NOCOUNT ON; END
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
--======================================================================================================================
-- Upgrade ESHOP db version
INSERT INTO tShpUpgrade ( VersionMajor, VersionMinor, UpgradeDate)
VALUES ( 0, 2, GETDATE() )
GO
--======================================================================================================================
-- Upgrade
--======================================================================================================================
