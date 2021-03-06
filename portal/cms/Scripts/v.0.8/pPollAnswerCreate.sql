ALTER PROCEDURE pPollAnswerCreate
	@PollOptionId INT,
	@InstanceId INT,
	@IP NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollAnswer ( InstanceId, PollOptionId, IP ) 
	VALUES ( @InstanceId, @PollOptionId, @IP )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollAnswerId = @Result

END
GO
