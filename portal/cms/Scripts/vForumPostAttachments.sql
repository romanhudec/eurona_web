
ALTER VIEW vForumPostAttachments
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	fpa.ForumPostAttachmentId, fpa.ForumPostId, fpa.Name, fpa.Description, fpa.Type, fpa.Url, fpa.Size, fpa.[Order]
FROM
	tForumPostAttachment fpa
GO
