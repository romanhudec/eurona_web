ALTER PROCEDURE pForumThreadModify
	@HistoryAccount INT,
	@ForumThreadId INT,
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

	IF NOT EXISTS(SELECT * FROM tForumThread WHERE ForumThreadId = @ForumThreadId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumThreadId %d', 16, 1, @ForumThreadId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForumThread ( InstanceId, ObjectId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, ObjectId, [Name], [Description], Icon, Locale, Locked, VisibleForRole, EditableForRole, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @ForumThreadId
		FROM tForumThread
		WHERE ForumThreadId = @ForumThreadId

		UPDATE tForumThread
		SET
			ObjectId=@ObjectId, [Name]=@Name, [Description]=@Description, Icon=@Icon, Locale=@Locale, Locked=@Locked, VisibleForRole=@VisibleForRole, EditableForRole=@EditableForRole, UrlAliasId=@UrlAliasId, 
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumThreadId = @ForumThreadId

		SET @Result = @ForumThreadId

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
