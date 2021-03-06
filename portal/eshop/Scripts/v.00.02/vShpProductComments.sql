
ALTER VIEW vShpProductComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	pc.ProductCommentId, pc.InstanceId, pc.ProductId, c.CommentId, c.ParentId, c.AccountId, AccountName = a.Login , c.Date, c.Title, c.Content, 
	Votes = ISNULL(c.Votes, 0 ) , TotalRating = ISNULL(c.TotalRating, 0),
	RatingResult =  ISNULL(c.TotalRating*1.0/c.Votes*1.0, 0 )
FROM
	tShpProductComment pc 
	INNER JOIN vComments c ON c.CommentId = pc.CommentId
	INNER JOIN vAccounts a ON a.AccountId = c.AccountId
GO
