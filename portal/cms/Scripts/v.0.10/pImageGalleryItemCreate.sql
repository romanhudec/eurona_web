ALTER PROCEDURE pImageGalleryItemCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@ImageGalleryId INT,
	@VirtualPath NVARCHAR(255),
	@VirtualThumbnailPath NVARCHAR(255),
	@Position INT, 
	@Description NVARCHAR(1000),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGalleryItem ( InstanceId, ImageGalleryId, VirtualPath, VirtualThumbnailPath, [Position], [Date], Description, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @ImageGalleryId, @VirtualPath, @VirtualThumbnailPath, @Position, GETDATE(), @Description,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryItemId = @Result

END
GO
