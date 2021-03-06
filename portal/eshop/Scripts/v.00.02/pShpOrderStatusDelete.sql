ALTER PROCEDURE pShpOrderStatusDelete
	@HistoryAccount INT,
	@OrderStatusId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @OrderStatusId IS NULL OR NOT EXISTS(SELECT * FROM cShpOrderStatus WHERE OrderStatusId = @OrderStatusId AND HistoryId IS NULL) 
		RAISERROR('Invalid @OrderStatusId=%d', 16, 1, @OrderStatusId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpOrderStatus
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @OrderStatusId
		WHERE OrderStatusId = @OrderStatusId

		SET @Result = @OrderStatusId

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
