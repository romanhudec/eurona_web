ALTER PROCEDURE pShpOrderCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@OrderDate DATETIME,
	@CartId INT,
	@OrderStatusCode VARCHAR(100) = NULL,								
	@ShipmentCode VARCHAR(100) = NULL,	
	@ShipmentPrice DECIMAL(19,2) = 0,
	@ShipmentPriceWVAT DECIMAL(19,2) = 0,			
	@PaymentCode VARCHAR(100) = NULL,		
	@DeliveryAddressId INT,
	@InvoiceAddressId INT,
	@InvoiceUrl VARCHAR(500) = NULL,		
	@PaydDate DATETIME = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Price DECIMAL(19,2) = 0,
	@PriceWVAT DECIMAL(19,2) = 0,
	@Notified BIT = 0,
	@Exported BIT = 0,
	@CurrencyId INT = NULL,
	@ParentId INT = NULL,/*Parent objednavka*/
	@AssociationAccountId INT = NULL,/*Pridruzienie tejto objednavky k objednavke pouzivatela*/
	@AssociationRequestStatus INT = 0,/*Status poziadavky na pridruzenie*/
	@CreatedByAccountId INT = NULL,/*Pouzivatel, ktory objednavku vytvoril*/
	@ShipmentFrom DATETIME = NULL,
	@ShipmentTo DATETIME = NULL,
	@NoPostage BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;	
	-------------------------------------------------------------------
	-- Vytvorenie cisla objednavky
	DECLARE @CounterId INT, @Count INT, @OrderNumber nvarchar(100)
	DECLARE @year nvarchar(4), @month nvarchar(2), @number nvarchar(5)

	-- TODO : 15.03.2011
	SELECT @CounterId = [CounterId], @Count = ISNULL([Counter], 0) + 1 FROM tShpOrderCounter WITH (TABLOCK) WHERE InstanceId=@InstanceId AND [Year] = YEAR(@OrderDate) AND [Month] = MONTH(@OrderDate)
	SET @Count = ISNULL(@Count, 1 )

	IF @CounterId IS NULL
	BEGIN
		INSERT INTO tShpOrderCounter WITH (TABLOCK) ( InstanceId, [Year], [Month], [Counter] ) VALUES ( @InstanceId, YEAR(@OrderDate), MONTH(@OrderDate), @Count )
	END
	ELSE
	BEGIN
		UPDATE tShpOrderCounter WITH (TABLOCK) SET [Counter] = @Count WHERE [CounterId] = @CounterId
	END

	--IF @InstanceId = 1 /*EURONA*/
	--BEGIN
	--	SELECT @number = COUNT(*) + 1 from tShpOrder WITH (NOLOCK) where HistoryId IS NULL AND InstanceId=@InstanceId AND YEAR(OrderDate) = YEAR(@OrderDate) AND MONTH(OrderDate) = MONTH(@OrderDate)
	--END
	--ELSE IF @InstanceId = 2 /*INTENZA*/
	--BEGIN
	--	SELECT @number = COUNT(*) + 1 from tShpOrder WITH (NOLOCK) where HistoryId IS NULL AND InstanceId=@InstanceId AND YEAR(OrderDate) = YEAR(@OrderDate) AND MONTH(OrderDate) = MONTH(@OrderDate)
	--END
	--ELSE IF @InstanceId = 3 /*CERNY for LIFE*/
	--BEGIN
	--	SELECT @number = COUNT(*) + 1 from tShpOrder WITH (NOLOCK) where HistoryId IS NULL AND InstanceId=@InstanceId AND YEAR(OrderDate) = YEAR(@OrderDate) AND MONTH(OrderDate) = MONTH(@OrderDate)
	--END

	-- Vytvorenie cisla objednavky
	SET @year =  CAST( YEAR(@OrderDate) as nvarchar(4) )
	SET @month = CAST( MONTH(@OrderDate) as nvarchar(2) )
	SET @month = replicate( 0, 2- LEN(@month) ) + @month
	SET @number = replicate( 0, 5- LEN(CAST(@Count as nvarchar(4) ) ) ) + CAST(@Count as nvarchar(4) )
	SET @OrderNumber = @year + @month + @number
	-------------------------------------------------------------------------------------

	INSERT INTO tShpOrder WITH (ROWLOCK) ( InstanceId, OrderNumber, CartId, OrderDate, OrderStatusCode, ShipmentCode, ShipmentPrice, ShipmentPriceWVAT, PaymentCode, DeliveryAddressId, InvoiceAddressId, InvoiceUrl, PaydDate, Notes, Price, PriceWVAT, Notified, Exported, CurrencyId, ParentId, AssociationAccountId, AssociationRequestStatus, CreatedByAccountId, ShipmentFrom, ShipmentTo, NoPostage,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @OrderNumber, @CartId, @OrderDate, @OrderStatusCode, @ShipmentCode, @ShipmentPrice, @ShipmentPriceWVAT,  @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, @InvoiceUrl, @PaydDate, @Notes, @Price, @PriceWVAT, @Notified, @Exported, @CurrencyId, @ParentId, @AssociationAccountId,@AssociationRequestStatus, @CreatedByAccountId, @ShipmentFrom,  @ShipmentTo, @NoPostage,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	-------------------------------------------------------------------------------------
	UPDATE tShpCart SET Closed=GETDATE() WHERE CartId=@CartId

	SELECT OrderId = @Result

END
GO
