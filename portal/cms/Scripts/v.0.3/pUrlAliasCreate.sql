ALTER PROCEDURE pUrlAliasCreate
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist!' , 16, 1, @Url, @Locale);
		RETURN
	END	

	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias (Url, Locale, Alias, [Name] ) 
	VALUES ( @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
