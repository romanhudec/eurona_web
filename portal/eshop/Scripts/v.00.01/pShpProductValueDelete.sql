ALTER PROCEDURE pShpProductValueDelete
	@HistoryAccount INT,
	@ProductValueId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ProductValueId IS NULL OR NOT EXISTS(SELECT * FROM tShpProductValue WHERE ProductValueId = @ProductValueId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ProductValueId=%d', 16, 1, @ProductValueId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProductValue
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ProductValueId
		WHERE ProductValueId = @ProductValueId

		SET @Result = @ProductValueId

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
