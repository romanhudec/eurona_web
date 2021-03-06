ALTER PROCEDURE pAccountDelete
	@HistoryAccount INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	UPDATE tAccount
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AccountId
	WHERE AccountId = @AccountId

	SET @Result = @AccountId

END
GO
