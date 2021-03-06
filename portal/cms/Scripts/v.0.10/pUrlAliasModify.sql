ALTER PROCEDURE pUrlAliasModify
	@UrlAliasId INT,
	@Url NVARCHAR(2000),
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId)  BEGIN
		RAISERROR('Invalid UrlAliasId %d', 16, 1, @UrlAliasId);
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
		SET @Alias = REPLACE( @Alias, '.', '-')
		SET @Alias = REPLACE( @Alias, '_', '-')

		UPDATE tUrlAlias
		SET Url = ISNULL(@Url, Url ), Alias = ISNULL(dbo.fMakeAnsi(@Alias), Alias), [Name] = ISNULL(@Name, [Name] )
		WHERE UrlAliasId = @UrlAliasId

		SET @Result = @UrlAliasId

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
