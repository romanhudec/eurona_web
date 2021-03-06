ALTER TABLE tAccount ADD SingleUserCookieLinkEnabled BIT DEFAULT(1)
GO
UPDATE tAccount SET SingleUserCookieLinkEnabled=1
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles, a.MustChangeAccount, a.PasswordChanged, a.SingleUserCookieLinkEnabled,
	Created = ISNULL(a.Created, GETDATE())
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac ON ac.AccountId = a.AccountId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]


GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pAccountCreate]
	@HistoryAccount INT,
	@InstanceId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000) = 'D41D8CD98F00B204E9800998ECF8427E', -- empty string
	@Email NVARCHAR(100) = NULL,
	@Enabled BIT = 1,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@Verified BIT = 0,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@SingleUserCookieLinkEnabled bit = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL) BEGIN
		-- ak account existuje vrati existujuce ID
		SELECT @Result=AccountId FROM tAccount WHERE [Login] = @Login AND InstanceId = @InstanceId AND HistoryId IS NULL
		SELECT AccountId = @Result
		RETURN
	END

	
	IF LEN(ISNULL(@VerifyCode, '')) = 0 BEGIN
		DECLARE @GeneratedCode NVARCHAR(1000)
		SET @GeneratedCode = CONVERT(NVARCHAR(1000), RAND(DATEPART(ms, GETDATE())) * 1000000)
		SET @GeneratedCode = SUBSTRING(@GeneratedCode, LEN(@GeneratedCode) - 4, 4)
		SET @VerifyCode = @GeneratedCode
	END

	INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [VerifyCode], [Verified], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled,
		HistoryStamp, HistoryType, HistoryAccount, Created)
	VALUES (@InstanceId, @TVD_Id, @Login, @Password, @Email, @Enabled, @VerifyCode, @Verified, @CanAccessIntensa, @CanAccessEurona, @Roles, @MustChangeAccount, @PasswordChanged, @SingleUserCookieLinkEnabled,
		GETDATE(), 'C', @HistoryAccount, GETDATE())
	
	SET @Result = SCOPE_IDENTITY()
	
	IF @Roles IS NOT NULL BEGIN
		INSERT INTO tAccountRole ( InstanceId, AccountId, RoleId)
		SELECT @InstanceId, @Result, r.RoleId
			FROM dbo.fStringToTable(@Roles, ';') x
			INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
	END	

	SELECT AccountId = @Result

END
GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pAccountModify]
	@HistoryAccount INT,
	@AccountId INT,
	@Login NVARCHAR(30),
	@Password NVARCHAR(1000),
	@Email NVARCHAR(100) = NULL,
	@Roles NVARCHAR(4000) = NULL,
	@MustChangeAccount BIT = 0,
	@PasswordChanged DATETIME = NULL,
	@Enabled BIT,
	@Locale CHAR(2),
	@Verified BIT = NULL,
	@VerifyCode NVARCHAR(1000) = NULL,
	@TVD_Id INT = NULL,
	@CanAccessIntensa INT = 0,
	@CanAccessEurona INT = 0,
	@SingleUserCookieLinkEnabled bit = 1,
	@Result INT = NULL OUTPUT
