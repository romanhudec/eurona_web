ALTER PROCEDURE pShpOrderModify
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderStatusId INT = NULL,								
	@ShipmentId INT,	
	@PaymentId INT,		
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder ( OrderNumber, CartId, OrderDate, OrderStatusId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, PaydDate, Notes,	
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			OrderNumber, CartId, OrderDate, OrderStatusId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, PaydDate, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder
		SET
			CartId=@CartId, OrderStatusId=@OrderStatusId, ShipmentId=@ShipmentId, PaymentId=@PaymentId, PaydDate=@PaydDate, Notes=@Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrderId = @OrderId

		SET @Result = @OrderId

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
