ALTER VIEW vForums
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumId, f.ForumThreadId, f.InstanceId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked, ViewCount = ISNULL(f.ViewCount,0),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId),
	LastPostId = fp.ForumPostId, LastPostDate = ISNULL(fp.Date, f.HistoryStamp), LastPostAccountId = ISNULL(fp.AccountId, f.HistoryAccount), LastPostAccountName = ISNULL(a.Login, ha.Login),
	CreatedByAccountId = f.HistoryAccount, CreatedDate = f.HistoryStamp, CreatedByAccountName = ha.Login,
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForum f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
	LEFT JOIN tForumPost fp ON fp.ForumPostId = (SELECT TOP 1 fp.ForumPostId FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId ORDER BY fp.Date DESC)
	LEFT JOIN tAccount a ON a.AccountId = fp.AccountId
	LEFT JOIN tAccount ha ON ha.AccountId = f.HistoryAccount

WHERE
	f.HistoryId IS NULL
GO