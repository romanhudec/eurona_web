ALTER FUNCTION fAccountRoles(@AccountId INT) RETURNS NVARCHAR(4000)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Roles NVARCHAR(200)
	SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=@AccountId
	RETURN @Roles
END
GO

/*
SELECT dbo.fAccountRoles(1)
*/
