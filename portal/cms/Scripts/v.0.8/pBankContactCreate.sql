ALTER PROCEDURE pBankContactCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tBankContact ( InstanceId, AccountNumber, BankName, BankCode, SWIFT, IBAN,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountNumber, @BankName, @BankCode, @SWIFT, @IBAN,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT BankContactId = @Result
END
GO
