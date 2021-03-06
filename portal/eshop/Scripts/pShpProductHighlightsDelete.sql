ALTER PROCEDURE pShpProductHighlightsDelete
	@ProductHighlightsId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductHighlightsId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductHighlights WHERE ProductHighlightsId = @ProductHighlightsId ) 
		RAISERROR('Invalid @ProductHighlightsId=%d', 16, 1, @ProductHighlightsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpProductHighlights WHERE ProductHighlightsId = @ProductHighlightsId
		SET @Result = @ProductHighlightsId

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
