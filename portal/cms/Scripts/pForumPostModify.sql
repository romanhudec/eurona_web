ALTER PROCEDURE pForumPostModify
	@HistoryAccount INT,
	@ForumPostId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tForumPost WHERE ForumPostId = @ForumPostId AND HistoryId IS NULL) 
		RAISERROR('Invalid ForumPostId %d', 16, 1, @ForumPostId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId )
		SELECT
			ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
			HistoryStamp, HistoryType, HistoryAccount, @ForumPostId
		FROM tForumPost
		WHERE ForumPostId = @ForumPostId

		UPDATE tForumPost
		SET
			ParentId=@ParentId, AccountId=@AccountId, IPAddress=@IPAddress, Title=@Title, Content=@Content,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ForumPostId = @ForumPostId

		SET @Result = @ForumPostId

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
