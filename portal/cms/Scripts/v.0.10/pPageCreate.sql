ALTER PROCEDURE pPageCreate
	@HistoryAccount INT,
	@ParentId INT = NULL,
	@InstanceId INT,
	@MasterPageId INT,
	@Locale [char](2) = 'en', 
	@Name NVARCHAR(100),
	@Title NVARCHAR(300),
	@UrlAliasId INT = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL,
	-----------------------------------------
	-- Subpages settings
	@SubPageCreateContents BIT = 0,
	@SubPageMasterPageId INT = NULL,
	-----------------------------------------
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	-- Normalizacia nazvu
	SET @Name = dbo.fMakeAnsi(@Name)
		
	INSERT INTO tPage ( InstanceId, ParentId, MasterPageId, Locale, [Name], Title, UrlAliasId, Content, ContentKeywords, RoleId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ParentId, @MasterPageId, @Locale, @Name, @Title, @UrlAliasId, @Content, @ContentKeywords, @RoleId,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()


	DECLARE @MasterPageContents INT, @UrlAlias NVARCHAR(255)
	SELECT @MasterPageContents=Contents FROM tMasterPage WHERE MasterPageId=@MasterPageId
	SELECT @UrlAlias=Alias FROM tUrlAlias WHERE UrlAliasId=@UrlAliasId
	
	IF @SubPageCreateContents = 1 AND @MasterPageContents > 1
	BEGIN
		DECLARE @SubPageUrl NVARCHAR(255)

		IF @SubPageMasterPageId IS NULL
			SELECT @SubPageMasterPageId = MasterPageId FROM tMasterPage WHERE [Default]=1 AND InstanceId=@InstanceId

		SELECT @SubPageUrl = PageUrl FROM tMasterPage WHERE MasterPageId=@SubPageMasterPageId

		DECLARE @i INT
		SET @i=1
		WHILE (@i<=@MasterPageContents)
		BEGIN
			
			DECLARE @SubPageUrlAliasId INT,@SubPageName NVARCHAR(255), @SubPageAlias NVARCHAR(255), @Url NVARCHAR(255)
			SET @SubPageName = @Name+'_content'+ CONVERT(VARCHAR(3),@i)
			SET @Url = @SubPageUrl+@SubPageName
			SET @SubPageAlias = @UrlAlias + '/' + CONVERT(VARCHAR(3),@i)

			EXEC pUrlAliasCreate @InstanceId=@InstanceId, @Url=@Url, @Locale=@Locale, @Alias = @SubPageAlias, @Name=@Title,
			@Result = @SubPageUrlAliasId OUTPUT

			EXEC pPageCreate @HistoryAccount=@HistoryAccount, @ParentId=@Result, @InstanceId=@InstanceId, @Locale=@Locale, @Name=@SubPageName, @Title=@Title,
				@UrlAliasId = @SubPageUrlAliasId, @MasterPageId = @SubPageMasterPageId

			SET @i=@i+1
		END
	END


	SELECT PageId = @Result
END
GO
