ALTER PROCEDURE pForumFavoritesModify
	@ForumFavoritesId INT,
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumFavorites WHERE ForumFavoritesId = @ForumFavoritesId ) 
		RAISERROR('Invalid ForumFavoritesId %d', 16, 1, @ForumFavoritesId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tForumFavorites SET ForumId= @ForumId, AccountId=@AccountId
		WHERE ForumFavoritesId = @ForumFavoritesId

		SET @Result = @ForumFavoritesId

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
