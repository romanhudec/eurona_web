ALTER PROCEDURE pForumTrackingCreate
	@ForumId INT,
	@AccountId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumTracking ( ForumId,  AccountId ) VALUES ( @ForumId, @AccountId )
	SET @Result = SCOPE_IDENTITY()

	SELECT ForumTrackingId = @Result

END
GO
