------------------------------------------------------------------------------------------------------------------------
-- 1) Script Eurona
-- 2) Script CL
-- 3) Upravit WebConfig Eurona
-- 4) Upravit WebConfig CL
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tOrganization ADD  
ZasilaniTiskovin BIT NOT NULL DEFAULT(0),
ZasilaniNewsletter BIT NOT NULL DEFAULT(0),
ZasilaniKatalogu BIT NOT NULL DEFAULT(0)
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
	Angel_team_clen, Angel_team_manager, Angel_team_manager_typ,
	ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu
FROM
	tOrganization o
	LEFT JOIN tPerson cp (NOLOCK) ON ContactPerson = cp.PersonId
	LEFT JOIN tAccount a (NOLOCK) ON a.AccountId = o.AccountId
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
	@ZasilaniTiskovin BIT = 0, 
	@ZasilaniNewsletter BIT = 0,
	@ZasilaniKatalogu BIT = 0,
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
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, 
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
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu,
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
			ZasilaniTiskovin = @ZasilaniTiskovin, ZasilaniNewsletter = @ZasilaniNewsletter, ZasilaniKatalogu=@ZasilaniKatalogu,
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
------------------------------------------------------------------------------------------------------------------------
ALTER VIEW [dbo].[vAdvisorAccounts]
--%%WITH ENCRYPTION%%
AS
SELECT DISTINCT TOP 100 PERCENT
	a.AccountId, a.TVD_Id, a.[InstanceId], a.[Login], a.[Password], a.[Email], a.[Enabled], a.Verified, a.VerifyCode, a.Locale, Credit = ISNULL(ac.Credit, 0 ),
	CanAccessIntensa = ISNULL(a.CanAccessIntensa, 0),
	CanAccessEurona = ISNULL(a.CanAccessEurona, 0), a.Roles,
	Created = ISNULL(a.Created, GETDATE()),
	AdvisorCode = o.Code, o.Name, Phone = o.ContactPhone, Mobile = o.ContactMobile,
	RegisteredAddress = (ar.Street + ', ' + ar.Zip + ', ' + ar.City +', ' + ar.State ),
	CorrespondenceAddress = (cr.Street + ', ' + cr.Zip + ', ' + cr.City +', ' + cr.State ),
	ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu
FROM
	tAccount a 
	LEFT JOIN vAccountsCredit ac (NOLOCK) ON ac.AccountId = a.AccountId
	INNER JOIN vOrganizations o (NOLOCK) ON o.AccountId = a.AccountId
	LEFT JOIN vAddresses ar (NOLOCK) ON ar.AddressId = o.RegisteredAddressId
	LEFT JOIN vAddresses cr (NOLOCK) ON cr.AddressId = o.CorrespondenceAddressId
WHERE
	a.HistoryId IS NULL
ORDER BY [Login]

GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [tError]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Stamp] [DATETIME] NOT NULL,
	[Location] NVARCHAR(500) NULL,
	[InstanceId] [int] NOT NULL,
	[Exception] NVARCHAR(MAX) NULL,
	[StackTrace] NVARCHAR(MAX) NULL
)
GO
------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tMimoradnaNabidka](
	[MimoradnaNabidkaId] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NULL,
	[Locale] [char](2) NULL CONSTRAINT [DF_tMimoradnaNabidka_Locale]  DEFAULT ('en'),
	[Date] [datetime] NULL,
	[Icon] [nvarchar](255) NULL,
	[Title] [nvarchar](500) NULL,
	[Teaser] [nvarchar](1000) NULL,
	[Content] [nvarchar](MAX) NULL,
	[UrlAliasId] [int] NULL,
 CONSTRAINT [PK_MimoradnaNabidkaId] PRIMARY KEY CLUSTERED ([MimoradnaNabidkaId] ASC)
)
GO

ALTER TABLE [tMimoradnaNabidka]  WITH CHECK 
	ADD  CONSTRAINT [FK_tMimoradnaNabidka_UrlAliasId] FOREIGN KEY([UrlAliasId])
	REFERENCES [tUrlAlias] ([UrlAliasId])
GO
ALTER TABLE [tMimoradnaNabidka] CHECK CONSTRAINT [FK_tMimoradnaNabidka_UrlAliasId]
GO

