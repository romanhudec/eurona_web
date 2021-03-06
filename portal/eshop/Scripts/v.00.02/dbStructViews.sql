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
CREATE VIEW vShpShipments AS SELECT [ShipmentId]=1, [Name]=1, [Icon]=1, [Price]=1, [PriceWVAT]=1
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
