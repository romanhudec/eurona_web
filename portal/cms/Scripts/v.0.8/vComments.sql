
ALTER VIEW vComments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[CommentId], [InstanceId], [ParentId], [AccountId], [Date], [Title], [Content], [Votes], [TotalRating]
FROM
	tComment
WHERE
	HistoryId IS NULL
GO
