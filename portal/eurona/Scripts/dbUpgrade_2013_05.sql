
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

ALTER TABLE tShpCart ADD DopravneEurosap DECIMAL(19,2) NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pShpCartModify]
	@CartId INT,
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
	@DopravneEurosap DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCart
		SET AccountId = @AccountId, SessionId = @SessionId, ShipmentCode = @ShipmentCode, PaymentCode = @PaymentCode,
			Closed = @Closed, Notes=@Notes, Price=@Price, PriceWVAT=@PriceWVAT, Discount=ISNULL(@Discount,Discount), [Status]=@Status,
			BodyEurosapTotal=@BodyEurosapTotal, KatalogovaCenaCelkemByEurosap=@KatalogovaCenaCelkemByEurosap,
			DopravneEurosap=@DopravneEurosap
		WHERE CartId = @CartId

		SET @Result = @CartId

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
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pShpCarts]
	@Locale CHAR(2),
	@InstanceId INT,
	@CartId INT = NULL,
	@AccountId INT = NULL,
	@SessionId INT = NULL,
	@Closed BIT = NULL
--%%WITH ENCRYPTION%%
AS
BEGIN
	SELECT
		c.CartId, c.InstanceId, c.AccountId, c.SessionId, c.Created, c.Closed, Locale = @Locale,
		c.ShipmentCode, ShipmentName = s.Name, ShipmentPrice = s.Price,
		c.PaymentCode, PaymentName = p.Name,
		c.DeliveryAddressId, c.InvoiceAddressId, c.[Notes],
		PriceTotal = c.Price, PriceTotalWVAT = c.PriceWVAT, c.Discount, c.[Status],
		c.[BodyEurosapTotal], c.[KatalogovaCenaCelkemByEurosap], c.[DopravneEurosap]
	FROM
		tShpCart c
		LEFT JOIN tAccount a ON a.AccountId = c.AccountId
		LEFT JOIN cShpShipment s ON s.Code = c.ShipmentCode
		LEFT JOIN cShpPayment p ON p.Code = c.PaymentCode
	WHERE 
		(@InstanceId IS NULL OR c.InstanceId = @InstanceId) AND 
		c.CartId = ISNULL( @CartId, c.CartId ) AND
		( @AccountId IS NULL OR c.AccountId = @AccountId ) AND
		( @SessionId IS NULL OR c.SessionId = @SessionId  ) AND
		( @Closed IS NULL OR ( @Closed =1 AND c.Closed IS NULL ) )
END

GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pShpCartCreate]
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
	@DopravneEurosap DECIMAL(19,2) = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DeliveryAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @DeliveryAddressId OUTPUT

	DECLARE @InvoiceAddressId INT
	EXEC pShpAddressCreate @HistoryAccount = 1, @InstanceId=@InstanceId, @Result = @InvoiceAddressId OUTPUT	

	INSERT INTO tShpCart ( InstanceId, AccountId, SessionId, ShipmentCode, PaymentCode, DeliveryAddressId, InvoiceAddressId, Created, Closed, Notes, Price, PriceWVAT, Discount, [Status], BodyEurosapTotal, KatalogovaCenaCelkemByEurosap, DopravneEurosap ) 
	VALUES ( @InstanceId, @AccountId, @SessionId, @ShipmentCode, @PaymentCode, @DeliveryAddressId, @InvoiceAddressId, GETDATE(), @Closed, @Notes, @Price, @PriceWVAT, @Discount, @Status, @BodyEurosapTotal, @KatalogovaCenaCelkemByEurosap, @DopravneEurosap )

	SET @Result = SCOPE_IDENTITY()

	SELECT CartId = @Result

END

GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE tSettings (
	[SettingsId] INT IDENTITY(1,1),
	[InstanceId] INT NOT NULL,
	[Code] NVARCHAR(50) NOT NULL,
	[GroupName] NVARCHAR(255) NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(500) NULL
)
GO

INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (@InstanceId, 'EMAIL_KOMENTAR_PRODUKTU', 'EMAIL', 'Při vložení komentáře produktu : ', '')
INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (@InstanceId, 'EMAIL_PRISPEVEK_DISKUSE', 'EMAIL', 'Při vložení přispěvku diskuse : ', '')
GO
------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------