--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tAccount WHERE AccountId = @AccountId AND HistoryId IS NULL) BEGIN
		RAISERROR('Invalid AccountId %d', 16, 1, @AccountId);
		RETURN
	END
	
	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO tAccount ( InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, CanAccessEurona, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId)
		SELECT
			InstanceId, TVD_Id, [Login], [Password], [Email], [Enabled], [Verified], [VerifyCode], [Locale], CanAccessIntensa, @CanAccessEurona, Roles, @MustChangeAccount, @PasswordChanged, @SingleUserCookieLinkEnabled,
			HistoryStamp, HistoryType, HistoryAccount, @AccountId
		FROM tAccount
		WHERE AccountId = @AccountId

		UPDATE tAccount 
		SET
			TVD_Id=ISNULL(@TVD_Id,TVD_Id), Roles=ISNULL(@Roles, Roles), [Login] = @Login, [Password] = @Password, Email = @Email, [Enabled] = @Enabled, [Locale] = @Locale, CanAccessIntensa=@CanAccessIntensa, CanAccessEurona=@CanAccessEurona, SingleUserCookieLinkEnabled=@SingleUserCookieLinkEnabled,
			MustChangeAccount = @MustChangeAccount, PasswordChanged = @PasswordChanged, Verified = ISNULL(@Verified, Verified), VerifyCode = ISNULL(@VerifyCode, VerifyCode),
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE AccountId = @AccountId
		
		IF @Roles IS NOT NULL BEGIN
			DELETE FROM tAccountRole WHERE AccountId = @AccountId
			INSERT INTO tAccountRole (AccountId, RoleId)
			SELECT @AccountId, r.RoleId
				FROM dbo.fStringToTable(@Roles, ';') x
				INNER JOIN tRole r (NOLOCK) ON r.Name = x.item
		END

		SET @Result = @AccountId

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

------------------------------------------------------------------------------------------------------------------------
INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (1, 'ACCOUNT_LINK_COOKIES_LIMIT', 'USER', 'Platnost cookie odkazu (dnů)', '30')
GO

------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tOrganization ADD RegistrationFromCookiesLinkAccountId INT NULL
GO
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vOrganizations]
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
	AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousTempAssignAt,AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
	AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
	Angel_team_clen, Angel_team_manager, Angel_team_manager_typ,
	ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, RegistrationFromCookiesLinkAccountId
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a (NOLOCK) ON a.AccountId = o.AccountId
WHERE
	o.HistoryId IS NULL
ORDER BY o.Name


