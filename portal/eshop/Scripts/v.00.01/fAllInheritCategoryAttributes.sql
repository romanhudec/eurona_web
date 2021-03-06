ALTER FUNCTION fAllInheritCategoryAttributes(@CategoryId as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId INT NOT NULL,
		ParentId INT NULL,
		AttributeId INT NULL
)
AS
BEGIN

	-- Ziskam prveho parent danej kategorie
	DECLARE @ParentId INT
	SELECT @ParentId = ParentId FROM vShpCategories WHERE CategoryId=@CategoryId	

	-- Vlozim prvy zaznam z informaciami o prvej kategorii (@CategoryId)
	INSERT INTO @table (CategoryId, ParentId, AttributeId ) 
		SELECT @CategoryId, @ParentId, AttributeId FROM vShpAttributes
		WHERE CategoryId=@CategoryId
	
	-- Dokila ma kategoria parent, plni sa tabulka.
	WHILE @ParentId IS NOT NULL BEGIN
		SELECT @ParentId = ParentId FROM vShpCategories WHERE CategoryId=@CategoryId	
				
		INSERT INTO @table (CategoryId, ParentId, AttributeId ) 
			SELECT c.CategoryId, c.ParentId, a.AttributeId FROM vShpAttributes a 
				INNER JOIN vShpCategories c ON c.CategoryId=a.CategoryId
			WHERE a.CategoryId=@ParentId		
			
		SET @CategoryId = @ParentId
	END

	RETURN;
END
GO

--SELECT * FROM dbo.fAllInheritCategoryAttributes(1)