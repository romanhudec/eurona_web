ALTER PROCEDURE pForumModify
	@HistoryAccount INT,
	@ForumId INT,
	@ForumThreadId INT,
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

	IF NOT EXISTS(SELECT * FROM tForum WHERE ForumId = @ForumId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumId %d', 16, 1, @ForumId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForum ( ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ForumThreadId, InstanceId, Icon, [Name], [Description], Pinned, Locked, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, @ForumId
		FROM tForum
		WHERE ForumId = @ForumId

		UPDATE tForum
		SET
			ForumThreadId=@ForumThreadId, Icon=@Icon, [Name]=@Name, [Description]=@Description, Pinned=@Pinned, Locked=@Locked, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumId = @ForumId

		SET @Result = @ForumId

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
