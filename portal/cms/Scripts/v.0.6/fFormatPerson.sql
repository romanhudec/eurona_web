ALTER FUNCTION fFormatPerson
(	@FirstName NVARCHAR(100), 
	@LastName NVARCHAR(100), 
	@Email NVARCHAR(100)
)
RETURNS NVARCHAR(100)
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result NVARCHAR(100)
	SET @Result = ISNULL(@FirstName, '') + ' ' + ISNULL(@LastName, '')
	IF @Email IS NOT NULL AND LEN(@Email) > 0 SET @Result = @Result + ' (' + @Email + ')'
	SET @Result = RTRIM(LTRIM(@Result))
	IF LEN(@Result) < 2 SET @Result = ''
	RETURN @Result
END
GO

/*
SELECT dbo.fFormatPerson('Jozef', 'Prídavok', 'jozef.pridavok@mothiva.com')
SELECT dbo.fFormatPerson('', 'Prídavok', '')
SELECT dbo.fFormatPerson(NULL, NULL, NULL)
*/
