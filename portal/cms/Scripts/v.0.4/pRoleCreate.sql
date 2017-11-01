ALTER PROCEDURE pRoleCreate
	@Name NVARCHAR(200),
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tRole ( [Name], [Notes] ) VALUES ( @Name, @Notes )
	SET @Result = SCOPE_IDENTITY()

	SELECT RoleId = @Result

END
GO
