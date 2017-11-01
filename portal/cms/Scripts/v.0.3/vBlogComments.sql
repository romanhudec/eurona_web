
ALTER VIEW vBlogComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	ac.BlogCommentId, ac.BlogId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tBlogComment ac 
	INNER JOIN vComments c ON c.CommentId = ac.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
