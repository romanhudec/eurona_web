ALTER PROCEDURE pPollCreate
	@HistoryAccount INT,
	@Closed BIT = 0,
	@Locale [char](2) = 'en', 
	@Question NVARCHAR(1000),
	@DateFrom DATETIME,
	@DateTo DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPoll ( Closed, Locale, Question, DateFrom, DateTo, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Closed, @Locale, @Question, @DateFrom, @DateTo, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PollId = @Result

END
GO
