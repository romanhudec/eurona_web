ALTER PROCEDURE pNavigationMenuItemCreate
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, UrlAliasId, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @NavigationMenuId, @Locale, @Order, @Name, @Icon, @UrlAliasId, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
