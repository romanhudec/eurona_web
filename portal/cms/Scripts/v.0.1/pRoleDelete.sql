ALTER PROCEDURE pRoleDelete
	@RoleId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @RoleId < 0  
		RAISERROR ('Can not delete system role!', 16, 1);
		
	IF @RoleId IS NULL OR NOT EXISTS(SELECT * FROM tRole WHERE RoleId = @RoleId ) 
		RAISERROR('Invalid @RoleId=%d', 16, 1, @RoleId);
		
	DELETE FROM tRole WHERE RoleId = @RoleId

END	

GO
