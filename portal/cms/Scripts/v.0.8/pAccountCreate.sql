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
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		RETURN
	END
	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified],
		HistoryStamp, HistoryType, HistoryAccount)
	VALUES (@InstanceId, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified,
		GETDATE(), 'C', @HistoryAccount)
	
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
