ALTER FUNCTION fMakeAnsi
(
	@Text NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET @Text = REPLACE(@Text, 'á', 'a')
	SET @Text = REPLACE(@Text, 'Á', 'A')
	SET @Text = REPLACE(@Text, 'ä', 'a')
	SET @Text = REPLACE(@Text, 'í', 'i')
	SET @Text = REPLACE(@Text, 'Í', 'i')
	SET @Text = REPLACE(@Text, 'ó', 'o')
	SET @Text = REPLACE(@Text, 'Ó', 'O')
	SET @Text = REPLACE(@Text, 'ô', 'o')
	SET @Text = REPLACE(@Text, 'é', 'e')
	SET @Text = REPLACE(@Text, 'ě', 'e')
	SET @Text = REPLACE(@Text, 'É', 'E')
	SET @Text = REPLACE(@Text, 'ú', 'u')
	SET @Text = REPLACE(@Text, 'Ú', 'U')
	SET @Text = REPLACE(@Text, 'ů', 'u')
	SET @Text = REPLACE(@Text, 'Ů', 'U')
	SET @Text = REPLACE(@Text, 'ľ', 'l')
	SET @Text = REPLACE(@Text, 'Ľ', 'L')
	SET @Text = REPLACE(@Text, 'ĺ', 'l')
	SET @Text = REPLACE(@Text, 'Ĺ', 'L')
	SET @Text = REPLACE(@Text, 'š', 's')
	SET @Text = REPLACE(@Text, 'Š', 's')
	SET @Text = REPLACE(@Text, 'č', 'c')
	SET @Text = REPLACE(@Text, 'Č', 'C')
	SET @Text = REPLACE(@Text, 'ť', 't')
	SET @Text = REPLACE(@Text, 'Ť', 'T')
	SET @Text = REPLACE(@Text, 'ž', 'z')
	SET @Text = REPLACE(@Text, 'Ž', 'Z')
	SET @Text = REPLACE(@Text, 'ř', 'r')
	SET @Text = REPLACE(@Text, 'Ř', 'R')
	SET @Text = REPLACE(@Text, 'ý', 'y')
	SET @Text = REPLACE(@Text, 'Ý', 'Y')
	SET @Text = REPLACE(@Text, 'ň', 'n')
	SET @Text = REPLACE(@Text, 'Ň', 'N')
	SET @Text = REPLACE(@Text, 'ď', 'd')
	SET @Text = REPLACE(@Text, 'ö', 'o')
	RETURN @Text
END
GO

--SELECT dbo.fCorMakeAnsi('Jozef Prídavok, ľščťžýáíé úôř')
