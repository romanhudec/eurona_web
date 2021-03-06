DECLARE @InstanceId INT,  @MasterPageId INT, @UrlAliasId INT, @PageId INT
SET @InstanceId = 1
SET @MasterPageId = 1
GO
ALTER TABLE [dbo].[tForumThread] ADD [ObjectId] [int] NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumThreads
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumThreadId, f.ObjectId, f.InstanceId, f.Name, f.Description, f.Icon, f.Locale, f.Locked, f.VisibleForRole, f.EditableForRole,
	f.UrlAliasId, alias.Alias, alias.Url,
	ForumsCount = (SELECT Count(*) FROM tForum WHERE HistoryId IS NULL AND ForumThreadId=f.ForumThreadId),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp INNER JOIN tForum fo ON fo.ForumId=fp.ForumId AND fo.HistoryId IS NULL
		WHERE fp.HistoryId IS NULL AND fo.ForumThreadId=f.ForumThreadId)
FROM
	tForumThread f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId

WHERE
	f.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
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
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForums
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	f.ForumId, f.ForumThreadId, f.InstanceId, f.Icon, f.Name, f.Description, f.Pinned, f.Locked, ViewCount = ISNULL(f.ViewCount,0),
	ForumPostCount = (SELECT Count(*) FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId),
	LastPostId = fp.ForumPostId, LastPostDate = ISNULL(fp.Date, f.HistoryStamp), LastPostAccountId = ISNULL(fp.AccountId, f.HistoryAccount), LastPostAccountName = ISNULL(a.Login, ha.Login),
	CreatedByAccountId = f.HistoryAccount, CreatedDate = f.HistoryStamp, CreatedByAccountName = ha.Login,
	f.UrlAliasId, alias.Alias, alias.Url
FROM
	tForum f 
	LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = f.UrlAliasId
	LEFT JOIN tForumPost fp ON fp.ForumPostId = (SELECT TOP 1 fp.ForumPostId FROM tForumPost fp WHERE fp.HistoryId IS NULL AND fp.ForumId=f.ForumId ORDER BY fp.Date DESC)
	LEFT JOIN tAccount a ON a.AccountId = fp.AccountId
	LEFT JOIN tAccount ha ON ha.AccountId = f.HistoryAccount

WHERE
	f.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vForumPosts
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	fp.ForumPostId, fp.ForumId, fp.InstanceId, fp.ParentId, fp.AccountId, fp.IPAddress, fp.Date, fp.Title, fp.Content,
	AccountName = a.Login,
	Votes = ISNULL(fp.Votes, 0), 
	TotalRating = ISNULL(fp.TotalRating, 0),
	RatingResult = CASE WHEN fp.TotalRating!= 0 AND fp.Votes!=0 THEN ISNULL(fp.TotalRating*1.0/fp.Votes*1.0, 0 ) ELSE 0 END
FROM
	tForumPost fp 
	INNER JOIN tAccount a ON a.AccountId = fp.AccountId
WHERE
	fp.HistoryId IS NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostCreate
	@HistoryAccount INT,
	@ForumId INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Date DATETIME,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ForumId, @InstanceId, @ParentId, @AccountId, @IPAddress, @Date, @Title, @Content, 0, 0,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumPostId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pForumPostModify
	@HistoryAccount INT,
	@ForumPostId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumPostId %d', 16, 1, @ForumPostId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, @ForumPostId
		FROM tForumPost
		WHERE ForumPostId = @ForumPostId

		UPDATE tForumPost
		SET
			ParentId=@ParentId, AccountId=@AccountId, IPAddress=@IPAddress, Title=@Title, Content=@Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumPostId = @ForumPostId

		SET @Result = @ForumPostId

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
