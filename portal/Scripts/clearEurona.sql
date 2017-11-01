declare @InstanceId int
SET @InstanceId = 1

delete from tShpCartProduct WHERE InstanceId=@InstanceId
delete from tShpOrder WHERE InstanceId=@InstanceId
delete from tShpCart WHERE InstanceId=@InstanceId
delete from tShpAddress WHERE InstanceId=@InstanceId

delete from tOrganization WHERE InstanceId=@InstanceId
delete from tPerson WHERE InstanceId=@InstanceId
delete from tAddress WHERE InstanceId=@InstanceId
delete from tBankContact WHERE InstanceId=@InstanceId

delete from tAccountCredit WHERE InstanceId=@InstanceId
delete from tAccountVote WHERE InstanceId=@InstanceId

delete from tArticle WHERE InstanceId=@InstanceId
delete from tArticleComment WHERE InstanceId=@InstanceId
delete from tShpProductComment WHERE InstanceId=@InstanceId
delete from tComment WHERE InstanceId=@InstanceId

delete from tShpProductCategories where InstanceId=@InstanceId
delete from tShpProductComment where InstanceId=@InstanceId
delete from tShpProductHighlights where InstanceId=@InstanceId
delete from tShpProductLocalization where ProductId in (select ProductId from tShpProduct where InstanceId=@InstanceId)
delete from tShpProductRelation where InstanceId=@InstanceId
delete from tShpProductReviews where InstanceId=@InstanceId
delete from tShpUcinkyProduktu where  ProductId in (select ProductId from tShpProduct where InstanceId=@InstanceId)
delete from tShpVlastnostiProduktu where  ProductId in (select ProductId from tShpProduct where InstanceId=@InstanceId)
delete from tShpCenyProduktu where  ProductId in (select ProductId from tShpProduct where InstanceId=@InstanceId)

delete from tShpProduct where InstanceId=@InstanceId
delete from tShpCartProduct where InstanceId=@InstanceId
delete from tShpOrder where InstanceId=@InstanceId
delete from tShpCart where InstanceId=@InstanceId
delete from tShpAddress where InstanceId=@InstanceId

delete from tOrganization where InstanceId=@InstanceId
delete from tPerson where InstanceId=@InstanceId
delete from tAddress where InstanceId=@InstanceId

delete from tAccountCredit where InstanceId=@InstanceId
delete from tAccountVote where InstanceId=@InstanceId

delete from tArticle where InstanceId=@InstanceId
delete from tArticleComment where InstanceId=@InstanceId
delete from tComment where InstanceId=@InstanceId

update tPage SET HistoryAccount = 1 WHERE HistoryAccount NOT IN (1, 2) AND InstanceId=@InstanceId
update tNavigationMenu SET HistoryAccount = 1 WHERE HistoryAccount NOT IN (1, 2) AND InstanceId=@InstanceId
update tNews SET HistoryAccount = 1 WHERE HistoryAccount NOT IN (1, 2) AND InstanceId=@InstanceId
delete from tAccountRole WHERE AccountId in (select AccountId from tAccount where AccountId NOT IN(1, 2) AND InstanceId=@InstanceId)
delete from tAccount where AccountId NOT IN(1, 2) AND InstanceId=@InstanceId

delete from tShpProduct where InstanceId=@InstanceId

delete from tUrlAlias WHERE InstanceId=@InstanceId AND Url LIKE '~/eshop/product.aspx?id=' + '%'

--==============================================================================================================
-- CLEAR CATEGORY
UPDATE tUrlAlias SET Alias='DELETED' where InstanceId=@InstanceId AND UrlAliasId IN (SELECT cl.UrlAliasId FROM tShpCategoryLocalization cl)
delete from tShpCategoryLocalization  WHERE CategoryId in (select categoryId from tShpCategory WHERE InstanceId=@InstanceId)
delete from tShpProductCategories WHERE InstanceId=@InstanceId
delete from tShpCategory WHERE InstanceId=@InstanceId
delete from tUrlAlias WHERE InstanceId=@InstanceId AND Alias='DELETED'
