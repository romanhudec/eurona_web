ALTER PROCEDURE pShpPaymentDelete
	@HistoryAccount INT,
	@PaymentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @PaymentId IS NULL OR NOT EXISTS(SELECT * FROM cShpPayment WHERE PaymentId = @PaymentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @PaymentId=%d', 16, 1, @PaymentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpPayment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PaymentId
		WHERE PaymentId = @PaymentId

		SET @Result = @PaymentId

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