GO

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pOrganizationCreate]
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
	@RegistrationFromCookiesLinkAccountId INT = NULL,
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
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, RegistrationFromCookiesLinkAccountId,
			HistoryStamp, HistoryType, HistoryAccount
		) VALUES (
			@InstanceId, @AccountId, @Id1, @Id2, @Id3, @Name, @Notes, @Web, 
			@ContactEMail, @ContactPhone, @ContactMobile, @ContactPersonId,
			@RegisteredAddressId, @CorrespondenceAddressId, @InvoicingAddressId, @BankContactId, 
			@ParentId, @Code, @VATPayment, @TopManager,
			@FAX, @Skype, @ICQ, @ContactBirthDay, @ContactCardId, @ContactWorkPhone, @PF, @RegionCode, @UserMargin, @Statut, @SelectedCount,
			@AnonymousRegistration, @AnonymousAssignBy, @AnonymousAssignAt, @AnonymousAssignToCode, @AnonymousCreatedAt, @AnonymousAssignStatus, @AnonymousAssignByCode, @ManageAnonymousAssign,
			@PredmetCinnosti, @AnonymousOvereniSluzeb, @AnonymousZmenaNaJineRegistracniCislo, @AnonymousZmenaNaJineRegistracniCisloText, @AnonymousSouhlasStavajicihoPoradce, @AnonymousSouhlasNavrzenehoPoradce,
			@ZasilaniTiskovin, @ZasilaniNewsletter, @ZasilaniKatalogu, @RegistrationFromCookiesLinkAccountId,
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

------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[pOrganizationModify]
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
	@AnonymousTempAssignAt DATETIME = NULL,
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
	@RegistrationFromCookiesLinkAccountId INT = NULL,
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
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousTempAssignAt, AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
			PredmetCinnosti, AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, RegistrationFromCookiesLinkAccountId,
			HistoryStamp, HistoryType, HistoryAccount, HistoryId
		)
		SELECT
			InstanceId, Id1, Id2, Id3, Name, Notes, Web, 
			ContactEMail, ContactPhone, ContactMobile, ContactPerson,
			RegisteredAddress, CorrespondenceAddress, InvoicingAddress, BankContact,
			ParentId, Code, VATPayment, TopManager,
			FAX, Skype, ICQ, ContactBirthDay, ContactCardId, ContactWorkPhone, PF, RegionCode, UserMargin, Statut, SelectedCount,
			AnonymousRegistration, AnonymousAssignBy, AnonymousAssignAt, AnonymousTempAssignAt, AnonymousAssignToCode, AnonymousCreatedAt, AnonymousAssignStatus, AnonymousAssignByCode, ManageAnonymousAssign,
			PredmetCinnosti, AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce,
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, RegistrationFromCookiesLinkAccountId,
			HistoryStamp, HistoryType, HistoryAccount, @OrganizationId
		FROM tOrganization
		WHERE OrganizationId = @OrganizationId

		UPDATE tOrganization 
		SET
			Id1 = @Id1, Id2 = @Id2, Id3 = @Id3, Name = @Name, Notes = @Notes, Web = @Web, 
			ContactEMail = @ContactEMail, ContactPhone = @ContactPhone, ContactMobile = @ContactMobile, 
			ParentId=@ParentId, Code=@Code, VATPayment=@VATPayment, TopManager=@TopManager,
			FAX=@FAX, Skype=@Skype, ICQ=@ICQ, ContactBirthDay=@ContactBirthDay, ContactCardId=@ContactCardId, ContactWorkPhone=@ContactWorkPhone, PF=@PF, RegionCode=@RegionCode, UserMargin=@UserMargin,Statut=@Statut, SelectedCount=@SelectedCount,
			AnonymousRegistration=@AnonymousRegistration, AnonymousAssignBy=@AnonymousAssignBy, AnonymousAssignAt=@AnonymousAssignAt, AnonymousTempAssignAt=@AnonymousTempAssignAt, AnonymousAssignToCode=@AnonymousAssignToCode,
			AnonymousCreatedAt=@AnonymousCreatedAt, AnonymousAssignStatus=@AnonymousAssignStatus, AnonymousAssignByCode=@AnonymousAssignByCode, ManageAnonymousAssign=@ManageAnonymousAssign,
			PredmetCinnosti=@PredmetCinnosti, AnonymousOvereniSluzeb=@AnonymousOvereniSluzeb, AnonymousZmenaNaJineRegistracniCislo=@AnonymousZmenaNaJineRegistracniCislo, AnonymousZmenaNaJineRegistracniCisloText=@AnonymousZmenaNaJineRegistracniCisloText, AnonymousSouhlasStavajicihoPoradce=@AnonymousSouhlasStavajicihoPoradce, AnonymousSouhlasNavrzenehoPoradce=@AnonymousSouhlasNavrzenehoPoradce,
			ZasilaniTiskovin = @ZasilaniTiskovin, ZasilaniNewsletter = @ZasilaniNewsletter, ZasilaniKatalogu=@ZasilaniKatalogu, RegistrationFromCookiesLinkAccountId=@RegistrationFromCookiesLinkAccountId,
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
CREATE TABLE tSingleUserCookieLinkActivity(
	Id INT IDENTITY(1,1) NOT NULL,
	[Url] NVARCHAR(500) NOT NULL,
	[UrlTimestamp] DATETIME NOT NULL,
	[IPAddress] NVARCHAR(50) NULL,
	[CookieAccountId] INT NOT NULL,
	[RegistrationAccountId] INT NULL,
	[RegistrationTimestamp] DATETIME NULL,
	Timestamp DATETIME NOT NULL,
 CONSTRAINT [PK_SingleUserCookieLinkActivity] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]

GO
------------------------------------------------------------------------------------------------------------------------

INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (1, 'ACCOUNT_LINK_COOKIES_ENABLED', 'USER', 'Povolení cookie odkazu', 'true')
GO
------------------------------------------------------------------------------------------------------------------------
INSERT INTO tSettings (InstanceId, Code, GroupName, Name, Value ) VALUES (1, 'ESHOP_PLATBAKARTOU_ZDRUZENE_OBJEDNAVKY', 'ESHOP', 'Povolení platby kartou združených objednávek', 'true')
GO






