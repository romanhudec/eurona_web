ALTER PROCEDURE pNavigationMenuItemModify
	@HistoryAccount INT,
	@NavigationMenuItemId INT,
	@Locale [char](2) = 'en', 
	@Order INT = NULL, 
	@Name NVARCHAR(100),
	@Icon NVARCHAR(255) = NULL,
	@PageId INT,
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tNavigationMenuItem WHERE NavigationMenuItemId = @NavigationMenuItemId AND HistoryId IS NULL) 
		RAISERROR('Invalid NavigationMenuItemId %d', 16, 1, @NavigationMenuItemId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tNavigationMenuItem ( NavigationMenuId, Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			NavigationMenuId, Locale, [Order], [Name], Icon, PageId, RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @NavigationMenuItemId
		FROM tNavigationMenuItem
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		UPDATE tNavigationMenuItem
		SET
			Locale = @Locale, [Order] = @Order, [Name] = @Name, Icon = @Icon, PageId = @PageId, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE NavigationMenuItemId = @NavigationMenuItemId

		SET @Result = @NavigationMenuItemId

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