------------------------------------------------------------------------------------------------------------------------
-- tAngelTeamSettings
CREATE TABLE [tAngelTeamSettings](
	[DisableATP] [BIT] NOT NULL DEFAULT(0), 
	[MaxViewPerMinute] [int] NOT NULL DEFAULT(0),
	[BlockATPHours] [int] NOT NULL DEFAULT(0),
)
GO
------------------------------------------------------------------------------------------------------------------------
-- tAngelTeamViews
CREATE TABLE [tAngelTeamViews](
	[AccountId] [int] NOT NULL,
	[ViewDate] [DATETIME] NOT NULL,
	[ViewCount] [int] NOT NULL DEFAULT(0),
)
GO
------------------------------------------------------------------------------------------------------------------------
ALTER TABLE tShpProduct ADD InternalStorageCount INT NOT NULL DEFAULT(-1)
GO
------------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE pShpProductModifyEx
	@HistoryAccount INT,
	@ProductId INT,
	@MaximalniPocetVBaleni INT = NULL,
	@VamiNejviceNakupovane INT = NULL,
	@InternalStorageCount INT = -1,
	@Result INT = NULL OUTPUT
	--%%WITH ENCRYPTION%%
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM tShpProduct WHERE ProductId = @ProductId AND HistoryId IS NULL) 
		RAISERROR('Invalid ProductId %d', 16, 1, @ProductId);

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE tShpProduct
		SET
			MaximalniPocetVBaleni=@MaximalniPocetVBaleni, VamiNejviceNakupovane=@VamiNejviceNakupovane, InternalStorageCount=@InternalStorageCount,
			HistoryStamp = GETDATE(), HistoryType = 'M', HistoryAccount = @HistoryAccount, HistoryId = NULL
		WHERE ProductId = @ProductId

		SET @Result = @ProductId

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
ALTER VIEW [dbo].[vShpProducts]
--%%WITH ENCRYPTION%%
AS
SELECT TOP 100 PERCENT
	p.ProductId, p.InstanceId, p.Code, pl.[Name], p.[Manufacturer], pl.[Description], pl.[DescriptionLong], pl.[AdditionalInformation], pl.[InstructionsForUse], p.Availability, 
	p.StorageCount, Price=cp.Cena, BeznaCena = ISNULL(cp.BeznaCena, 0 ), MarzePovolena = ISNULL( cp.MarzePovolena, 0 ), MarzePovolenaMinimalni = ISNULL( cp.MarzePovolenaMinimalni, 0 ),
	cp.CurrencyId, CurrencySymbol=cur.Symbol, CurrencyCode=cur.Code , cp.Body, p.[Novinka], p.[Inovace], p.[Doprodej], p.[Vyprodano], p.[ProdejUkoncen], p.[Top], 
	p.[Megasleva],  p.[Supercena], p.[CLHit], p.[Action], p.[Vyprodej], p.[OnWeb], p.[InternalStorageCount],
	p.Parfumacia, p.Discount, cp.DynamickaSleva, cp.StatickaSleva, p.VamiNejviceNakupovane,
	p.VAT, a.UrlAliasId, a.Url, a.Alias,
	-- Comments and Votes (rating)
	CommentsCount = ( SELECT Count(*) FROM vShpProductComments WHERE ProductId = p.ProductId ),
	SalesCount = ( SELECT SUM(Quantity) FROM vShpCartProducts WHERE ProductId = p.ProductId ),
	ViewCount = ISNULL(p.ViewCount, 0 ), 
	Votes = ISNULL(p.Votes, 0), 
	TotalRating = ISNULL(p.TotalRating, 0),
	RatingResult =  ISNULL(p.TotalRating*1.0/p.Votes*1.0, 0 ),
	pl.Locale, p.MaximalniPocetVBaleni, BonusovyKredit = cp.[CenaBK]
FROM
	tShpProduct p 
	LEFT JOIN tShpProductLocalization pl ON pl.ProductId = p.ProductId
	LEFT JOIN tShpCenyProduktu cp ON cp.ProductId = p.ProductId AND cp.Locale = pl.Locale
	LEFT JOIN cShpCurrency cur ON cur.CurrencyId=cp.CurrencyId
	LEFT JOIN tUrlAlias a ON a.UrlAliasId = pl.UrlAliasId AND a.Locale = pl.Locale
WHERE
	p.HistoryId IS NULL

GO
------------------------------------------------------------------------------------------------------------------------
UPDATE tOrganization SET 
ZasilaniTiskovin = 1, ZasilaniNewsletter = 1, ZasilaniKatalogu = 1
GO
------------------------------------------------------------------------------------------------------------------------
/*
------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

DECLARE @MasterPageId INT, @ProductsFBMasterPageId INT
SET @MasterPageId = 1

DECLARE @UrlAliasId INT
DECLARE @PageId INT
DECLARE @pageTitle NVARCHAR(100), @pageName NVARCHAR(100), @pageUrl NVARCHAR(100), @pageAlias NVARCHAR(100)


--Error content	
-- !!! stranka volana v kontente inej stranky !!! nemusi mat UrlAlias !!!	
SET IDENTITY_INSERT tPage ON
INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1991, @MasterPageId, '', 'sk', 'error-content', 'Chyba stránky', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1992, @MasterPageId, '', 'cs', 'error-content', 'Chyba stránky', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1993, @MasterPageId, '', 'en', 'error-content', 'Chyba stránky', GETDATE(), 'C', 1)

INSERT INTO tPage (InstanceId,PageId, MasterPageId, Content, Locale, [Name], Title, HistoryStamp, HistoryType, HistoryAccount ) 
VALUES (@InstanceId, -1994, @MasterPageId, '', 'pl', 'error-content', 'Chyba stránky', GETDATE(), 'C', 1)
SET IDENTITY_INSERT tPage OFF

GO
*/
------------------------------------------------------------------------------------------------------------------------