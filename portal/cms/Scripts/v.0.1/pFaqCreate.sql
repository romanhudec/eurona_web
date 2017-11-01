ALTER PROCEDURE pFaqCreate
	@HistoryAccount INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Question NVARCHAR(4000), 
	@Answer NVARCHAR(4000) = NULL, 
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tFaq ( Locale, [Order], Question, Answer,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Locale, @Order, @Question, @Answer,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT FaqId = @Result

END
GO
