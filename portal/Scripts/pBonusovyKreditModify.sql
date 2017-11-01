ALTER PROCEDURE [dbo].[pBonusovyKreditModify]
	@InstanceId INT,
	@BonusovyKreditId INT,
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
		
	IF NOT EXISTS(SELECT * FROM tBonusovyKredit WHERE BonusovyKreditId = @BonusovyKreditId  AND InstanceId=@InstanceId) BEGIN
		RAISERROR( 'Invalid BonusovyKreditId %d', 16, 1, @BonusovyKreditId );
		RETURN
	END
	
	UPDATE tBonusovyKredit SET Typ=@Typ, HodnotaOd=@HodnotaOd, Kredit=@Kredit, Aktivni=@Aktivni,
	HodnotaDo=@HodnotaDo, HodnotaOdSK=@HodnotaOdSK, HodnotaDoSK=@HodnotaDoSK, HodnotaOdPL=@HodnotaOdPL, HodnotaDoPL=@HodnotaDoPL
	WHERE BonusovyKreditId = @BonusovyKreditId AND InstanceId=@InstanceId

	SET @Result = @BonusovyKreditId

END
GO