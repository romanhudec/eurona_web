ALTER PROCEDURE pNewsModify
	@HistoryAccount INT,
	@UrlAliasId INT = NULL,
	@NewsId INT,
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(255) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNews WHERE NewsId = @NewsId AND HistoryId IS NULL) 
		RAISERROR('Invalid NewsId %d', 16, 1, @NewsId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNews ( Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Locale, [Date], Icon, Title, Teaser, Content, ContentKeywords, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @NewsId
		FROM tNews
		WHERE NewsId = @NewsId

		UPDATE tNews
		SET
			[Date] = @Date, Icon = @Icon, Title = @Title, Teaser = @Teaser, Content = @Content, ContentKeywords = @ContentKeywords, UrlAliasId=@UrlAliasId, 
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NewsId = @NewsId

		SET @Result = @NewsId

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
