ALTER PROCEDURE pShpCartProductModify
	@CartProductId INT,
	@CartId INT,
	@ProductId INT,
	@Quantity INT = 1,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId) 
		RAISERROR('Invalid CartProductId %d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCartProduct
		SET CartId = @CartId, ProductId = @ProductId, Quantity = @Quantity
		WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
