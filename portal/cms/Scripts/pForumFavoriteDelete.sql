ALTER PROCEDURE pForumFavoritesDelete
	@ForumFavoritesId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @ForumFavoritesId IS NULL OR NOT EXISTS(SELECT * FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId) 
		RAISERROR('Invalid @ForumFavoritesId=%d', 16, 1, @ForumFavoritesId);

	BEGIN TRANSACTION;

	BEGIN TRY

		DELETE FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId
		SET @Result = @ForumFavoritesId

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
