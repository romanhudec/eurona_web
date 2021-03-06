ALTER VIEW vForumPosts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	fp.ForumPostId, fp.ForumId, fp.InstanceId, fp.ParentId, fp.AccountId, fp.IPAddress, fp.Date, fp.Title, fp.Content,
	AccountName = a.Login,
	Votes = ISNULL(fp.Votes, 0), 
	TotalRating = ISNULL(fp.TotalRating, 0),
	RatingResult = CASE WHEN fp.TotalRating!= 0 AND fp.Votes!=0 THEN ISNULL(fp.TotalRating*1.0/fp.Votes*1.0, 0 ) ELSE 0 END
FROM
	tForumPost fp 
	INNER JOIN tAccount a ON a.AccountId = fp.AccountId
WHERE
	fp.HistoryId IS NULL
GO