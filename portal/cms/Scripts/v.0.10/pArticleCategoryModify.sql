ALTER PROCEDURE pArticleCategoryModify
	@HistoryAccount INT,
	@ArticleCategoryId INT,
	@Name NVARCHAR(100) = '',
	@Code VARCHAR(100) = '',
	@Notes NVARCHAR(2000) = '',
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM cArticleCategory WHERE ArticleCategoryId = @ArticleCategoryId AND HistoryId IS NULL) 
		RAISERROR('Invalid ArticleCategoryId %d', 16, 1, @ArticleCategoryId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO cArticleCategory ( InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT InstanceId, Locale, [Name], [Code], [Notes], HistoryStamp, HistoryType, HistoryAccount, @ArticleCategoryId
		FROM cArticleCategory
		WHERE ArticleCategoryId = @ArticleCategoryId

		UPDATE cArticleCategory
		SET
			[Name] = @Name, [Code] = @Code, [Notes] = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ArticleCategoryId = @ArticleCategoryId

		SET @Result = @ArticleCategoryId

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
