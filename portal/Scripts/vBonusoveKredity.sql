ALTER VIEW [dbo].[vBonusoveKredity]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	BonusovyKreditId, InstanceId, Typ, HodnotaOd, HodnotaDo, HodnotaOdSK, HodnotaDoSK, HodnotaOdPL, HodnotaDoPL, Kredit
FROM
	tBonusovyKredit

GO