
ALTER VIEW vProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ProfileId, InstanceId, [Name], [Type], [Description]
FROM tProfile
GO
