ALTER PROCEDURE pImageGalleryCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@EnableComments BIT = 1,
	@EnableVotes BIT = 1,
	@Name NVARCHAR(255),
	@Description NVARCHAR(255) = NULL,
	@Date DATETIME = NULL,
	@RoleId INT = NULL,
	@UrlAliasId INT = NULL,
	@Visible BIT = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tImageGallery ( InstanceId, [Name], [Description], RoleId, Visible, UrlAliasId, [Date], EnableComments, EnableVotes, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @Name, @Description, @RoleId, @Visible, @UrlAliasId, ISNULL(@Date,GETDATE()), @EnableComments, @EnableVotes, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ImageGalleryId = @Result

END
GO
