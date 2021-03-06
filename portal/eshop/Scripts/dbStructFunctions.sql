------------------------------------------------------------------------------------------------------------------------
-- Functions declarations
------------------------------------------------------------------------------------------------------------------------
-- Vráti všetky aj zdedené atribúty pre danú kategóriu
CREATE FUNCTION fAllInheritCategoryAttributes(@CategoryId INT)
	RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId INT NOT NULL,
		ParentId INT NULL,
		AttributeId INT NULL
)
AS 
BEGIN
	RETURN
END
GO

-- Vráti kategórie a podkategórie pre danú kategoriu
CREATE FUNCTION fAllChildCategories(@CategoryId INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS 
BEGIN
	RETURN
END
GO
------------------------------------------------------------------------------------------------------------------------
-- EOF Functions declarations
------------------------------------------------------------------------------------------------------------------------
