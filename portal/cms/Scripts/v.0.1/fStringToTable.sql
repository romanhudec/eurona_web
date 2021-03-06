ALTER FUNCTION fStringToTable(@InputString NVARCHAR(4000), @separator NVARCHAR(10))
	RETURNS @table
		TABLE ([index] INT, item NVARCHAR(200))
WITH ENCRYPTION
AS 
BEGIN
	DECLARE @index INT
	SET @index = 0
	DECLARE @item nvarchar(200)
	DECLARE @str nvarchar(4000)
	SET @str = @InputString + @separator

	DECLARE @position int
	SET @position = CHARINDEX(@separator,@str,0)
	WHILE (@position > 0)
	BEGIN
		SET @item = NULL

		DECLARE @PartialStr varchar(8000)
		SET @PartialStr = LEFT(@str,@position - 1)
		SET @item = @PartialStr
		SET @str = RIGHT(@str,LEN(@str) - @position)
		SET @position = CHARINDEX(@separator,@str,0)

		INSERT @table([index], item) VALUES (@index, @item)
		SET @index = @index + 1
	END

	RETURN
END
GO

/*

SELECT * FROM fStringToTable('a;b;c;d', ';')

*/
