ALTER PROCEDURE pMenuCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@PageId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tMenu ( Locale, [Order], [Name], Icon, PageId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Name, @Icon, @PageId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO
