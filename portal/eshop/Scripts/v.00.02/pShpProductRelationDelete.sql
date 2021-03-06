ALTER PROCEDURE pShpProductRelationDelete
	@ProductRelationId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductRelationId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductRelation WHERE ProductRelationId = @ProductRelationId ) 
		RAISERROR('Invalid @ProductRelationId=%d', 16, 1, @ProductRelationId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpProductRelation WHERE ProductRelationId = @ProductRelationId
		SET @Result = @ProductRelationId

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
