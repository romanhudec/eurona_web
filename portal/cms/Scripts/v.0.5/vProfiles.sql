
ALTER VIEW vProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ProfileId, [Name], [Type], [Description]
FROM tProfile
GO
