CREATE FUNCTION [dbo].[fGetOdberateleStrom](@Id_Odberatele as INT)
RETURNS @table TABLE( Id_Odberatele int null, LineageId nvarchar(2000) null)
AS
BEGIN

	DECLARE @LineageId NVARCHAR(2000)
	SELECT @LineageId = LineageId FROM www_odberatele_strom WHERE Id_Odberatele=@Id_Odberatele

	IF @LineageId IS NULL
	BEGIN
		INSERT INTO @table
			SELECT Id_Odberatele, LineageId FROM dbo.fOdberateleStrom(@Id_Odberatele)
    END
	ELSE
	BEGIN
		INSERT INTO @table
			SELECT Id_Odberatele, LineageId FROM www_odberatele_strom WHERE LineageId LIKE @LineageId + '%'
	END
	
	RETURN;
END
GO



