ALTER PROCEDURE pArticleCommentCreate
	@HistoryAccount INT,
	@ArticleId INT, 
	@AccountId INT,
	@ParentId INT = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Date DATETIME
	SET @Date = GETDATE()

	DECLARE @CommentId INT
	EXEC pCommentCreate @HistoryAccount = @HistoryAccount, @AccountId=@AccountId, 
	@ParentId=@ParentId, @Date=@Date, @Title=@Title, @Content=@Content, @Result = @CommentId OUTPUT
	
	INSERT INTO tArticleComment ( CommentId, ArticleId ) VALUES ( @CommentId, @ArticleId )

END
GO