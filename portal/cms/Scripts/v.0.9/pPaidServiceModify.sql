ALTER PROCEDURE pPaidServiceModify
	@HistoryAccount INT,
	@PaidServiceId INT,
	@CreditCost DECIMAL(19,2) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cPaidService WHERE PaidServiceId = @PaidServiceId AND HistoryId IS NULL) 
		RAISERROR('Invalid PaidServiceId %d', 16, 1, @PaidServiceId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cPaidService ( [Name], [Notes], CreditCost,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			[Name], [Notes], CreditCost,
			HistoryStamp, HistoryType, HistoryAccount, @PaidServiceId
		FROM cPaidService
		WHERE PaidServiceId = @PaidServiceId

		UPDATE cPaidService
		SET
			CreditCost = @CreditCost,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PaidServiceId = @PaidServiceId

		SET @Result = @PaidServiceId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
