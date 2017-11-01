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
