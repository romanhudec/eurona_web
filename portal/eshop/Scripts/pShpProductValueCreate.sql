ALTER PROCEDURE pShpProductValueCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ProductId INT,
	@AttributeId INT,
	@Value NVARCHAR(1000)  = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductValue ( InstanceId, ProductId, AttributeId, Value,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ProductId, @AttributeId, @Value,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductValueId = @Result

END
GO
