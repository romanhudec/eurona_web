ALTER PROCEDURE pShpProductModify
	@HistoryAccount INT,
	@ProductId INT,
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

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tShpProduct ( Code, [Name], Manufacturer, [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, Locale, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			Code, [Name], [Manufacturer], [Description], DescriptionLong, Availability, StorageCount, Price, VATId, Discount, Locale, UrlAliasId,
			HistoryStamp, HistoryType, HistoryAccount, @ProductId
		FROM tShpProduct
		WHERE ProductId = @ProductId

		UPDATE tShpProduct
		SET
			Code = @Code, [Name] = @Name, Manufacturer = @Manufacturer, [Description]=@Description, DescriptionLong=@DescriptionLong, Availability = @Availability, StorageCount=@StorageCount, Price = @Price, VATId = @VATId, Discount = @Discount, Locale = @Locale, UrlAliasId=@UrlAliasId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductId = @ProductId

		SET @Result = @ProductId

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
