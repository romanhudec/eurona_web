ALTER PROCEDURE pPollAnswerCreate
	@PollOptionId INT,
	@IP NVARCHAR(255),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tPollAnswer ( PollOptionId, IP ) 
	VALUES ( @PollOptionId, @IP )

	SET @Result = SCOPE_IDENTITY()

	SELECT PollAnswerId = @Result

END
GO
