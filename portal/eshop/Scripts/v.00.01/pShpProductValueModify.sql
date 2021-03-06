ALTER PROCEDURE pShpProductValueModify
	@HistoryAccount INT,
	@ProductValueId INT,
	@ProductId INT,
	@AttributeId INT,
	@Value NVARCHAR(1000)  = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProductValue WHERE ProductValueId = @ProductValueId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductValueId %d', 16, 1, @ProductValueId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpProductValue ( ProductId, AttributeId, Value,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ProductId, AttributeId, Value,
			HistoryStamp, HistoryType, HistoryAccount, @ProductValueId
		FROM tShpProductValue
		WHERE ProductValueId = @ProductValueId

		UPDATE tShpProductValue
		SET
			ProductId = @ProductId, AttributeId = @AttributeId, Value = @Value,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductValueId = @ProductValueId

		SET @Result = @ProductValueId

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
