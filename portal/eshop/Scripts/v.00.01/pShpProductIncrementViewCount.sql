ALTER PROCEDURE pShpProductIncrementViewCount
	@ProductId INT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	UPDATE tShpProduct SET ViewCount = ISNULL(ViewCount, 0) + 1 WHERE ProductId = @ProductId

END
GO
