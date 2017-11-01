ALTER VIEW vPollOptions
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	o.[PollOptionId], o.InstanceId, o.[PollId], o.[Order], o.[Name], 
	Votes = (SELECT COUNT(*) FROM tPollAnswer WHERE PollOptionId = o.[PollOptionId] )
FROM
	tPollOption o 
GO

-- SELECT * FROM vPollOptions
