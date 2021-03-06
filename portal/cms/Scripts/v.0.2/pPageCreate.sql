ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@Url NVARCHAR(2000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, Url, Content, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @Url, @Content, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO
