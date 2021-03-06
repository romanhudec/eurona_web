ALTER PROCEDURE pNewsletterModify
	@NewsletterId INT,
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

	IF NOT EXISTS(SELECT * FROM tNewsletter WHERE NewsletterId = @NewsletterId ) 
		RAISERROR('Invalid NewsletterId %d', 16, 1, @NewsletterId);

	BEGIN TRANSACTION;

	BEGIN TRY
		UPDATE tNewsletter
		SET
			[Date] = @Date, Icon = @Icon, Subject = @Subject, Content = @Content, Attachment = @Attachment, Roles = @Roles, SendDate = @SendDate
		WHERE NewsletterId = @NewsletterId

		SET @Result = @NewsletterId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

END
GO
