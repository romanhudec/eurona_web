ALTER PROCEDURE pShpCategoryModify
	@HistoryAccount INT,
	@CategoryId INT,
	@ParentId INT = NULL,
	@Name NVARCHAR(500) = NULL,
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpCategory WHERE CategoryId = @CategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid CategoryId %d', 16, 1, @CategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpCategory ( ParentId, [Name], Locale, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ParentId, [Name], Locale, UrlAliasId, 
			HistoryStamp, HistoryType, HistoryAccount, @CategoryId
		FROM tShpCategory
		WHERE CategoryId = @CategoryId

		UPDATE tShpCategory
		SET
			ParentId = @ParentId, Locale = @Locale, [Name] = @Name, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE CategoryId = @CategoryId

		SET @Result = @CategoryId

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
