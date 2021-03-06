ALTER VIEW vBankContacts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[BankContactId], [BankName], [BankCode], [AccountNumber], [IBAN], [SWIFT]
FROM
	tBankContact b
WHERE
	b.HistoryId IS NULL
GO

/*
SELECT * FROM vBankContacts
*/
