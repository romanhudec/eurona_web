ALTER PROCEDURE pShpProductCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@Manufacturer NVARCHAR(500),
	@Code NVARCHAR(500) = NULL,
	@Name NVARCHAR(500),
	@Description NVARCHAR(1000) = NULL,
	@DescriptionLong NVARCHAR(MAX) = NULL,
	@Availability NVARCHAR(500)  = NULL, /*dostupnost ('na objednanie', '24Ks', ...)*/
	@StorageCount INT = NULL, /*Pocet KS na sklade*/
	@Price DECIMAL(19,2), /*Cena BEZ DPH*/	
	@VATId INT = NULL, /*DPH%*/	
	@Discount DECIMAL(19,2) = 0, /*Zlava %*/	
	@Locale CHAR(2) = 'en',
	@UrlAliasId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProduct ( InstanceId, Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, Locale, UrlAliasId,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Code, @Name, @Manufacturer, @Description, @DescriptionLong, @Availability, @StorageCount, @Price, @VATId, @Discount, @Locale, @UrlAliasId, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductId = @Result

END
GO
