ALTER PROCEDURE pForumPostAttachmentModify
	@ForumPostAttachmentId INT,
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

	IF NOT EXISTS(SELECT * FROM tForumPostAttachment WHERE ForumPostAttachmentId = @ForumPostAttachmentId ) 
		RAISERROR('Invalid ForumPostAttachmentId %d', 16, 1, @ForumPostAttachmentId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumPostAttachment SET ForumPostId= @ForumPostId, [Name]=@Name, [Description]=@Description, [Type]=@Type, Url=@Url, [Size]=@Size, [Order]=@Order
		WHERE ForumPostAttachmentId = @ForumPostAttachmentId

		SET @Result = @ForumPostAttachmentId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
