ALTER VIEW vNews
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[NewsId], [Locale], [Date], [Icon], [Head], [Description], [Content]
FROM
	tNews
WHERE
	HistoryId IS NULL
GO

-- SELECT * FROM vNews
