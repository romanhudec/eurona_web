ALTER PROCEDURE [dbo].[pBonusovyKreditCreate]
	@InstanceId INT,
	@Typ INT = 0,
	@HodnotaOd DECIMAL(19,2) = NULL,
	@HodnotaDo DECIMAL(19,2) = NULL,
	@HodnotaOdSK DECIMAL(19,2) = NULL,
	@HodnotaDoSK DECIMAL(19,2) = NULL,
	@HodnotaOdPL DECIMAL(19,2) = NULL,
	@HodnotaDoPL DECIMAL(19,2) = NULL,
	@Kredit DECIMAL,
	@Aktivni BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO tBonusovyKredit ( InstanceId, Typ, HodnotaOd, HodnotaDo, HodnotaOdSK, HodnotaDoSK, HodnotaOdPL, HodnotaDoPL, Kredit, Aktivni )
	VALUES (@InstanceId, @Typ, @HodnotaOd, @HodnotaDo, @HodnotaOdSK, @HodnotaDoSK, @HodnotaOdPL, @HodnotaDoPL, @Kredit, @Aktivni )
	
	SET @Result = SCOPE_IDENTITY()
	SELECT BonusovyKreditId = @Result

END
GO