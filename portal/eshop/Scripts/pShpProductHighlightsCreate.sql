ALTER PROCEDURE pShpProductHighlightsCreate
	@InstanceId INT,
	@ProductId INT,
	@HighlightId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tShpProductHighlights ( InstanceId, ProductId, HighlightId ) 
	VALUES ( @InstanceId, @ProductId, @HighlightId )

	SET @Result = SCOPE_IDENTITY()

	SELECT ProductHighlightsId = @Result

END
GO
