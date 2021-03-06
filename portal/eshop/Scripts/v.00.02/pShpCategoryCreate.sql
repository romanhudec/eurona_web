ALTER PROCEDURE pShpCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpCategory ( InstanceId, ParentId, [Name], Locale, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @Name, @Locale, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CategoryId = @Result

END
GO
