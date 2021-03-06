
ALTER VIEW vAccountProfiles
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ap.AccountProfileId, ap.AccountId, ap.ProfileId, ap.[Value], ProfileType = p.Type, ProfileName = p.Name
FROM tAccountProfile ap 
INNER JOIN tProfile p ON p.ProfileId = ap.ProfileId
WHERE ap.HistoryId IS NULL
GO
