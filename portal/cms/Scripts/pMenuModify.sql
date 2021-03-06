ALTER PROCEDURE pMenuModify
	@HistoryAccount INT,
	@MenuId INT,
	@Locale [char](2) = 'en', 
	@Code NVARCHAR(100),
	@Name NVARCHAR(100),
	@RoleId INT = NULL,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tMenu WHERE MenuId = @MenuId AND HistoryId IS NULL) 
		RAISERROR('Invalid MenuId %d', 16, 1, @MenuId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tMenu ( InstanceId, Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Locale, [Code], [Name], RoleId,
			HistoryStamp, HistoryType, HistoryAccount, @MenuId
		FROM tMenu
		WHERE MenuId = @MenuId

		UPDATE tMenu
		SET
			Locale = @Locale, Code = @Code, [Name] = @Name, RoleId = @RoleId,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE MenuId = @MenuId

		SET @Result = @MenuId

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
