ALTER VIEW vRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[RoleId], [InstanceId], [Name], [Notes]
FROM tRole
GO

-- SELECT * FROM vRoles
