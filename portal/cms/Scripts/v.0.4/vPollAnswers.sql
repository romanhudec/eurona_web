ALTER VIEW vPollAnswers
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[PollAnswerId], [PollOptionId], [IP]
FROM
	tPollAnswer
GO

-- SELECT * FROM vPollAnswers
