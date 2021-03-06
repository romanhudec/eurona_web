ALTER PROCEDURE pShpCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Order INT = NULL,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCategory ( InstanceId, [Order], ParentId, [Name], Locale, Icon, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Order, @ParentId, @Name, @Locale, @Icon, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CategoryId = @Result

END
GO
