ALTER FUNCTION fFormatAddress
(	@Street NVARCHAR(100), 
	@Zip NVARCHAR(100), 
	@City NVARCHAR(100)
)
RETURNS NVARCHAR(100)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result NVARCHAR(100)
	SET @Result = ISNULL(@Street, '') + ', ' + ISNULL(@Zip, '') + ' ' + ISNULL(@City, '')
	SET @Result = RTRIM(LTRIM(@Result))
	IF LEN(@Result) < 2 SET @Result = ''
	RETURN @Result
END
GO

/*
SELECT dbo.fFormatAddress('Sásovská cesta 16/A', '97411', 'Banská Bystrica')
SELECT dbo.fFormatAddress('', '', '')
SELECT dbo.fFormatAddress(NULL, NULL, NULL)
*/
