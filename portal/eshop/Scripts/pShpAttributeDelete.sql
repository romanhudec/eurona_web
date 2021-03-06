ALTER PROCEDURE pShpAttributeDelete
	@HistoryAccount INT,
	@AttributeId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AttributeId IS NULL OR NOT EXISTS(SELECT * FROM tShpAttribute WHERE AttributeId = @AttributeId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AttributeId=%d', 16, 1, @AttributeId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpAttribute
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AttributeId
		WHERE AttributeId = @AttributeId

		SET @Result = @AttributeId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
