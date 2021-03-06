ALTER PROCEDURE pBankContactModify
	@HistoryAccount INT,
	@BankContactId INT,
	@BankName NVARCHAR(100) = '',
	@BankCode NVARCHAR(100) = '',
	@AccountNumber NVARCHAR(100) = '',
	@IBAN NVARCHAR(100) = '',
	@SWIFT NVARCHAR(100) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
	
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBankContact WHERE BankContactId = @BankContactId AND HistoryId IS NULL) 
		RAISERROR('Invalid BankContactId %d', 16, 1, @BankContactId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tBankContact ( InstanceId, AccountNumber, BankName, BankCode, IBAN, SWIFT,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, AccountNumber, BankName, BankCode, IBAN, SWIFT,
			HistoryStamp, HistoryType, HistoryAccount, @BankContactId
		FROM tBankContact
		WHERE BankContactId = @BankContactId

		UPDATE tBankContact
		SET
			BankName = @BankName, BankCode = @BankCode, AccountNumber = @AccountNumber, IBAN = @IBAN, SWIFT = @SWIFT,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE BankContactId = @BankContactId

		SET @Result = @BankContactId

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
