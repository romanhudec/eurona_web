ALTER VIEW [dbo].[vAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles, a.MustChangeAccount, a.PasswordChanged,
	Created = ISNULL(a.Created, GETDATE())
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]

GO