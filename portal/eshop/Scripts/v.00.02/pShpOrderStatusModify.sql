ALTER PROCEDURE pShpOrderStatusModify
	@HistoryAccount INT,
	@OrderStatusId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpOrderStatus WHERE OrderStatusId = @OrderStatusId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderStatusId %d', 16, 1, @OrderStatusId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpOrderStatus ( Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Name], [Notes], Code, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @OrderStatusId
		FROM cShpOrderStatus
		WHERE OrderStatusId = @OrderStatusId

		UPDATE cShpOrderStatus
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Icon = @Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderStatusId = @OrderStatusId

		SET @Result = @OrderStatusId

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
