ALTER PROCEDURE pTranslationModify
	@HistoryAccount INT,
	@TranslationId INT,
	@Translation NVARCHAR(4000) = NULL, 
	@Notes NVARCHAR(4000), 
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tTranslation WHERE TranslationId = @TranslationId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid @Translation %d', 16, 1, @Translation);
		RETURN;
	END

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tTranslation (InstanceId, VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, VocabularyId, Term, Translation, Notes,
			HistoryStamp, HistoryType, HistoryAccount, @TranslationId
		FROM tTranslation
		WHERE Translation = @Translation

		UPDATE tTranslation
		SET
			Translation = @Translation, Notes = @Notes,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE TranslationId = @TranslationId

		SET @Result = @TranslationId

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
