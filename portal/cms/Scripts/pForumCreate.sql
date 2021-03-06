ALTER PROCEDURE pForumCreate
	@HistoryAccount INT,
	@ForumThreadId INT,
	@InstanceId INT,
	@Icon NVARCHAR(255) = NULL,
	@Name NVARCHAR(255),
	@Description  NVARCHAR(2000) = NULL,
	@Pinned BIT = 0,
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	--ForumId, ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked,
	INSERT INTO tForum ( ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ForumThreadId, @InstanceId, @Icon, @Name, @Description, @Pinned, @Locked,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumId = @Result

END
GO
