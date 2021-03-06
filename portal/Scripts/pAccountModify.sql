ALTER PROCEDURE [dbo].[pAccountModify]
	@HistoryAccount INT,
	@AccountId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000),
	@Email NVARCHAR(100) = NULL,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@Enabled BIT,
	@Locale CHAR(2),
	@Verified BIT = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@Result INT = NULL OUTPUT
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

		INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, @CanAccessEurona, Roles, @MustChangeAccount, @PasswordChanged, 
			HistoryStamp, HistoryType, HistoryAccount, @AccountId
		FROM tAccount
		WHERE AccountId = @AccountId

		UPDATE tAccount 
		SET
			TVD_Id=ISNULL(@TVD_Id,TVD_Id), Roles=ISNULL(@Roles, Roles), [Login] = @Login, [Password] = @Password, Email = @Email, [Enabled] = @Enabled, [Locale] = @Locale, CanAccessIntensa=@CanAccessIntensa, CanAccessEurona=@CanAccessEurona,
			MustChangeAccount = @MustChangeAccount, PasswordChanged = @PasswordChanged, Verified = ISNULL(@Verified, Verified), VerifyCode = ISNULL(@VerifyCode, VerifyCode),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountId = @AccountId
		
		IF @Roles IS NOT NULL BEGIN
			DELETE FROM tAccountRole WHERE AccountId = @AccountId
			INSERT INTO tAccountRole (AccountId, RoleId)
			SELECT @AccountId, r.RoleId
				FROM dbo.fStringToTable(@Roles, ';') x
				INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
		END

		SET @Result = @AccountId

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