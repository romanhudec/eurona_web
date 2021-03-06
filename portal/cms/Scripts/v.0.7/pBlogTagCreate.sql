ALTER PROCEDURE pBlogTagCreate
	@HistoryAccount INT,
	@BlogId INT, 
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
	
	IF NOT EXISTS(SELECT TagId, BlogId FROM vBlogTags WHERE TagId=@TagId AND BlogId=@BlogId) BEGIN
		INSERT INTO tBlogTag ( TagId, BlogId ) VALUES ( @TagId, @BlogId )
	END

END
GO
