ALTER PROCEDURE pShpVATCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Percent DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpVAT ( InstanceId, Locale, [Name], [Notes], Code, [Percent], Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Percent, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO
