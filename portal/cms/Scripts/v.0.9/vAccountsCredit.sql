
ALTER VIEW vAccountsCredit
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	[AccountCreditId], [InstanceId], [AccountId], [Credit]
FROM
	tAccountCredit
WHERE
	HistoryId IS NULL
GO
