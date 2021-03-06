
ALTER VIEW vPersons
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.PersonId, p.InstanceId, p.AccountId, p.Title, p.LastName, p.FirstName, ISNULL(p.Email, a.EMail) as Email, p.Notes,
	p.Phone, p.Mobile, p.AddressHomeId, p.AddressTempId
FROM
	tPerson p LEFT JOIN
	tAccount a ON a.AccountId = p.AccountId	
WHERE
	p.HistoryId IS NULL
ORDER BY p.LastName, p.FirstName
GO

-- SELECT * FROM vPersons
