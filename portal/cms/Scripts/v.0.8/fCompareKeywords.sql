ALTER FUNCTION fCompareKeywords
(	@Pattern NVARCHAR(4000), 
	@Test NVARCHAR(400)
)
RETURNS INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	DECLARE @Result INT
	SET @Result = 0
	
	IF EXISTS(
		SELECT * FROM dbo.fStringToTable(@Test, ',') t
		INNER JOIN dbo.fStringToTable(@Pattern, ',') p ON dbo.fMakeAnsi(LTRIM(RTRIM(lower(t.item)))) = dbo.fMakeAnsi(LTRIM(RTRIM(lower(p.item))))
	)
		SET @Result = 1
	
	RETURN @Result
END
GO

/*
SELECT dbo.fCompareKeywords('software, development, application', 'software')
SELECT dbo.fCompareKeywords('software, development, application', 'softwares')
SELECT dbo.fCompareKeywords('software, development, application', 'development')
SELECT dbo.fCompareKeywords('software, development, application', 'application')
SELECT dbo.fCompareKeywords('software, development, application', 'software, application')
SELECT dbo.fCompareKeywords('software, development, application', 'software, applycation')
SELECT dbo.fCompareKeywords('software, development, application', 'zoftware, applycation')
SELECT dbo.fCompareKeywords('software, development, application', 'zoftware, application')
SELECT dbo.fCompareKeywords('software, development, application', 'software application')
SELECT dbo.fCompareKeywords('software, development, application', 'hardware')
SELECT dbo.fCompareKeywords('software, development, application', '')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hračky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hraČky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky, čačky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracky, cacky')
SELECT dbo.fCompareKeywords('hračky, mačky, Čačky', 'hracka, cicka')
*/
