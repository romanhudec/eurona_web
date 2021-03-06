ALTER PROCEDURE pShpCartModify
	@CartId INT,
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

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpCart
		SET AccountId = @AccountId, SessionId = @SessionId, ShipmentId = @ShipmentId, PaymentId = @PaymentId,
			Closed = @Closed, Notes=@Notes
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
