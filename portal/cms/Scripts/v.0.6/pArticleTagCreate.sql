ALTER PROCEDURE pArticleTagCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ArticleId FROM vArticleTags WHERE TagId=@TagId AND ArticleId=@ArticleId) BEGIN
		INSERT INTO tArticleTag ( TagId, ArticleId ) VALUES ( @TagId, @ArticleId )
	END

END
GO
