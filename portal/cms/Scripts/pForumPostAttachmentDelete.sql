ALTER PROCEDURE pForumPostAttachmentDelete
	@ForumPostAttachmentId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumPostAttachmentId IS NULL OR NOT EXISTS(SELECT * FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId) 
		RAISERROR('Invalid @ForumPostAttachmentId=%d', 16, 1, @ForumPostAttachmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId
		SET @Result = @ForumPostAttachmentId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
