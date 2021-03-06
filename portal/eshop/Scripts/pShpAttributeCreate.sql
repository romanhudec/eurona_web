ALTER PROCEDURE pShpAttributeCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CategoryId INT,
	@Name NVARCHAR(500) = NULL,
	@Description NVARCHAR(1000)  = NULL,
	@DefaultValue NVARCHAR(1000)  = NULL,
	@Type INT,
	@TypeLimit NVARCHAR(MAX) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpAttribute ( InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @CategoryId, @Name, @Description, @DefaultValue, @Type, @TypeLimit, @Locale,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AttributeId = @Result

END
GO
