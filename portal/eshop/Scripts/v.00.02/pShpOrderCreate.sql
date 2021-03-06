ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@CartId INT,
	@OrderStatusId INT = NULL,								
	@ShipmentId INT,		
	@PaymentId INT,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(5)

	SET @year =  CAST( YEAR(GETDATE()) as nvarchar(4) )
	SET @month = CAST( MONTH(GETDATE()) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month

	SELECT @number = COUNT(*) + 1 from tShpOrder 
	SET @number = replicate( 0, 5- LEN(@number) ) + @number

	SET @OrderNumber = 'OBJ-' + @year + @month + '-' + @number
	-------------------------------------------------------------------
	
	INSERT INTO tShpOrder ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusId, ShipmentId, PaymentId, DeliveryAddressId, InvoiceAddressId, PaydDate, Notes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, GETDATE(), @OrderStatusId, @ShipmentId,  @PaymentId, @DeliveryAddressId, @InvoiceAddressId, @PaydDate, @Notes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT OrderId = @Result

END
GO
