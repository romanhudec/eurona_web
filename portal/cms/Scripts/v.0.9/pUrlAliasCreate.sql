ALTER PROCEDURE pUrlAliasCreate
	@InstanceId INT,
	@Url NVARCHAR(2000) = NULL,
	@Locale [char](2) = 'en', 
	@Alias NVARCHAR(2000),
	@Name NVARCHAR(500),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT * FROM tUrlAlias WHERE Url = @Url AND Locale = @Locale AND InstanceId = @InstanceId)  BEGIN
		RAISERROR('UrlAlias with @Url=%s and @Locale=%s exist! and @InstanceId=%d' , 16, 1, @Url, @Locale, @InstanceId);
		RETURN
	END	

	SET @Alias = REPLACE( LOWER(@Alias), ' ', '-')
	SET @Alias = REPLACE( @Alias, '.', '-')
	SET @Alias = REPLACE( @Alias, '_', '-')
	SET @Alias = REPLACE( @Alias, ':', '-')

	INSERT INTO tUrlAlias ( InstanceId, Url, Locale, Alias, [Name] ) 
	VALUES ( @InstanceId, @Url, @Locale, dbo.fMakeAnsi( @Alias ), @Name)	

	SET @Result = SCOPE_IDENTITY()

	SELECT UrlAliasId = @Result

END
GO
