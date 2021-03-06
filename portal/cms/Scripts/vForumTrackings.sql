
ALTER VIEW vForumTrackings
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ft.ForumTrackingId, ft.ForumId, ft.AccountId, a.Email, AccountName = a.Login,
	f.ForumThreadId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked,
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=ft.ForumId),
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForumTracking ft
	INNER JOIN tAccount a ON a.AccountId = ft.AccountId
	INNER JOIN tForum f ON f.ForumId = ft.ForumId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
GO
