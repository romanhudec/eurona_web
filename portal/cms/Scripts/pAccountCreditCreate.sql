ALTER PROCEDURE pAccountCreditCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@Credit DECIMAL(19,2),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountCredit ( InstanceId, AccountId, Credit, HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @Credit, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountCreditId = @Result

END
GO
