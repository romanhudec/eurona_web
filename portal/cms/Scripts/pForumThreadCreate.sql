ALTER PROCEDURE pForumThreadCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ObjectId INT = NULL,
	@Name NVARCHAR(255),
	@Description NVARCHAR(2000) = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Locale CHAR(2) = 'en',
	@Locked BIT = 0, /*Priznak ci ma byt dane vlakno uzamknute*/
	@VisibleForRole  NVARCHAR(2000) = NULL, /*Role pre ktore sa vlakno bude zobrazovat*/
	@EditableForRole  NVARCHAR(2000) = NULL, /*Role pre ktore bude vlakno pristupne a vytvaranie prispevkov*/
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	
	INSERT INTO tForumThread ( InstanceId, ObjectId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ObjectId, @Name, @Description, @Icon, @Locale, @Locked, @VisibleForRole, @EditableForRole, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumThreadId = @Result

END
GO
