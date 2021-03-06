ALTER FUNCTION fCompareStrings
(
	@A NVARCHAR(1000), 
	@B NVARCHAR(1000)
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	IF @A IS NULL AND @B IS NULL RETURN 1
	IF LTRIM(RTRIM(LOWER(dbo.fMakeAnsi(@A)))) = LTRIM(RTRIM(LOWER(dbo.fMakeAnsi(@B)))) RETURN 1
	RETURN 0
END
GO


/*
SELECT dbo.fCompareStrings(NULL, NULL)
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Pridavok')
SELECT dbo.fCompareStrings('Jozef Prídavok', '')
SELECT dbo.fCompareStrings('Jozef Prídavok', NULL)
SELECT dbo.fCompareStrings('Jozef Prídavok', 'jozef pridavok')
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Prídavok ')
SELECT dbo.fCompareStrings('Jozef Prídavok', 'Jozef Prydavok ')
SELECT dbo.fCompareStrings('BanskÁ BystricA', ' banska bystrica')
SELECT dbo.fCompareStrings('Praha', ' praha ')
*/
