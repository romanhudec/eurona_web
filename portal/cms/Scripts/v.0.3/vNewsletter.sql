ALTER VIEW vNewsletter
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	[NewsletterId], [Locale], [Date], [Icon], [Subject], [Content], [Attachment], [Roles], [SendDate]
FROM
	tNewsletter
GO

-- SELECT * FROM vNewsletter
