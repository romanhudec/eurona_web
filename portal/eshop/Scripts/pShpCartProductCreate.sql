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
