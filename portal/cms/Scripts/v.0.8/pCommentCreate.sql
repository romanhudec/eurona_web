ALTER PROCEDURE pCommentCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@Date DATETIME,
	@Title NVARCHAR(255) = NULL,
	@Content NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tComment ( InstanceId, ParentId, AccountId, [Date], Title, Content,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @AccountId, @Date, @Title, @Content, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT CommentId = @Result

END
GO
