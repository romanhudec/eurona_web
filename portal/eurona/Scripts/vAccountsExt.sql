
ALTER VIEW vAccountsExt
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.InstanceId, a.AdvisorId, AdvisorPersonId = p.PersonId
FROM
	tAccountExt a 
	LEFT JOIN tPerson p ON p.AccountId = a.AdvisorId
GO