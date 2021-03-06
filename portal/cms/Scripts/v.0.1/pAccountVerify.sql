ALTER PROCEDURE pAccountVerify
	@HistoryAccount INT,
	@AccountId INT,
	@VerifyCode NVARCHAR(1000),
	@Result BIT = 0 OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @ExpectedCode NVARCHAR(1000)
		SELECT @ExpectedCode = VerifyCode FROM tAccount WHERE AccountId = @AccountId
		
		IF ISNULL(@ExpectedCode, '') = ISNULL(@VerifyCode, '') BEGIN
		
			INSERT INTO tAccount ([Login], [Password], [Email], [Enabled], Verified, VerifyCode, [Locale], 
				HistoryStamp, HistoryType, HistoryAccount, HistoryId)
			SELECT
				[Login], [Password], [Email], [Enabled], Verified, VerifyCode, [Locale],
				HistoryStamp, HistoryType, HistoryAccount, @AccountId
			FROM tAccount
			WHERE AccountId = @AccountId

			UPDATE tAccount 
			SET
				Verified = 1,
				HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
			WHERE AccountId = @AccountId
			
			SET @Result = 1
		
		END

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

-- EXEC pAccountModify @HistoryAccount = 1, @Login='system', @Password= '29C2132DB2C521E07D653BFC0FFBEB68', @Enabled = 1, @Locale = 'en', @AccountId = 1, @Email = 'roman.hude@admin.en'
