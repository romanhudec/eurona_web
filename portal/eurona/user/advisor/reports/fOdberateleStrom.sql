CREATE FUNCTION [dbo].[fOdberateleStrom](@Id_Odberatele as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		Id_Odberatele int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS
BEGIN

	Declare @Tier as int
	SET @Tier = 2

	INSERT INTO @table (Id_Odberatele,Level,LineageId) 
	VALUES(@Id_Odberatele, 1, '(1)')

	INSERT INTO @table
	Select Id_Odberatele, 2, '(1)' from odberatele where Stav_odberatele !='Z' AND Cislo_nadrizeneho = @Id_Odberatele

	UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'

	WHILE @@rowcount > 0 BEGIN
		SET @Tier = @Tier + 1
		/*Go get children nodes for the next tier that are not already accounted for */

		INSERT INTO @table (Id_Odberatele,Level,LineageId)
		SELECT Id_Odberatele, @Tier, (select LineageId from @table where Id_Odberatele = Cislo_nadrizeneho) 
		FROM odberatele 
		WHERE Cislo_nadrizeneho IN (select Id_Odberatele from @table) 
		AND Id_Odberatele NOT in (select Id_Odberatele from @table)

		UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'
	END
	
	RETURN;
END
GO


