ALTER PROCEDURE pAddressDelete
	@HistoryAccount INT,
	@AddressId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @AddressId IS NULL OR NOT EXISTS(SELECT * FROM tAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid @AddressId=%d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tAddress
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @AddressId
		WHERE AddressId = @AddressId

		SET @Result = @AddressId

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
