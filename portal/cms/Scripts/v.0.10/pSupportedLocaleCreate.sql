ALTER PROCEDURE pSupportedLocaleCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO cSupportedLocale ( InstanceId, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT VATId = @Result

END
GO
