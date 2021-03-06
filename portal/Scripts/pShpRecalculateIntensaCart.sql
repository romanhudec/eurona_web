ALTER PROCEDURE pShpRecalculateIntensaCart
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
	DECLARE @EuronaInstance INT,@IntensaInstance INT
	SET @EuronaInstance = 1
	SET @IntensaInstance = 2

	IF NOT EXISTS(SELECT * FROM tShpCart WHERE CartId = @CartId) 
		RAISERROR('Invalid CartId %d', 16, 1, @CartId);
	
	-- default sa vrati to co prislo
	SET @out_MarzeEurona = @MarzeEurona
	SET @out_MarzeIntensa = @MarzeIntensa
	SET @out_ChibiBodu = @ChibiBodu
	SET @out_ChibiRegistraci = @ChibiRegistraci

	---- Ak mu nechybaju nziadne body, nic sa nerobi a aplikuje sa marza z www_esp_marze
	--IF @ChibiBodu <= 0 
	--BEGIN
	--	UPDATE tShpCart SET Discount=@MarzeIntensa WHERE CartId=@CartId
	--	RETURN
	--END

	---------------------------------------------------------------------------------------------------
	-- Vupocet aktualneho poctu bodov v Eurona Kosiku
	---------------------------------------------------------------------------------------------------
	DECLARE @EuronaCurrentCartProductBody INT
	SELECT @EuronaCurrentCartProductBody = SUM(ISNULL(p.Body,0) * cp.Quantity)
	FROM tShpCartProduct cp 
	INNER JOIN vShpProducts p ON p.ProductId = cp.ProductId
	INNER JOIN vShpCurrencies cur ON cur.CurrencyId = cp.CurrencyId AND p.Locale = cur.Locale--pre locale daneho prodktu
	WHERE cp.CartId=@CartId AND cp.InstanceId=@EuronaInstance

	-- Vypocet kolko bodov mu este chyba do marze ako v EURONA.
	SET @out_ChibiBodu = @ChibiBodu - ISNULL(@EuronaCurrentCartProductBody, 0 )

	--------------------------------------------------------
	-- Ak NEsplnil podmeinky, uplatnuje sa mnozstevna zlava
	--------------------------------------------------------
	IF @out_ChibiBodu > 0 
	BEGIN
		DECLARE @MnozstevniSlevaPercent DECIMAl(19,2), @IntensaCartKatalogoveCenySum DECIMAl(19,2), @CurrencyId INT

		--Zistim currency produktov v kosiku
		SELECT TOP 1 @CurrencyId=cp.CurrencyId FROM tShpCartProduct cp WHERE cp.CartId=@CartId AND cp.InstanceId=@IntensaInstance

		SELECT @IntensaCartKatalogoveCenySum = SUM(p.Price*cp.Quantity)
		FROM tShpCartProduct cp 
		INNER JOIN vShpProducts p ON p.ProductId = cp.ProductId
		INNER JOIN vShpCurrencies cur ON cur.CurrencyId = cp.CurrencyId AND p.Locale = cur.Locale--pre locale daneho prodktu
		WHERE cp.CartId=@CartId AND cp.InstanceId=@IntensaInstance

		SELECT @MnozstevniSlevaPercent = DiscountPercent FROM vShpDiscounts
		WHERE @IntensaCartKatalogoveCenySum >= PriceFrom AND ( PriceTo IS NULL OR @IntensaCartKatalogoveCenySum <= PriceTo )
		AND CurrencyId=@CurrencyId
			
		--Normalizacia mnozstevni slevy
		SET @MnozstevniSlevaPercent = ISNULL( @MnozstevniSlevaPercent, 0 )
		UPDATE tShpCart SET Discount=@MnozstevniSlevaPercent WHERE CartId=@CartId
			
		-- Ak sa maju prepocitat ceny produktov kosiku
		IF @RecalculateCartProducts = 1
		BEGIN
			--Math.Round( ( product.Price * 100m ) / ( 100m + product.VAT ), 2 );
			UPDATE tShpCartProduct 
				SET PriceWVAT = p.Price,  
					Price = ROUND(p.Price*100/(100+p.VAT), 2),
					PriceTotalWVAT =  p.Price * cp.Quantity,
					PriceTotal =  ROUND(p.Price*100/(100+p.VAT), 2) * cp.Quantity
			FROM tShpCartProduct cp 
			INNER JOIN vShpProducts p ON cp.ProductId=p.ProductId AND cp.CurrencyId=p.CurrencyId
			WHERE cp.CartId=@CartId AND cp.InstanceId=@IntensaInstance
		END
		RETURN
	END
	ELSE 
	--------------------------------------------------------
	-- Ak SPLNIL podmeinky, vypocitaju sa ceny produktov podla jeho marze
	--------------------------------------------------------
	BEGIN
		--ak splnil podmienky, tak marze intansa je rovna marze eurona
		SET @out_MarzeIntensa = @MarzeEurona
		
		IF NOT EXISTS(SELECT * FROM tShpCartProduct WHERE CartId = @CartId AND InstanceId=@IntensaInstance) 
		BEGIN
			-- Ak NIE su v kosiku ziadne INTENSA produkty			
			UPDATE tShpCart SET Discount=0 WHERE CartId=@CartId
		END
		ELSE
		BEGIN
			-- Ak su zaktualizujem zlavu na intensa produkty.
			UPDATE tShpCart SET Discount=@MarzeEurona WHERE CartId=@CartId
		END
		-- vypocet cien produktov podla marze
		IF @RecalculateCartProducts = 1
		BEGIN
			UPDATE tShpCartProduct 
				SET 
					PriceWVAT = CASE p.MarzePovolena 
						WHEN 0 THEN p.Price --Ak nieje povolene uplatnenie marze
						WHEN 1 THEN dbo.fGetPriceWithMargin(p.Price, @MarzeEurona, p.VAT, 1)
					END,  
					
					Price = CASE p.MarzePovolena 
						WHEN 0 THEN ROUND(p.Price*100/(100+p.VAT), 2) --Ak nieje povolene uplatnenie marze
						WHEN 1 THEN dbo.fGetPriceWithMargin(p.Price, @MarzeEurona, p.VAT, 0)
					END,

					PriceTotalWVAT = CASE p.MarzePovolena 
						WHEN 0 THEN p.Price * cp.Quantity --Ak nieje povolene uplatnenie marze
						WHEN 1 THEN dbo.fGetPriceWithMargin(p.Price, @MarzeEurona, p.VAT, 1) * cp.Quantity
					END,

					PriceTotal  = CASE p.MarzePovolena 
						WHEN 0 THEN ROUND(p.Price*100/(100+p.VAT), 2) * cp.Quantity --Ak nieje povolene uplatnenie marze
						WHEN 1 THEN dbo.fGetPriceWithMargin(p.Price, @MarzeEurona, p.VAT, 0) * cp.Quantity
					END
			FROM tShpCartProduct cp 
			INNER JOIN vShpProducts p ON cp.ProductId=p.ProductId AND cp.CurrencyId=p.CurrencyId
			WHERE cp.CartId=@CartId AND cp.InstanceId=@IntensaInstance
		END
		RETURN
	END

END
GO
