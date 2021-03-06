
------------------------------------------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------
DECLARE @InstanceId INT
SET @InstanceId = 1

ALTER TABLE tOrganization ADD AnonymousTempAssignAt DATETIME NULL
GO

ALTER TABLE tAnonymniRegistrace ADD MaxPocetPrijetychNovacku INT NULL
GO

ALTER TABLE tAnonymniRegistrace ADD ZobrazitVSeznamuLimit NVARCHAR(MAX) NULL
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
			ZasilaniTiskovin, ZasilaniNewsletter, ZasilaniKatalogu, 
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
			AnonymousRegistration=@AnonymousRegistration, AnonymousAssignBy=@AnonymousAssignBy, AnonymousAssignAt=@AnonymousAssignAt, AnonymousTempAssignAt=@AnonymousTempAssignAt, AnonymousAssignToCode=@AnonymousAssignToCode,
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
delete from tUrlAlias where UrlAliasId in (
	select UrlAliasId from tUrlAlias where Url  LIKE '~/eshop/product.aspx?id=%'
	AND UrlAliasId not IN (SELECT UrlAliasId from tShpProductLocalization)
)
GO

delete from tShpProductLocalization where UrlAliasId in (select UrlAliasId from tUrlAlias where Url LIKE '%ProductLiv.aspx%')
delete from tUrlAlias where Url LIKE '%ProductLiv.aspx%'
GO
------------------------------------------------------------------------------------------------------------------------

-- TVD
CREATE TABLE [dbo].[www_odberatele_stromZ](
	[Id] [int] NOT NULL,
	[Id_Odberatele] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[LineageId] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_www_odberatele_stromZ] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
------------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[fOdberateleStromZ](@Id_Odberatele as INT)
RETURNS @table TABLE(ID INT IDENTITY(1,1) NOT NULL,
		Id_Odberatele int null,
		Level int NULL,
		LineageId nvarchar(2000)
)
AS
BEGIN

	Declare @Tier as int
	SET @Tier = 2

	INSERT INTO @table (Id_Odberatele,Level,LineageId) 
	VALUES(@Id_Odberatele, 1, '(1)')

	INSERT INTO @table
	Select Id_Odberatele, 2, '(1)' from odberatele where Cislo_nadrizeneho = @Id_Odberatele AND Id_odberatele!=Cislo_nadrizeneho

	UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'

	WHILE @@rowcount > 0 BEGIN
		SET @Tier = @Tier + 1
		/*Go get children nodes for the next tier that are not already accounted for */

		INSERT INTO @table (Id_Odberatele,Level,LineageId)
		SELECT Id_Odberatele, @Tier, (select LineageId from @table where Id_Odberatele = Cislo_nadrizeneho) 
		FROM odberatele 
		WHERE Id_odberatele!=Cislo_nadrizeneho AND Cislo_nadrizeneho IN (select Id_Odberatele from @table) 
		AND Id_Odberatele NOT in (select Id_Odberatele from @table)

		UPDATE @table SET LineageId = LineageId + '(' + LTRIM(STR(ID)) + ')' WHERE LineageId NOT LIKE '%(' + LTRIM(STR(ID)) + ')%'
	END
	
	RETURN;
END
GO
------------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[fGetOdberateleStromZ](@Id_Odberatele as INT)
RETURNS @table TABLE( Id_Odberatele int null, LineageId nvarchar(2000) null )
AS
BEGIN

	DECLARE @LineageId NVARCHAR(2000)
	SELECT @LineageId = LineageId FROM www_odberatele_stromZ WHERE Id_Odberatele=@Id_Odberatele

	IF @LineageId IS NULL
	BEGIN
		INSERT INTO @table
			SELECT Id_Odberatele, LineageId FROM dbo.fOdberateleStromZ(@Id_Odberatele)
    END
	ELSE
	BEGIN
		INSERT INTO @table
			SELECT Id_Odberatele, LineageId FROM www_odberatele_stromZ WHERE LineageId LIKE @LineageId + '%'
	END
	
	RETURN;
END