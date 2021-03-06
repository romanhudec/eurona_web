ALTER PROCEDURE pShpOrderDelete
	@HistoryAccount INT,
	@OrderId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @OrderId IS NULL OR NOT EXISTS(SELECT * FROM tShpOrder WITH (NOLOCK) WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid @OrderId=%d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpOrder WITH (ROWLOCK)
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @OrderId
		WHERE OrderId = @OrderId
		
		SET @Result = @OrderId

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
