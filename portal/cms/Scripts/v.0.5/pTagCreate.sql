ALTER PROCEDURE pTagCreate
	@HistoryAccount INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tTag ( Tag, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @Tag, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT TagId = @Result

END
GO
