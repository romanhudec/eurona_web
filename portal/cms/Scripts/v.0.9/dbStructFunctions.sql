------------------------------------------------------------------------------------------------------------------------
-- Functions declarations
------------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION fFormatAddress(@Street NVARCHAR(100), @Zip NVARCHAR(100), @City NVARCHAR(100)) RETURNS NVARCHAR(100) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fFormatPerson(@FirstName NVARCHAR(100), @LastName NVARCHAR(100), @EMail NVARCHAR(100)) RETURNS NVARCHAR(100) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fAccountRoles(@AccountId INT) RETURNS NVARCHAR(4000) AS BEGIN RETURN '' END
GO

CREATE FUNCTION fStringToTable(@InputString NVARCHAR(4000), @separator NVARCHAR(10))
	RETURNS @table TABLE ([index] INT, item NVARCHAR(200))
AS 
BEGIN
	RETURN
END
GO

CREATE FUNCTION fMakeAnsi(@Text NVARCHAR(4000)) RETURNS NVARCHAR(4000) AS BEGIN RETURN 1 END
GO

CREATE FUNCTION fCompareKeywords(@Pattern NVARCHAR(4000), @Test NVARCHAR(4000)) RETURNS INT AS BEGIN RETURN '' END
GO

CREATE FUNCTION fCompareKeywordsEx(@Pattern NVARCHAR(4000), @Test NVARCHAR(4000), @Spliter CHAR) RETURNS INT AS BEGIN RETURN '' END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Functions declarations
------------------------------------------------------------------------------------------------------------------------
