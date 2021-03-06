ALTER PROCEDURE pShpCartCreate
	@InstanceId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentCode VARCHAR(100) = NULL,		
	@PaymentCode VARCHAR(100) = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = NULL,
	@PriceWVAT DECIMAL(19,2) = NULL,
	@Discount DECIMAL(19,2) = NULL,
	@Status INT = 0,
	@BodyEurosapTotal INT = 0,
	@KatalogovaCenaCelkemByEurosap DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes, Price, PriceWVAT, Discount, [Status], BodyEurosapTotal, KatalogovaCenaCelkemByEurosap ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentCode, @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes, @Price, @PriceWVAT, @Discount, @Status, @BodyEurosapTotal, @KatalogovaCenaCelkemByEurosap )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END
GO
