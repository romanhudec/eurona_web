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
