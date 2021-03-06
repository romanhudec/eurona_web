ALTER PROCEDURE pShpCartProductDelete
	@CartProductId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CartProductId IS NULL OR NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartProductId = @CartProductId ) 
		RAISERROR('Invalid @CartProductId=%d', 16, 1, @CartProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpCartProduct WHERE CartProductId = @CartProductId

		SET @Result = @CartProductId

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
