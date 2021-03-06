ALTER PROCEDURE pShpUzavierkaModify
	@UzavierkaId INT,
	@Povolena BIT = 0, 
	@UzavierkaOd DATETIME = NULL,
	@UzavierkaDo DATETIME = NULL,
	@OperatorOrderOd DATETIME = NULL,
	@OperatorOrderDo DATETIME = NULL, 
	@OperatorOrderDate DATETIME = NULL, 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpUzavierka WITH (NOLOCK) WHERE UzavierkaId = @UzavierkaId ) 
		RAISERROR('Invalid UzavierkaId %d', 16, 1, @UzavierkaId);


	UPDATE tShpUzavierka WITH (ROWLOCK)
	SET
		Povolena=@Povolena, UzavierkaOd=@UzavierkaOd, UzavierkaDo=@UzavierkaDo, OperatorOrderOd=@OperatorOrderOd, OperatorOrderDo=@OperatorOrderDo, OperatorOrderDate=@OperatorOrderDate
	WHERE UzavierkaId = @UzavierkaId

	SET @Result = @UzavierkaId
END
GO
