ALTER PROCEDURE pShpCartCreate
	@InstanceId INT,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@ShipmentId INT = NULL,		
	@PaymentId INT = NULL,	
	@Closed DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentId, @PaymentId, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END
GO
