ALTER PROCEDURE pShpAttributeModify
	@HistoryAccount INT,
	@AttributeId INT,
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

	IF NOT EXISTS(SELECT * FROM tShpAttribute WHERE AttributeId = @AttributeId AND HistoryId IS NULL) 
		RAISERROR('Invalid AttributeId %d', 16, 1, @AttributeId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpAttribute ( InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, CategoryId, [Name], Description, DefaultValue, Type, TypeLimit, Locale,
			HistoryStamp, HistoryType, HistoryAccount, @AttributeId
		FROM tShpAttribute
		WHERE AttributeId = @AttributeId

		UPDATE tShpAttribute
		SET
			CategoryId = @CategoryId, [Name] = @Name, Description = @Description, DefaultValue = @DefaultValue, Type = @Type, TypeLimit = @TypeLimit, Locale = @Locale,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AttributeId = @AttributeId

		SET @Result = @AttributeId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
