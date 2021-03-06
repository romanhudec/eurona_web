ALTER VIEW vLoggedAccounts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.[Login], a.Email, la.LoggedAt, a.TVD_Id, a.[InstanceId], o.Code, o.Name, LoggedMinutes=DATEDIFF(minute, la.LoggedAt , GETDATE()),
	o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ
FROM
	tLoggedAccount la 
	INNER JOIN tAccount a ON a.AccountId = la.AccountId
	LEFT JOIN tOrganization o ON o.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
GO