ALTER FUNCTION fGetPriceWithMargin(@PriceWVAT DECIMAL(19,2), @Margin DECIMAL(19,2), @VAT DECIMAL(19,2), @WithVAT BIT ) 
RETURNS DECIMAL(19,2)
--%%WITH ENCRYPTION%%
AS
BEGIN
	
	IF ( @Margin IS NULL ) RETURN @PriceWVAT;
	IF ( @Margin <= 0 ) return @PriceWVAT;

	DECLARE @PriceWithMargin DECIMAL(19,2) = @PriceWVAT

	--KC s DPH / 1,19; zaokrouhlit na 2 destinná místa / 1,2; 1,25; nebo 1,3 (dle marže A,B,C); zaokrouhlit na 2 destinná místa; x 1,2 (DPH 20%); zaokrouhlit na 2 destinná místa
	--KC s DPH / 1,19; zaokrouhlit na 2 destinná místa / 1,2; 1,25; nebo 1,3 (dle marže A,B,C); zaokrouhlit na 2 destinná místa; x 1,19 (DPH 19%); zaokrouhlit na 2 destinná místa
	DECLARE @dph DECIMAL(19,2) = 1 +ROUND( ( @VAT / 100 ), 2 )
	DECLARE @marze DECIMAL(19,2) = 1 + ROUND( @Margin / 100, 2 )
	DECLARE @constant DECIMAL(19,2) = 1.19
	DECLARE @advisorPrice  DECIMAL(19,2) = ( ROUND( @PriceWVAT / @constant, 2 ) / @marze * @dph)

	IF @WithVAT = 1 
	BEGIN
		SET @PriceWithMargin = ROUND( @advisorPrice, 2 )
	END
	ELSE 
	BEGIN
		
		DECLARE @dphPrice DECIMAL(19,2) = ( ( @advisorPrice * @VAT ) / 100 )
		SET @PriceWithMargin = ROUND( @advisorPrice - @dphPrice, 2 )
	END

	RETURN @PriceWithMargin
END
GO

