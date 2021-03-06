
ALTER VIEW vForumThreads
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumThreadId, f.ObjectId, f.InstanceId, f.Name, f.Description, f.Icon, f.Locale, f.Locked, f.VisibleForRole, f.EditableForRole,
	f.UrlAliasId, alias.Alias, alias.Url,
	ForumsCount = (SELECT Count(*) FROM tForum WHERE HistoryId IS NULL AND ForumThreadId=f.ForumThreadId),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp INNER JOIN tForum fo ON fo.ForumId=fp.ForumId AND fo.HistoryId IS NULL
		WHERE fp.HistoryId IS NULL AND fo.ForumThreadId=f.ForumThreadId)
FROM
	tForumThread f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId

WHERE
	f.HistoryId IS NULL
GO
