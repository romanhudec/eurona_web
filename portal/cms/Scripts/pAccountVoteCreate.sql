ALTER PROCEDURE pAccountVoteCreate
	@InstanceId INT,
	@AccountId INT,
	@ObjectType INT,
	@ObjectId INT,
	@Rating INT,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO tAccountVote ( InstanceId, AccountId, ObjectType, ObjectId, Rating, [Date]) 
	VALUES ( @InstanceId, @AccountId, @ObjectType, @ObjectId, @Rating, GETDATE())

	SET @Result = SCOPE_IDENTITY()

	SELECT AccountVoteId = @Result

END
GO
