ALTER VIEW vPollAnswers
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[PollAnswerId], [InstanceId], [PollOptionId], [IP]
FROM
	tPollAnswer
GO

-- SELECT * FROM vPollAnswers
