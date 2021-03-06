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
