ALTER PROCEDURE pAccountProfileCreate
	@HistoryAccount INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @AccountId, @ProfileId, @Value, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountProfileId = @Result

END
GO
