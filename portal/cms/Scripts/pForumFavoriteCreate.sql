ALTER PROCEDURE pForumFavoritesCreate
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumFavorites ( ForumId,  AccountId ) VALUES ( @ForumId, @AccountId )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumFavoritesId = @Result

END
GO
