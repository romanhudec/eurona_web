
ALTER TABLE tAccount 
ADD MustChangeAccount BIT NOT NULL DEFAULT(0), PasswordChanged DATETIME NULL DEFAULT('01-01-2015')
GO

UPDATE tAccount SET PasswordChanged='01-01-2015'
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles, a.MustChangeAccount, a.PasswordChanged,
	Created = ISNULL(a.Created, GETDATE())
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pAccountCreate]
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		-- ak account existuje vrati existujuce ID
		SELECT @Result=AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL
		SELECT AccountId = @Result
		RETURN
	END

	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged,
		HistoryStamp, HistoryType, HistoryAccount, Created)
	VALUES (@InstanceId, @TVD_Id, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified, @CanAccessIntensa, @CanAccessEurona, @Roles, @MustChangeAccount, @PasswordChanged,
		GETDATE(), 'C', @HistoryAccount, GETDATE())
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole ( InstanceId, AccountId, RoleId)
		SELECT @InstanceId, @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END

GO

------------------------------------------------------------------------------------------------------------------------
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

DECLARE @InstanceId INT
SET @InstanceId = 1
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/changePassword.aspx', @Locale='cs', @Alias = '~/zmena-hesla', @Name='Zmena hesla'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/changePassword.aspx', @Locale='sk', @Alias = '~/zmena-hesla', @Name='Zmena hesla'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/changePassword.aspx', @Locale='en', @Alias = '~/zmena-hesla', @Name='Zmena hesla'
EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url = '~/changePassword.aspx', @Locale='pl', @Alias = '~/zmena-hesla', @Name='Zmena hesla'


