ALTER PROCEDURE pUrlAliasDelete
	@UrlAliasId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @UrlAliasId IS NULL OR NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId) BEGIN
		RAISERROR('Invalid @UrlAliasId=%d', 16, 1, @UrlAliasId);
		RETURN
	END

	UPDATE tPage SET UrlAliasId = NULL WHERE UrlAliasId = @UrlAliasId
	DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId

	SET @Result = @UrlAliasId

END	

GO
