ALTER PROCEDURE pNewsCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Head NVARCHAR(255) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNews ( Locale, [Date], Icon, Head, Description, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Date, @Icon, @Head, @Description, @Content,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsId = @Result

END
GO
