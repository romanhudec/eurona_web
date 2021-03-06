
ALTER VIEW vAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	Roles = dbo.fAccountRoles(a.AccountId)
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	HistoryId IS NULL
ORDER BY [Login]
GO

/*
SELECT * FROM vAccounts
SELECT * FROM vAccountRoles

DECLARE @Roles NVARCHAR(200)
SELECT @Roles = COALESCE(@Roles + ';', '') + RoleName FROM vAccountRoles WHERE AccountId=0
SELECT @Roles
*/