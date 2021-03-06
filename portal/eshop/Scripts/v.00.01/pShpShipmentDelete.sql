ALTER PROCEDURE pShpShipmentDelete
	@HistoryAccount INT,
	@ShipmentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ShipmentId IS NULL OR NOT EXISTS(SELECT * FROM cShpShipment WHERE ShipmentId = @ShipmentId AND HistoryId IS NULL) 
		RAISERROR('Invalid @ShipmentId=%d', 16, 1, @ShipmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE cShpShipment
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @ShipmentId
		WHERE ShipmentId = @ShipmentId

		SET @Result = @ShipmentId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
