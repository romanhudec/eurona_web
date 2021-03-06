ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Code NVARCHAR(100),
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( InstanceId, Locale, Code, [Name], RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Code, @Name, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT MenuId = @Result

END
GO
