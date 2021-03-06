ALTER PROCEDURE pShpCartDelete
	@CartId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @CartId IS NULL OR NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId ) 
		RAISERROR('Invalid @CartId=%d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tShpCart WHERE CartId = @CartId

		SET @Result = @CartId

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
