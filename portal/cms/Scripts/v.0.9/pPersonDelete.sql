ALTER PROCEDURE pPersonDelete
	@HistoryAccount INT,
	@PersonId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPerson WHERE PersonId = @PersonId AND HistoryId IS NULL) 
		RAISERROR('Invalid PersonId %d', 16, 1, @PersonId);

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @AddressHomeId INT, @AddressTempId INT
		SELECT @AddressHomeId = AddressHomeId, @AddressTempId = AddressTempId 
		FROM tPerson WHERE PersonId = @PersonId

		-- mark home address as deleted
		IF @AddressHomeId IS NOT NULL
			EXEC pAddressDelete @HistoryAccount = @HistoryAccount, @AddressId = @AddressHomeId
		

		-- mark temp address as deleted
		IF @AddressTempId IS NOT NULL
			EXEC pAddressDelete @HistoryAccount = @HistoryAccount, @AddressId = @AddressTempId

	
		-- mark person as deleted
		UPDATE tPerson 
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @PersonId
		WHERE PersonId = @PersonId

		SET @Result = @PersonId

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
