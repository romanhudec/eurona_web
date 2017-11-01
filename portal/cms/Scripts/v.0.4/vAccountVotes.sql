
ALTER VIEW vAccountVotes
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	AccountVoteId, ObjectType, ObjectId, AccountId, Rating, [Date]
FROM tAccountVote
GO
