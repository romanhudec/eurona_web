ALTER PROCEDURE pForumPostAttachmentCreate
	@ForumPostId INT,
	@Name NVARCHAR(255) = NULL,
	@Description NVARCHAR(2000) = NULL,
	@Type INT = 1,
	@Url NVARCHAR(255) = NULL,
	@Size INT = 0,
	@Order INT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumPostAttachment ( ForumPostId,  Name, [Description], [Type], Url, Size, [Order] ) VALUES ( @ForumPostId, @Name, @Description, @Type, @Url, @Size, @Order )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumPostAttachmentId = @Result

END
GO
