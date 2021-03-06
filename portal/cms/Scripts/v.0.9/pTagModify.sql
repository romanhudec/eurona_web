ALTER PROCEDURE pTagModify
	@HistoryAccount INT,
	@TagId INT,
	@Tag NVARCHAR(255),
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTag WHERE TagId = @TagId AND HistoryId IS NULL) 
		RAISERROR('Invalid TagId %d', 16, 1, @TagId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTag ( InstanceId, Tag,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			InstanceId, Tag,
			HistoryStamp, HistoryType, HistoryAccount, @TagId
		FROM tTag
		WHERE TagId = @TagId

		UPDATE tTag
		SET
			Tag = ISNULL(@Tag, Tag ),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TagId = @TagId

		SET @Result = @TagId

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
