ALTER PROCEDURE pBlogModify
	@HistoryAccount INT,
	@BlogId INT,
	@AccountId INT,
	@UrlAliasId INT = NULL,
	@Locale CHAR(2) = 'en',
	@Icon NVARCHAR(255) = NULL,
	@Title NVARCHAR(500) = NULL,
	@Teaser NVARCHAR(1000) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@ContentKeywords NVARCHAR(MAX) = NULL,
	@RoleId INT = NULL, /*Role pre ktore sa clanok bude zobrazovat*/
	@Country NVARCHAR(255 ) = NULL, /*Stat, ktoreho sa clanok tyka*/
	@City NVARCHAR(255 ) = NULL /*Mesto, ktoreho sa clanok tyka*/,
	@Approved BIT = 0, /*Indikuje, ci je clanok schvaleny redaktorom*/
	@ReleaseDate DATETIME, /*Datum a cas zverejnenia clanku*/
	@ExpiredDate DATETIME = NULL, /*Datum a cas stiahnutia clanku (uz nebude verejne dostupny)*/
	@EnableComments BIT = 1,
	@Visible BIT = 1, /*Priznak ci ma byt dany clanok viditelny*/
	/*@ViewCount INT = 0,-- Pocet zobrazeni clanku
	@Votes INT = 0, -- Pocet hlasov, ktore clanok obdrzal
	@TotalRating INT = NULL, -- Sucet vsetkych bodov, kore clanok dostal pri hlasovani*/
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tBlog WHERE BlogId = @BlogId AND HistoryId IS NULL) 
		RAISERROR('Invalid BlogId %d', 16, 1, @BlogId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tBlog ( AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			AccountId, Locale, Icon, Title, Teaser, Content, ContentKeywords, RoleId, UrlAliasId, 
			Country, City, Approved, ReleaseDate, ExpiredDate, EnableComments, Visible,
			HistoryStamp, HistoryType, HistoryAccount, @BlogId
		FROM tBlog
		WHERE BlogId = @BlogId

		UPDATE tBlog
		SET
			AccountId=ISNULL(@AccountId, AccountId), [Locale] = @Locale, Icon=@Icon, Title=@Title, Teaser=@Teaser, Content=@Content, ContentKeywords=@ContentKeywords, RoleId=@RoleId, UrlAliasId=@UrlAliasId,
			Country=@Country, City=@City, Approved=@Approved, ReleaseDate=ISNULL(@ReleaseDate, ReleaseDate), ExpiredDate=@ExpiredDate, EnableComments=@EnableComments, Visible=@Visible,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE BlogId = @BlogId

		SET @Result = @BlogId

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
