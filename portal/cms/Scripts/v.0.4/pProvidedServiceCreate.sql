ALTER PROCEDURE pProvidedServiceCreate
	@HistoryAccount INT,
	@AccountId INT,
	@PaidServiceId INT,
	@ObjectId INT = NULL,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
	INSERT INTO tProvidedService ( AccountId, PaidServiceId, ObjectId, ServiceDate ) 
	VALUES ( @AccountId, @PaidServiceId, @ObjectId, GETDATE() )
	SET @Result = SCOPE_IDENTITY()
	
	DECLARE @CreditCost DECIMAL(19,2)
	SELECT @CreditCost = CreditCost FROM vPaidServices WHERE PaidServiceId = @PaidServiceId
	
	--Update aktualny kredit pouzivatela
	IF @CreditCost IS NOT NULL
	BEGIN
		DECLARE @AccountCreditId INT, @CurrentCredit DECIMAL(19,2), @NewCredit DECIMAL(19,2)
		SET @NewCredit = @CreditCost*(-1)
		-- A neexistuje zaznam o kredite pouzivatela, vytvorim ho a odpocitam jeho credit od aktualneho kreditu
		SELECT @AccountCreditId=AccountCreditId, @CurrentCredit=Credit FROM vAccountsCredit WHERE AccountId=@AccountId 	
		IF @AccountCreditId IS NULL
		BEGIN
			EXEC pAccountCreditCreate @HistoryAccount = @HistoryAccount, @AccountId = @AccountId, @Credit=@NewCredit
		END	
		ELSE
		BEGIN
			SET @NewCredit = ( @CurrentCredit - @CreditCost)
			EXEC pAccountCreditModify @HistoryAccount = @HistoryAccount, @AccountCreditId = @AccountCreditId, @Credit=@NewCredit
		END

	END	
	
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

		SET @Result = NULL

	END CATCH	

	SELECT ProvidedServiceId = @Result

END
GO
