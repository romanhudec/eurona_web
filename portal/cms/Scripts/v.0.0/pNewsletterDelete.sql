ALTER PROCEDURE pNewsletterDelete
	@NewsletterId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NewsletterId IS NULL OR NOT EXISTS(SELECT * FROM tNewsletter WHERE NewsletterId = @NewsletterId) 
		RAISERROR('Invalid @NewsletterId=%d', 16, 1, @NewsletterId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tNewsletter WHERE NewsletterId = @NewsletterId
		SET @Result = @NewsletterId

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
