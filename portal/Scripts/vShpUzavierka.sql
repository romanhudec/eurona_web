
ALTER VIEW vShpUzavierka
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT UzavierkaId, Povolena, UzavierkaOd, UzavierkaDo, OperatorOrderOd, OperatorOrderDo, OperatorOrderDate
FROM tShpUzavierka
GO
