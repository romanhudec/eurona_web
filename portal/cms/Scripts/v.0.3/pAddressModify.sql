ALTER PROCEDURE pAddressModify
	@HistoryAccount INT,
	@AddressId INT,
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@City NVARCHAR(100) = '',
	@District NVARCHAR(100) = '',
	@Region NVARCHAR(100) = '',
	@Country NVARCHAR(100) = '',
	@State NVARCHAR(100)= '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid AddressId %d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAddress ( City, Street, Zip, District, Region, Country, State, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			City, Street, Zip, District, Region, Country, State, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @AddressId
		FROM tAddress
		WHERE AddressId = @AddressId

		UPDATE tAddress
		SET
			Street = @Street, Zip = @Zip, District = @District, Region = @Region, Country = @Country, State = @State, 
			City = @City,
			Notes = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AddressId = @AddressId

		SET @Result = @AddressId

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
