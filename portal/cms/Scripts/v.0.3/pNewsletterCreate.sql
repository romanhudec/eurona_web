ALTER PROCEDURE pNewsletterCreate
	@Locale [char](2) = 'en', 
	@Date DATETIME = NULL,
	@Icon NVARCHAR(255) = NULL,
	@Subject NVARCHAR(255) = NULL,
	@Content NVARCHAR(MAX) = NULL,
	@Attachment IMAGE = NULL,
	@SendDate DATETIME = NULL,
	@Roles NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tNewsletter ( Locale, [Date], Icon, Subject, Attachment, Content, Roles, SendDate )
	VALUES ( @Locale, @Date, @Icon, @Subject, @Attachment, @Content, @Roles, @SendDate )

	SET @Result = SCOPE_IDENTITY()

	SELECT NewsletterId = @Result

END
GO
