ALTER PROCEDURE pShpCartProductCreate
	@InstanceId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCartProduct ( InstanceId, CartId, ProductId, Quantity ) 
	VALUES ( @InstanceId, @CartId, @ProductId, @Quantity )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartProductId = @Result

END
GO
