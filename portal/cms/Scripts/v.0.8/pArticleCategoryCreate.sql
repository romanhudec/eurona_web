ALTER PROCEDURE pArticleCategoryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Locale [char](2) = 'en', 
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cArticleCategory ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Code, @Notes, GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ArticleCategoryId = @Result

END
GO
