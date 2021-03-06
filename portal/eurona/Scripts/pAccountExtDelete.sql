ALTER PROCEDURE pAccountExtDelete
	@InstanceId INT,
	@AccountId INT,
	@AdvisorId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId=@InstanceId) BEGIN
		RAISERROR( 'Invalid AccountId %d', 16, 1, @AccountId );
		RETURN
	END
	
	DELETE FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId=@InstanceId
	SET @Result = @AccountId

END
GO
