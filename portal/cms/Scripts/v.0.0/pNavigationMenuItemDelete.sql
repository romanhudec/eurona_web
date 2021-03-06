ALTER PROCEDURE pNavigationMenuItemDelete
	@HistoryAccount INT,
	@NavigationMenuItemId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NavigationMenuItemId IS NULL OR NOT EXISTS(SELECT * FROM tNavigationMenuItem WHERE NavigationMenuItemId = @NavigationMenuItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NavigationMenuItemId=%d', 16, 1, @NavigationMenuItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tNavigationMenuItem
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NavigationMenuItemId
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		SET @Result = @NavigationMenuItemId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END	

GO
