ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage (MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES (@MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PageId = @Result

END
GO
