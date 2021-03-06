ALTER PROCEDURE pShpAddressModify
	@HistoryAccount INT,
	@AddressId INT,
	@FirstName NVARCHAR(200) = NULL,
	@LastName NVARCHAR(200) = NULL,
	@Organization NVARCHAR(200) = NULL,
	@Id1 NVARCHAR(100) = NULL,
	@Id2 NVARCHAR(100) = NULL,
	@Id3 NVARCHAR(100) = NULL,	
	@City NVARCHAR(100) = '',
	@Street NVARCHAR(200) = '',
	@Zip NVARCHAR(30) = '',
	@State NVARCHAR(100)= '',
	@Phone NVARCHAR(100) = NULL,
	@Email NVARCHAR(100) = NULL,
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpAddress WHERE AddressId = @AddressId AND HistoryId IS NULL) 
		RAISERROR('Invalid AddressId %d', 16, 1, @AddressId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpAddress ( FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @AddressId
		FROM tShpAddress
		WHERE AddressId = @AddressId

		UPDATE tShpAddress
		SET
			FirstName=@FirstName, LastName=@LastName, Organization=@Organization, Id1=@Id1, Id2=@Id2, Id3=@Id3, 
			City=@City, Street=@Street, Zip=@Zip, State=@State, Phone=@Phone, Email=@Email, Notes=@Notes,
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
