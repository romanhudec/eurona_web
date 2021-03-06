ALTER PROCEDURE pAccountProfileCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT,
	@ProfileId INT,
	@Value NVARCHAR(MAX) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountProfile ( InstanceId, AccountId, ProfileId, Value, 
		HistoryStamp, HistoryType, HistoryAccount ) 
	VALUES ( @InstanceId, @AccountId, @ProfileId, @Value, 
		GETDATE(), 'C', @HistoryAccount)

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountProfileId = @Result

END
GO
