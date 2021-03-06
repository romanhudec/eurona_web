ALTER PROCEDURE pShpAddressCreate
	@HistoryAccount INT,
	@InstanceId INT,
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

	INSERT INTO tShpAddress ( InstanceId, FirstName, LastName, Organization, Id1, Id2, Id3, City, Street, Zip, State, Phone, Email, Notes,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @FirstName, @LastName, @Organization, @Id1, @Id2, @Id3, @City, @Street, @Zip, @State, @Phone, @Email, @Notes,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AddressId = @Result

END
GO
