ALTER VIEW vForumFavorites
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ff.ForumFavoritesId, ff.ForumId, ff.AccountId,
	f.ForumThreadId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked,
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=ff.ForumId),
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForumFavorites ff
	INNER JOIN tForum f ON f.ForumId = ff.ForumId
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
GO