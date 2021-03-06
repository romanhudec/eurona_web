ALTER PROCEDURE pShpCurrencyCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Rate DECIMAL(19,2) = 0,
	@Symbol VARCHAR(100) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cShpCurrency ( InstanceId, Locale, [Name], [Notes], Code, Rate, Symbol, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Rate, @Symbol, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CurrencyId = @Result

END
GO
