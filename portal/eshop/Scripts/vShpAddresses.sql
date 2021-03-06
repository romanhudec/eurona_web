ALTER VIEW vShpAddresses
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[AddressId], [InstanceId], [FirstName], [LastName], [Organization], [Id1], [Id2], [Id3],
	[City], [Street], [Zip], [State],
	[Phone], [Email], [Notes]
FROM
	tShpAddress  WITH (NOLOCK)
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vShpAddresses
