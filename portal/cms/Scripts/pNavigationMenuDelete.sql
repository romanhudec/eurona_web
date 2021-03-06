ALTER PROCEDURE pNavigationMenuDelete
	@HistoryAccount INT,
	@NavigationMenuId INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF @NavigationMenuId IS NULL OR NOT EXISTS(SELECT * FROM tNavigationMenu WHERE NavigationMenuId = @NavigationMenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid @NavigationMenuId=%d', 16, 1, @NavigationMenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tNavigationMenu
		SET HistoryStamp = GETDATE(), HistoryType = 'D', HistoryAccount = @HistoryAccount, HistoryId = @NavigationMenuId
		WHERE NavigationMenuId = @NavigationMenuId

		SET @Result = @NavigationMenuId

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
