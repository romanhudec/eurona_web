ALTER PROCEDURE pShpRecalculateCernyForLifeCart
	@CartId INT,
	@MarzeEurona DECIMAL(19,2) = NULL,
	@MarzeIntensa DECIMAL(19,2) = NULL,
	@ChibiBodu INT = NULL,
	@ChibiRegistraci INT = NULL,
	@RecalculateCartProducts BIT = 0,
	@out_MarzeEurona DECIMAL(19,2) = 0 OUTPUT,
	@out_MarzeIntensa DECIMAL(19,2) = 0 OUTPUT,
	@out_ChibiBodu INT = 0  OUTPUT,
	@out_ChibiRegistraci INT = 0  OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @EuronaInstance INT,@IntensaInstance INT, @CLInstance INT
	SET @EuronaInstance = 1
	SET @IntensaInstance = 2
	SET @CLInstance = 3

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);
	
	-- default sa vrati to co prislo
	SET @out_MarzeEurona = @MarzeEurona
	SET @out_MarzeIntensa = @MarzeIntensa
	SET @out_ChibiBodu = @ChibiBodu
	SET @out_ChibiRegistraci = @ChibiRegistraci

	-- Ak su zaktualizujem zlavu na intensa produkty.
	UPDATE tShpCart SET Discount=@MarzeEurona WHERE CartId=@CartId

END
GO
