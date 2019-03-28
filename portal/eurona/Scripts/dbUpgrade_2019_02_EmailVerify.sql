ALTER TABLE tAccount ADD 
	EmailVerifyCode NVARCHAR(500) NULL,
	EmailToVerify NVARCHAR(100) NULL,
	EmailVerifyStatus INT NULL,
	EmailVerified DATETIME NULL
GO
--
ALTER TABLE tAccount ADD 
	LoginBeforeVerify NVARCHAR(100) NULL,
	EmailBeforeVerify NVARCHAR(100) NULL
GO

update tAccount set LoginBeforeVerify=Login, EmailBeforeVerify=Email
GO
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	a.EmailVerifyCode, a.EmailToVerify, a.EmailVerifyStatus, a.EmailVerified, a.LoginBeforeVerify, a.EmailBeforeVerify,
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles, a.MustChangeAccount, a.PasswordChanged, a.SingleUserCookieLinkEnabled,
	Created = ISNULL(a.Created, GETDATE())
FROM
	tAccount a WITH (NOLOCK)
	LEFT JOIN vAccountsCredit ac WITH (NOLOCK) ON ac.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pAccountCreate]
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(100),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@EmailVerifyCode NVARCHAR(500) = NULL,
	@EmailToVerify NVARCHAR(100) = NULL,
	@EmailVerifyStatus INT = NULL,
	@EmailVerified DATETIME = NULL,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@SingleUserCookieLinkEnabled bit = 1,
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

	INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled,
		HistoryStamp, HistoryType, HistoryAccount, Created,
		EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify)
	VALUES (@InstanceId, @TVD_Id, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified, @CanAccessIntensa, @CanAccessEurona, @Roles, @MustChangeAccount, @PasswordChanged, @SingleUserCookieLinkEnabled,
		GETDATE(), 'C', @HistoryAccount, GETDATE(),
		@EmailVerifyCode, @EmailToVerify, @EmailVerifyStatus, @EmailVerified, @Email, @Login )
	
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
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pAccountModify]
	@HistoryAccount INT,
	@AccountId INT,
	@Login NVARCHAR(100),
	@Password NVARCHAR(1000),
	@Email NVARCHAR(100) = NULL,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@Enabled BIT,
	@Locale CHAR(2),
	@Verified BIT = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@EmailVerifyCode NVARCHAR(500) = NULL,
	@EmailToVerify NVARCHAR(100) = NULL,
	@EmailVerifyStatus INT = NULL,
	@EmailVerified DATETIME = NULL,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@SingleUserCookieLinkEnabled bit = 1,
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

		INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId,
			EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify)
		SELECT
			InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, @CanAccessEurona, Roles, @MustChangeAccount, @PasswordChanged, @SingleUserCookieLinkEnabled,
			HistoryStamp, HistoryType, HistoryAccount, @AccountId,
			EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify
		FROM tAccount
		WHERE AccountId = @AccountId

		UPDATE tAccount 
		SET
			TVD_Id=ISNULL(@TVD_Id,TVD_Id), Roles=ISNULL(@Roles, Roles), [Login] = @Login, [Password] = @Password, Email = @Email, [Enabled] = @Enabled, [Locale] = @Locale, CanAccessIntensa=@CanAccessIntensa, CanAccessEurona=@CanAccessEurona, SingleUserCookieLinkEnabled=@SingleUserCookieLinkEnabled,
			MustChangeAccount = @MustChangeAccount, PasswordChanged = @PasswordChanged, Verified = ISNULL(@Verified, Verified), VerifyCode = ISNULL(@VerifyCode, VerifyCode),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL,
			EmailVerifyCode=@EmailVerifyCode, EmailToVerify=@EmailToVerify, EmailVerifyStatus=@EmailVerifyStatus, EmailVerified=@EmailVerified
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
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vAdvisorAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	a.EmailVerifyCode, a.EmailToVerify, a.EmailVerifyStatus, a.EmailVerified,  a.LoginBeforeVerify, a.EmailBeforeVerify,
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles,
	Created = ISNULL(a.Created, GETDATE()),
	AdvisorCode = o.Code, o.Name, Phone = o.ContactPhone, Mobile = o.ContactMobile,
	RegisteredAddress = (ar.Street + ', ' + ar.Zip + ', ' + ar.City +', ' + ar.State ),
	CorrespondenceAddress = (cr.Street + ', ' + cr.Zip + ', ' + cr.City +', ' + cr.State ),
	ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu
FROM
	tAccount a (NOLOCK)
	LEFT JOIN vAccountsCredit ac (NOLOCK) ON ac.AccountId = a.AccountId
	INNER JOIN vOrganizations o (NOLOCK) ON o.AccountId = a.AccountId
	LEFT JOIN vAddresses ar (NOLOCK) ON ar.AddressId = o.RegisteredAddressId
	LEFT JOIN vAddresses cr (NOLOCK) ON cr.AddressId = o.CorrespondenceAddressId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]

GO


