ALTER PROCEDURE pOrganizationCreate
	@HistoryAccount INT,
	@InstanceId INT,
	@AccountId INT = NULL,
	@Id1 NVARCHAR(100) = NULL, @Id2 NVARCHAR(100) = NULL, @Id3 NVARCHAR(100) = NULL,
	@Name NVARCHAR(100),
	@Notes NVARCHAR(2000) = NULL,
	@Web NVARCHAR(100) = NULL,
	@ContactEmail NVARCHAR(100) = NULL, @ContactPhone NVARCHAR(100) = NULL, @ContactMobile NVARCHAR(100) = NULL,
	@ParentId INT = NULL,
	@Code NVARCHAR(100) = NULL,
	@VATPayment BIT = 0,
	@TopManager INT = 0,
	@FAX NVARCHAR(100) = NULL, 
	@Skype NVARCHAR(100) = NULL, 
	@ICQ NVARCHAR(100) = NULL, 
	@ContactBirthDay DATETIME = NULL, 
	@ContactCardId NVARCHAR(100) = NULL, 
	@ContactWorkPhone NVARCHAR(100) = NULL, 
	@PF CHAR(1) = NULL, 
	@RegionCode NVARCHAR(100) = NULL,
	@UserMargin DECIMAL(19,2) = NULL,
	@Statut NVARCHAR(10) = NULL,
	@SelectedCount INT = 0,
	@AnonymousRegistration BIT = 0,
	@AnonymousAssignBy INT = NULL,
	@AnonymousAssignAt DATETIME = NULL,
	@AnonymousAssignToCode NVARCHAR(100) = NULL,
	@AnonymousCreatedAt DATETIME = NULL,
	@AnonymousAssignStatus NVARCHAR(1000) = NULL,
	@AnonymousAssignByCode NVARCHAR(100) = NULL,
	@ManageAnonymousAssign BIT = 0,
	@PredmetCinnosti NVARCHAR(500) =  NULL,
	@AnonymousOvereniSluzeb BIT = 0,
	@AnonymousZmenaNaJineRegistracniCislo BIT = 0,
	@AnonymousZmenaNaJineRegistracniCisloText NVARCHAR(100) = NULL,
	@AnonymousSouhlasStavajicihoPoradce BIT  = 0,
	@AnonymousSouhlasNavrzenehoPoradce BIT = 0,
	@ZasilaniTiskovin BIT = 0, 
	@ZasilaniNewsletter BIT = 0,
	@ZasilaniKatalogu BIT = 0,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
	
		DECLARE @RegisteredAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @RegisteredAddressId OUTPUT

		DECLARE @CorrespondenceAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @CorrespondenceAddressId OUTPUT
		
		DECLARE @InvoicingAddressId INT
		EXEC pAddressCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @InvoicingAddressId OUTPUT

		DECLARE @BankContactId INT
		EXEC pBankContactCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @BankContactId OUTPUT

		DECLARE @ContactPersonId INT
		EXEC pPersonCreate @HistoryAccount = @HistoryAccount, @InstanceId=@InstanceId, @Result = @ContactPersonId OUTPUT

		INSERT INTO tOrganization (
			InstanceId, AccountId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
			PredmetCinnosti, AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			@ParentId, @Code, @VATPayment, @TopManager,
			@FAX, @Skype, @ICQ, @ContactBirthDay, @ContactCardId, @ContactWorkPhone, @PF, @RegionCode, @UserMargin, @Statut, @SelectedCount,
			@AnonymousRegistration, @AnonymousAssignBy, @AnonymousAssignAt, @AnonymousAssignToCode, @AnonymousCreatedAt, @AnonymousAssignStatus, @AnonymousAssignByCode, @ManageAnonymousAssign,
			@PredmetCinnosti, @AnonymousOvereniSluzeb, @AnonymousZmenaNaJineRegistracniCislo, @AnonymousZmenaNaJineRegistracniCisloText, @AnonymousSouhlasStavajicihoPoradce, @AnonymousSouhlasNavrzenehoPoradce,
			@ZasilaniTiskovin, @ZasilaniNewsletter, @ZasilaniKatalogu,
			GETDATE(), 'C', @HistoryAccount)
			
		SET @Result = SCOPE_IDENTITY()

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure NVARCHAR(200);
		DECLARE @ErrorLine INT;
		DECLARE @ErrorNumber INT;

		SELECT 	@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorNumber = ERROR_LINE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH	

	SELECT OrganizationId = @Result

END
GO