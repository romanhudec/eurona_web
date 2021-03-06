ALTER PROCEDURE pShpPaymentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Name NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = NULL,
	@Code VARCHAR(100) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM cShpPayment WHERE Code = @Code AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('Code with @Code=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Code, @Locale, @InstanceId);
		RETURN
	END	

	INSERT INTO cShpPayment ( InstanceId, Locale, [Name], [Notes], Code, Icon,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Locale, @Name, @Notes, @Code, @Icon,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT PaymentId = @Result

END
GO
