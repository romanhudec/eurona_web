ALTER PROCEDURE pShpOrderModify
	@HistoryAccount INT,
	@OrderId INT,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100),	
	@PaymentCode VARCHAR(100),		
	@PaydDate DATETIME = NULL,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpOrder WITH (NOLOCK) WHERE OrderId = @OrderId AND HistoryId IS NULL) 
		RAISERROR('Invalid OrderId %d', 16, 1, @OrderId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported,
			HistoryStamp, HistoryType, HistoryAccount, @OrderId
		FROM tShpOrder
		WHERE OrderId = @OrderId

		UPDATE tShpOrder WITH (ROWLOCK)
		SET
			CartId=@CartId, OrderStatusCode=@OrderStatusCode, ShipmentCode=@ShipmentCode, PaymentCode=@PaymentCode, PaydDate=@PaydDate, InvoiceUrl=@InvoiceUrl, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Notified=@Notified, Exported=@Exported,
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
