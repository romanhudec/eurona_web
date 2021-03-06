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
