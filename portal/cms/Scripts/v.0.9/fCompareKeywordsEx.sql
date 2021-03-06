ALTER FUNCTION fCompareKeywordsEx
(	@Pattern NVARCHAR(4000), 
	@Test NVARCHAR(400),
	@Spliter CHAR = NULL
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result INT
	SET @Result = 0
	
	IF EXISTS(
		SELECT * FROM dbo.fStringToTable(@Test, ISNULL(@Spliter,',')) t
		INNER JOIN dbo.fStringToTable(@Pattern, ISNULL(@Spliter, ',')) p ON dbo.fMakeAnsi(LTRIM(RTRIM(lower(t.item)))) = dbo.fMakeAnsi(LTRIM(RTRIM(lower(p.item))))
	)
		SET @Result = 1
	
	RETURN @Result
END
GO

/*
SELECT dbo.fCompareKeywordsEx('software, development, application', 'software')
SELECT dbo.fCompareKeywordsEx('Nákladní / Užitkové', 'Nákladní', '/' )
*/
