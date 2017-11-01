
DECLARE @InstanceId INT
SET @InstanceId = 3

--select * from tShpCategory where InstanceId = @InstanceId
delete from tShpProductCategories where CategoryId in (select CategoryId from tShpCategory where InstanceId = @InstanceId)
delete from tShpCategoryLocalization where CategoryId in (select CategoryId from tShpCategory where InstanceId = @InstanceId)
delete from tShpCategory where InstanceId = @InstanceId
delete from tUrlAlias where InstanceId = @InstanceId AND Url LIKE '~/eshop/category.aspx?id='+'%'