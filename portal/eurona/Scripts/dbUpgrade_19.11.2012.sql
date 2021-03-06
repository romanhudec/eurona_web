------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tAccount ADD Created DATETIME NULL
GO

UPDATE tAccount SET Created=( SELECT MIN(HistoryStamp) FROM tAccount a WHERE a.AccountId=aa.AccountId )
FROM tAccount aa
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles,
	Created = ISNULL(a.Created, GETDATE())
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pAccountCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
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

	INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified], CanAccessIntensa, CanAccessEurona, Roles,
		HistoryStamp, HistoryType, HistoryAccount, Created)
	VALUES (@InstanceId, @TVD_Id, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified, @CanAccessIntensa, @CanAccessEurona, @Roles,
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

-- EXEC pAccountCreate @HistoryAccount = NULL, @InstanceId=1, @Login = 'aaa', @Enabled = 1, @Password= '29C2132DB2C521E07D653BFC0FFBEB68' -- @Password=0987oiuk

------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------