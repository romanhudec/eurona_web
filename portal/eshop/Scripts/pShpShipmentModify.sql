ALTER PROCEDURE pShpShipmentModify
	@HistoryAccount INT,
	@ShipmentId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Price DECIMAL(19,2) = 0,
	@VATId INT = NULL, /*DPH%*/	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cShpShipment WHERE ShipmentId = @ShipmentId AND HistoryId IS NULL) 
		RAISERROR('Invalid ShipmentId %d', 16, 1, @ShipmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cShpShipment ( InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
			HistoryStamp, HistoryType, HistoryAccount, @ShipmentId
		FROM cShpShipment
		WHERE ShipmentId = @ShipmentId

		UPDATE cShpShipment
		SET
			Locale = @Locale, [Name] = @Name, [Notes] = @Notes, Code = @Code, Price = @Price, VATId=@VATId, Icon=@Icon,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ShipmentId = @ShipmentId

		SET @Result = @ShipmentId

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
