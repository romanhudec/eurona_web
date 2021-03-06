ALTER PROCEDURE pShpShipmentCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	IF EXISTS(SELECT * FROM cShpShipment WHERE Code = @Code AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('Code with @Code=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Code, @Locale, @InstanceId);
		RETURN
	END	

	INSERT INTO cShpShipment ( InstanceId, Locale, [Name], [Notes], Code, Price, VATId, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Price, @VATId, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ShipmentId = @Result

END
GO
