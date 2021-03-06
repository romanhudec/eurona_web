ALTER PROCEDURE pShpSearchProducts
	@Keywords NVARCHAR(255),
	@Locale CHAR(2),
	@InstanceId INT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id = p.ProductId, Title = p.Name, 
		Content = ISNULL(p.Manufacturer,'') + ISNULL( p.Description, '')/* + ISNULL(p.DescriptionLong, '')*/ , 
		UrlAlias = a.Alias, ImageUrl = NULL
	FROM tShpProduct p INNER JOIN
	tUrlAlias a ON a.UrlAliasId = p.UrlAliasId
	WHERE p.HistoryId IS NULL AND p.Locale = @Locale AND p.InstanceId = @InstanceId AND 
	(
		p.Name LIKE '%'+@Keywords+'%' OR 
		p.Description LIKE '%'+@Keywords+'%' OR
		--p.DescriptionLong LIKE '%'+@Keywords+'%' OR
		p.Manufacturer LIKE '%'+@Keywords+'%' 
	)
	
END
GO