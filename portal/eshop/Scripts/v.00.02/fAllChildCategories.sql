ALTER FUNCTION fAllChildCategories(@CategoryId as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		CategoryId int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS
BEGIN

	Declare @Tier as int
	SET @Tier = 2

	INSERT INTO @table (CategoryId,Level,LineageId) 
	VALUES(@CategoryId, 1, '(1)')

	INSERT INTO @table
	Select CategoryId, 2, '(1)' from vShpCategories where ParentId = @CategoryId

	UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'

	WHILE @@rowcount > 0 BEGIN
		SET @Tier = @Tier + 1
		/*Go get children nodes for the next tier that are not already accounted for */

		INSERT INTO @table (CategoryId,Level,LineageId)
		SELECT CategoryId, @Tier, (select LineageId from @table where CategoryId = ParentId) 
		FROM vShpCategories 
		WHERE ParentId IN (select CategoryId from @table) 
		AND CategoryId NOT in (select CategoryId from @table)

		UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'
	END
	
	RETURN;
END
GO

--SELECT * FROM fAllChildCategories(1)