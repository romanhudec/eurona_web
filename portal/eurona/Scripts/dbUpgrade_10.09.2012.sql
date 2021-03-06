------------------------------------------------------------------------------------------------------------------------
-- UPGRADE Eurona
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT, @ProductsFBMasterPageId INT
SET @MasterPageId = 1

DECLARE @UrlAliasId INT
DECLARE @PageId INT
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tOrganization ADD  
PredmetCinnosti NVARCHAR(500) NULL,
AnonymousOvereniSluzeb BIT NOT NULL DEFAULT(0),
AnonymousZmenaNaJineRegistracniCislo BIT NOT NULL DEFAULT(0),
AnonymousZmenaNaJineRegistracniCisloText NVARCHAR(100) NULL,
AnonymousSouhlasStavajicihoPoradce BIT NOT NULL DEFAULT(0),
AnonymousSouhlasNavrzenehoPoradce BIT NOT NULL DEFAULT(0)
GO

------------------------------------------------------------------------------------------------------------------------
ALTER VIEW vOrganizations
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	OrganizationId = o.OrganizationId, o.InstanceId,
	AccountId = o.AccountId, a.TVD_Id, a.Verified,
	Id1 = o.Id1, Id2 = o.Id2, Id3 = o.Id3, [Name], Notes = o.Notes, 
	Web = o.Web, ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
	ContactPersonId = o.ContactPerson, ContactPersonFirstName = cp.FirstName, ContactPersonLastName = cp.LastName,
	RegisteredAddressId = o.RegisteredAddress,
	CorrespondenceAddressId = o.CorrespondenceAddress,
	InvoicingAddressId = o.InvoicingAddress,
	BankContactId = o.BankContact,
	o.ParentId, o.Code, o.VATPayment, o.TopManager,
	o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin,  RestrictedAccess = ISNULL(o.RestrictedAccess, 0), o.Statut,
	Created = ( SELECT MIN(HistoryStamp) FROM tAccount WHERE ( AccountId=a.AccountId OR HistoryId=a.AccountId )  ),
	SelectedCount, PredmetCinnosti,
	AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt,AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
	AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
	Angel_team_clen, Angel_team_manager, Angel_team_manager_typ
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pOrganizationModify
	@HistoryAccount INT,
	@OrganizationId INT,
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
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tOrganization WHERE OrganizationId = @OrganizationId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid OrganizationId %d', 16, 1, @OrganizationId)
		RETURN
	END

	BEGIN TRANSACTION;

	BEGIN TRY
	
		INSERT INTO tOrganization (
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
			PredmetCinnosti, AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
			PredmetCinnosti, AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
			HistoryStamp, HistoryType, HistoryAccount, @OrganizationId
		FROM tOrganization
		WHERE OrganizationId = @OrganizationId

		UPDATE tOrganization 
		SET
			Id1 = @Id1, Id2 = @Id2, Id3 = @Id3, Name = @Name, Notes = @Notes, Web = @Web, 
			ContactEMail = @ContactEMail, ContactPhone = @ContactPhone, ContactMobile = @ContactMobile, 
			ParentId=@ParentId, Code=@Code, VATPayment=@VATPayment, TopManager=@TopManager,
			FAX=@FAX, Skype=@Skype, ICQ=@ICQ, ContactBirthDay=@ContactBirthDay, ContactCardId=@ContactCardId, ContactWorkPhone=@ContactWorkPhone, PF=@PF, RegionCode=@RegionCode, UserMargin=@UserMargin,Statut=@Statut, SelectedCount=@SelectedCount,
			AnonymousRegistration=@AnonymousRegistration, AnonymousAssignBy=@AnonymousAssignBy, AnonymousAssignAt=@AnonymousAssignAt, AnonymousAssignToCode=@AnonymousAssignToCode,
			AnonymousCreatedAt=@AnonymousCreatedAt, AnonymousAssignStatus=@AnonymousAssignStatus, AnonymousAssignByCode=@AnonymousAssignByCode, ManageAnonymousAssign=@ManageAnonymousAssign,
			PredmetCinnosti=@PredmetCinnosti, AnonymousOvereniSluzeb=@AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo=@AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText=@AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce=@AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce=@AnonymousSouhlasNavrzenehoPoradce,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE OrganizationId = @OrganizationId

		SET @Result = @OrganizationId

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

END
GO

------------------------------------------------------------------------------------------------------------------------
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
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			@ParentId, @Code, @VATPayment, @TopManager,
			@FAX, @Skype, @ICQ, @ContactBirthDay, @ContactCardId, @ContactWorkPhone, @PF, @RegionCode, @UserMargin, @Statut, @SelectedCount,
			@AnonymousRegistration, @AnonymousAssignBy, @AnonymousAssignAt, @AnonymousAssignToCode, @AnonymousCreatedAt, @AnonymousAssignStatus, @AnonymousAssignByCode, @ManageAnonymousAssign,
			@PredmetCinnosti, @AnonymousOvereniSluzeb, @AnonymousZmenaNaJineRegistracniCislo, @AnonymousZmenaNaJineRegistracniCisloText, @AnonymousSouhlasStavajicihoPoradce, @AnonymousSouhlasNavrzenehoPoradce,
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

