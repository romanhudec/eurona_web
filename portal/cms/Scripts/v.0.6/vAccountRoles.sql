ALTER VIEW vAccountRoles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, ar.AccountRoleId, r.[RoleId], RoleName = r.[Name]
FROM tRole r
INNER JOIN tAccountRole ar (NOLOCK) ON ar.RoleId = r.RoleId
INNER JOIN tAccount a (NOLOCK) ON ar.AccountId = a.AccountId
GO

-- SELECT * FROM vAccountRoles
