ALTER PROCEDURE pPersonModify
	@HistoryAccount INT,
	@PersonId INT,
	@Title NVARCHAR(20),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@Email NVARCHAR(100),
	@Phone NVARCHAR(100), @Mobile NVARCHAR(100),
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tPerson WHERE PersonId = @PersonId AND HistoryId IS NULL) 
		RAISERROR('Invalid PersonId %d', 16, 1, @PersonId);

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tPerson ( AccountId, Title, FirstName, LastName, Email,
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			AccountId, Title, FirstName, LastName, Email, 
			Phone, Mobile, AddressHomeId, AddressTempId,
			HistoryStamp, HistoryType, HistoryAccount, @PersonId
		FROM tPerson
		WHERE PersonId = @PersonId

		UPDATE tPerson 
		SET
			Title = @Title, FirstName = @FirstName, LastName = @LastName, Email = @Email,
			Phone = @Phone, Mobile = @Mobile,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE PersonId = @PersonId

		SET @Result = @PersonId

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		SELECT	ERROR_NUMBER() AS ErrorNumber,
				ERROR_SEVERITY() AS ErrorSeverity,
				ERROR_STATE() as ErrorState,
				ERROR_PROCEDURE() as ErrorProcedure,
				ERROR_LINE() as ErrorLine,
				ERROR_MESSAGE() as ErrorMessage;

	END CATCH	

END
GO

/*
DECLARE @Result INT
--EXEC pPersonModify @HistoryAccount = 7, @PersonId = 19, @FirstName='Jozef', @LastName='Prídavok'
EXEC pPersonModify @HistoryAccount = 7, @PersonId = 24, @FirstName='Roman', @LastName='Hudec'
SELECT @Result
SELECT * FROM tPerson

select * from tPerson where historyid=19 order by historystamp
*/