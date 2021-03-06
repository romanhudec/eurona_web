ALTER PROCEDURE pImageGalleryTagCreate
	@HistoryAccount INT,
	@ImageGalleryId INT, 
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TagId INT
	SELECT @TagId = TagId FROM vTags WHERE Tag = @Tag
	
	IF @TagId IS NULL 
	BEGIN
		EXEC pTagCreate @HistoryAccount = @HistoryAccount, @Tag=@Tag, @Result = @TagId OUTPUT
	END
	
	IF NOT EXISTS(SELECT TagId, ImageGalleryId FROM vImageGalleryTags WHERE TagId=@TagId AND ImageGalleryId=@ImageGalleryId) BEGIN
		INSERT INTO tImageGalleryTag ( TagId, ImageGalleryId ) VALUES ( @TagId, @ImageGalleryId )
	END

END
GO
