ALTER VIEW vMenu
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	m.MenuId, m.InstanceId, m.Locale, m.[Code], m.[Name], m.RoleId
FROM tMenu m
WHERE m.HistoryId IS NULL
GO
-- SELECT * FROM vMenu