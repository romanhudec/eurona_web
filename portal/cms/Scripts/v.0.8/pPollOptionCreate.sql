ALTER PROCEDURE pPollOptionCreate
	@PollId INT,
	@InstanceId INT,
	@Order INT = NULL,
	@Name NVARCHAR(1000) = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollOption ( InstanceId, PollId, [Order], [Name] )
	VALUES ( @InstanceId, @PollId, @Order, @Name )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollOptionId = @Result

END
GO
