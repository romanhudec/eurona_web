
ALTER VIEW vProvidedServices
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT 
	ps.[ProvidedServiceId], ps.InstanceId, ps.[AccountId], ps.[PaidServiceId], ps.ObjectId, ps.[ServiceDate], p.CreditCost, p.[Name], p.[Notes]
FROM
	tProvidedService ps INNER JOIN
	vPaidServices p ON p.PaidServiceId = ps.PaidServiceId
GO
