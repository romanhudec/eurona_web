ALTER PROCEDURE pShpVATDelete
	@HistoryAccount INT,
	@VATId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @VATId IS NULL OR NOT EXISTS(SELECT * FROM cShpVAT WHERE VATId = @VATId AND HistoryId IS NULL) 
		RAISERROR('Invalid @VATId=%d', 16, 1, @VATId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpVAT
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @VATId
		WHERE VATId = @VATId

		SET @Result = @VATId

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
