ALTER PROCEDURE pForumPostCreate
	@HistoryAccount INT,
	@ForumId INT,
	@InstanceId INT,
	@ParentId INT = NULL,
	@AccountId INT,
	@IPAddress NVARCHAR(255) = NULL,
	@Date DATETIME,
	@Title NVARCHAR(255),
	@Content NVARCHAR(MAX) = NULL,
	--@Votes INT = 0, /*Pocet hlasov, ktore post obdrzal*/
	--@TotalRating INT = 0, /*Sucet vsetkych bodov, kore post dostal pri hlasovani*/	
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tForumPost ( ForumId, InstanceId, ParentId, AccountId, IPAddress, [Date], Title, Content, Votes, TotalRating,
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @ForumId, @InstanceId, @ParentId, @AccountId, @IPAddress, @Date, @Title, @Content, 0, 0,
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT ForumPostId = @Result

END
GO
