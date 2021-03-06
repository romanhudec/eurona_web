ALTER PROCEDURE pAccountExtCreate
	@InstanceId INT,
	@AccountId INT,
	@AdvisorId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT * FROM tAccountExt WHERE AccountId = @AccountId AND InstanceId = @InstanceId ) BEGIN
		RETURN
	END
	
	INSERT INTO tAccountExt ( InstanceId, AccountId, AdvisorId )
	VALUES (@InstanceId, @AccountId, @AdvisorId )
	
	SET @Result = @AccountId
	SELECT AccountId = @AccountId

END
GO
