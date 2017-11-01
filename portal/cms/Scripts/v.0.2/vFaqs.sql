ALTER VIEW vFaqs
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[FaqId], [Locale], [Order], [Question], [Answer]
FROM
	tFaq
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vFaqs
