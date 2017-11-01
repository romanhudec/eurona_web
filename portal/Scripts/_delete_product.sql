select * from tUrlAlias where Url='~/eshop/product.aspx?id=3207'

DECLARE @code NVARCHAR(100), @productId INT
SET @code = '1081'

select @productId = ProductId from tShpProduct where Code=@code

delete from tShpProductCategories where ProductId=@productId
delete from tShpProductLocalization where ProductId=@productId
delete from tShpProductComment where ProductId=@productId
delete from tShpProductHighlights where ProductId=@productId
delete from tShpProductRelation where ProductId=@productId
delete from tShpPiktogramyProduktu where ProductId=@productId
delete from tShpVlastnostiProduktu where ProductId=@productId
delete from tShpUcinkyProduktu where ProductId=@productId

delete from tUrlAlias where UrlAliasId IN (3499, 3500, 3668, 3669 )